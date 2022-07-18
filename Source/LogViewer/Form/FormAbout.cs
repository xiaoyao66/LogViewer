﻿using System.Windows.Forms;

namespace LogViewer
{
    public partial class FormAbout : Form
    {
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public FormAbout()
        {
            InitializeComponent();

            lblApp.Text = Application.ProductName;
            lblVer.Text = "v" + Application.ProductVersion;
        }
        #endregion

        #region Link Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkEmail_LinkClicked(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.FileName = "mailto:" + linkEmail.Text;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkWeb_LinkClicked(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.FileName = "http://" + linkWeb.Text;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkIcons8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.RedirectStandardOutput = false;
            process.StartInfo.FileName = "https://" + linkIcons8.Text;
            process.StartInfo.UseShellExecute = true;
            process.Start();
        }
        #endregion

        #region Button Event Handlers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        #endregion
    }
}
