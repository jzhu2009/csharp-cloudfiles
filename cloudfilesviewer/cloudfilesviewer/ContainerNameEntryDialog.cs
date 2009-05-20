using System;
using System.Windows.Forms;

namespace CloudFSViewer
{
    public partial class ContainerNameEntryDialog : Form
    {
        private string containerName;

        public ContainerNameEntryDialog()
        {
            InitializeComponent();
        }

        public string ContainerName
        {
            get { return containerName; }
        }

        private void textBoxContainerName_TextChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = (textBoxContainerName.Text.Length > 0);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ContainerNameEntryDialog_Load(object sender, EventArgs e)
        {
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            containerName = textBoxContainerName.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}