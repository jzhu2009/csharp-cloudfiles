namespace cloudfilesviewer
{
    partial class ProgressDialog
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
            this.transferProgressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.bytesTransferredLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.totalBytesLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // transferProgressBar
            // 
            this.transferProgressBar.Location = new System.Drawing.Point(12, 32);
            this.transferProgressBar.Name = "transferProgressBar";
            this.transferProgressBar.Size = new System.Drawing.Size(234, 23);
            this.transferProgressBar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Progress";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // bytesTransferredLabel
            // 
            this.bytesTransferredLabel.AutoSize = true;
            this.bytesTransferredLabel.Location = new System.Drawing.Point(12, 58);
            this.bytesTransferredLabel.Name = "bytesTransferredLabel";
            this.bytesTransferredLabel.Size = new System.Drawing.Size(13, 13);
            this.bytesTransferredLabel.TabIndex = 2;
            this.bytesTransferredLabel.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "/";
            // 
            // totalBytesLabel
            // 
            this.totalBytesLabel.AutoSize = true;
            this.totalBytesLabel.Location = new System.Drawing.Point(92, 58);
            this.totalBytesLabel.Name = "totalBytesLabel";
            this.totalBytesLabel.Size = new System.Drawing.Size(13, 13);
            this.totalBytesLabel.TabIndex = 4;
            this.totalBytesLabel.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(156, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Bytes Transferred";
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 84);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.totalBytesLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bytesTransferredLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.transferProgressBar);
            this.Name = "ProgressDialog";
            this.Text = "ProgressDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar transferProgressBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label bytesTransferredLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label totalBytesLabel;
        private System.Windows.Forms.Label label5;
    }
}