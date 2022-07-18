using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogViewer
{
    /// <summary>
    /// 管理文件内存映射，从文件中读取数据到内存中缓存着，如果需要的数据过多会释放掉早期的
    /// </summary>
    public class FileSlice : IDisposable
    {
        internal class Slice
        {
            internal Slice(long alignStart, int alignBufLength)
            {
                Debug.Assert(alignStart % s_defaultBufferLength == 0);
                Debug.Assert(alignBufLength > 0 && 0 == alignBufLength % s_defaultBufferLength);
                Buffer = new byte[alignBufLength];
                AlignStart = alignStart;
            }

            internal long AlignEnd { get => AlignStart + Buffer.Length; }
            internal long AlignStart = 0;
            internal int DataSize = 0;
            internal byte[] Buffer;
            internal int Length { get => Buffer.Length; }
            internal long DataPosition { get => AlignStart + DataSize; }

            internal void Extend(int newLength)
            {
                if (newLength <= Buffer.Length) return;

                byte[] newBuf = new byte[newLength];
                if (DataSize > 0)
                    Array.Copy(Buffer, newBuf, DataSize);
                Buffer = newBuf;
            }
        }

        public Stream PeekStream() {
            var s = fs;
            fs = null;
            return s;
        }

        #region public Property

        public long FreshFileLength() { FileLength = fs.Length; return FileLength; }
        public long FileLength { get; private set; } = 0;
        public long NewData { get => FreshFileLength() - lastFileLength; }
        public bool CanRead { get => fs.CanRead; }
        public long SpaceUsage { get { lock (lck) return slices.Sum(s => s.Value.Length); } }
        public double Progress { get => ((double)lastFileLength / (double)FileLength * 100); }

        #endregion public Property

        #region private field

        static readonly int s_defaultBufferLength = 8 * 1024 * 1024;
        readonly object lck = new object();
        readonly SortedList<long, Slice> slices = new SortedList<long, Slice>();
        Stream fs;
        long lastFileLength = 0;
        bool dispose = false;

        #endregion private field

        public FileSlice(Stream _fs)
        {
            Debug.Assert(_fs.CanRead && _fs.CanSeek);
            fs = _fs;
        }

        public void Dispose()
        {
            if (!dispose)
            {
                dispose = true;
                lock (lck)
                {
                    fs?.Close();
                    slices?.Clear();
                }
            }
        }

        /// <summary>
        /// 读满整个Slice或者读取到文件结尾
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool ReadSlice(Slice s)
        {
            lock (lck)
            {
                if (s.DataSize >= s.Length)
                {
                    Debug.Assert(s.DataSize == s.Length);
                    return true;
                }

                Debug.Assert(lastFileLength <= FileLength);
                long length = Math.Min(s.AlignEnd, FileLength) - s.DataPosition;
                if (length <= 0) return false;

                Debug.Assert(length < int.MaxValue);
                FileStream f = fs as FileStream;
                try
                {
                    f?.Lock(s.AlignStart, length);

                    fs.Seek(s.DataPosition, SeekOrigin.Begin);
                    int readSize = fs.Read(s.Buffer, s.DataSize, (int)length);
                    if (readSize > 0)
                    {
                        Debug.Assert(readSize <= length);
                        s.DataSize += readSize;
                        if (s.DataPosition > lastFileLength)
                        {
                            lastFileLength = s.DataPosition;
                        }
                        return true;
                    }
                    return false;
                }
                finally
                {
                    f?.Unlock(s.AlignStart, length);
                }
            }
        }

        Slice GetBuffer(long offset, int length)
        {
            if (offset >= FileLength) return null;

            long alignStart = offset - (offset % s_defaultBufferLength);
            long alignEnd = alignStart + s_defaultBufferLength;
            while (alignEnd < offset + length)
                alignEnd += s_defaultBufferLength;

            lock (lck)
            {
                for (int i = slices.Count - 1; i >= 0; --i)
                {
                    var s = slices[i];
                    if (s.AlignStart > alignStart) continue;

                    if (s.AlignEnd >= alignEnd) return s;

                    if (alignEnd - s.AlignEnd <= s_defaultBufferLength)
                    {
                        s.Extend(s.Length + s_defaultBufferLength);
                        return s;
                    }

                    break;
                }

                var newSlice = new Slice(alignStart, (int)(alignEnd - alignStart));
                slices[newSlice.AlignStart] = newSlice;
                return newSlice;
            }
        }

        public string Read(long fileOffset, int length, Encoding enc)
        {
            var bs = Read(fileOffset, length);
            if (bs == null) return null;

            enc = enc ?? Tools.GetEncoding(bs);
            return enc?.GetString(bs);
        }

        public byte[] Read(long fileOffset, int length)
        {
            if (length <= 0) return null;

            var s = GetBuffer(fileOffset, length);
            if (s == null) return null;

            if (s.DataPosition < fileOffset + length)
            {
                ReadSlice(s);
            }

            long end = Math.Min(s.DataPosition, fileOffset + length);

            int newLength = (int)(end - fileOffset);
            if (newLength > 0)
            {
                byte[] res = new byte[newLength];
                System.Buffer.BlockCopy(s.Buffer, (int)(fileOffset - s.AlignStart), res, 0, newLength);
                return res;
            }
            return null;
        }
    }
}
