using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogViewer
{
    public partial class FormSFTPLoad : Form
    {
        public Collection<string> SFTP_URLs { get; set; }
        public string Password { get; private set; }
        public string UserName { get; private set; }

        public string URL { get; private set; }
        public Uri Uri { get; private set; }

        public FormSFTPLoad()
        {
            InitializeComponent();
            // this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
        }

        private void comboBoxURL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(comboBoxURL.Text))
            {
                MessageBox.Show("URL不能为空");
                return;
            }

            try
            {
                if (comboBoxURL.Text.IndexOf("://") < 0)
                {
                    comboBoxURL.Text = "sftp://" + comboBoxURL.Text;
                }
                var url = new Uri(comboBoxURL.Text);
                if (!string.IsNullOrWhiteSpace(url.Host) &&
                    !string.IsNullOrWhiteSpace(url.Scheme) &&
                    !string.IsNullOrWhiteSpace(url.AbsolutePath))
                {
                    if (!string.IsNullOrWhiteSpace(textBoxUserName.Text))
                    {
                        UserName = textBoxUserName.Text.Trim();
                    }
                    else if (!string.IsNullOrWhiteSpace(url.UserInfo))
                    {
                        UserName = url.UserInfo.Trim();
                    }
                    if (string.IsNullOrWhiteSpace(UserName))
                    {
                        if (MessageBox.Show("用户名为空，确定登录么？", "信息", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(textBoxPassword.Text))
                        {
                            if (MessageBox.Show("用户名为空，确定登录么？", "信息", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }

                    this.URL = comboBoxURL.Text;
                    this.Uri = url;
                    this.Password = textBoxPassword.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { }


            MessageBox.Show("URL路径不能为空。");
            comboBoxURL.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxURL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxPassword.Focus();
            }
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.Focus();
            }
        }

        private void FormSFTPLoad_Load(object sender, EventArgs e)
        {
            if (SFTP_URLs.Count > 0)
                comboBoxURL.Items.AddRange(SFTP_URLs.ToArray());
        }
    }
}
