using BrightIdeasSoftware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogViewer
{
    public class FilterRule
    {
        public int NumContextLines = 0;
        public string Filter1 = null;
        public string Filter2 = null;
        public string Filter3 = null;
        public bool FilterOrAnd = true; // true='OR' false='AND'
        public string Module = null;
        public TimeFilter Dt = TimeFilter.None;
        public int Pid = -1;
        public int Tid = -1;
        public int Seq = -1;
        public ItemLevel Level = ItemLevel.None;

        public bool Valid
        {
            get => Filter1 != null ||
                Filter2 != null ||
                Filter3 != null ||
                Module != null ||
                Dt != TimeFilter.None ||
                Pid != -1 ||
                Tid != -1 ||
                Seq != -1 ||
                Level != ItemLevel.None;
        }

        internal bool CheckFilter(LogItem li, string msg)
        {
            return CheckFilter(li) && CheckFilter(msg);
        }

        internal bool CheckFilter(string msg)
        {
            if (Filter1 == null && Filter2 == null && Filter3 == null)
                return true;

            if (FilterOrAnd)
            {
                if (Filter1 != null && msg.Contains(Filter1)) return true;
                if (Filter2 != null && msg.Contains(Filter2)) return true;
                if (Filter3 != null && msg.Contains(Filter3)) return true;

                return false;
            }
            else
            {
                if (Filter1 != null && !msg.Contains(Filter1)) return false;
                if (Filter2 != null && !msg.Contains(Filter2)) return false;
                if (Filter3 != null && !msg.Contains(Filter3)) return false;

                return true;
            }
        }
        internal bool CheckFilter(LogItem li)
        {
            if (li != null)
            {
                if (Pid != -1 && Pid != li.Pid) return false;
                if (Tid != -1 && Tid != li.Tid) return false;
                if (Seq != -1 && Seq > li.Seq) return false;
                if (Module != null && Module != li.Module) return false;
                if (Level > li.Level) return false;
                return Dt.CheckNow(li.Time);
            }

            return true;
        }

        public static bool operator !=(FilterRule a, FilterRule b) => !(a == b);
        public static bool operator ==(FilterRule a, FilterRule b) => Equals(a, b);
        public static bool Equals(FilterRule a, FilterRule b)
        {
            a = a ?? new FilterRule();
            b = b ?? new FilterRule();
            return a.Dt == b.Dt &&
                   a.Filter1 == b.Filter1 &&
                   a.Filter2 == b.Filter2 &&
                   a.Filter3 == b.Filter3 &&
                   a.FilterOrAnd == b.FilterOrAnd &&
                   a.Level == b.Level &&
                   a.Module == b.Module &&
                   a.Pid == b.Pid &&
                   a.Tid == b.Tid &&
                   a.Seq == b.Seq;
        }
        public override bool Equals(Object b)
        {
            if (b is FilterRule bb)
            {
                return this == bb;
            }
            return false;
        }
        public override int GetHashCode()
        {
            if (this == null) return 0;
            int code = 0;
            code ^= this.Pid;
            code ^= this.Tid;
            code ^= this.Seq;
            code ^= this.Filter1.GetHashCode();
            code ^= this.Filter2.GetHashCode();
            code ^= this.Filter3.GetHashCode();
            code ^= this.Module.GetHashCode();
            code ^= (int)this.Level;

            return code;
        }
    }

    internal class LogFileInfo : IDisposable
    {
        public void Dispose()
        {
            this.Items.Clear();
            this.Lines.Clear();
            this.LongestLine = new LogLine();
            this.LineCount = 0;
            this.Pids.Clear();
            this.Tids.Clear();
            this.fs.Dispose();
        }
        internal LogFileInfo(Stream _fs, string _headFormatSeed, Collection<string> _headerFormatKeys, Encoding enc)
        {
            fs = new FileSlice(_fs);
            this.headFormatSeed = new HeaderFormatSeed(_headFormatSeed, _headerFormatKeys);
            Enc = enc;
        }

        readonly Mutex readMutex = new Mutex();
        readonly Encoding Enc;
        readonly HeaderFormatSeed headFormatSeed;

        internal readonly FileSlice fs;

        public FilterRule Filter { get; set; }
        public List<LogItem> Items { get; } = new List<LogItem>();
        public List<LogLine> Lines { get; } = new List<LogLine>();

        public Collection<int> Pids { get; private set; } = new Collection<int>();
        /// <summary>
        /// key: process id
        /// value: Collection with thread id
        /// </summary>
        public Dictionary<int, Collection<int>> Tids { get; private set; } = new Dictionary<int, Collection<int>>();

        public Stream PeekStream() => fs.PeekStream();

        public LogLine LongestLine { get; private set; } = new LogLine();
        public int LineCount { get; private set; } = 0;

        public int FilterItemCount { get; set; } = 0;

        internal string GetEncoding(byte[] bs) => GetEncoding(bs, 0, bs.Length);
        internal string GetEncoding(byte[] bs, int offset, int len)
        {
            if (Enc != null) return Enc.GetString(bs, offset, len);
            return Tools.GetEncoding(bs, offset, len).GetString(bs, offset, len);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="charCount"></param>
        internal void AddLine(long offset, int charCount)
        {
            LogLine ll = new LogLine
            {
                Offset = offset,
                CharCount = charCount,
                LineNumber = this.LineCount
            };

            this.Lines.Add(ll);
            if (charCount > this.LongestLine.CharCount)
            {
                this.LongestLine.CharCount = charCount;
                this.LongestLine.LineNumber = ll.LineNumber;
            }

            this.LineCount++;
            LogItem li = new LogItem();
            string s = GetLine(ll.LineNumber);
            if (Tools.TryParseHeader(s, li, headFormatSeed))
            {
                if (this.Items.Count > 0)
                {
                    Debug.Assert(ll.LineNumber > 0);
                    this.Items.Last().LineNumberEnd = ll.LineNumber - 1;
                }
                li.LineNumberStart = ll.LineNumber;
                ll.Item = li;
                if (li.Pid != -1)
                {
                    if (!Pids.Contains(li.Pid)) Pids.Add(li.Pid);
                    if (!Tids.ContainsKey(li.Pid)) Tids.Add(li.Pid, new Collection<int>());
                    if (li.Tid != -1 && !Tids[li.Pid].Contains(li.Tid)) Tids[li.Pid].Add(li.Tid);
                }

                this.Items.Add(li);
            }
            else
            {
                if (this.Items.Count > 0)
                {
                    ll.Item = this.Items.Last();
                    li = ll.Item;
                }

                Debug.Assert(s != null);
            }

            if (Filter != null)
            {
                if (Filter.CheckFilter(li, s)) ll.IsFilterLine = true;

                int index = ll.LineNumber - Filter.NumContextLines;
                index = (index >= 0 ? index : 0);
                for (int i = index; i < ll.LineNumber; i++)
                {
                    if (Lines[i].IsFilterLine)
                    {
                        ll.IsContextFilter = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定行的内容
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public string GetLine(int lineNumber)
        {
            if (lineNumber >= Lines.Count)
            {
                return string.Empty;
            }

            byte[] buffer = null;
            try
            {
                this.readMutex.WaitOne();
                if (Lines[lineNumber].CharCount > 0)
                    buffer = fs.Read(Lines[lineNumber].Offset, Lines[lineNumber].CharCount);
                this.readMutex.ReleaseMutex();
            }
            catch (Exception) { }

            if (buffer == null) return "";

            //return Regex.Replace(Encoding.ASCII.GetString(buffer), "[\0-\b\n\v\f\x000E-\x001F\x007F-ÿ]", "", RegexOptions.Compiled);
            return Regex.Replace(GetEncoding(buffer), "[\0-\b\n\v\f\x000E-\x001F\x007F-ÿ]", "", RegexOptions.Compiled);
        }
    }

    internal class LogFile
    {
        #region Delegates
        public delegate void FilterCompleteEvent(LogFile lf, string fileName, TimeSpan duration, long matches, bool cancelled);
        public delegate void SearchCompleteEvent(LogFile lf, string fileName, TimeSpan duration, long matches, int numSearchTerms, bool cancelled);
        public delegate void CompleteEvent(LogFile lf, string fileName, TimeSpan duration, bool cancelled);
        public delegate void BoolEvent(string fileName, bool val);
        public delegate void MessageEvent(string fileName, string message);
        public delegate void ProgressUpdateEvent(int percent);
        #endregion

        #region Events
        public event FilterCompleteEvent FilterComplete;
        public event SearchCompleteEvent SearchComplete;
        public event CompleteEvent LoadComplete;
        public event CompleteEvent UpdateComplete;
        public event CompleteEvent ExportComplete;
        public event ProgressUpdateEvent ProgressUpdate;
        public event MessageEvent LoadError;
        #endregion

        LogFileInfo info = null;

        #region Member Variables
        Mutex mutex = new Mutex();

        Encoding Enc { get; set; }
        string headerFormat { get; }
        Collection<string> headerFormatKeys { get; }
        public FilterRule Filter { get => info.Filter; set => info.Filter = value; }
        public int FilterItemCount { get => info.FilterItemCount; set => info.FilterItemCount = value; }
        public List<LogLine> Lines { get => info.Lines; }
        public List<LogItem> Items { get => info.Items; }
        public Collection<int> Pids { get => info.Pids; }
        public Dictionary<int, Collection<int>> Tids { get => info.Tids; }
        public int LineCount { get => info.LineCount; }
        public LogLine LongestLine { get => info.LongestLine; }
        public string GetLine(int num) => info.GetLine(num);

        private Color filterColour { get; set; } = Color.AliceBlue;
        private Color highlightColour { get; set; } = Color.Lime;
        private Color contextColour { get; set; } = Color.LightGray;
        public Searches Searches { get; set; }
        public Global.ViewMode ViewMode { get; set; } = Global.ViewMode.Standard;

        public Uri Url { get; private set; }
        public Stream FileStream { get; private set; } = null;
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public Stream PeekStream() { this.FileStream = null; return info.PeekStream(); }
        public List<ushort> FilterIds { get; private set; } = new List<ushort>();
        public FastObjectListView List { get; set; }
        public string Guid { get; private set; }

        public int NumTabSpaces { get; set; } = 4;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public LogFile(string _headerFormat, Collection<string> keys, Encoding enc)
        {
            this.Enc = enc;
            this.Guid = System.Guid.NewGuid().ToString();
            this.Searches = new Searches();
            this.headerFormat = _headerFormat;
            this.headerFormatKeys = keys;
        }

        public Stream Trunc()
        {
            string fPath = FilePath;
            string fName = FileName;
            Uri uri = Url;
            Stream fs = FileStream;

            try
            {
                if (fs != null && fs.CanSeek && fs.CanWrite)
                {
                    lock (mutex)
                    {
                        PeekStream();
                        fs.Seek(0, SeekOrigin.Begin);
                        fs.SetLength(0);
                    }
                    Dispose();
                }
                else if (!string.IsNullOrEmpty(fPath))
                {
                    fs.Close();
                    fs = null;
                    Dispose();
                    using (var fs1 = new FileStream(fPath, FileMode.Truncate))
                    {
                        fs1.Flush();
                        fs1.Close();
                    }
                }

            }
            finally
            {
                FilePath = fPath;
                FileName = fName;
                Url = uri;
                FileStream = fs;
            }
            return fs;
        }

#if DEBUG
        private string sBufferRemainder;
#endif
        // Calcs and finally point the position to the end of the line
        internal long position = 0;
        private int loadFile(DateTime start, CancellationToken ct, bool bProgressUpdate)
        {
            if (this.info == null)
            {
                Stream fs = null;
                if (FileStream != null && FileStream.CanRead)
                {
                    fs = FileStream;
                }
                else
                {
                    try
                    {
                        fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read | FileAccess.Write, FileShare.ReadWrite | FileShare.Delete, 4096, FileOptions.RandomAccess);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, 4096, FileOptions.RandomAccess);
                    }

                    FileStream = fs;
                }

                Enc = Tools.GetTxtCodePage(fs) ?? Enc;
                this.info = new LogFileInfo(fs, headerFormat, headerFormatKeys, Enc);
                position = 0;
            }

            // int counter = 0;
            int result = 0;
            while (info.fs.NewData > 0)
            {
                int length = 8 * 1024 * 1024;
                byte[] tempBuffer = info.fs.Read(position, length);
                if (tempBuffer == null)
                    break;

                int startIndex = 0;
                int indexOf;
                while (startIndex < tempBuffer.Length && (indexOf = FindLineEnd(tempBuffer, startIndex, tempBuffer.Length)) != -1)
                {
                    int charCount = 0;
                    long lineStartOffset = position;
                    // Check if the line contains a CR as well, if it does then we remove the last char as the char count
                    if (indexOf != 0 && (int)tempBuffer[Math.Max(0, indexOf - 1)] == 13)
                    {
                        charCount = (indexOf - startIndex - 1);
                        position += (long)charCount + 2L;
                    }
                    else
                    {
                        charCount = (indexOf - startIndex);
                        position += (long)charCount + 1L;
                    }

                    info.AddLine(lineStartOffset, charCount);
                    result++;

                    // Set the offset to the end of the last line that has just been added
                    lineStartOffset = position;
                    startIndex = indexOf + 1;
                }

#if DEBUG
                if (startIndex < tempBuffer.Length)
                {
                    sBufferRemainder = info.GetEncoding(tempBuffer, startIndex, tempBuffer.Length - startIndex);
                }
#endif

                //if (counter++ % 10 == 0)
                {
                    if (bProgressUpdate)
                        OnProgressUpdate((int)info.fs.Progress);

                    if (ct.IsCancellationRequested)
                        break;
                }
            } // WHILE

            if (bProgressUpdate)
            {
                DateTime end = DateTime.Now;
                OnProgressUpdate(100);
                if (result > 0 || ct == null || !ct.IsCancellationRequested)
                    OnLoadComplete(end - start, false);
                else
                    OnLoadComplete(end - start, true);
            }

            return result;
        }

        internal int FindLineEnd(byte[] buf, int start, int end)
        {
            Debug.Assert(start <= end);
            int i = start;
            for (; i < end; ++i)
            {
                if (buf[i] == '\n')
                {
                    if (info.GetEncoding(buf, start, i + 1 - start).EndsWith("\n"))
                        return i;
                }
            }
            return -1;
        }

        #region Public Methods
        public void Load(Uri uri, Stream fs, SynchronizationContext _, CancellationToken ct)
        {
            this.Dispose();
            this.FilePath = null;
            this.FileName = Path.GetFileName(uri.Host + uri.PathAndQuery);
            this.Url = uri;
            this.FileStream = fs;

            Task.Run(() =>
            {
                mutex.WaitOne();
                DateTime start = DateTime.Now;
                bool cancelled = false;
                bool error = false;
                try
                {
                    loadFile(start, ct, true);
                }
                catch (IOException ex)
                {
                    OnLoadError(ex.Message);
                    error = true;
                }
                finally
                {
                    if (error == false)
                    {
                        DateTime end = DateTime.Now;

                        OnProgressUpdate(100);
                        OnLoadComplete(end - start, cancelled);
                    }

                    mutex.ReleaseMutex();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ct"></param>
        public void Load(string filePath, SynchronizationContext _, CancellationToken ct)
        {
            this.Dispose();
            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);

            Task.Run(() =>
            {
                mutex.WaitOne();
                DateTime start = DateTime.Now;
                bool cancelled = false;
                bool error = false;
                try
                {
                    loadFile(start, ct, true);
                }
                catch (IOException ex)
                {
                    OnLoadError(ex.Message);
                    error = true;
                }
                finally
                {
                    if (error == false)
                    {
                        DateTime end = DateTime.Now;

                        OnProgressUpdate(100);
                        OnLoadComplete(end - start, cancelled);
                    }

                    mutex.ReleaseMutex();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Searches = new Searches();

            this.FileName = String.Empty;
            this.FilePath = String.Empty;
            this.FileStream?.Close();
            this.FileStream = null;
            this.Url = null;
            this.FileStream = null;
            this.FilterIds = new List<ushort>();
            this.List.ModelFilter = null;
            this.FilterIds.Clear();
            this.List.ClearObjects();
            this.Enc = null;
            this.position = 0;

            this.info?.Dispose();
            this.info = null;
        }

        public bool TryUpdate() => info.fs.NewData > 0;

        public void Update(CancellationToken ct)
        {
            Task.Run(() =>
            {
                mutex.WaitOne();
                DateTime start = DateTime.Now;
                bool updated = false;
                try
                {
                    var old = position;
                    updated = loadFile(start, ct, false) > 0;
                    Debug.Assert(old < position || !updated);
                }
                finally
                {
                    DateTime end = DateTime.Now;

                    if (updated)
                        OnProgressUpdate(100);

                    OnUpdateComplete(end - start, updated);
                    mutex.ReleaseMutex();
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="searchType"></param>
        public void SearchMulti(List<SearchCriteria> scs, CancellationToken ct, int numContextLines)
        {
            Task.Run(() =>
            {

                DateTime start = DateTime.Now;
                bool cancelled = false;
                long matches = 0;
                try
                {
                    long counter = 0;
                    string line = string.Empty;
                    bool located = false;

                    foreach (LogLine ll in info.Lines)
                    {
                        // Reset the match flag
                        ll.SearchMatches.Clear();
                        ClearContextLine(ll.LineNumber, numContextLines);

                        foreach (SearchCriteria sc in scs)
                        {
                            line = info.GetLine(ll.LineNumber);

                            located = false;
                            switch (sc.Type)
                            {
                                case Global.SearchType.SubStringCaseInsensitive:
                                    if (line.IndexOf(sc.Pattern, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                    {
                                        located = true;
                                    }
                                    break;

                                case Global.SearchType.SubStringCaseSensitive:
                                    if (line.IndexOf(sc.Pattern, 0, StringComparison.Ordinal) > -1)
                                    {
                                        located = true;
                                    }
                                    break;

                                case Global.SearchType.RegexCaseInsensitive:
                                    if (Regex.Match(line, sc.Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled) != Match.Empty)
                                    {
                                        located = true;
                                    }
                                    break;

                                case Global.SearchType.RegexCaseSensitive:
                                    if (Regex.Match(line, sc.Pattern, RegexOptions.Compiled) != Match.Empty)
                                    {
                                        located = true;
                                    }
                                    break;

                                default:
                                    break;
                            }

                            if (located == true)
                            {
                                matches++;
                                ll.SearchMatches.Add(sc.Id);

                                if (numContextLines > 0)
                                {
                                    this.SetContextLines(ll.LineNumber, numContextLines);
                                }
                            }
                        }

                        if (counter++ % 50 == 0)
                        {
                            OnProgressUpdate((int)((double)counter / (double)this.Lines.Count * 100));

                            if (ct.IsCancellationRequested)
                            {
                                cancelled = true;
                                return;
                            }
                        }
                    }
                }
                finally
                {
                    DateTime end = DateTime.Now;

                    OnProgressUpdate(100);
                    OnSearchComplete(end - start, matches, scs.Count, cancelled);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="searchType"></param>
        public void Search(SearchCriteria sc, bool cumulative, CancellationToken ct, int numContextLines)
        {
            Task.Run(() =>
            {

                DateTime start = DateTime.Now;
                bool cancelled = false;
                long matches = 0;
                try
                {
                    long counter = 0;
                    string line = string.Empty;
                    bool located = false;

                    foreach (LogLine ll in info.Lines)
                    {
                        if (cumulative == false)
                        {
                            // Reset the match flag
                            ll.SearchMatches.Clear();
                            //ll.IsContextSearch = false;

                            ClearContextLine(ll.LineNumber, numContextLines);
                        }
                        else
                        {
                            if (ll.SearchMatches.Count > 0)
                            {
                                continue;
                            }
                        }

                        line = info.GetLine(ll.LineNumber);

                        located = false;
                        switch (sc.Type)
                        {
                            case Global.SearchType.SubStringCaseInsensitive:
                                if (line.IndexOf(sc.Pattern, 0, StringComparison.OrdinalIgnoreCase) > -1)
                                {
                                    located = true;
                                }
                                break;

                            case Global.SearchType.SubStringCaseSensitive:
                                if (line.IndexOf(sc.Pattern, 0, StringComparison.Ordinal) > -1)
                                {
                                    located = true;
                                }
                                break;

                            case Global.SearchType.RegexCaseInsensitive:
                                if (Regex.Match(line, sc.Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled) != Match.Empty)
                                {
                                    located = true;
                                }
                                break;

                            case Global.SearchType.RegexCaseSensitive:
                                if (Regex.Match(line, sc.Pattern, RegexOptions.Compiled) != Match.Empty)
                                {
                                    located = true;
                                }
                                break;

                            default:
                                break;
                        }

                        if (located == false)
                        {
                            ll.SearchMatches.Remove(sc.Id);
                        }
                        else
                        {
                            matches++;
                            ll.SearchMatches.Add(sc.Id);

                            if (numContextLines > 0)
                            {
                                this.SetContextLines(ll.LineNumber, numContextLines);
                            }
                        }

                        if (counter++ % 50 == 0)
                        {
                            OnProgressUpdate((int)((double)counter / (double)info.Lines.Count * 100));

                            if (ct.IsCancellationRequested)
                            {
                                cancelled = true;
                                return;
                            }
                        }
                    }
                }
                finally
                {
                    DateTime end = DateTime.Now;

                    OnProgressUpdate(100);
                    OnSearchComplete(end - start, matches, 1, cancelled);
                }
            });
        }

        public void FilterIt(FilterRule fr, CancellationToken ct, int numContextLines)
        {
            Task.Run(() =>
            {
                DateTime start = DateTime.Now;
                bool cancelled = false;
                int matches = 0;
                try
                {
                    long counter = 0;
                    string line = string.Empty;
                    for (int i = 0; i < Lines.Count && i < numContextLines; ++i)
                    {
                        Lines[i].IsContextFilter = false;
                    }

                    for (int i = 0; i < Lines.Count; ++i)
                    {
                        var ll = Lines[i];
                        var li = ll.Item;

                        if (fr == null)
                        {
                            ll.IsFilterLine = false;
                            ll.IsContextFilter = false;
                            continue;
                        }

                        if (li != null)
                        {
                            Debug.Assert(li.LineNumberStart == i);
                            Debug.Assert(li.LineNumberEnd != -1 || li == Items.Last());

                            ClearContextFilter(li, numContextLines);

                            int end = li.LineNumberEnd != -1 ? li.LineNumberEnd : Lines.Count - 1;
                            if (!fr.CheckFilter(li))
                            {
                                for (int j = li.LineNumberStart; j <= end; j++)
                                    Lines[j].IsFilterLine = false;
                            }
                            else
                            {
                                bool ck = false;
                                for (int j = li.LineNumberStart; j <= end; j++)
                                {
                                    if (fr.CheckFilter(GetLine(j)))
                                    {
                                        ck = true;
                                        SetContextFilter(li, numContextLines);
                                        matches++;
                                        break;
                                    }
                                }

                                for (int j = li.LineNumberStart; j <= end; j++)
                                    Lines[j].IsFilterLine = ck;
                            }

                            i = end;

                            if (counter++ % 50 == 0)
                            {
                                OnProgressUpdate((int)((double)counter / (double)info.Lines.Count * 100));

                                if (ct.IsCancellationRequested)
                                {
                                    cancelled = true;
                                    return;
                                }
                            }

                            continue;
                        }

                        Debug.Assert(Items.Count == 0 || Items[0].LineNumberStart > i);
                        ll.IsFilterLine = fr.CheckFilter(info.GetLine(i));
                    }
                }
                finally
                {
                    DateTime end = DateTime.Now;
                    OnProgressUpdate(100);
                    OnFilterConplete(end - start, matches, cancelled);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ct"></param>
        public void Export(string filePath, CancellationToken ct)
        {
            this.ExportToFile(info.Lines, filePath, ct);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="filePath"></param>
        /// <param name="ct"></param>
        public void Export(IEnumerable lines, string filePath, CancellationToken ct)
        {
            this.ExportToFile(lines, filePath, ct);
        }
        #endregion

        public TabPage Initialise(string filePath)
        {
            OLVColumn colLineNumber = ((OLVColumn)(new OLVColumn()));
            OLVColumn colText = ((OLVColumn)(new OLVColumn()));
            colLineNumber.Text = "Line No.";
            colLineNumber.Width = 95;
            colText.Text = "Data";

            colLineNumber.AspectGetter = delegate (object x)
            {
                if (((LogLine)x) == null)
                {
                    return "";
                }

                return (((LogLine)x).LineNumber + 1);
            };

            colText.AspectGetter = delegate (object x)
            {
                if (((LogLine)x) == null)
                {
                    return "";
                }

                return StringTab2Spaces.ToTabified(info.GetLine(((LogLine)x).LineNumber), NumTabSpaces);
            };

            FastObjectListView lv = new FastObjectListView();

            lv.AllColumns.Add(colLineNumber);
            lv.AllColumns.Add(colText);
            lv.AllowDrop = true;
            lv.AutoArrange = false;
            lv.CausesValidation = false;
            lv.CellEditUseWholeCell = false;
            lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colLineNumber,
            colText});
            //lv.ContextMenuStrip = this.contextMenu;
            lv.Dock = System.Windows.Forms.DockStyle.Fill;
            lv.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lv.FullRowSelect = true;
            lv.GridLines = true;
            lv.HasCollapsibleGroups = false;
            lv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            lv.HideSelection = false;
            lv.IsSearchOnSortColumn = false;
            lv.Location = new System.Drawing.Point(3, 3);
            lv.Margin = new System.Windows.Forms.Padding(4);
            lv.Name = "listLines0";
            lv.SelectColumnsMenuStaysOpen = false;
            lv.SelectColumnsOnRightClick = false;
            lv.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.None;
            lv.ShowFilterMenuOnRightClick = false;
            lv.ShowGroups = false;
            lv.ShowSortIndicators = false;
            lv.Size = new System.Drawing.Size(1679, 940);
            lv.TabIndex = 1;
            lv.TriggerCellOverEventsWhenOverHeader = false;
            lv.UseCompatibleStateImageBehavior = false;
            lv.UseFiltering = true;
            lv.View = System.Windows.Forms.View.Details;
            lv.VirtualMode = true;
            lv.Tag = this.Guid;
            lv.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.FormatRow);

            this.List = lv;

            TabPage tp = new TabPage();
            tp.Controls.Add(lv);
            tp.Location = new System.Drawing.Point(4, 33);
            tp.Name = "tabPage" + this.Guid;
            tp.Padding = new System.Windows.Forms.Padding(3);
            tp.Size = new System.Drawing.Size(1685, 946);
            tp.TabIndex = 0;
            tp.Text = "Loading...";
            tp.UseVisualStyleBackColor = true;
            tp.Tag = this.Guid;
            tp.ToolTipText = filePath;

            return tp;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetContextMenu(ContextMenuStrip ctx)
        {
            this.List.ContextMenuStrip = ctx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FormatRow(object sender, FormatRowEventArgs e)
        {
            if ((LogLine)e.Model == null)
            {
                return;
            }

            if (((LogLine)e.Model).SearchMatches.Intersect(this.FilterIds).Any() == true)
            {
                e.Item.BackColor = highlightColour;
            }
            else if (((LogLine)e.Model).IsFilterLine)
            {
                e.Item.BackColor = filterColour;
            }
            else if (((LogLine)e.Model).IsContextSearch || ((LogLine)e.Model).IsContextFilter)
            {
                e.Item.BackColor = contextColour;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="numLines"></param>
        private void SetContextLines(long lineNumber, int numLines)
        {
            long temp = numLines;
            if (lineNumber < info.Lines.Count)
            {
                if (numLines + lineNumber > info.Lines.Count - 1)
                {
                    temp = info.Lines.Count - lineNumber - 1;
                }
                for (int index = 1; index <= temp; index++)
                {
                    info.Lines[(int)lineNumber + index].IsContextSearch = true;
                }
            }

            temp = numLines;
            if (lineNumber > 0)
            {
                if (lineNumber - numLines < 0)
                {
                    temp = lineNumber;
                }
                for (int index = 1; index <= temp; index++)
                {
                    info.Lines[(int)lineNumber - index].IsContextSearch = true;
                }
            }
        }

        private void SetContextFilter(LogItem li, int numLines)
        {
            long temp = numLines;
            if (li.LineNumberEnd < info.Lines.Count)
            {
                if (numLines + li.LineNumberEnd > info.Lines.Count - 1)
                {
                    temp = info.Lines.Count - li.LineNumberEnd - 1;
                }

                for (int index = 1; index <= temp; index++)
                {
                    info.Lines[(int)li.LineNumberEnd + index].IsContextFilter = true;
                }
            }

            if (li.LineNumberStart > 0)
            {
                temp = numLines;
                if (li.LineNumberStart - numLines < 0)
                {
                    temp = li.LineNumberStart;
                }
                for (int index = 1; index <= temp; index++)
                {
                    info.Lines[(int)li.LineNumberStart - index].IsContextFilter = true;
                }
            }
        }

        /// <summary>
        /// Clear the line that is the next after the farthest context
        /// line, so the flag is reset and we won't overwrite
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="numLines"></param>
        private void ClearContextLine(long lineNumber, int numLines)
        {
            if ((int)lineNumber + numLines + 1 < Lines.Count - 1)
            {
                Lines[(int)lineNumber + numLines + 1].IsContextSearch = false;
            }
        }
        private void ClearContextFilter(LogItem li, int numLines)
        {
            for (int i = li.LineNumberStart + numLines + 1; i < Lines.Count; ++i)
            {
                Lines[i].IsContextFilter = false;
                if (i > li.LineNumberEnd + numLines)
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="ct"></param>
        private void ExportToFile(IEnumerable lines, string filePath, CancellationToken ct)
        {
            Task.Run(() =>
            {
                DateTime start = DateTime.Now;
                bool cancelled = false;
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        string lineStr = string.Empty;
                        byte[] lineBytes;
                        byte[] endLine = new byte[2] { 13, 10 };

                        long counter = 0;
                        foreach (LogLine ll in lines)
                        {
                            lineStr = info.GetLine(ll.LineNumber);
                            lineBytes = Encoding.UTF8.GetBytes(lineStr);
                            fs.Write(lineBytes, 0, lineBytes.Length);
                            // Add \r\n
                            fs.Write(endLine, 0, 2);

                            if (counter++ % 50 == 0)
                            {
                                OnProgressUpdate((int)((double)counter / (double)info.Lines.Count * 100));

                                if (ct.IsCancellationRequested)
                                {
                                    cancelled = true;
                                    return;
                                }
                            }
                        }

                    }
                }
                finally
                {
                    DateTime end = DateTime.Now;

                    OnProgressUpdate(100);
                    OnExportComplete(end - start, cancelled);
                }
            });
        }

        #region Event Methods
        /// <summary>
        /// 
        /// </summary>
        private void OnLoadError(string message)
        {
            LoadError?.Invoke(this.FileName, message);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnProgressUpdate(int progress)
        {
            ProgressUpdate?.Invoke(progress);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnLoadComplete(TimeSpan duration, bool cancelled)
        {
            LoadComplete?.Invoke(this, this.FileName, duration, cancelled);
        }

        private void OnUpdateComplete(TimeSpan duration, bool updated)
        {
            UpdateComplete?.Invoke(this, this.FileName, duration, updated);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnExportComplete(TimeSpan duration, bool cancelled)
        {
            ExportComplete?.Invoke(this, this.FileName, duration, cancelled);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnSearchComplete(TimeSpan duration, long matches, int numTerms, bool cancelled)
        {
            SearchComplete?.Invoke(this, this.FileName, duration, matches, numTerms, cancelled);
        }
        private void OnFilterConplete(TimeSpan duration, long matches, bool cancelled)
        {
            FilterComplete?.Invoke(this, this.FileName, duration, matches, cancelled);
        }
        #endregion

        internal bool LambdaIfLineShow(LogLine ll, int oldIndex, ref int newIndex)
        {
            if (Filter?.Valid ?? false)
            {
                if (!ll.IsFilterLine && !ll.IsContextFilter)
                    return false;
            }

            bool result = true;
            if ((ViewMode & Global.ViewMode.FilterShow) != 0)
            {
                result = ll.SearchMatches.Intersect(FilterIds).Any() ||
                         ll.IsContextSearch;
            }
            else if ((ViewMode & Global.ViewMode.FilterHide) != 0)
            {
                result = !ll.SearchMatches.Intersect(FilterIds).Any();
            }

            if (result && oldIndex >= 0 && ll.LineNumber <= oldIndex)
            {
                newIndex = ll.LineNumber;
            }

            return result;
        }
    }
}
