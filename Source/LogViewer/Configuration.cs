using System;
using System.IO;
using System.Drawing;
using Nett;
using woanware;
using System.Collections.ObjectModel;
using System.Reflection;

namespace LogViewer
{
    /// <summary>
    /// Allows us to save/load the configuration file to/from TOML
    /// </summary>
    public class Configuration
    {
        #region Member Variables
        public string HighlightColour { get; set; } = "Lime";
        public string ContextColour { get; set; } = "LightGray";
        public int MultiSelectLimit { get; set; } = 1000;
        public int NumContextLines { get; set; } = 0;
        public int UpdateIntervalLocalDrive { get; set; } = 500;

        public Collection<string> SFTP_URLs { get; set; } = new Collection<string>();

        /// <summary>
        /// aks. [8764-1236:9854] 2022-05-17 19:39:44.124 [WARN] - [Radar]xxxxxxxxx
        /// </summary>
        public string HeaderFormat { get; set; } = "[{0}-{1}:{2}] {3} {4} [{5}] - ";
        public Collection<string> HeaderFormatKeys { get; set; } = new Collection<string>{
            "pid", "tid", "seq", "time_format=yyyy-MM-dd", "time_formats=|HH:mm:ss.fff|HH:mm:ss", "level", "PARSER_END"};
        public Collection<string> Modules { get; set; } = new Collection<string> { "Radar" };
        public Collection<string> Filter1 { get; set; } = new Collection<string> { "\"ps_", "\"cmd_", "\"process_create\"" };
        public Collection<string> Filter2 { get; set; } = new Collection<string>() { "FILTER", "MERGE", "RULESCTL"};
        public Collection<string> Filter3 { get; set; } = new Collection<string>();

        private const string FILENAME = "SysLogViewer.toml";
        #endregion

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Load()
        {
            try
            {
                if (File.Exists(this.GetPath()) == false)
                {
                    return string.Empty;
                }

                Configuration c = Toml.ReadFile<Configuration>(this.GetPath());
                var t = c.GetType();
                PropertyInfo[] props = c.GetType().GetProperties(BindingFlags.Public|BindingFlags.DeclaredOnly|BindingFlags.Instance);
                foreach (var prop in props)
                {
                    if (prop.CanRead)
                        prop.SetValue(this, prop.GetValue(c));
                }
                //this.HighlightColour = c.HighlightColour;
                //this.ContextColour = c.ContextColour;
                //this.MultiSelectLimit = c.MultiSelectLimit;
                //this.NumContextLines = c.NumContextLines;
                //this.Modules = c.Modules;
                //this.Filter1 = c.Filter1;
                //this.Filter2 = c.Filter2;
                //this.Filter3 = c.Filter3;

                if (this.MultiSelectLimit > 10000)
                {
                    this.MultiSelectLimit = 10000;
                    return "The multiselect limit is 10000";
                }

                if (this.NumContextLines > 10)
                {
                    this.NumContextLines = 10;
                    return "The maximum number of context lines is 10";
                }
                return string.Empty;
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                return fileNotFoundEx.Message;
            }
            catch (UnauthorizedAccessException unauthAccessEx)
            {
                return unauthAccessEx.Message;
            }
            catch (IOException ioEx)
            {
                return ioEx.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            try
            {
                Toml.WriteFile(this, this.GetPath());
                return string.Empty;
            }
            catch (FileNotFoundException fileNotFoundEx)
            {
                return fileNotFoundEx.Message;
            }
            catch (UnauthorizedAccessException unauthAccessEx)
            {
                return unauthAccessEx.Message;
            }
            catch (IOException ioEx)
            {
                return ioEx.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color GetHighlightColour()
        {
            Color temp = Color.FromName(this.HighlightColour);
            if (temp.IsKnownColor == false)
            {
                return Color.Lime;
            }

            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color GetContextColour()
        {
            Color temp = Color.FromName(this.ContextColour);
            if (temp.IsKnownColor == false)
            {
                return Color.LightGray;
            }

            return temp;
        }
        #endregion

        #region Misc Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetPath()
        {
            //string path = Misc.GetApplicationDirectory();
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            path = System.IO.Path.Combine(path, FILENAME);
            return path;
        }
        #endregion
    }
}