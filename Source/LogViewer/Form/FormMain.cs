using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using woanware;
using System.Threading;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BrightIdeasSoftware;
using System.Reflection;
using System.IO;

namespace LogViewer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormMain : Form
    {
        #region Member Variables
        private readonly SynchronizationContext synchronizationContext;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationTokenSource cancelUpdate;
        private HourGlass hourGlass;
        private bool processing;
        private Color highlightColour = Color.Lime;
        private Color contextColour = Color.LightGray;
        private Configuration config;
        private Dictionary<string, LogFile> logs;

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public FormMain()
        {
            InitializeComponent();

            synchronizationContext = SynchronizationContext.Current;
            dropdownSearchType.SelectedIndex = 0;
            dropdownSeq.SelectedIndex = 0;
            dropdownPid.SelectedIndex = 0;
            dropdownTid.SelectedIndex = 0;
            dropdownLevel.SelectedIndex = 0;
            dropdownTime.SelectedIndex = 0;
            dropdownCodePage.SelectedIndex = 0;

            logs = new Dictionary<string, LogFile>();
        }
        #endregion

        #region Form Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.config = new Configuration();
            string ret = this.config.Load();
            if (ret.Length > 0)
            {
                UserInterface.DisplayErrorMessageBox(this, ret);
            }

            foreach (var item in config.Modules) dropdownModule.Items.Add(item);
            foreach (var item in config.Filter1) dropdownFilter1.Items.Add(item);
            foreach (var item in config.Filter2) dropdownFilter2.Items.Add(item);
            foreach (var item in config.Filter3) dropdownFilter3.Items.Add(item);

            this.highlightColour = config.GetHighlightColour();
            this.contextColour = config.GetContextColour();

            menuFileOpen.Enabled = false;
            menuFileClose.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            config.HighlightColour = this.highlightColour.ToKnownColor().ToString();
            string ret = config.Save();
            if (ret.Length > 0)
            {
                UserInterface.DisplayErrorMessageBox(this, ret);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            if (processing == true)
            {
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 0)
            {
                return;
            }

            if (files.Length > 1)
            {
                UserInterface.DisplayMessageBox(this, "Only one file can be processed at one time", MessageBoxIcon.Exclamation);
                return;
            }

            if (logs.Count == 0)
            {
                LoadFile(files[0], true);
            }
            else
            {
                LoadFile(files[0], false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (processing == true)
            {
                return;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        #region Log File Methods
        private void LoadSFTP(Uri uri, Stream sf, bool newTab)
        {
            this.processing = true;
            this.hourGlass = new HourGlass(this);
            SetProcessingState(false);
            statusProgress.Visible = true;
            this.cancellationTokenSource = new CancellationTokenSource();
            menuToolsMultiStringSearch.Enabled = true;

            if (newTab)
            {
                Encoding enc = null;
                if (string.Equals(dropdownCodePage.Text, "UTF8", StringComparison.OrdinalIgnoreCase))
                {
                    enc = Encoding.UTF8;
                }
                else if (string.Equals(dropdownCodePage.Text, "GBK", StringComparison.OrdinalIgnoreCase))
                {
                    enc = Encoding.GetEncoding(54936); // GB18030
                }
                else if (string.Equals(dropdownCodePage.Text, "Auto", StringComparison.OrdinalIgnoreCase))
                {
                    ;
                }
                else if (!string.IsNullOrWhiteSpace(dropdownCodePage.Text))
                {
                    enc = Encoding.GetEncoding(dropdownCodePage.Text);
                }
                LogFile lf = new LogFile(config.HeaderFormat, config.HeaderFormatKeys, enc);
                logs.Add(lf.Guid, lf);

                tabControl.TabPages.Add(lf.Initialise(uri.OriginalString));
                lf.SetContextMenu(contextMenu);
                lf.ViewMode = Global.ViewMode.Standard;
                lf.ProgressUpdate += LogFile_LoadProgress;
                lf.LoadComplete += LogFile_LoadComplete;
                lf.UpdateComplete += LogFile_UpdateComplete;
                lf.FilterComplete += LogFile_FilterComplete;
                lf.SearchComplete += LogFile_SearchComplete;
                lf.ExportComplete += LogFile_ExportComplete;
                lf.LoadError += LogFile_LoadError;
                lf.List.ItemActivate += new EventHandler(this.listLines_ItemActivate);
                lf.List.DragDrop += new DragEventHandler(this.listLines_DragDrop);
                lf.List.DragEnter += new DragEventHandler(this.listLines_DragEnter);
                lf.Load(uri, sf, synchronizationContext, cancellationTokenSource.Token);
            }
            else
            {
                if (tabControl.SelectedTab == null)
                {
                    UserInterface.DisplayMessageBox(this, "Cannot identify current tab", MessageBoxIcon.Exclamation);
                    return;
                }

                if (!logs.ContainsKey(tabControl.SelectedTab.Tag.ToString()))
                {
                    UserInterface.DisplayMessageBox(this, "Cannot identify current tab", MessageBoxIcon.Exclamation);
                    return;
                }

                // Get the current selected log file and open the file using that object
                LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
                tabControl.SelectedTab.ToolTipText = uri.OriginalString;
                lf.Dispose();
                lf.Load(uri, sf, synchronizationContext, cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private void LoadFile(string filePath, bool newTab)
        {
            this.processing = true;
            this.hourGlass = new HourGlass(this);
            SetProcessingState(false);
            statusProgress.Visible = true;
            this.cancellationTokenSource = new CancellationTokenSource();
            menuToolsMultiStringSearch.Enabled = true;

            if (newTab == true)
            {
                Encoding enc = null;
                if (string.Equals(dropdownCodePage.Text, "UTF8", StringComparison.OrdinalIgnoreCase))
                {
                    enc = Encoding.UTF8;
                }
                else if (string.Equals(dropdownCodePage.Text, "GBK", StringComparison.OrdinalIgnoreCase))
                {
                    enc = Encoding.GetEncoding(54936); // GB18030
                }
                else if (string.Equals(dropdownCodePage.Text, "Auto", StringComparison.OrdinalIgnoreCase))
                {
                    ;
                }
                else if (!string.IsNullOrWhiteSpace(dropdownCodePage.Text))
                {
                    enc = Encoding.GetEncoding(dropdownCodePage.Text);
                }
                LogFile lf = new LogFile(config.HeaderFormat, config.HeaderFormatKeys, enc);
                logs.Add(lf.Guid, lf);

                tabControl.TabPages.Add(lf.Initialise(filePath));
                lf.SetContextMenu(contextMenu);
                lf.ViewMode = Global.ViewMode.Standard;
                lf.ProgressUpdate += LogFile_LoadProgress;
                lf.LoadComplete += LogFile_LoadComplete;
                lf.UpdateComplete += LogFile_UpdateComplete;
                lf.FilterComplete += LogFile_FilterComplete;
                lf.SearchComplete += LogFile_SearchComplete;
                lf.ExportComplete += LogFile_ExportComplete;
                lf.LoadError += LogFile_LoadError;
                lf.List.ItemActivate += new EventHandler(this.listLines_ItemActivate);
                lf.List.DragDrop += new DragEventHandler(this.listLines_DragDrop);
                lf.List.DragEnter += new DragEventHandler(this.listLines_DragEnter);
                lf.Load(filePath, synchronizationContext, cancellationTokenSource.Token);
            }
            else
            {
                if (tabControl.SelectedTab == null)
                {
                    UserInterface.DisplayMessageBox(this, "Cannot identify current tab", MessageBoxIcon.Exclamation);
                    return;
                }

                if (!logs.ContainsKey(tabControl.SelectedTab.Tag.ToString()))
                {
                    UserInterface.DisplayMessageBox(this, "Cannot identify current tab", MessageBoxIcon.Exclamation);
                    return;
                }

                // Get the current selected log file and open the file using that object
                LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
                tabControl.SelectedTab.ToolTipText = filePath;
                lf.Dispose();
                lf.Load(filePath, synchronizationContext, cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SearchFile()
        {
            if (tabControl.SelectedTab == null) return;
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

            SearchCriteria sc = new SearchCriteria();
            sc.Type = (Global.SearchType)dropdownSearchType.SelectedIndex;
            sc.Pattern = textSearch.Text;
            sc.Id = lf.Searches.Add(sc, toolButtonCumulative.Checked);

            if (sc.Id == 0)
            {
                UserInterface.DisplayMessageBox(this, "The search pattern already exists", MessageBoxIcon.Exclamation);
                return;
            }

            // Add the ID so that any matches show up straight away
            lf.FilterIds.Add(sc.Id);

            this.processing = true;
            this.hourGlass = new HourGlass(this);
            SetProcessingState(false);
            statusProgress.Visible = true;
            this.cancellationTokenSource = new CancellationTokenSource();
            lf.Search(sc, toolButtonCumulative.Checked, cancellationTokenSource.Token, config.NumContextLines);
        }

        private void FilterFile(FilterRule fr)
        {
            if (tabControl.SelectedTab == null) return;

            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
            if (lf.Filter == fr) return;

            lf.Filter = fr;

            this.processing = true;
            this.hourGlass = new HourGlass(this);
            SetProcessingState(false);
            statusProgress.Visible = true;
            this.cancellationTokenSource = new CancellationTokenSource();
            lf.FilterIt(fr, cancellationTokenSource.Token, config.NumContextLines);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private void Export(string filePath)
        {
            this.processing = true;
            this.hourGlass = new HourGlass(this);
            SetProcessingState(false);
            statusProgress.Visible = true;
            this.cancellationTokenSource = new CancellationTokenSource();

            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

            if (lf.List.ModelFilter == null)
            {
                lf.Export(filePath, cancellationTokenSource.Token);
            }
            else
            {
                lf.Export(lf.List.FilteredObjects, filePath, cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        private void ExportSelected(string filePath)
        {
            this.processing = true;
            this.hourGlass = new HourGlass(this);
            SetProcessingState(false);
            statusProgress.Visible = true;
            this.cancellationTokenSource = new CancellationTokenSource();

            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
            lf.Export(lf.List.SelectedObjects, filePath, cancellationTokenSource.Token);
        }
        #endregion

        #region Log File Object Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void LogFile_LoadError(string fileName, string message)
        {
            UserInterface.DisplayErrorMessageBox(this, message + " (" + fileName + ")");

            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                statusProgress.Visible = false;
                this.hourGlass.Dispose();
                SetProcessingState(true);
                this.cancellationTokenSource.Dispose();
                this.processing = false;

                // Lets clear the LogFile state and set the UI correctly
                menuFileClose_Click(this, null);

            }), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="percent"></param>
        private void LogFile_LoadProgress(int percent)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                statusProgress.Value = (int)o;
            }), percent);
        }

        /// <summary>
        /// 
        /// </summary>
        private void LogFile_SearchComplete(LogFile lf, string fileName, TimeSpan duration, long matches, int numTerms, bool cancelled)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                statusProgress.Visible = false;
                lf.List.Refresh();
                this.hourGlass.Dispose();
                SetProcessingState(true);
                this.cancellationTokenSource.Dispose();
                UpdateStatusLabel("Matched " + matches + " lines (Search Terms: " + numTerms + ") # Duration: " + duration + " (" + fileName + ")", statusLabelMain);

                this.processing = false;

            }), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val"></param>
        private void LogFile_ExportComplete(LogFile lf, string fileName, TimeSpan duration, bool val)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                statusProgress.Visible = false;
                this.hourGlass.Dispose();
                SetProcessingState(true);
                this.cancellationTokenSource.Dispose();
                UpdateStatusLabel("Export complete # Duration: " + duration + " (" + fileName + ")", statusLabelMain);

                this.processing = false;
            }), null);
        }

        private void LogFile_FilterComplete(LogFile lf, string fileName, TimeSpan duration, long matches, bool cancelled)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                lf.FilterItemCount = (int)matches;

                int selectedIndex = (lf.List.SelectedObject as LogLine)?.LineNumber ?? -1;
                int newSelectedIndex = -1;

                if (lf.Filter?.Valid ?? false)
                {
                    lf.ViewMode |= Global.ViewMode.FilterShow2;
                }
                else
                {
                    lf.ViewMode &= (~Global.ViewMode.FilterShow2);
                }
                lf.List.ModelFilter = new ModelFilter(delegate (object x)
                {
                    if (x == null) return false;
                    return lf.LambdaIfLineShow((LogLine)x, selectedIndex, ref newSelectedIndex);
                });

                statusProgress.Visible = false;
                lf.List.Refresh();
                this.hourGlass.Dispose();
                SetProcessingState(true);
                this.cancellationTokenSource.Dispose();
                UpdateStatusLabel("Matched " + matches + " lines # Duration: " + duration + " (" + fileName + ")", statusLabelMain);
                RefreshView(lf, false);

                if (newSelectedIndex >= 0 && lf.List.Items.Count > newSelectedIndex)
                {
                    lf.List.EnsureVisible(newSelectedIndex);
                    lf.List.SelectedIndex = newSelectedIndex;
                    if (lf.List.SelectedItem != null)
                    {
                        lf.List.FocusedItem = lf.List.SelectedItem;
                    }
                }

                this.processing = false;
            }), null);
        }

        private void LogFile_UpdateComplete(LogFile lf, string fileName, TimeSpan duration, bool updated)
        {
            if (updated)
            {
                synchronizationContext.Post(new SendOrPostCallback(o =>
                {
                    lf.List.SetObjects(lf.Lines);
                    RefreshView(lf, true);
                    this.processing = false;
                }), null);
            }
        }

        private void SetFilterRule(FilterRule fr)
        {
            fr = fr ?? new FilterRule();
            dropdownFilter1.Text = fr.Filter1 ?? "";
            dropdownFilter2.Text = fr.Filter2 ?? "";
            dropdownFilter3.Text = fr.Filter3 ?? "";
            toolButtonFilterType.Checked = fr.FilterOrAnd;
            dropdownModule.Text = fr.Module ?? "";

            dropdownPid.Text = fr.Pid == -1 ? "" : fr.Pid.ToString();
            dropdownTid.Text = fr.Tid == -1 ? "" : fr.Tid.ToString();
            dropdownSeq.Text = fr.Seq == -1 ? "" : fr.Seq.ToString();
        }

        private FilterRule GetFilterRule()
        {
            FilterRule fr = new FilterRule();
            if (!string.IsNullOrWhiteSpace(dropdownFilter1.Text)) fr.Filter1 = dropdownFilter1.Text;
            if (!string.IsNullOrWhiteSpace(dropdownFilter2.Text)) fr.Filter2 = dropdownFilter2.Text;
            if (!string.IsNullOrWhiteSpace(dropdownFilter3.Text)) fr.Filter3 = dropdownFilter3.Text;
            fr.FilterOrAnd = toolButtonFilterType.Checked;

            if (!string.IsNullOrWhiteSpace(dropdownModule.Text)) fr.Module = dropdownModule.Text;
            // DateTime Dt = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(dropdownPid.Text) && long.TryParse(dropdownPid.Text, out long result))
                fr.Pid = (int)result;
            if (!string.IsNullOrWhiteSpace(dropdownTid.Text) && long.TryParse(dropdownTid.Text, out result))
                fr.Tid = (int)result;
            if (!string.IsNullOrWhiteSpace(dropdownSeq.Text) && long.TryParse(dropdownSeq.Text, out result))
                fr.Seq = (int)result;
            if (!string.IsNullOrWhiteSpace(dropdownLevel.Text))
                fr.Level = Tools.LevelToInt(dropdownLevel.Text);

            return fr.Valid ? fr : null;
        }

        private void RefreshView(LogFile lf, bool scroll)
        {
            if (lf == null) return;

            toolTxtAllCount.Text = lf.Items.Count.ToString();
            toolTxtFiltered.Text = lf.FilterItemCount.ToString();
            if (scroll && toolButtonAutoScroll.Checked && lf.List.Items.Count > 1)
            {
                lf.List.Items[lf.List.Items.Count - 1].EnsureVisible();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LogFile_LoadComplete(LogFile lf, string fileName, TimeSpan duration, bool cancelled)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                lf.List.SetObjects(lf.Lines);

                // Try and measure the length of the longest line in pixels
                // This is rough, and tends to be too short, but cannot find
                // another method to make column wide enough :-)
                using (var image = new Bitmap(1, 1))
                {
                    using (var g = Graphics.FromImage(image))
                    {
                        string temp = lf.GetLine(lf.LongestLine.LineNumber);
                        var result = g.MeasureString(temp, new Font("Consolas", 9, FontStyle.Regular, GraphicsUnit.Pixel));
                        lf.List.Columns[1].Width = Convert.ToInt32(result.Width + 200);
                    }
                }

                lf.List.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                statusProgress.Visible = false;

                SetProcessingState(true);
                this.cancellationTokenSource.Dispose();
                UpdateStatusLabel(lf.Lines.Count + " Lines # Duration: " + duration + " (" + fileName + ")", statusLabelMain);
                menuFileClose.Enabled = true;
                menuFileOpen.Enabled = true; // Enable the standard file open, since we can now open in an existing tab, since at least one tab exists
                int index = tabControl.TabPages.IndexOfKey("tabPage" + lf.Guid);
                tabControl.TabPages[index].Text = lf.FileName;
                RefreshView(lf, true);
                this.hourGlass.Dispose();
                this.processing = false;

                if (!cancelled && cancelUpdate == null)
                {
                    cancelUpdate = new CancellationTokenSource();
                    timerUpdate.Interval = Math.Max(config.UpdateIntervalLocalDrive, 500);
                    timerUpdate.Start();
                }
            }), null);
        }
        #endregion

        #region List Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listLines_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            if ((LogLine)e.Model == null)
            {
                return;
            }

            LogFile lf = logs[e.ListView.Tag.ToString()];

            if (((LogLine)e.Model).SearchMatches.Intersect(lf.FilterIds).Any() == true)
            {
                e.Item.BackColor = highlightColour;
            }
            else if (((LogLine)e.Model).IsContextLine == true)
            {
                e.Item.BackColor = contextColour;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listLines_DragEnter(object sender, DragEventArgs e)
        {
            if (processing == true)
            {
                return;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listLines_DragDrop(object sender, DragEventArgs e)
        {
            if (processing == true)
            {
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length == 0)
            {
                return;
            }

            if (files.Length > 1)
            {
                UserInterface.DisplayMessageBox(this, "Only one file can be processed at one time", MessageBoxIcon.Exclamation);
                return;
            }

            LoadFile(files[0], false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listLines_ItemActivate(object sender, EventArgs e)
        {
            var lv = (FastObjectListView)sender;
            if (lv.SelectedObjects.Count != 1)
            {
                return;
            }

            LogFile lf = logs[lv.Tag.ToString()];
            LogLine ll = (LogLine)lv.SelectedObjects[0];
            using (FormLine f = new FormLine(lf.GetLine(ll.LineNumber)))
            {
                f.ShowDialog(this);
            }
        }
        #endregion

        #region Context Menu Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuFilterClear_Click(object sender, EventArgs e)
        {
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

            // Get the currently selected row
            int selectedIndex = ((LogLine)lf.List.SelectedObject).LineNumber;
            int newSelectedIndex = selectedIndex;

            if (lf.Filter?.Valid ?? false)
            {
                lf.ViewMode = Global.ViewMode.FilterShow2;
                lf.List.ModelFilter = new ModelFilter(delegate (object x)
                {
                    if (x == null) return false;
                    LogLine llx = ((LogLine)x);
                    if (llx.IsFilterLine || llx.IsContextFilter)
                    {
                        if (selectedIndex > llx.LineNumber)
                            newSelectedIndex = llx.LineNumber;
                        return true;
                    }
                    return false;
                });
            }
            else
            {
                lf.ViewMode = Global.ViewMode.Standard;
                lf.List.ModelFilter = null;
            }

            if (lf.List.Items.Count > newSelectedIndex)
            {
                lf.List.EnsureVisible(newSelectedIndex);
                lf.List.SelectedIndex = newSelectedIndex;
                if (lf.List.SelectedItem != null)
                {
                    lf.List.FocusedItem = lf.List.SelectedItem;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuFilterShowMatched_Click(object sender, EventArgs e)
        {
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
            lf.ViewMode |= Global.ViewMode.FilterShow;
            lf.ViewMode &= (~Global.ViewMode.FilterHide);

            int selectedIndex = ((LogLine)lf.List.SelectedObject).LineNumber;
            int newSelectedIndex = -1;

            lf.List.ModelFilter = new ModelFilter(delegate (object x)
            {
                if (x == null) return false;
                return lf.LambdaIfLineShow((LogLine)x, selectedIndex, ref newSelectedIndex);
            });

            if (newSelectedIndex >= 0 && lf.List.Items.Count > newSelectedIndex)
            {
                lf.List.Items[newSelectedIndex].EnsureVisible();
                lf.List.SelectedIndex = newSelectedIndex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuFilterHideMatched_Click(object sender, EventArgs e)
        {
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
            lf.ViewMode |= Global.ViewMode.FilterHide;
            lf.ViewMode &= (~Global.ViewMode.FilterShow);

            int selectedIndex = (lf.List.SelectedObject as LogLine)?.LineNumber ?? -1;
            int newSelectedIndex = -1;

            lf.List.ModelFilter = new ModelFilter(delegate (object x)
            {
                if (x == null) return false;
                return lf.LambdaIfLineShow((LogLine)x, selectedIndex, ref newSelectedIndex);
            });

            if (newSelectedIndex >= 0)
            {
                lf.List.EnsureVisible(newSelectedIndex);
                lf.List.SelectedIndex = newSelectedIndex;
                if (lf.List.SelectedItem != null)
                {
                    lf.List.FocusedItem = lf.List.SelectedItem;
                }
            }
        }

        /// <summary>
        /// Show the Searches window to allow the user to enable/disable search terms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuSearchViewTerms_Click(object sender, EventArgs e)
        {
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

            using (FormSearchTerms f = new FormSearchTerms(lf.Searches))
            {
                DialogResult dr = f.ShowDialog(this);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                lf.Searches = f.Searches;
                lf.FilterIds.Clear();
                foreach (SearchCriteria sc in lf.Searches.Items)
                {
                    if (sc.Enabled == false)
                    {
                        continue;
                    }

                    lf.FilterIds.Add(sc.Id);
                }

                lf.List.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuSearchColourMatch_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            DialogResult dr = cd.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }

            this.highlightColour = cd.Color;

            logs[tabControl.SelectedTab.Tag.ToString()].List.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuSearchColourContext_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            DialogResult dr = cd.ShowDialog(this);
            if (dr == DialogResult.Cancel)
            {
                return;
            }

            this.contextColour = cd.Color;

            logs[tabControl.SelectedTab.Tag.ToString()].List.Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuExportAll_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "All Files|*.*";
            sfd.FileName = "*.*";
            sfd.Title = "Select export file";

            if (sfd.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            Export(sfd.FileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuExportSelected_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "All Files|*.*";
            sfd.FileName = "*.*";
            sfd.Title = "Select export file";

            if (sfd.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            ExportSelected(sfd.FileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuCopy_Click(object sender, EventArgs e)
        {
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

            StringBuilder sb = new StringBuilder();
            foreach (LogLine ll in lf.List.SelectedObjects)
            {
                sb.AppendLine(lf.GetLine(ll.LineNumber));
            }

            Clipboard.SetText(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextLinesGoToLine_Click(object sender, EventArgs e)
        {
            using (FormGoToLine f = new FormGoToLine())
            {
                DialogResult dr = f.ShowDialog(this);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

                lf.List.EnsureVisible(f.LineNumber - 1);
                var ll = lf.Lines.SingleOrDefault(x => x.LineNumber == f.LineNumber);
                if (ll != null)
                {
                    lf.List.SelectedIndex = ll.LineNumber - 1;
                    if (lf.List.SelectedItem != null)
                    {
                        lf.List.FocusedItem = lf.List.SelectedItem;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextLinesGoToFirstLine_Click(object sender, EventArgs e)
        {
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

            lf.List.EnsureVisible(0);
            lf.List.SelectedIndex = 0;
            if (lf.List.SelectedItem != null)
            {
                lf.List.FocusedItem = lf.List.SelectedItem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextLinesGoToLastLine_Click(object sender, EventArgs e)
        {
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

            lf.List.EnsureVisible(lf.LineCount - 1);
            lf.List.SelectedIndex = lf.LineCount - 1;
            if (lf.List.SelectedItem != null)
            {
                lf.List.FocusedItem = lf.List.SelectedItem;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool enableLineOps = true;

            LogFile lf = null;
            if (tabControl.SelectedTab != null)
            {
                lf = logs[tabControl.SelectedTab.Tag.ToString()];
            }

            if (lf == null)
            {
                enableLineOps = false;
            }
            else
            {
                if (lf.LineCount == 0)
                {
                    enableLineOps = false;
                }
            }

            contextLinesGoToFirstLine.Enabled = enableLineOps;
            contextLinesGoToLastLine.Enabled = enableLineOps;
            contextLinesGoToLine.Enabled = enableLineOps;

            if (lf != null)
            {
                if (lf.List.SelectedObjects.Count > this.config.MultiSelectLimit)
                {
                    contextMenuCopy.Enabled = false;
                    contextMenuExportSelected.Enabled = false;
                    return;
                }
            }

            contextMenuCopy.Enabled = true;
            contextMenuExportSelected.Enabled = true;
        }
        #endregion

        #region Toolbar Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolButtonSearch_Click(object sender, EventArgs e)
        {
            if (dropdownSearchType.SelectedIndex == -1)
            {
                UserInterface.DisplayMessageBox(this, "The search type is not selected", MessageBoxIcon.Exclamation);
                dropdownSearchType.Select();
                return;
            }

            SearchFile();
        }
        #endregion

        #region Menu Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";
            openFileDialog.FileName = "*.*";
            openFileDialog.Title = "Select log file";

            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            LoadFile(openFileDialog.FileName, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileOpenNewTab_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";
            openFileDialog.FileName = "*.*";
            openFileDialog.Title = "Select log file";

            if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }

            LoadFile(openFileDialog.FileName, true);
        }

        /// <summary>
        /// Close the resources used for opening and processing the log file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileClose_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == null || tabControl.SelectedIndex == -1)
            {
                return;
            }

            var tag = tabControl.SelectedTab.Tag.ToString();

            // Get rid of the event handlers to prevent a memory leak
            logs[tag].ProgressUpdate -= LogFile_LoadProgress;
            logs[tag].LoadComplete -= LogFile_LoadComplete;
            logs[tag].UpdateComplete -= LogFile_UpdateComplete;
            logs[tag].FilterComplete -= LogFile_FilterComplete;
            logs[tag].SearchComplete -= LogFile_SearchComplete;
            logs[tag].ExportComplete -= LogFile_ExportComplete;
            logs[tag].LoadError -= LogFile_LoadError;
            // Clear the rest
            logs[tag].List.ClearObjects();
            logs[tag].Dispose();
            logs.Remove(tag);

            tabControl.TabPages.Remove(tabControl.SelectedTab);

            if (logs.Count == 0)
            {
                menuFileOpen.Enabled = false;
                menuFileClose.Enabled = false;
            }

            UpdateStatusLabel("", statusLabelMain);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelpHelp_Click(object sender, EventArgs e)
        {
            Misc.ShellExecuteFile(System.IO.Path.Combine(Misc.GetApplicationDirectory(), "help.pdf"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            using (FormAbout f = new FormAbout())
            {
                f.ShowDialog(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuToolsMultiStringSearch_Click(object sender, EventArgs e)
        {
            LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];

            using (FormSearch f = new FormSearch(lf.Searches))
            {
                DialogResult dr = f.ShowDialog(this);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }

                // Clear any existing filter ID's as we will only show the multi-string search
                lf.FilterIds.Clear();
                lf.Searches.Reset();
                foreach (SearchCriteria sc in f.NewSearches)
                {
                    // Add the ID so that any matches show up straight away
                    lf.FilterIds.Add(sc.Id);
                    lf.Searches.Add(sc);
                }

                this.processing = true;
                this.hourGlass = new HourGlass(this);
                SetProcessingState(false);
                statusProgress.Visible = true;
                this.cancellationTokenSource = new CancellationTokenSource();
                lf.SearchMulti(f.NewSearches, cancellationTokenSource.Token, config.NumContextLines);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuToolsConfiguration_Click(object sender, EventArgs e)
        {
            using (FormConfiguration f = new FormConfiguration(this.config))
            {
                f.ShowDialog(this);
            }
        }
        #endregion

        #region UI Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        private void SetProcessingState(bool enabled)
        {
            MethodInvoker methodInvoker = delegate
            {
                menuFileOpen.Enabled = enabled;
                menuFileOpenNewTab.Enabled = enabled;
                menuFileClose.Enabled = enabled;
                menuFileExit.Enabled = enabled;
                toolButtonCumulative.Enabled = enabled;
                toolButtonFilterType.Enabled = enabled;
                toolButtonSearch.Enabled = enabled;
            };

            if (this.InvokeRequired == true)
            {
                this.BeginInvoke(methodInvoker);
            }
            else
            {
                methodInvoker.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        private void UpdateStatusLabel(string text, ToolStripStatusLabel control)
        {
            synchronizationContext.Post(new SendOrPostCallback(o =>
            {
                control.Text = (string)o;
            }), text);
        }
        #endregion

        #region private functions
        private void saveWidgetTxtValue(ToolStripComboBox widget, Collection<string> txts)
        {
            string txt = widget.Text;
            if (!string.IsNullOrWhiteSpace(txt) && !txts.Contains(txt))
            {
                txts.Add(txt);
                widget.SelectedIndex = widget.Items.Add(txt);
            }
        }

        private void saveWidgetTxtValue()
        {
            saveWidgetTxtValue(dropdownModule, config.Modules);
            saveWidgetTxtValue(dropdownFilter1, config.Filter1);
            saveWidgetTxtValue(dropdownFilter2, config.Filter2);
            saveWidgetTxtValue(dropdownFilter3, config.Filter3);
        }
        #endregion private functions

        #region Other Control Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statusProgress_Click(object sender, EventArgs e)
        {
            this.cancellationTokenSource.Cancel();
            this.cancelUpdate?.Cancel();
        }

        private void DebugShowInfo(object sender, EventArgs e)
        {
#if DEBUG && true
            string msg = "[控件：" + sender.ToString() + "] 事件：" + e.ToString();
            var propName = sender.GetType().GetProperty("Name");
            if (propName != null)
            {
                string name = propName.GetValue(sender) as string;
                if (!string.IsNullOrEmpty(name))
                {
                    msg = "控件：[" + sender.ToString() + "]:" + name + " 事件：" + e.ToString();
                }
            }
            MessageBox.Show(msg);
#endif
        }

        private void toolButtonFilterType_Click(object sender, EventArgs e)
        {
            if (toolButtonFilterType.Checked)
            {
                toolButtonFilterType.Text = "OR";
            }
            else
            {
                toolButtonFilterType.Text = "AND";
            }
        }

        private void dropdownFilter3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DebugShowInfo(sender, e);
        }

        private void dropdownFilter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DebugShowInfo(sender, e);
        }

        private void dropdownFilter1_Click(object sender, EventArgs e) { }

        private void toolButtonFilterApply_Click(object sender, EventArgs e)
        {
            saveWidgetTxtValue();

            if (tabControl.SelectedTab != null)
            {
                LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
                var newF = GetFilterRule();
                if (lf.Filter != newF)
                {
                    FilterFile(newF);
                }
            }
        }

        private void dropdownFilter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DebugShowInfo(sender, e);
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            dropdownFilter1.Text = "";
            dropdownFilter2.Text = "";
            dropdownFilter3.Text = "";
            if (tabControl.SelectedTab != null)
            {
                LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
                if (lf.Filter != null)
                {
                    FilterFile(null);
                }
            }
        }

        private void toolStripButtonTruncation_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab != null)
            {
                LogFile lf = logs[tabControl.SelectedTab.Tag.ToString()];
                try
                {
                    Stream fs = lf.Trunc();
                    if (fs != null)
                        LoadSFTP(lf.Url, fs, false);
                    else
                        LoadFile(lf.FilePath, false);
                }
                finally
                {
                }

            }
        }

        #endregion Other Control Event Handlers

        private void toolStripButtonAutoScroll_Click(object sender, EventArgs e)
        {
            if (toolButtonAutoScroll.Checked)
            {
                toolButtonAutoScroll.Text = "Scroll";
            }
            else
            {
                toolButtonAutoScroll.Text = "NoScroll";
            }
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (tabControl.TabCount == 0)
            {
                timerUpdate.Stop();
                cancelUpdate?.Dispose();
                cancelUpdate = null;
                return;
            }

            LogFile lf = logs[tabControl.SelectedTab?.Tag.ToString()];
            if (lf?.TryUpdate() ?? false && cancelUpdate != null)
            {
                this.processing = true;
                lf.Update(cancelUpdate.Token);
            }
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (Width > 500)
            {
                int filterWidth = (Width - 300) / 3;
                var size = new Size(filterWidth, dropdownFilter1.Size.Height);
                dropdownFilter1.Size = size;
                dropdownFilter2.Size = size;
                dropdownFilter3.Size = size;
            }
        }

        private void dropdownSeq_KeyPress(object sender, KeyPressEventArgs e)
        {
            int key = e.KeyChar;
            if ((key < '0' || key > '9' && key != 8 && key != 46))
                e.Handled = true;
        }

        private void FormMain_ResizeEnd(object sender, EventArgs e)
        {
            if (Width > 500)
            {
                int filterWidth = (Width - 300) / 3;
                var size = new Size(filterWidth, dropdownFilter1.Size.Height);
                dropdownFilter1.Size = size;
                dropdownFilter2.Size = size;
                dropdownFilter3.Size = size;
                toolStrip.Refresh();
                toolStrip2.Refresh();
                toolStrip3.Refresh();
                toolStrip4.Refresh();
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.timerUpdate.Stop();
            }
            if (this.WindowState == FormWindowState.Normal)
            {
                FormMain_ResizeEnd(sender, e);
                if (!timerUpdate.Enabled)
                    this.timerUpdate.Start();
            }
        }

        private Stream OpenSFtp(string host, int port, string fpath, string user, string pass)
        {

            Renci.SshNet.SftpClient sftp = null;
            if (port == -1)
            {
                sftp = new Renci.SshNet.SftpClient(host, user, pass);
                sftp.Connect();
            }
            else
            {
                sftp = new Renci.SshNet.SftpClient(host, port, user, pass);
                sftp.Connect();
            }

            return (Stream)sftp.Open(fpath, FileMode.Open, FileAccess.Read);
        }

        private void openSFTPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new FormSFTPLoad();
            dlg.SFTP_URLs = config.SFTP_URLs;
            DialogResult res = dlg.ShowDialog();
            if (res == DialogResult.OK)
            {
                try
                {
                    Stream fs = OpenSFtp(dlg.Uri.Host, dlg.Uri.Port, dlg.Uri.PathAndQuery, dlg.UserName, dlg.Password);
                    if (!config.SFTP_URLs.Contains(dlg.URL))
                    {
                        config.SFTP_URLs.Add(dlg.URL);
                    }
                    LoadSFTP(dlg.Uri, fs, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally { }
            }
        }
    }
}
