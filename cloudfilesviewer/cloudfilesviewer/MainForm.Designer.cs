namespace CloudFSViewer
{
    partial class MainForm
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.readOnlyContainerInformationGroup = new System.Windows.Forms.GroupBox();
            this.textBoxContainerInfo = new System.Windows.Forms.TextBox();
            this.readOnlyFileInformationGroup = new System.Windows.Forms.GroupBox();
            this.textBoxStorageObjectInformation = new System.Windows.Forms.TextBox();
            this.containerInformationGroup = new System.Windows.Forms.GroupBox();
            this.containerContentsGroup = new System.Windows.Forms.GroupBox();
            this.treeViewStorageObjects = new System.Windows.Forms.TreeView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getAccountInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createContainerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadFileAsStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteStorageObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteContainerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignMetaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.deleteAllContainersButton = new System.Windows.Forms.Button();
            this.cmObject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadAsStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblUploading = new System.Windows.Forms.Label();
            this.readOnlyContainerInformationGroup.SuspendLayout();
            this.readOnlyFileInformationGroup.SuspendLayout();
            this.containerInformationGroup.SuspendLayout();
            this.containerContentsGroup.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.cmObject.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.Location = new System.Drawing.Point(6, 19);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(169, 320);
            this.treeView1.TabIndex = 0;
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // readOnlyContainerInformationGroup
            // 
            this.readOnlyContainerInformationGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.readOnlyContainerInformationGroup.Controls.Add(this.textBoxContainerInfo);
            this.readOnlyContainerInformationGroup.Location = new System.Drawing.Point(376, 19);
            this.readOnlyContainerInformationGroup.Name = "readOnlyContainerInformationGroup";
            this.readOnlyContainerInformationGroup.Size = new System.Drawing.Size(175, 119);
            this.readOnlyContainerInformationGroup.TabIndex = 4;
            this.readOnlyContainerInformationGroup.TabStop = false;
            this.readOnlyContainerInformationGroup.Text = "Container Information";
            // 
            // textBoxContainerInfo
            // 
            this.textBoxContainerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContainerInfo.Location = new System.Drawing.Point(6, 19);
            this.textBoxContainerInfo.Multiline = true;
            this.textBoxContainerInfo.Name = "textBoxContainerInfo";
            this.textBoxContainerInfo.ReadOnly = true;
            this.textBoxContainerInfo.Size = new System.Drawing.Size(163, 94);
            this.textBoxContainerInfo.TabIndex = 0;
            // 
            // readOnlyFileInformationGroup
            // 
            this.readOnlyFileInformationGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.readOnlyFileInformationGroup.Controls.Add(this.textBoxStorageObjectInformation);
            this.readOnlyFileInformationGroup.Location = new System.Drawing.Point(382, 144);
            this.readOnlyFileInformationGroup.Name = "readOnlyFileInformationGroup";
            this.readOnlyFileInformationGroup.Size = new System.Drawing.Size(175, 220);
            this.readOnlyFileInformationGroup.TabIndex = 5;
            this.readOnlyFileInformationGroup.TabStop = false;
            this.readOnlyFileInformationGroup.Text = "File Information";
            // 
            // textBoxStorageObjectInformation
            // 
            this.textBoxStorageObjectInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStorageObjectInformation.Location = new System.Drawing.Point(6, 19);
            this.textBoxStorageObjectInformation.Multiline = true;
            this.textBoxStorageObjectInformation.Name = "textBoxStorageObjectInformation";
            this.textBoxStorageObjectInformation.ReadOnly = true;
            this.textBoxStorageObjectInformation.Size = new System.Drawing.Size(163, 195);
            this.textBoxStorageObjectInformation.TabIndex = 1;
            this.textBoxStorageObjectInformation.TextChanged += new System.EventHandler(this.textBoxStorageObjectInformation_TextChanged);
            // 
            // containerInformationGroup
            // 
            this.containerInformationGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.containerInformationGroup.Controls.Add(this.treeView1);
            this.containerInformationGroup.Location = new System.Drawing.Point(21, 19);
            this.containerInformationGroup.Name = "containerInformationGroup";
            this.containerInformationGroup.Size = new System.Drawing.Size(181, 345);
            this.containerInformationGroup.TabIndex = 6;
            this.containerInformationGroup.TabStop = false;
            this.containerInformationGroup.Text = "Container Information";
            // 
            // containerContentsGroup
            // 
            this.containerContentsGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.containerContentsGroup.Controls.Add(this.treeViewStorageObjects);
            this.containerContentsGroup.Location = new System.Drawing.Point(208, 19);
            this.containerContentsGroup.Name = "containerContentsGroup";
            this.containerContentsGroup.Size = new System.Drawing.Size(168, 345);
            this.containerContentsGroup.TabIndex = 7;
            this.containerContentsGroup.TabStop = false;
            this.containerContentsGroup.Text = "Container Contents";
            // 
            // treeViewStorageObjects
            // 
            this.treeViewStorageObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewStorageObjects.Location = new System.Drawing.Point(6, 19);
            this.treeViewStorageObjects.Name = "treeViewStorageObjects";
            this.treeViewStorageObjects.Size = new System.Drawing.Size(156, 320);
            this.treeViewStorageObjects.TabIndex = 0;
            this.treeViewStorageObjects.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewStorageObjects_MouseUp);
            this.treeViewStorageObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewStorageObjects_AfterSelect);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.operationsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(602, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // operationsToolStripMenuItem
            // 
            this.operationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.getAccountInformationToolStripMenuItem,
            this.createContainerToolStripMenuItem,
            this.uploadFileToolStripMenuItem,
            this.uploadFileAsStreamToolStripMenuItem,
            this.deleteStorageObjectToolStripMenuItem,
            this.deleteContainerToolStripMenuItem,
            this.downloadFileToolStripMenuItem,
            this.dowToolStripMenuItem,
            this.assignMetaToolStripMenuItem});
            this.operationsToolStripMenuItem.Name = "operationsToolStripMenuItem";
            this.operationsToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.operationsToolStripMenuItem.Text = "Operations";
            // 
            // getAccountInformationToolStripMenuItem
            // 
            this.getAccountInformationToolStripMenuItem.Name = "getAccountInformationToolStripMenuItem";
            this.getAccountInformationToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.getAccountInformationToolStripMenuItem.Text = "Get Account Information";
            this.getAccountInformationToolStripMenuItem.Click += new System.EventHandler(this.getAccountInformationToolStripMenuItem_Click);
            // 
            // createContainerToolStripMenuItem
            // 
            this.createContainerToolStripMenuItem.Name = "createContainerToolStripMenuItem";
            this.createContainerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.C)));
            this.createContainerToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.createContainerToolStripMenuItem.Text = "Create Container";
            this.createContainerToolStripMenuItem.Click += new System.EventHandler(this.createContainerToolStripMenuItem_Click);
            // 
            // uploadFileToolStripMenuItem
            // 
            this.uploadFileToolStripMenuItem.Enabled = false;
            this.uploadFileToolStripMenuItem.Name = "uploadFileToolStripMenuItem";
            this.uploadFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.U)));
            this.uploadFileToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.uploadFileToolStripMenuItem.Text = "Upload File";
            this.uploadFileToolStripMenuItem.Click += new System.EventHandler(this.uploadFileToolStripMenuItem_Click);
            // 
            // uploadFileAsStreamToolStripMenuItem
            // 
            this.uploadFileAsStreamToolStripMenuItem.Enabled = false;
            this.uploadFileAsStreamToolStripMenuItem.Name = "uploadFileAsStreamToolStripMenuItem";
            this.uploadFileAsStreamToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.S)));
            this.uploadFileAsStreamToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.uploadFileAsStreamToolStripMenuItem.Text = "Upload File as Stream";
            this.uploadFileAsStreamToolStripMenuItem.Click += new System.EventHandler(this.uploadFileAsStreamToolStripMenuItem_Click);
            // 
            // deleteStorageObjectToolStripMenuItem
            // 
            this.deleteStorageObjectToolStripMenuItem.Enabled = false;
            this.deleteStorageObjectToolStripMenuItem.Name = "deleteStorageObjectToolStripMenuItem";
            this.deleteStorageObjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.D)));
            this.deleteStorageObjectToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.deleteStorageObjectToolStripMenuItem.Text = "Delete Storage Object";
            this.deleteStorageObjectToolStripMenuItem.Click += new System.EventHandler(this.deleteStorageObjectToolStripMenuItem_Click);
            // 
            // deleteContainerToolStripMenuItem
            // 
            this.deleteContainerToolStripMenuItem.Enabled = false;
            this.deleteContainerToolStripMenuItem.Name = "deleteContainerToolStripMenuItem";
            this.deleteContainerToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.X)));
            this.deleteContainerToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.deleteContainerToolStripMenuItem.Text = "Delete Container";
            this.deleteContainerToolStripMenuItem.Click += new System.EventHandler(this.deleteContainerToolStripMenuItem_Click);
            // 
            // downloadFileToolStripMenuItem
            // 
            this.downloadFileToolStripMenuItem.Enabled = false;
            this.downloadFileToolStripMenuItem.Name = "downloadFileToolStripMenuItem";
            this.downloadFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.F)));
            this.downloadFileToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.downloadFileToolStripMenuItem.Text = "Download File";
            this.downloadFileToolStripMenuItem.Click += new System.EventHandler(this.downloadFileToolStripMenuItem_Click);
            // 
            // dowToolStripMenuItem
            // 
            this.dowToolStripMenuItem.Enabled = false;
            this.dowToolStripMenuItem.Name = "dowToolStripMenuItem";
            this.dowToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.T)));
            this.dowToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.dowToolStripMenuItem.Text = "Download File as Stream";
            this.dowToolStripMenuItem.Click += new System.EventHandler(this.dowToolStripMenuItem_Click);
            // 
            // assignMetaToolStripMenuItem
            // 
            this.assignMetaToolStripMenuItem.Enabled = false;
            this.assignMetaToolStripMenuItem.Name = "assignMetaToolStripMenuItem";
            this.assignMetaToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.M)));
            this.assignMetaToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.assignMetaToolStripMenuItem.Text = "Assign Meta";
            this.assignMetaToolStripMenuItem.Click += new System.EventHandler(this.assignMetaToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.containerInformationGroup);
            this.groupBox5.Controls.Add(this.containerContentsGroup);
            this.groupBox5.Controls.Add(this.readOnlyFileInformationGroup);
            this.groupBox5.Controls.Add(this.readOnlyContainerInformationGroup);
            this.groupBox5.Location = new System.Drawing.Point(12, 27);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(560, 368);
            this.groupBox5.TabIndex = 9;
            this.groupBox5.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 440);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(602, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel1.Text = "[Empty]";
            // 
            // deleteAllContainersButton
            // 
            this.deleteAllContainersButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteAllContainersButton.Enabled = false;
            this.deleteAllContainersButton.Location = new System.Drawing.Point(458, 401);
            this.deleteAllContainersButton.Name = "deleteAllContainersButton";
            this.deleteAllContainersButton.Size = new System.Drawing.Size(114, 23);
            this.deleteAllContainersButton.TabIndex = 11;
            this.deleteAllContainersButton.Text = "Delete All Containers";
            this.deleteAllContainersButton.UseVisualStyleBackColor = true;
            this.deleteAllContainersButton.Click += new System.EventHandler(this.deleteAllContainersButton_Click);
            // 
            // cmObject
            // 
            this.cmObject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem1,
            this.downloadToolStripMenuItem,
            this.downloadAsStreamToolStripMenuItem,
            this.assignMetadataToolStripMenuItem});
            this.cmObject.Name = "cmObject";
            this.cmObject.Size = new System.Drawing.Size(185, 92);
            // 
            // deleteToolStripMenuItem1
            // 
            this.deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            this.deleteToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.deleteToolStripMenuItem1.Text = "Delete";
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.downloadToolStripMenuItem.Text = "Download";
            // 
            // downloadAsStreamToolStripMenuItem
            // 
            this.downloadAsStreamToolStripMenuItem.Name = "downloadAsStreamToolStripMenuItem";
            this.downloadAsStreamToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.downloadAsStreamToolStripMenuItem.Text = "Download As Stream";
            // 
            // assignMetadataToolStripMenuItem
            // 
            this.assignMetadataToolStripMenuItem.Name = "assignMetadataToolStripMenuItem";
            this.assignMetadataToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.assignMetadataToolStripMenuItem.Text = "Assign Metadata";
            // 
            // lblUploading
            // 
            this.lblUploading.AutoSize = true;
            this.lblUploading.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploading.Location = new System.Drawing.Point(13, 401);
            this.lblUploading.Name = "lblUploading";
            this.lblUploading.Size = new System.Drawing.Size(403, 31);
            this.lblUploading.TabIndex = 13;
            this.lblUploading.Text = "Uploading File.  Please wait...";
            this.lblUploading.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 462);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.lblUploading);
            this.Controls.Add(this.deleteAllContainersButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "com.mosso.cloudfiles C# Example";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.readOnlyContainerInformationGroup.ResumeLayout(false);
            this.readOnlyContainerInformationGroup.PerformLayout();
            this.readOnlyFileInformationGroup.ResumeLayout(false);
            this.readOnlyFileInformationGroup.PerformLayout();
            this.containerInformationGroup.ResumeLayout(false);
            this.containerContentsGroup.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmObject.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox readOnlyContainerInformationGroup;
        private System.Windows.Forms.GroupBox readOnlyFileInformationGroup;
        private System.Windows.Forms.GroupBox containerInformationGroup;
        private System.Windows.Forms.TextBox textBoxContainerInfo;
        private System.Windows.Forms.GroupBox containerContentsGroup;
        private System.Windows.Forms.TreeView treeViewStorageObjects;
        private System.Windows.Forms.TextBox textBoxStorageObjectInformation;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteStorageObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteContainerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem createContainerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem assignMetaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadFileAsStreamToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button deleteAllContainersButton;
        private System.Windows.Forms.ToolStripMenuItem getAccountInformationToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmObject;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadAsStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assignMetadataToolStripMenuItem;
        private System.Windows.Forms.Label lblUploading;
    }
}

