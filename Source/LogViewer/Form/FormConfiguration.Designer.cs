namespace LogViewer
{
    partial class FormConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguration));
            this.checkShowContextLines = new System.Windows.Forms.CheckBox();
            this.comboNumLines = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxSetEnv = new System.Windows.Forms.CheckBox();
            this.checkBoxOpenFile = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkShowContextLines
            // 
            this.checkShowContextLines.AutoSize = true;
            this.checkShowContextLines.Location = new System.Drawing.Point(20, 23);
            this.checkShowContextLines.Margin = new System.Windows.Forms.Padding(4);
            this.checkShowContextLines.Name = "checkShowContextLines";
            this.checkShowContextLines.Size = new System.Drawing.Size(234, 25);
            this.checkShowContextLines.TabIndex = 0;
            this.checkShowContextLines.Text = "Show context lines";
            this.checkShowContextLines.UseVisualStyleBackColor = true;
            this.checkShowContextLines.CheckedChanged += new System.EventHandler(this.checkShowContextLines_CheckedChanged);
            // 
            // comboNumLines
            // 
            this.comboNumLines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboNumLines.FormattingEnabled = true;
            this.comboNumLines.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.comboNumLines.Location = new System.Drawing.Point(296, 60);
            this.comboNumLines.Margin = new System.Windows.Forms.Padding(4);
            this.comboNumLines.Name = "comboNumLines";
            this.comboNumLines.Size = new System.Drawing.Size(110, 29);
            this.comboNumLines.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 62);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of lines to show";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(334, 201);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(147, 42);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(180, 201);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(147, 42);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxSetEnv
            // 
            this.checkBoxSetEnv.AutoSize = true;
            this.checkBoxSetEnv.Location = new System.Drawing.Point(20, 108);
            this.checkBoxSetEnv.Name = "checkBoxSetEnv";
            this.checkBoxSetEnv.Size = new System.Drawing.Size(322, 25);
            this.checkBoxSetEnv.TabIndex = 5;
            this.checkBoxSetEnv.Text = "Set App to Envirnment Path";
            this.checkBoxSetEnv.UseVisualStyleBackColor = true;
            this.checkBoxSetEnv.CheckedChanged += new System.EventHandler(this.checkBoxSetEnv_CheckedChanged);
            // 
            // checkBoxOpenFile
            // 
            this.checkBoxOpenFile.AutoSize = true;
            this.checkBoxOpenFile.Location = new System.Drawing.Point(19, 149);
            this.checkBoxOpenFile.Name = "checkBoxOpenFile";
            this.checkBoxOpenFile.Size = new System.Drawing.Size(399, 25);
            this.checkBoxOpenFile.TabIndex = 6;
            this.checkBoxOpenFile.Text = "Set App default \'.log\' file open.";
            this.checkBoxOpenFile.UseVisualStyleBackColor = true;
            this.checkBoxOpenFile.CheckedChanged += new System.EventHandler(this.checkBoxOpenFile_CheckedChanged);
            // 
            // FormConfiguration
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(496, 255);
            this.Controls.Add(this.checkBoxOpenFile);
            this.Controls.Add(this.checkBoxSetEnv);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboNumLines);
            this.Controls.Add(this.checkShowContextLines);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfiguration";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.FormConfiguration_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkShowContextLines;
        private System.Windows.Forms.ComboBox comboNumLines;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxSetEnv;
        private System.Windows.Forms.CheckBox checkBoxOpenFile;
    }
}