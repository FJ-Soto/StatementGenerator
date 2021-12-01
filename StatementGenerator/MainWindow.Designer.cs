namespace FTDStatementPrinter
{
    partial class MainWindow
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnCombinedOS = new System.Windows.Forms.Button();
            this.btnCombinedApp = new System.Windows.Forms.Button();
            this.btnGenerateCombined = new System.Windows.Forms.Button();
            this.gpFileSavingOpt = new System.Windows.Forms.GroupBox();
            this.btnChangeFileLocation = new System.Windows.Forms.Button();
            this.btnOpenFileExplorer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGenAll = new System.Windows.Forms.Button();
            this.btnAddStatement = new System.Windows.Forms.Button();
            this.ftdHeader = new FTDStatementPrinter.StatementControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gpFileSavingOpt.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.gpFileSavingOpt);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.ftdHeader);
            this.splitContainer1.Panel1MinSize = this.splitContainer1.Panel1.Height;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Margin = new System.Windows.Forms.Padding(10);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.splitContainer1.Size = new System.Drawing.Size(884, 691);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnCombinedOS);
            this.groupBox3.Controls.Add(this.btnCombinedApp);
            this.groupBox3.Controls.Add(this.btnGenerateCombined);
            this.groupBox3.Font = new System.Drawing.Font("Myanmar Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(603, 14);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(268, 100);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Combined Statement Options";
            // 
            // btnCombinedOS
            // 
            this.btnCombinedOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCombinedOS.Location = new System.Drawing.Point(137, 61);
            this.btnCombinedOS.Name = "btnCombinedOS";
            this.btnCombinedOS.Size = new System.Drawing.Size(125, 24);
            this.btnCombinedOS.TabIndex = 11;
            this.btnCombinedOS.Text = "Open in OS";
            this.btnCombinedOS.UseVisualStyleBackColor = true;
            this.btnCombinedOS.Click += new System.EventHandler(this.btnOpenCombined_Click);
            // 
            // btnCombinedApp
            // 
            this.btnCombinedApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCombinedApp.Location = new System.Drawing.Point(6, 61);
            this.btnCombinedApp.Name = "btnCombinedApp";
            this.btnCombinedApp.Size = new System.Drawing.Size(125, 24);
            this.btnCombinedApp.TabIndex = 10;
            this.btnCombinedApp.Text = "Open in App";
            this.btnCombinedApp.UseVisualStyleBackColor = true;
            this.btnCombinedApp.Click += new System.EventHandler(this.btnOpenCombined_Click);
            // 
            // btnGenerateCombined
            // 
            this.btnGenerateCombined.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateCombined.Location = new System.Drawing.Point(6, 31);
            this.btnGenerateCombined.Name = "btnGenerateCombined";
            this.btnGenerateCombined.Size = new System.Drawing.Size(256, 24);
            this.btnGenerateCombined.TabIndex = 9;
            this.btnGenerateCombined.Text = "Combine Generated PDFs";
            this.btnGenerateCombined.UseVisualStyleBackColor = true;
            this.btnGenerateCombined.Click += new System.EventHandler(this.btnCombine_Click);
            // 
            // gpFileSavingOpt
            // 
            this.gpFileSavingOpt.Controls.Add(this.btnChangeFileLocation);
            this.gpFileSavingOpt.Controls.Add(this.btnOpenFileExplorer);
            this.gpFileSavingOpt.Font = new System.Drawing.Font("Myanmar Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpFileSavingOpt.Location = new System.Drawing.Point(204, 13);
            this.gpFileSavingOpt.Name = "gpFileSavingOpt";
            this.gpFileSavingOpt.Size = new System.Drawing.Size(193, 100);
            this.gpFileSavingOpt.TabIndex = 10;
            this.gpFileSavingOpt.TabStop = false;
            this.gpFileSavingOpt.Text = "File Options";
            // 
            // btnChangeFileLocation
            // 
            this.btnChangeFileLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeFileLocation.Location = new System.Drawing.Point(6, 61);
            this.btnChangeFileLocation.Name = "btnChangeFileLocation";
            this.btnChangeFileLocation.Size = new System.Drawing.Size(177, 24);
            this.btnChangeFileLocation.TabIndex = 10;
            this.btnChangeFileLocation.Text = "Change Saving Location";
            this.btnChangeFileLocation.UseVisualStyleBackColor = true;
            this.btnChangeFileLocation.Click += new System.EventHandler(this.btnChangeFileLocation_Click);
            // 
            // btnOpenFileExplorer
            // 
            this.btnOpenFileExplorer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFileExplorer.Location = new System.Drawing.Point(6, 31);
            this.btnOpenFileExplorer.Name = "btnOpenFileExplorer";
            this.btnOpenFileExplorer.Size = new System.Drawing.Size(177, 24);
            this.btnOpenFileExplorer.TabIndex = 9;
            this.btnOpenFileExplorer.Text = "Open Statement Folder";
            this.btnOpenFileExplorer.UseVisualStyleBackColor = true;
            this.btnOpenFileExplorer.Click += new System.EventHandler(this.btnOpenFileExplorer_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGenAll);
            this.groupBox1.Controls.Add(this.btnAddStatement);
            this.groupBox1.Font = new System.Drawing.Font("Myanmar Text", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 101);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Statement Options";
            // 
            // btnGenAll
            // 
            this.btnGenAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenAll.Location = new System.Drawing.Point(6, 62);
            this.btnGenAll.Name = "btnGenAll";
            this.btnGenAll.Size = new System.Drawing.Size(163, 24);
            this.btnGenAll.TabIndex = 1;
            this.btnGenAll.Text = "Generate All Statements";
            this.btnGenAll.UseVisualStyleBackColor = true;
            this.btnGenAll.Click += new System.EventHandler(this.btnGenAll_Click);
            // 
            // btnAddStatement
            // 
            this.btnAddStatement.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddStatement.Location = new System.Drawing.Point(6, 32);
            this.btnAddStatement.Name = "btnAddStatement";
            this.btnAddStatement.Size = new System.Drawing.Size(163, 24);
            this.btnAddStatement.TabIndex = 0;
            this.btnAddStatement.Text = "Add Statement";
            this.btnAddStatement.UseVisualStyleBackColor = true;
            this.btnAddStatement.Click += new System.EventHandler(this.btnAddStatement_Click);
            // 
            // ftdHeader
            // 
            this.ftdHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ftdHeader.Credentials = null;
            this.ftdHeader.Location = new System.Drawing.Point(10, 120);
            this.ftdHeader.Master = null;
            this.ftdHeader.Name = "ftdHeader";
            this.ftdHeader.Size = new System.Drawing.Size(828, 34);
            this.ftdHeader.Statements = null;
            this.ftdHeader.TabIndex = 8;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 691);
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(780, 39);
            this.Name = "MainWindow";
            this.Text = "Statement Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.gpFileSavingOpt.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private StatementControl ftdHeader;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGenAll;
        private System.Windows.Forms.Button btnAddStatement;
        private System.Windows.Forms.GroupBox gpFileSavingOpt;
        private System.Windows.Forms.Button btnOpenFileExplorer;
        private System.Windows.Forms.Button btnChangeFileLocation;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnGenerateCombined;
        private System.Windows.Forms.Button btnCombinedApp;
        private System.Windows.Forms.Button btnCombinedOS;
    }
}

