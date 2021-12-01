namespace FTDStatementPrinter
{
    partial class StatementControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblBillDateStart = new System.Windows.Forms.Label();
            this.btnViewWindow = new System.Windows.Forms.Button();
            this.btnRegenerate = new System.Windows.Forms.Button();
            this.btnDeleteStatement = new System.Windows.Forms.Button();
            this.btnDeleteFile = new System.Windows.Forms.Button();
            this.lblAccountID = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.lblPrinter = new System.Windows.Forms.Label();
            this.lblDelete = new System.Windows.Forms.Label();
            this.btnEditCreds = new System.Windows.Forms.Button();
            this.btnViewOS = new System.Windows.Forms.Button();
            this.cbAccIDs = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblBillAmount = new System.Windows.Forms.Label();
            this.lblBillCycleStartlbl = new System.Windows.Forms.Label();
            this.lblBillAmountDetaillbl = new System.Windows.Forms.Label();
            this.lblBillCycleEndlbl = new System.Windows.Forms.Label();
            this.lblBillDateEnd = new System.Windows.Forms.Label();
            this.lblBillDueDatelbl = new System.Windows.Forms.Label();
            this.lblBillDateDue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblBillDateStart
            // 
            this.lblBillDateStart.AutoSize = true;
            this.lblBillDateStart.Location = new System.Drawing.Point(345, 7);
            this.lblBillDateStart.Name = "lblBillDateStart";
            this.lblBillDateStart.Size = new System.Drawing.Size(106, 13);
            this.lblBillDateStart.TabIndex = 1;
            this.lblBillDateStart.Text = "[Insert Bill Date Start]";
            // 
            // btnViewWindow
            // 
            this.btnViewWindow.Location = new System.Drawing.Point(592, 7);
            this.btnViewWindow.Name = "btnViewWindow";
            this.btnViewWindow.Size = new System.Drawing.Size(96, 24);
            this.btnViewWindow.TabIndex = 1;
            this.btnViewWindow.Text = "View in App";
            this.btnViewWindow.UseVisualStyleBackColor = true;
            this.btnViewWindow.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnRegenerate
            // 
            this.btnRegenerate.Location = new System.Drawing.Point(479, 7);
            this.btnRegenerate.Name = "btnRegenerate";
            this.btnRegenerate.Size = new System.Drawing.Size(96, 24);
            this.btnRegenerate.TabIndex = 0;
            this.btnRegenerate.Text = "Generate";
            this.btnRegenerate.UseVisualStyleBackColor = true;
            this.btnRegenerate.Click += new System.EventHandler(this.btnRegenerate_Click);
            // 
            // btnDeleteStatement
            // 
            this.btnDeleteStatement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDeleteStatement.Location = new System.Drawing.Point(705, 37);
            this.btnDeleteStatement.Name = "btnDeleteStatement";
            this.btnDeleteStatement.Size = new System.Drawing.Size(132, 24);
            this.btnDeleteStatement.TabIndex = 6;
            this.btnDeleteStatement.Text = "Delete Account";
            this.btnDeleteStatement.UseVisualStyleBackColor = false;
            this.btnDeleteStatement.Click += new System.EventHandler(this.btnDeleteStatement_Click);
            // 
            // btnDeleteFile
            // 
            this.btnDeleteFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDeleteFile.Location = new System.Drawing.Point(705, 7);
            this.btnDeleteFile.Name = "btnDeleteFile";
            this.btnDeleteFile.Size = new System.Drawing.Size(132, 24);
            this.btnDeleteFile.TabIndex = 5;
            this.btnDeleteFile.Text = "Delete File";
            this.btnDeleteFile.UseVisualStyleBackColor = false;
            this.btnDeleteFile.Click += new System.EventHandler(this.btnDeleteFile_Click);
            // 
            // lblAccountID
            // 
            this.lblAccountID.AutoSize = true;
            this.lblAccountID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountID.Location = new System.Drawing.Point(119, 7);
            this.lblAccountID.Name = "lblAccountID";
            this.lblAccountID.Size = new System.Drawing.Size(96, 13);
            this.lblAccountID.TabIndex = 0;
            this.lblAccountID.Text = "[Insert Account ID]";
            // 
            // lblPreview
            // 
            this.lblPreview.AutoSize = true;
            this.lblPreview.Location = new System.Drawing.Point(589, 7);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(116, 13);
            this.lblPreview.TabIndex = 7;
            this.lblPreview.Text = "[Insert Statement Date]";
            this.lblPreview.Visible = false;
            // 
            // lblPrinter
            // 
            this.lblPrinter.AutoSize = true;
            this.lblPrinter.Location = new System.Drawing.Point(476, 7);
            this.lblPrinter.Name = "lblPrinter";
            this.lblPrinter.Size = new System.Drawing.Size(116, 13);
            this.lblPrinter.TabIndex = 9;
            this.lblPrinter.Text = "[Insert Statement Date]";
            this.lblPrinter.Visible = false;
            // 
            // lblDelete
            // 
            this.lblDelete.AutoSize = true;
            this.lblDelete.Location = new System.Drawing.Point(702, 7);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(116, 13);
            this.lblDelete.TabIndex = 10;
            this.lblDelete.Text = "[Insert Statement Date]";
            this.lblDelete.Visible = false;
            // 
            // btnEditCreds
            // 
            this.btnEditCreds.Location = new System.Drawing.Point(479, 37);
            this.btnEditCreds.Name = "btnEditCreds";
            this.btnEditCreds.Size = new System.Drawing.Size(96, 24);
            this.btnEditCreds.TabIndex = 4;
            this.btnEditCreds.Text = "Edit Credentials";
            this.btnEditCreds.UseVisualStyleBackColor = true;
            this.btnEditCreds.Click += new System.EventHandler(this.btnEditCreds_Click);
            // 
            // btnViewOS
            // 
            this.btnViewOS.Location = new System.Drawing.Point(592, 37);
            this.btnViewOS.Name = "btnViewOS";
            this.btnViewOS.Size = new System.Drawing.Size(96, 24);
            this.btnViewOS.TabIndex = 2;
            this.btnViewOS.Text = "View in OS";
            this.btnViewOS.UseVisualStyleBackColor = true;
            this.btnViewOS.Click += new System.EventHandler(this.btnViewOS_Click);
            // 
            // cbAccIDs
            // 
            this.cbAccIDs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccIDs.FormattingEnabled = true;
            this.cbAccIDs.Location = new System.Drawing.Point(122, 8);
            this.cbAccIDs.Name = "cbAccIDs";
            this.cbAccIDs.Size = new System.Drawing.Size(126, 21);
            this.cbAccIDs.TabIndex = 11;
            this.cbAccIDs.Visible = false;
            this.cbAccIDs.SelectedIndexChanged += new System.EventHandler(this.cbAccIDs_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(3, 7);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(43, 13);
            this.lblType.TabIndex = 12;
            this.lblType.Text = "[Type]";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBillAmount
            // 
            this.lblBillAmount.AutoSize = true;
            this.lblBillAmount.Location = new System.Drawing.Point(345, 73);
            this.lblBillAmount.Name = "lblBillAmount";
            this.lblBillAmount.Size = new System.Drawing.Size(94, 13);
            this.lblBillAmount.TabIndex = 13;
            this.lblBillAmount.Text = "[Insert Bill Amount]";
            // 
            // lblBillCycleStartlbl
            // 
            this.lblBillCycleStartlbl.AutoSize = true;
            this.lblBillCycleStartlbl.Location = new System.Drawing.Point(268, 7);
            this.lblBillCycleStartlbl.Name = "lblBillCycleStartlbl";
            this.lblBillCycleStartlbl.Size = new System.Drawing.Size(74, 13);
            this.lblBillCycleStartlbl.TabIndex = 14;
            this.lblBillCycleStartlbl.Text = "Bill Date Start:";
            // 
            // lblBillAmountDetaillbl
            // 
            this.lblBillAmountDetaillbl.AutoSize = true;
            this.lblBillAmountDetaillbl.Location = new System.Drawing.Point(280, 73);
            this.lblBillAmountDetaillbl.Name = "lblBillAmountDetaillbl";
            this.lblBillAmountDetaillbl.Size = new System.Drawing.Size(62, 13);
            this.lblBillAmountDetaillbl.TabIndex = 15;
            this.lblBillAmountDetaillbl.Text = "Bill Amount:";
            // 
            // lblBillCycleEndlbl
            // 
            this.lblBillCycleEndlbl.AutoSize = true;
            this.lblBillCycleEndlbl.Location = new System.Drawing.Point(271, 29);
            this.lblBillCycleEndlbl.Name = "lblBillCycleEndlbl";
            this.lblBillCycleEndlbl.Size = new System.Drawing.Size(71, 13);
            this.lblBillCycleEndlbl.TabIndex = 17;
            this.lblBillCycleEndlbl.Text = "Bill Date End:";
            // 
            // lblBillDateEnd
            // 
            this.lblBillDateEnd.AutoSize = true;
            this.lblBillDateEnd.Location = new System.Drawing.Point(345, 29);
            this.lblBillDateEnd.Name = "lblBillDateEnd";
            this.lblBillDateEnd.Size = new System.Drawing.Size(103, 13);
            this.lblBillDateEnd.TabIndex = 18;
            this.lblBillDateEnd.Text = "[Insert Bill Date End]";
            // 
            // lblBillDueDatelbl
            // 
            this.lblBillDueDatelbl.AutoSize = true;
            this.lblBillDueDatelbl.Location = new System.Drawing.Point(270, 51);
            this.lblBillDueDatelbl.Name = "lblBillDueDatelbl";
            this.lblBillDueDatelbl.Size = new System.Drawing.Size(72, 13);
            this.lblBillDueDatelbl.TabIndex = 20;
            this.lblBillDueDatelbl.Text = "Bill Due Date:";
            // 
            // lblBillDateDue
            // 
            this.lblBillDateDue.AutoSize = true;
            this.lblBillDateDue.Location = new System.Drawing.Point(345, 51);
            this.lblBillDateDue.Name = "lblBillDateDue";
            this.lblBillDateDue.Size = new System.Drawing.Size(104, 13);
            this.lblBillDateDue.TabIndex = 19;
            this.lblBillDateDue.Text = "[Insert Bill Due Date]";
            // 
            // StatementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblBillDueDatelbl);
            this.Controls.Add(this.lblBillDateDue);
            this.Controls.Add(this.lblBillDateEnd);
            this.Controls.Add(this.lblBillCycleEndlbl);
            this.Controls.Add(this.lblBillAmountDetaillbl);
            this.Controls.Add(this.lblBillCycleStartlbl);
            this.Controls.Add(this.lblBillAmount);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.btnDeleteStatement);
            this.Controls.Add(this.lblBillDateStart);
            this.Controls.Add(this.lblAccountID);
            this.Controls.Add(this.cbAccIDs);
            this.Controls.Add(this.btnEditCreds);
            this.Controls.Add(this.btnRegenerate);
            this.Controls.Add(this.btnViewOS);
            this.Controls.Add(this.btnViewWindow);
            this.Controls.Add(this.lblDelete);
            this.Controls.Add(this.lblPrinter);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.btnDeleteFile);
            this.Name = "StatementControl";
            this.Size = new System.Drawing.Size(848, 97);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblBillDateStart;
        private System.Windows.Forms.Button btnViewWindow;
        private System.Windows.Forms.Button btnRegenerate;
        private System.Windows.Forms.Button btnDeleteStatement;
        private System.Windows.Forms.Button btnDeleteFile;
        private System.Windows.Forms.Label lblAccountID;
        private System.Windows.Forms.Label lblPreview;
        private System.Windows.Forms.Label lblPrinter;
        private System.Windows.Forms.Label lblDelete;
        private System.Windows.Forms.Button btnEditCreds;
        private System.Windows.Forms.Button btnViewOS;
        private System.Windows.Forms.ComboBox cbAccIDs;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblBillAmount;
        private System.Windows.Forms.Label lblBillCycleStartlbl;
        private System.Windows.Forms.Label lblBillAmountDetaillbl;
        private System.Windows.Forms.Label lblBillCycleEndlbl;
        private System.Windows.Forms.Label lblBillDateEnd;
        private System.Windows.Forms.Label lblBillDueDatelbl;
        private System.Windows.Forms.Label lblBillDateDue;
    }
}
