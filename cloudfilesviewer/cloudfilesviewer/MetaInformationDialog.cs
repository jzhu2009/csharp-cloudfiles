using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CloudFSViewer
{
    public partial class MetaInformationDialog : Form
    {
        private readonly Dictionary<string, string> metaKeyCollection;

        public MetaInformationDialog()
        {
            InitializeComponent();
            metaKeyCollection = new Dictionary<string, string>();
        }

        public Dictionary<string, string> MetaKeyCollection
        {
            get { return metaKeyCollection; }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void listBoxKVP_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDeletePair.Enabled = false;
            if (listBoxKVP.SelectedIndex != -1)
            {
                buttonDeletePair.Enabled = true;
            }
        }

        private void buttonAddPair_Click(object sender, EventArgs e)
        {
            if (textBoxKey.Text.Length > 0 && textBoxValue.Text.Length > 0)
            {
                listBoxKVP.Items.Add(textBoxKey.Text + " -> " + textBoxValue.Text);
                metaKeyCollection.Add(textBoxKey.Text, textBoxValue.Text);
                if (listBoxKVP.Items.Count > 0)
                    buttonOk.Enabled = true;
                else buttonOk.Enabled = false;
            }
        }

        private void buttonDeletePair_Click(object sender, EventArgs e)
        {
            if (listBoxKVP.SelectedIndex != -1)
            {
                string key = metaKeyCollection[listBoxKVP.SelectedIndex.ToString()];
                metaKeyCollection.Remove(key);

                listBoxKVP.Items.RemoveAt(listBoxKVP.SelectedIndex);
                listBoxKVP.SelectedIndex = -1;

                if (listBoxKVP.Items.Count == 0)
                    buttonOk.Enabled = false;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (listBoxKVP.Items.Count > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}