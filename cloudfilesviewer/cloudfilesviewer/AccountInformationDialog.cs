using System;
using System.Windows.Forms;

namespace CloudFSViewer
{
    public partial class AccountInformationDialog : Form
    {
        private readonly string bytesUsed;
        private readonly string numContainers;

        public AccountInformationDialog(string bytesUsed, string numContainers)
        {
            this.bytesUsed = bytesUsed;
            this.numContainers = numContainers;

            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void AccountInformationDialog_Load(object sender, EventArgs e)
        {
            textBoxBytesUsed.Text = bytesUsed;
            textBoxNumContainers.Text = numContainers;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}