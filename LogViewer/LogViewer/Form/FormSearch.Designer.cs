﻿namespace LogViewer
{
    partial class FormSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSearch));
            this.buttonImport = new System.Windows.Forms.Button();
            this.comboType = new System.Windows.Forms.ComboBox();
            this.labelSearchType = new System.Windows.Forms.Label();
            this.listTerms = new BrightIdeasSoftware.ObjectListView();
            this.olvcLine = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.listTerms)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonImport.Location = new System.Drawing.Point(21, 531);
            this.buttonImport.Margin = new System.Windows.Forms.Padding(4);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(147, 48);
            this.buttonImport.TabIndex = 0;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // comboType
            // 
            this.comboType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboType.FormattingEnabled = true;
            this.comboType.Items.AddRange(new object[] {
            "Sub String Case Insensitive",
            "Sub String Case Sensitive",
            "Regex Case Insensitive",
            "Regex Case Sensitive"});
            this.comboType.Location = new System.Drawing.Point(76, 12);
            this.comboType.Margin = new System.Windows.Forms.Padding(4);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(526, 32);
            this.comboType.TabIndex = 1;
            // 
            // labelSearchType
            // 
            this.labelSearchType.AutoSize = true;
            this.labelSearchType.Location = new System.Drawing.Point(16, 16);
            this.labelSearchType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSearchType.Name = "labelSearchType";
            this.labelSearchType.Size = new System.Drawing.Size(57, 25);
            this.labelSearchType.TabIndex = 3;
            this.labelSearchType.Text = "Type";
            // 
            // listTerms
            // 
            this.listTerms.AllColumns.Add(this.olvcLine);
            this.listTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTerms.CellEditUseWholeCell = false;
            this.listTerms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvcLine});
            this.listTerms.Cursor = System.Windows.Forms.Cursors.Default;
            this.listTerms.FullRowSelect = true;
            this.listTerms.HideSelection = false;
            this.listTerms.Location = new System.Drawing.Point(20, 66);
            this.listTerms.Margin = new System.Windows.Forms.Padding(4);
            this.listTerms.Name = "listTerms";
            this.listTerms.OwnerDraw = false;
            this.listTerms.ShowFilterMenuOnRightClick = false;
            this.listTerms.ShowGroups = false;
            this.listTerms.Size = new System.Drawing.Size(585, 451);
            this.listTerms.TabIndex = 4;
            this.listTerms.UseCompatibleStateImageBehavior = false;
            this.listTerms.View = System.Windows.Forms.View.Details;
            // 
            // olvcLine
            // 
            this.olvcLine.AspectName = "Data";
            this.olvcLine.FillsFreeSpace = true;
            this.olvcLine.Text = "Pattern";
            this.olvcLine.Width = 247;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(455, 531);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(147, 48);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(300, 531);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(147, 48);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // FormSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(621, 592);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.listTerms);
            this.Controls.Add(this.labelSearchType);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.buttonImport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(642, 636);
            this.Name = "FormSearch";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search";
            ((System.ComponentModel.ISupportInitialize)(this.listTerms)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.ComboBox comboType;
        private System.Windows.Forms.Label labelSearchType;
        private BrightIdeasSoftware.ObjectListView listTerms;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private BrightIdeasSoftware.OLVColumn olvcLine;
    }
}