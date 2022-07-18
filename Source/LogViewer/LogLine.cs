using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace LogViewer
{

    internal enum LogLineType
    {
        None = 0,
        /// <summary>
        /// Context with Search
        /// </summary>
        Context1 = 0x1,
        /// <summary>
        /// Context with Filter
        /// </summary>
        Context2 = 0x2,
        /// <summary>
        /// 此行是Filter过滤命中的行（命中的显示，未命中的显示）
        /// </summary>
        Filtered = 0x4,
        /// <summary>
        /// 最后一行没有回车，可能没有收全
        /// </summary>
        Incomplete = 0x8,
    }

    internal class LogLine
    {
        #region Member Variables/Properties
        public int LineNumber { get; set; } = 0;
        /// <summary>
        /// 此行的字符数，此值可以为0,表示是一行空行
        /// </summary>
        public int CharCount { get; set; } = 0;
        public long Offset { get; set; } = 0;
        public List<ushort> SearchMatches { get; set; } = new List<ushort>();
        public bool IsContextLine
        {
            get => 0 != (Type & LogLineType.Context1);
            set => Type = value ? (Type | LogLineType.Context1) : (Type & ~LogLineType.Context1);
        }
        public bool IsContextFilter
        {
            get => 0 != (Type & LogLineType.Context2);
            set => Type = value ? (Type | LogLineType.Context2) : (Type & ~LogLineType.Context2);
        }
        public bool IsFilterLine
        {
            get => 0 != (Type & LogLineType.Filtered);
            set => Type = value ? (Type | LogLineType.Filtered) : (Type & ~LogLineType.Filtered);
        }
        public bool IsIncomplete
        {
            get => 0 != (Type & LogLineType.Incomplete);
            set => Type = value ? (Type | LogLineType.Incomplete) : (Type & ~LogLineType.Incomplete);
        }


        public LogLineType Type { get; set; } = LogLineType.None;


        /// <summary>
        /// 此行所属的Item
        /// </summary>
        public LogItem Item { get; set; } = null;

        /// <summary>
        /// 缓存的消息内容
        /// </summary>
        internal string Msg { get; set; } = null;
        #endregion
    }

    public enum ItemLevel
    {
        /// <summary>
        /// None表示所有都显示
        /// </summary>
        None = 0,
        Verbose,
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }

    /// <summary>
    /// 一条日志记录的标准头，保存了记录时间、进程ID、线程ID、debug级别等
    /// </summary>
    public class ItemHeader
    {
        /// <summary>
        /// 一行log中的头部分
        /// </summary>
        public string Head;

        /// <summary>
        /// 一行log中去掉头后面的部分，贝爷说了去掉头都能吃！
        /// </summary>
        public string Msg;

        public DateTime Time = new DateTime(0);
        public ItemLevel Level = ItemLevel.None;
        /// <summary>
        /// 进程ID
        /// </summary>
        public int Pid = 0;
        /// <summary>
        /// 线程ID
        /// </summary>
        public int Tid = 0;
        /// <summary>
        /// 日志顺序
        /// </summary>
        public Int64 Seq = 0;
        /// <summary>
        /// 所属模块
        /// </summary>
        public string Module;
        /// <summary>
        /// 其他属性
        /// </summary>
        public Dictionary<string, string> Others;
    }

    internal class LogItem : ItemHeader
    {
        internal int LineNumberStart = -1;
        internal int LineNumberEnd = -1;
    }

    public class HeaderFormatSeed
    {
        public HeaderFormatSeed(string headerFormat, Collection<string> keys)
        {
            HeaderFormat = headerFormat;
            HeaderFormatKeys = keys;
            parse();
        }

        private void parse()
        {
            List<string> parseIndex = new List<string>();
            int a = 0, b = 0;
            for (int i = 0; i < HeaderFormatKeys.Count; i++)
            {
                string key = "{" + i.ToString() + "}";
                b = HeaderFormat.IndexOf(key);
                if (b > 0 && a >= b)
                {
                    throw new Exception("Log数据头格式定义错误：" + HeaderFormat);
                }

                if (b < 0 || HeaderFormatKeys.Count == i || HeaderFormatKeys[i] == "PARSER_END")
                {
                    parseIndex.Add(HeaderFormat.Substring(a));
                    break;
                }

                parseIndex.Add(HeaderFormat.Substring(a, b - a));

                a = b + key.Length;
            }
            if (parseIndex.Count < 2)
            {
                throw new Exception("Log数据头格式分析出错，数量太少：" + HeaderFormat);
            }

            ParseIndex = parseIndex.ToArray();
        }

        internal string HeaderFormat;
        internal Collection<string> HeaderFormatKeys;
        internal string[] ParseIndex;
    }

    public static class Tools
    {
        public static bool TryParseHeader(string line, ItemHeader item, HeaderFormatSeed seed)
        {
            if (string.IsNullOrEmpty(line)) return false;

            string[] parseIndex = seed.ParseIndex;
            Collection<string> keyValue = seed.HeaderFormatKeys;

            int a = 0, b = 0;
            DateTime dt = new DateTime(0, DateTimeKind.Local);
            for (int i = 0; i < parseIndex.Length; i++)
            {
                if (i == 0 && string.IsNullOrEmpty(parseIndex[0])) continue;

                string value;
                a = b;

                if (string.IsNullOrEmpty(parseIndex[i]))
                {
                    value = line.Substring(a);
                }
                else
                {
                    b = line.IndexOf(parseIndex[i], b);
                    if (b < 0)
                    {
                        return false;
                    }
                    if (i == 0 && b != 0)
                    {
                        return false;
                    }


                    if (b > a)
                        value = line.Substring(a, b - a);
                    else
                        value = "";

                    b += parseIndex[i].Length;
                }

                if (i == 0 || string.IsNullOrEmpty(value))
                {
                    continue;
                }

                int index = i - 1;

                try
                {
                    if (keyValue[index] == "pid")
                    {
                        item.Pid = Int32.Parse(value);
                    }
                    else if (keyValue[index] == "tid")
                    {
                        item.Tid = Int32.Parse(value);
                    }
                    else if (keyValue[index] == "seq")
                    {
                        item.Seq = Int64.Parse(value);
                    }
                    else if (keyValue[index] == "year")
                    {
                        dt = dt.AddYears(Int32.Parse(value) - 1);
                    }
                    else if (keyValue[index] == "month")
                    {
                        dt = dt.AddMonths(Int32.Parse(value) - 1);
                    }
                    else if (keyValue[index] == "day")
                    {
                        dt = dt.AddDays(Int32.Parse(value) - 1);
                    }
                    else if (keyValue[index] == "hour")
                    {
                        dt = dt.AddHours(Int32.Parse(value));
                    }
                    else if (keyValue[index] == "minute")
                    {
                        dt = dt.AddMinutes(Int32.Parse(value));
                    }
                    else if (keyValue[index] == "second")
                    {
                        try
                        {
                            if (value.Contains("."))
                            {
                                var f = double.Parse(value);
                                f *= 1000;
                                dt = dt.AddMilliseconds(f);
                            }
                            else
                            {
                                dt = dt.AddSeconds(Int32.Parse(value));
                            }
                        }
                        finally { }
                    }
                    else if (keyValue[index] == "millisecond")
                    {
                        dt = dt.AddMilliseconds(double.Parse(value));
                    }
                    else if (keyValue[index].StartsWith("time_format="))
                    {
                        string timeFormat = keyValue[index].Substring("time_format=".Length);
                        DateTime dt2;
                        if (!DateTime.TryParseExact(value, timeFormat, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt2))
                        {
                            continue;
                        }
                        dt += (dt2 - new DateTime(0));
                    }
                    else if (keyValue[index].StartsWith("time_formats="))
                    {
                        string timeFormats = keyValue[index].Substring("time_formats=".Length);
                        string[] fs = timeFormats.Split(new char[] { timeFormats[0] }, StringSplitOptions.RemoveEmptyEntries);
                        DateTime dt2;
                        if (!DateTime.TryParseExact(value, fs, CultureInfo.CurrentCulture, DateTimeStyles.None, out dt2))
                        {
                            continue;
                        }
                        if (dt == DateTime.MinValue)
                        {
                            dt = dt2;
                        }
                        else
                        {
                            if (dt.Millisecond == 0) dt = dt.AddMilliseconds(dt2.Millisecond);
                            if (dt.Second == 0) dt = dt.AddSeconds(dt2.Second);
                            if (dt.Minute == 0) dt = dt.AddMinutes(dt2.Minute);
                            if (dt.Hour == 0) dt = dt.AddHours(dt2.Hour);
                            if (dt.Date == DateTime.MinValue)
                            {
                                dt = dt.AddDays(dt2.Day);
                                dt = dt.AddMonths(dt2.Month);
                                dt = dt.AddYears(dt2.Year);
                            }

                        }
                    }
                    else if (keyValue[index] == "level")
                    {
                        item.Level = LevelToInt(value);
                    }
                    else if (keyValue[index] == "PARSER_END")
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message + ":" + line);
                    return false;
                }
                finally { }
            }


            item.Head = line.Substring(0, b);
            item.Msg = line.Substring(b);
            if (!string.IsNullOrEmpty(item.Msg) && item.Msg[0] == '[')
            {
                int end = item.Msg.IndexOf(']');
                if (end > 0)
                {
                    item.Module = item.Msg.Substring(1, end - 1);
                }
            }
            item.Time = dt;
            return true;
        }

        public static ItemLevel LevelToInt(string level)
        {
            if (string.IsNullOrEmpty(level)) return ItemLevel.None;

            if (level.ToLower().Contains("all")) return ItemLevel.None;
            if (level.ToLower().Contains("verb")) return ItemLevel.Verbose;
            if (level.ToLower().Contains("debug")) return ItemLevel.Debug;
            if (level.ToLower().Contains("info")) return ItemLevel.Information;
            if (level.ToLower().Contains("warn")) return ItemLevel.Warning;
            if (level.ToLower().Contains("error")) return ItemLevel.Error;
            if (level.ToLower().Contains("fatal")) return ItemLevel.Fatal;

            return ItemLevel.None;
        }


        public static Encoding GetEncoding(byte[] bs) => GetEncoding(bs, 0, bs.Length);

        public static Encoding GetEncoding(byte[] bs, int offset, int len)
        {
            Encoding usedCode = Encoding.UTF8;
            Ude.ICharsetDetector ude = new Ude.CharsetDetector();
            ude.Feed(bs, offset, len);
            ude.DataEnd();
            if (!string.IsNullOrEmpty(ude.Charset))
            {
                usedCode = Encoding.GetEncoding(ude.Charset);
                if (typeof(System.Text.ASCIIEncoding).Equals(usedCode))
                {
                    usedCode = Encoding.UTF8;
                }
            }
            return usedCode;
        }
        public static Encoding GetTxtCodePage(Stream filestream)
        {
            filestream.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(filestream);
            Byte[] buffer = new Byte[3];
            int count = br.Read(buffer, 0, 3);
            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                {
                    return Encoding.UTF8; // 65001
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    filestream.Seek(2, SeekOrigin.Begin);
                    return Encoding.BigEndianUnicode; // 1201
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    filestream.Seek(2, SeekOrigin.Begin);
                    return Encoding.Unicode;  // 1200 UTF-16 Little endian(unicode)
                }
            }

            filestream.Seek(0, SeekOrigin.Begin);
            return null;
        }
    }
}
