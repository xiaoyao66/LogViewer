namespace LogViewer
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpenNewTab = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsMultiStringSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuToolsConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabelMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolLabelSearch = new System.Windows.Forms.ToolStripLabel();
            this.textSearch = new System.Windows.Forms.ToolStripTextBox();
            this.toolLabelType = new System.Windows.Forms.ToolStripLabel();
            this.dropdownSearchType = new System.Windows.Forms.ToolStripComboBox();
            this.toolButtonCumulative = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.toolButtonFilterType = new System.Windows.Forms.ToolStripButton();
            this.toolButtonFilterApply = new System.Windows.Forms.ToolStripButton();
            this.toolLabelCodePage = new System.Windows.Forms.ToolStripLabel();
            this.dropdownSeq = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownPid = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownTid = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownLevel = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownTime = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownModule = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownCodePage = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownFilter1 = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownFilter2 = new System.Windows.Forms.ToolStripComboBox();
            this.dropdownFilter3 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolTxtAllCount = new System.Windows.Forms.ToolStripTextBox();
            this.toolLabelSeq = new System.Windows.Forms.ToolStripLabel();
            this.toolLabelPid = new System.Windows.Forms.ToolStripLabel();
            this.toolLabelTid = new System.Windows.Forms.ToolStripLabel();
            this.toolLabelLevel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonTruncation = new System.Windows.Forms.ToolStripButton();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolLabelTime = new System.Windows.Forms.ToolStripLabel();
            this.toolLabelModule = new System.Windows.Forms.ToolStripLabel();
            this.toolButtonAutoScroll = new System.Windows.Forms.ToolStripButton();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolTxtFiltered = new System.Windows.Forms.ToolStripTextBox();
            this.toolLabelFilter = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuFilterShowMatched = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuFilterHideMatched = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuFilterClear = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSearchViewTerms = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuSearchColour = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSearchMatch = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSearchColourContext = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuExport = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuExportAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuExportSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.contextLines = new System.Windows.Forms.ToolStripMenuItem();
            this.contextLinesGoToLine = new System.Windows.Forms.ToolStripMenuItem();
            this.contextLinesGoToFirstLine = new System.Windows.Forms.ToolStripMenuItem();
            this.contextLinesGoToLastLine = new System.Windows.Forms.ToolStripMenuItem();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.openSFTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuTools,
            this.menuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(1693, 37);
            this.menuStrip.TabIndex = 1;
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileOpen,
            this.menuFileOpenNewTab,
            this.openSFTPToolStripMenuItem,
            this.menuFileSep1,
            this.menuFileClose,
            this.toolStripMenuItem3,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(65, 33);
            this.menuFile.Text = "&File";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Enabled = false;
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size(315, 40);
            this.menuFileOpen.Text = "&Open";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuFileOpenNewTab
            // 
            this.menuFileOpenNewTab.Name = "menuFileOpenNewTab";
            this.menuFileOpenNewTab.Size = new System.Drawing.Size(315, 40);
            this.menuFileOpenNewTab.Text = "Open (New Tab)";
            this.menuFileOpenNewTab.Click += new System.EventHandler(this.menuFileOpenNewTab_Click);
            // 
            // menuFileSep1
            // 
            this.menuFileSep1.Name = "menuFileSep1";
            this.menuFileSep1.Size = new System.Drawing.Size(312, 6);
            // 
            // menuFileClose
            // 
            this.menuFileClose.Name = "menuFileClose";
            this.menuFileClose.Size = new System.Drawing.Size(315, 40);
            this.menuFileClose.Text = "Close";
            this.menuFileClose.Click += new System.EventHandler(this.menuFileClose_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(312, 6);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Size = new System.Drawing.Size(315, 40);
            this.menuFileExit.Text = "&Exit";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
            // 
            // menuTools
            // 
            this.menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolsMultiStringSearch,
            this.toolStripMenuItem2,
            this.menuToolsConfiguration});
            this.menuTools.Name = "menuTools";
            this.menuTools.Size = new System.Drawing.Size(84, 33);
            this.menuTools.Text = "Tools";
            // 
            // menuToolsMultiStringSearch
            // 
            this.menuToolsMultiStringSearch.Enabled = false;
            this.menuToolsMultiStringSearch.Name = "menuToolsMultiStringSearch";
            this.menuToolsMultiStringSearch.Size = new System.Drawing.Size(326, 40);
            this.menuToolsMultiStringSearch.Text = "Multi-String Search";
            this.menuToolsMultiStringSearch.Click += new System.EventHandler(this.menuToolsMultiStringSearch_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(323, 6);
            // 
            // menuToolsConfiguration
            // 
            this.menuToolsConfiguration.Name = "menuToolsConfiguration";
            this.menuToolsConfiguration.Size = new System.Drawing.Size(326, 40);
            this.menuToolsConfiguration.Text = "Configuration";
            this.menuToolsConfiguration.Click += new System.EventHandler(this.menuToolsConfiguration_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpHelp,
            this.menuHelpSep1,
            this.menuHelpAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(77, 33);
            this.menuHelp.Text = "&Help";
            // 
            // menuHelpHelp
            // 
            this.menuHelpHelp.Name = "menuHelpHelp";
            this.menuHelpHelp.Size = new System.Drawing.Size(191, 40);
            this.menuHelpHelp.Text = "&Help";
            this.menuHelpHelp.Click += new System.EventHandler(this.menuHelpHelp_Click);
            // 
            // menuHelpSep1
            // 
            this.menuHelpSep1.Name = "menuHelpSep1";
            this.menuHelpSep1.Size = new System.Drawing.Size(188, 6);
            // 
            // menuHelpAbout
            // 
            this.menuHelpAbout.Name = "menuHelpAbout";
            this.menuHelpAbout.Size = new System.Drawing.Size(191, 40);
            this.menuHelpAbout.Text = "&About";
            this.menuHelpAbout.Click += new System.EventHandler(this.menuHelpAbout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusProgress,
            this.statusLabelMain});
            this.statusStrip.Location = new System.Drawing.Point(0, 926);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 17, 0);
            this.statusStrip.Size = new System.Drawing.Size(1693, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusProgress
            // 
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.Size = new System.Drawing.Size(122, 12);
            this.statusProgress.Visible = false;
            this.statusProgress.Click += new System.EventHandler(this.statusProgress_Click);
            // 
            // statusLabelMain
            // 
            this.statusLabelMain.Name = "statusLabelMain";
            this.statusLabelMain.Size = new System.Drawing.Size(0, 13);
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolLabelSearch,
            this.textSearch,
            this.toolLabelType,
            this.dropdownSearchType,
            this.toolButtonCumulative,
            this.toolStripSeparator1,
            this.toolButtonSearch});
            this.toolStrip.Location = new System.Drawing.Point(0, 151);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(1693, 38);
            this.toolStrip.TabIndex = 4;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolLabelSearch
            // 
            this.toolLabelSearch.Name = "toolLabelSearch";
            this.toolLabelSearch.Size = new System.Drawing.Size(80, 32);
            this.toolLabelSearch.Text = "Search";
            // 
            // textSearch
            // 
            this.textSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textSearch.Name = "textSearch";
            this.textSearch.Size = new System.Drawing.Size(800, 38);
            // 
            // toolLabelType
            // 
            this.toolLabelType.Name = "toolLabelType";
            this.toolLabelType.Size = new System.Drawing.Size(60, 32);
            this.toolLabelType.Text = "Type";
            // 
            // dropdownSearchType
            // 
            this.dropdownSearchType.AutoSize = false;
            this.dropdownSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdownSearchType.Items.AddRange(new object[] {
            "String Case Insensitive",
            "String Case Sensitive",
            "Regex Case Insensitive",
            "Regex Case Sensitive"});
            this.dropdownSearchType.Name = "dropdownSearchType";
            this.dropdownSearchType.Size = new System.Drawing.Size(170, 36);
            // 
            // toolButtonCumulative
            // 
            this.toolButtonCumulative.Checked = true;
            this.toolButtonCumulative.CheckOnClick = true;
            this.toolButtonCumulative.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolButtonCumulative.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonCumulative.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonCumulative.Image")));
            this.toolButtonCumulative.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonCumulative.Name = "toolButtonCumulative";
            this.toolButtonCumulative.Size = new System.Drawing.Size(131, 32);
            this.toolButtonCumulative.Text = "Cumulative";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // toolButtonSearch
            // 
            this.toolButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolButtonSearch.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonSearch.Image")));
            this.toolButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonSearch.Name = "toolButtonSearch";
            this.toolButtonSearch.Size = new System.Drawing.Size(40, 32);
            this.toolButtonSearch.ToolTipText = "Search";
            this.toolButtonSearch.Click += new System.EventHandler(this.toolButtonSearch_Click);
            // 
            // toolButtonFilterType
            // 
            this.toolButtonFilterType.Checked = true;
            this.toolButtonFilterType.CheckOnClick = true;
            this.toolButtonFilterType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolButtonFilterType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonFilterType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonFilterType.Name = "toolButtonFilterType";
            this.toolButtonFilterType.Size = new System.Drawing.Size(47, 32);
            this.toolButtonFilterType.Text = "OR";
            this.toolButtonFilterType.Click += new System.EventHandler(this.toolButtonFilterType_Click);
            // 
            // toolButtonFilterApply
            // 
            this.toolButtonFilterApply.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolButtonFilterApply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonFilterApply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonFilterApply.Name = "toolButtonFilterApply";
            this.toolButtonFilterApply.Size = new System.Drawing.Size(74, 32);
            this.toolButtonFilterApply.Text = "Apply";
            this.toolButtonFilterApply.ToolTipText = "Apply filter(s)";
            this.toolButtonFilterApply.Click += new System.EventHandler(this.toolButtonFilterApply_Click);
            // 
            // toolLabelCodePage
            // 
            this.toolLabelCodePage.Name = "toolLabelCodePage";
            this.toolLabelCodePage.Size = new System.Drawing.Size(119, 32);
            this.toolLabelCodePage.Text = "CodePage:";
            // 
            // dropdownSeq
            // 
            this.dropdownSeq.Items.AddRange(new object[] {
            ""});
            this.dropdownSeq.MaxLength = 12;
            this.dropdownSeq.Name = "dropdownSeq";
            this.dropdownSeq.Size = new System.Drawing.Size(155, 38);
            this.dropdownSeq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dropdownSeq_KeyPress);
            // 
            // dropdownPid
            // 
            this.dropdownPid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdownPid.Items.AddRange(new object[] {
            ""});
            this.dropdownPid.MaxLength = 9;
            this.dropdownPid.Name = "dropdownPid";
            this.dropdownPid.Size = new System.Drawing.Size(105, 38);
            // 
            // dropdownTid
            // 
            this.dropdownTid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdownTid.Items.AddRange(new object[] {
            ""});
            this.dropdownTid.MaxLength = 9;
            this.dropdownTid.Name = "dropdownTid";
            this.dropdownTid.Size = new System.Drawing.Size(105, 38);
            // 
            // dropdownLevel
            // 
            this.dropdownLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdownLevel.Items.AddRange(new object[] {
            "",
            "Verbose",
            "Debug",
            "Information",
            "Warning",
            "Error",
            "Fatal"});
            this.dropdownLevel.Name = "dropdownLevel";
            this.dropdownLevel.Size = new System.Drawing.Size(155, 38);
            // 
            // dropdownTime
            // 
            this.dropdownTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdownTime.Items.AddRange(new object[] {
            "None"});
            this.dropdownTime.Name = "dropdownTime";
            this.dropdownTime.Size = new System.Drawing.Size(305, 38);
            // 
            // dropdownModule
            // 
            this.dropdownModule.Name = "dropdownModule";
            this.dropdownModule.Size = new System.Drawing.Size(305, 38);
            // 
            // dropdownCodePage
            // 
            this.dropdownCodePage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropdownCodePage.Items.AddRange(new object[] {
            "Auto",
            "UTF8",
            "GBK"});
            this.dropdownCodePage.Name = "dropdownCodePage";
            this.dropdownCodePage.Size = new System.Drawing.Size(105, 38);
            // 
            // dropdownFilter1
            // 
            this.dropdownFilter1.Items.AddRange(new object[] {
            ""});
            this.dropdownFilter1.Name = "dropdownFilter1";
            this.dropdownFilter1.Size = new System.Drawing.Size(305, 38);
            this.dropdownFilter1.SelectedIndexChanged += new System.EventHandler(this.dropdownFilter1_SelectedIndexChanged);
            // 
            // dropdownFilter2
            // 
            this.dropdownFilter2.Items.AddRange(new object[] {
            ""});
            this.dropdownFilter2.Name = "dropdownFilter2";
            this.dropdownFilter2.Size = new System.Drawing.Size(305, 38);
            this.dropdownFilter2.SelectedIndexChanged += new System.EventHandler(this.dropdownFilter2_SelectedIndexChanged);
            // 
            // dropdownFilter3
            // 
            this.dropdownFilter3.Items.AddRange(new object[] {
            ""});
            this.dropdownFilter3.Name = "dropdownFilter3";
            this.dropdownFilter3.Size = new System.Drawing.Size(305, 38);
            this.dropdownFilter3.SelectedIndexChanged += new System.EventHandler(this.dropdownFilter3_SelectedIndexChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolTxtAllCount,
            this.toolLabelSeq,
            this.dropdownSeq,
            this.toolLabelPid,
            this.dropdownPid,
            this.toolLabelTid,
            this.dropdownTid,
            this.toolLabelLevel,
            this.dropdownLevel,
            this.toolStripButtonTruncation});
            this.toolStrip2.Location = new System.Drawing.Point(0, 113);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(1693, 38);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(44, 32);
            this.toolStripLabel2.Text = "All:";
            // 
            // toolTxtAllCount
            // 
            this.toolTxtAllCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolTxtAllCount.Enabled = false;
            this.toolTxtAllCount.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolTxtAllCount.MaxLength = 16;
            this.toolTxtAllCount.Name = "toolTxtAllCount";
            this.toolTxtAllCount.ReadOnly = true;
            this.toolTxtAllCount.Size = new System.Drawing.Size(100, 38);
            // 
            // toolLabelSeq
            // 
            this.toolLabelSeq.Name = "toolLabelSeq";
            this.toolLabelSeq.Size = new System.Drawing.Size(54, 32);
            this.toolLabelSeq.Text = "Seq:";
            // 
            // toolLabelPid
            // 
            this.toolLabelPid.Name = "toolLabelPid";
            this.toolLabelPid.Size = new System.Drawing.Size(49, 32);
            this.toolLabelPid.Text = "Pid:";
            // 
            // toolLabelTid
            // 
            this.toolLabelTid.Name = "toolLabelTid";
            this.toolLabelTid.Size = new System.Drawing.Size(48, 32);
            this.toolLabelTid.Text = "Tid:";
            // 
            // toolLabelLevel
            // 
            this.toolLabelLevel.Name = "toolLabelLevel";
            this.toolLabelLevel.Size = new System.Drawing.Size(64, 32);
            this.toolLabelLevel.Text = "Level";
            // 
            // toolStripButtonTruncation
            // 
            this.toolStripButtonTruncation.BackColor = System.Drawing.Color.Red;
            this.toolStripButtonTruncation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonTruncation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTruncation.Name = "toolStripButtonTruncation";
            this.toolStripButtonTruncation.Size = new System.Drawing.Size(125, 32);
            this.toolStripButtonTruncation.Text = "Truncation";
            this.toolStripButtonTruncation.ToolTipText = "删除Log文件中所有内容";
            this.toolStripButtonTruncation.Click += new System.EventHandler(this.toolStripButtonTruncation_Click);
            // 
            // toolStrip3
            // 
            this.toolStrip3.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolLabelTime,
            this.dropdownTime,
            this.toolLabelModule,
            this.dropdownModule,
            this.toolLabelCodePage,
            this.dropdownCodePage,
            this.toolButtonAutoScroll});
            this.toolStrip3.Location = new System.Drawing.Point(0, 75);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip3.Size = new System.Drawing.Size(1693, 38);
            this.toolStrip3.TabIndex = 7;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolLabelTime
            // 
            this.toolLabelTime.Name = "toolLabelTime";
            this.toolLabelTime.Size = new System.Drawing.Size(67, 32);
            this.toolLabelTime.Text = "Time:";
            // 
            // toolLabelModule
            // 
            this.toolLabelModule.Name = "toolLabelModule";
            this.toolLabelModule.Size = new System.Drawing.Size(95, 32);
            this.toolLabelModule.Text = "Module:";
            // 
            // toolButtonAutoScroll
            // 
            this.toolButtonAutoScroll.Checked = true;
            this.toolButtonAutoScroll.CheckOnClick = true;
            this.toolButtonAutoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolButtonAutoScroll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolButtonAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAutoScroll.Name = "toolButtonAutoScroll";
            this.toolButtonAutoScroll.Size = new System.Drawing.Size(72, 32);
            this.toolButtonAutoScroll.Text = "Scroll";
            this.toolButtonAutoScroll.ToolTipText = "新消息来时自动滚动到末尾";
            this.toolButtonAutoScroll.Click += new System.EventHandler(this.toolStripButtonAutoScroll_Click);
            // 
            // toolStrip4
            // 
            this.toolStrip4.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolTxtFiltered,
            this.toolLabelFilter,
            this.dropdownFilter1,
            this.dropdownFilter2,
            this.dropdownFilter3,
            this.toolButtonFilterType,
            this.toolButtonFilterApply,
            this.toolStripButtonClear});
            this.toolStrip4.Location = new System.Drawing.Point(0, 37);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip4.Size = new System.Drawing.Size(1693, 38);
            this.toolStrip4.TabIndex = 8;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(78, 32);
            this.toolStripLabel1.Text = "Count:";
            // 
            // toolTxtFiltered
            // 
            this.toolTxtFiltered.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toolTxtFiltered.Enabled = false;
            this.toolTxtFiltered.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolTxtFiltered.MaxLength = 16;
            this.toolTxtFiltered.Name = "toolTxtFiltered";
            this.toolTxtFiltered.ReadOnly = true;
            this.toolTxtFiltered.Size = new System.Drawing.Size(100, 38);
            // 
            // toolLabelFilter
            // 
            this.toolLabelFilter.Name = "toolLabelFilter";
            this.toolLabelFilter.Size = new System.Drawing.Size(68, 32);
            this.toolLabelFilter.Text = "Filter:";
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripButtonClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(68, 32);
            this.toolStripButtonClear.Text = "Clear";
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.tabControl);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 189);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1693, 737);
            this.panelMain.TabIndex = 5;
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowToolTips = true;
            this.tabControl.Size = new System.Drawing.Size(1693, 737);
            this.tabControl.TabIndex = 1;
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuFilter,
            this.contextMenuSep1,
            this.contextMenuSearch,
            this.contextMenuSep2,
            this.contextMenuExport,
            this.contextMenuSep3,
            this.contextMenuCopy,
            this.toolStripMenuItem5,
            this.contextLines});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenu.Size = new System.Drawing.Size(168, 198);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // contextMenuFilter
            // 
            this.contextMenuFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuFilterShowMatched,
            this.contextMenuFilterHideMatched,
            this.toolStripMenuItem1,
            this.contextMenuFilterClear});
            this.contextMenuFilter.Name = "contextMenuFilter";
            this.contextMenuFilter.Size = new System.Drawing.Size(167, 34);
            this.contextMenuFilter.Text = "Filtering";
            // 
            // contextMenuFilterShowMatched
            // 
            this.contextMenuFilterShowMatched.Name = "contextMenuFilterShowMatched";
            this.contextMenuFilterShowMatched.Size = new System.Drawing.Size(279, 40);
            this.contextMenuFilterShowMatched.Text = "Show matched";
            this.contextMenuFilterShowMatched.Click += new System.EventHandler(this.contextMenuFilterShowMatched_Click);
            // 
            // contextMenuFilterHideMatched
            // 
            this.contextMenuFilterHideMatched.Name = "contextMenuFilterHideMatched";
            this.contextMenuFilterHideMatched.Size = new System.Drawing.Size(279, 40);
            this.contextMenuFilterHideMatched.Text = "Hide matched";
            this.contextMenuFilterHideMatched.Click += new System.EventHandler(this.contextMenuFilterHideMatched_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(276, 6);
            // 
            // contextMenuFilterClear
            // 
            this.contextMenuFilterClear.Name = "contextMenuFilterClear";
            this.contextMenuFilterClear.Size = new System.Drawing.Size(279, 40);
            this.contextMenuFilterClear.Text = "Clear";
            this.contextMenuFilterClear.Click += new System.EventHandler(this.contextMenuFilterClear_Click);
            // 
            // contextMenuSep1
            // 
            this.contextMenuSep1.Name = "contextMenuSep1";
            this.contextMenuSep1.Size = new System.Drawing.Size(164, 6);
            // 
            // contextMenuSearch
            // 
            this.contextMenuSearch.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuSearchViewTerms,
            this.toolStripMenuItem4,
            this.contextMenuSearchColour});
            this.contextMenuSearch.Name = "contextMenuSearch";
            this.contextMenuSearch.Size = new System.Drawing.Size(167, 34);
            this.contextMenuSearch.Text = "Search";
            // 
            // contextMenuSearchViewTerms
            // 
            this.contextMenuSearchViewTerms.Name = "contextMenuSearchViewTerms";
            this.contextMenuSearchViewTerms.Size = new System.Drawing.Size(315, 40);
            this.contextMenuSearchViewTerms.Text = "View Terms";
            this.contextMenuSearchViewTerms.Click += new System.EventHandler(this.contextMenuSearchViewTerms_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(312, 6);
            // 
            // contextMenuSearchColour
            // 
            this.contextMenuSearchColour.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuSearchMatch,
            this.contextMenuSearchColourContext});
            this.contextMenuSearchColour.Name = "contextMenuSearchColour";
            this.contextMenuSearchColour.Size = new System.Drawing.Size(315, 40);
            this.contextMenuSearchColour.Text = "Colour";
            // 
            // contextMenuSearchMatch
            // 
            this.contextMenuSearchMatch.Name = "contextMenuSearchMatch";
            this.contextMenuSearchMatch.Size = new System.Drawing.Size(208, 40);
            this.contextMenuSearchMatch.Text = "Match";
            this.contextMenuSearchMatch.Click += new System.EventHandler(this.contextMenuSearchColourMatch_Click);
            // 
            // contextMenuSearchColourContext
            // 
            this.contextMenuSearchColourContext.Name = "contextMenuSearchColourContext";
            this.contextMenuSearchColourContext.Size = new System.Drawing.Size(208, 40);
            this.contextMenuSearchColourContext.Text = "Context";
            this.contextMenuSearchColourContext.Click += new System.EventHandler(this.contextMenuSearchColourContext_Click);
            // 
            // contextMenuSep2
            // 
            this.contextMenuSep2.Name = "contextMenuSep2";
            this.contextMenuSep2.Size = new System.Drawing.Size(164, 6);
            // 
            // contextMenuExport
            // 
            this.contextMenuExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuExportAll,
            this.contextMenuExportSelected});
            this.contextMenuExport.Name = "contextMenuExport";
            this.contextMenuExport.Size = new System.Drawing.Size(167, 34);
            this.contextMenuExport.Text = "Export";
            // 
            // contextMenuExportAll
            // 
            this.contextMenuExportAll.Name = "contextMenuExportAll";
            this.contextMenuExportAll.Size = new System.Drawing.Size(315, 40);
            this.contextMenuExportAll.Text = "All";
            this.contextMenuExportAll.Click += new System.EventHandler(this.contextMenuExportAll_Click);
            // 
            // contextMenuExportSelected
            // 
            this.contextMenuExportSelected.Name = "contextMenuExportSelected";
            this.contextMenuExportSelected.Size = new System.Drawing.Size(315, 40);
            this.contextMenuExportSelected.Text = "Selected";
            this.contextMenuExportSelected.Click += new System.EventHandler(this.contextMenuExportSelected_Click);
            // 
            // contextMenuSep3
            // 
            this.contextMenuSep3.Name = "contextMenuSep3";
            this.contextMenuSep3.Size = new System.Drawing.Size(164, 6);
            // 
            // contextMenuCopy
            // 
            this.contextMenuCopy.Name = "contextMenuCopy";
            this.contextMenuCopy.Size = new System.Drawing.Size(167, 34);
            this.contextMenuCopy.Text = "Copy";
            this.contextMenuCopy.Click += new System.EventHandler(this.contextMenuCopy_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(164, 6);
            // 
            // contextLines
            // 
            this.contextLines.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextLinesGoToLine,
            this.contextLinesGoToFirstLine,
            this.contextLinesGoToLastLine});
            this.contextLines.Name = "contextLines";
            this.contextLines.Size = new System.Drawing.Size(167, 34);
            this.contextLines.Text = "Lines";
            // 
            // contextLinesGoToLine
            // 
            this.contextLinesGoToLine.Name = "contextLinesGoToLine";
            this.contextLinesGoToLine.Size = new System.Drawing.Size(315, 40);
            this.contextLinesGoToLine.Text = "Go To Line";
            this.contextLinesGoToLine.Click += new System.EventHandler(this.contextLinesGoToLine_Click);
            // 
            // contextLinesGoToFirstLine
            // 
            this.contextLinesGoToFirstLine.Name = "contextLinesGoToFirstLine";
            this.contextLinesGoToFirstLine.Size = new System.Drawing.Size(315, 40);
            this.contextLinesGoToFirstLine.Text = "Go To First Line";
            this.contextLinesGoToFirstLine.Click += new System.EventHandler(this.contextLinesGoToFirstLine_Click);
            // 
            // contextLinesGoToLastLine
            // 
            this.contextLinesGoToLastLine.Name = "contextLinesGoToLastLine";
            this.contextLinesGoToLastLine.Size = new System.Drawing.Size(315, 40);
            this.contextLinesGoToLastLine.Text = "Go To Last Line";
            this.contextLinesGoToLastLine.Click += new System.EventHandler(this.contextLinesGoToLastLine_Click);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Interval = 500;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // openSFTPToolStripMenuItem
            // 
            this.openSFTPToolStripMenuItem.Name = "openSFTPToolStripMenuItem";
            this.openSFTPToolStripMenuItem.Size = new System.Drawing.Size(315, 40);
            this.openSFTPToolStripMenuItem.Text = "Open SFTP";
            this.openSFTPToolStripMenuItem.Click += new System.EventHandler(this.openSFTPToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1693, 948);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.toolStrip3);
            this.Controls.Add(this.toolStrip4);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LogViewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResizeEnd += new System.EventHandler(this.FormMain_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelMain;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripLabel toolLabelSearch;
        private System.Windows.Forms.ToolStripTextBox textSearch;
        private System.Windows.Forms.ToolStripButton toolButtonSearch;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpHelp;
        private System.Windows.Forms.ToolStripMenuItem menuHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripSeparator menuFileSep1;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripProgressBar statusProgress;
        private System.Windows.Forms.ToolStripSeparator menuHelpSep1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripComboBox dropdownSearchType;
        private System.Windows.Forms.ToolStripComboBox dropdownSeq;
        private System.Windows.Forms.ToolStripComboBox dropdownPid;
        private System.Windows.Forms.ToolStripComboBox dropdownTid;
        private System.Windows.Forms.ToolStripComboBox dropdownLevel;
        private System.Windows.Forms.ToolStripComboBox dropdownTime;
        private System.Windows.Forms.ToolStripComboBox dropdownModule;
        private System.Windows.Forms.ToolStripComboBox dropdownCodePage;
        private System.Windows.Forms.ToolStripComboBox dropdownFilter1;
        private System.Windows.Forms.ToolStripComboBox dropdownFilter2;
        private System.Windows.Forms.ToolStripComboBox dropdownFilter3;
        private System.Windows.Forms.ToolStripLabel toolLabelType;
        private System.Windows.Forms.ToolStripLabel toolLabelCodePage;
        private System.Windows.Forms.ToolStripLabel toolLabelSeq;
        private System.Windows.Forms.ToolStripLabel toolLabelPid;
        private System.Windows.Forms.ToolStripLabel toolLabelTid;
        private System.Windows.Forms.ToolStripLabel toolLabelLevel;
        private System.Windows.Forms.ToolStripLabel toolLabelTime;
        private System.Windows.Forms.ToolStripLabel toolLabelModule;
        private System.Windows.Forms.ToolStripLabel toolLabelFilter;

        private System.Windows.Forms.ToolStripMenuItem contextMenuFilter;
        private System.Windows.Forms.ToolStripMenuItem contextMenuFilterClear;
        private System.Windows.Forms.ToolStripButton toolButtonCumulative;
        private System.Windows.Forms.ToolStripButton toolButtonFilterType;
        private System.Windows.Forms.ToolStripButton toolButtonFilterApply;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuFilterShowMatched;
        private System.Windows.Forms.ToolStripMenuItem contextMenuFilterHideMatched;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator contextMenuSep1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuSearch;
        private System.Windows.Forms.ToolStripSeparator contextMenuSep2;
        private System.Windows.Forms.ToolStripMenuItem contextMenuExport;
        private System.Windows.Forms.ToolStripSeparator contextMenuSep3;
        private System.Windows.Forms.ToolStripMenuItem contextMenuCopy;
        private System.Windows.Forms.ToolStripMenuItem contextMenuSearchViewTerms;
        private System.Windows.Forms.ToolStripMenuItem contextMenuExportAll;
        private System.Windows.Forms.ToolStripMenuItem contextMenuExportSelected;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem menuToolsMultiStringSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuToolsConfiguration;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem contextMenuSearchColour;
        private System.Windows.Forms.ToolStripMenuItem contextMenuSearchMatch;
        private System.Windows.Forms.ToolStripMenuItem contextMenuSearchColourContext;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem contextLines;
        private System.Windows.Forms.ToolStripMenuItem contextLinesGoToLine;
        private System.Windows.Forms.ToolStripMenuItem contextLinesGoToFirstLine;
        private System.Windows.Forms.ToolStripMenuItem contextLinesGoToLastLine;
        private System.Windows.Forms.ToolStripMenuItem menuFileClose;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpenNewTab;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox toolTxtFiltered;
        private System.Windows.Forms.ToolStripButton toolStripButtonClear;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox toolTxtAllCount;
        private System.Windows.Forms.ToolStripButton toolStripButtonTruncation;
        private System.Windows.Forms.ToolStripButton toolButtonAutoScroll;
        private System.Windows.Forms.Timer timerUpdate;
        private System.Windows.Forms.ToolStripMenuItem openSFTPToolStripMenuItem;
    }
}

