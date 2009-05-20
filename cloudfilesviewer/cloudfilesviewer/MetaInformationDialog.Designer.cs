namespace CloudFSViewer
{
    partial class MetaInformationDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxValue = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxKVP = new System.Windows.Forms.ListBox();
            this.buttonAddPair = new System.Windows.Forms.Button();
            this.buttonDeletePair = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonAddPair);
            this.groupBox1.Controls.Add(this.textBoxValue);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxKey);
            this.groupBox1.Location = new System.Drawing.Point(8, 166);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 111);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Meta Key/Value";
            // 
            // textBoxKey
            // 
            this.textBoxKey.Location = new System.Drawing.Point(94, 19);
            this.textBoxKey.Name = "textBoxKey";
            this.textBoxKey.Size = new System.Drawing.Size(100, 20);
            this.textBoxKey.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Meta Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Meta Value";
            // 
            // textBoxValue
            // 
            this.textBoxValue.Location = new System.Drawing.Point(94, 48);
            this.textBoxValue.Name = "textBoxValue";
            this.textBoxValue.Size = new System.Drawing.Size(100, 20);
            this.textBoxValue.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonDeletePair);
            this.groupBox2.Controls.Add(this.listBoxKVP);
            this.groupBox2.Location = new System.Drawing.Point(8, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 148);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Keys / Values";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // listBoxKVP
            // 
            this.listBoxKVP.FormattingEnabled = true;
            this.listBoxKVP.Location = new System.Drawing.Point(9, 19);
            this.listBoxKVP.Name = "listBoxKVP";
            this.listBoxKVP.Size = new System.Drawing.Size(185, 95);
            this.listBoxKVP.TabIndex = 0;
            this.listBoxKVP.SelectedIndexChanged += new System.EventHandler(this.listBoxKVP_SelectedIndexChanged);
            // 
            // buttonAddPair
            // 
            this.buttonAddPair.Location = new System.Drawing.Point(119, 74);
            this.buttonAddPair.Name = "buttonAddPair";
            this.buttonAddPair.Size = new System.Drawing.Size(75, 23);
            this.buttonAddPair.TabIndex = 4;
            this.buttonAddPair.Text = "Add Pair";
            this.buttonAddPair.UseVisualStyleBackColor = true;
            this.buttonAddPair.Click += new System.EventHandler(this.buttonAddPair_Click);
            // 
            // buttonDeletePair
            // 
            this.buttonDeletePair.Enabled = false;
            this.buttonDeletePair.Location = new System.Drawing.Point(119, 120);
            this.buttonDeletePair.Name = "buttonDeletePair";
            this.buttonDeletePair.Size = new System.Drawing.Size(75, 23);
            this.buttonDeletePair.TabIndex = 5;
            this.buttonDeletePair.Text = "Delete Pair";
            this.buttonDeletePair.UseVisualStyleBackColor = true;
            this.buttonDeletePair.Click += new System.EventHandler(this.buttonDeletePair_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(133, 283);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(52, 283);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // MetaInformationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 312);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MetaInformationDialog";
            this.Text = "Set Meta Information";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxKey;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBoxKVP;
        private System.Windows.Forms.Button buttonAddPair;
        private System.Windows.Forms.Button buttonDeletePair;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
    }
}