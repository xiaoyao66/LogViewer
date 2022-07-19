using System;
using System.Windows.Forms;

namespace LogViewer
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormConfiguration : Form
    {
        #region Member Variables/Properties
        public Configuration Config { get; private set; }
        public string AppPath = System.Windows.Forms.Application.ExecutablePath;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public FormConfiguration(Configuration config)
        {
            InitializeComponent();
            this.Config = config;
            if (this.Config.NumContextLines > 0)
            {
                checkShowContextLines.Checked = true;
                comboNumLines.SelectedIndex = this.Config.NumContextLines - 1;
            }
            else
            {
                checkShowContextLines.Checked = false;
                comboNumLines.SelectedIndex = 0;
            }

            checkShowContextLines_CheckedChanged(this, null);
        }
        #endregion

        #region Buttton Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (checkShowContextLines.Checked == true)
            {
                Config.NumContextLines = comboNumLines.SelectedIndex + 1;
            }
            else
            {
                Config.NumContextLines = 0;
            }

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkShowContextLines_CheckedChanged(object sender, EventArgs e)
        {
            if (checkShowContextLines.Checked == true)
            {
                comboNumLines.Enabled = true;
            }
            else
            {
                comboNumLines.Enabled = false;
            }
        }

        private void checkBoxSetEnv_CheckedChanged(object sender, EventArgs e)
        {
            if (!loaded) return;

            string dir = System.IO.Path.GetDirectoryName(AppPath);
            if (checkBoxSetEnv.Checked)
            {
                SystemConfig.SysEnvironment.SetPath(dir);
            }
            else
            {
                SystemConfig.SysEnvironment.RemovePath(dir);
            }
        }

        private void checkBoxOpenFile_CheckedChanged(object sender, EventArgs e)
        {
            if (!loaded) return;
            
            if (checkBoxOpenFile.Checked)
            {
                if (SystemConfig.SetDefaultFileOpen.IsAdministrator())
                    SystemConfig.SetDefaultFileOpen.SetFileOpenApp(".log", AppPath, AppPath);
                else
                {
                    MessageBox.Show("Please reopen the App with right of administrators.");
                    checkBoxOpenFile.Checked = false;
                }
            }
            else
            {

            }
        }

        private bool loaded = false;

        private void FormConfiguration_Load(object sender, EventArgs e)
        {
            string dir = System.IO.Path.GetDirectoryName(AppPath);
            checkBoxSetEnv.Checked = SystemConfig.SysEnvironment.CheckPath(dir);
            checkBoxOpenFile.Checked = SystemConfig.SetDefaultFileOpen.CheckFileOpenApp(".log", AppPath);
            loaded = true;
        }
    }
}
