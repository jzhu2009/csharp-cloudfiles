using System;
using System.Windows.Forms;

namespace CloudFSViewer
{
    public partial class CredentialsDialog : Form
    {
        private string username;
        private string api_access_key;

        public CredentialsDialog()
        {
            InitializeComponent();
            textBoxUsername.Focus();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBoxUsername.Select();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            username = textBoxUsername.Text;
            api_access_key = textBoxApiAccessKey.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        public string Username
        {
            get { return username; }
        }

        public string ApiAccessKey
        {
            get { return api_access_key; }
        }
    }
}