using System;
using System.Windows.Forms;

namespace CloudFSViewer
{
    public partial class CredentialsDialog : Form
    {
        private string username;
        private string password;
        private string account;
        private string authurl;

        public CredentialsDialog()
        {
            InitializeComponent();
            textBoxUsername.Focus();
        }

        public string Authurl
        {
            get { return authurl; }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBoxAuthUrl.Select();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            authurl = textBoxAuthUrl.Text;
            username = textBoxUsername.Text;
            password = textBoxPassword.Text;
            account = textBoxAccount.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        public string Username
        {
            get { return username; }
        }

        public string Password
        {
            get { return password; }
        }

        public string Account
        {
            get { return account; }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }
    }
}