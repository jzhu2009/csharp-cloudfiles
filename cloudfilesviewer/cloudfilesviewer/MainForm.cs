using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.services;

namespace CloudFSViewer
{
    public partial class MainForm : Form
    {
        private string username, api_access_key;
        private Connection connection;
        private const string AUTH_URL = "https://api.mosso.com/auth";

        public MainForm()
        {
            InitializeComponent();
            ClearStatusBar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            username = "";
            api_access_key = "";
        }

        private void PopulateTree(List<string> folders)
        {
            containerInformationGroup.Text = "Container Information (" + folders.Count + ")";
            foreach (string s in folders)
            {
                TreeNode node = new TreeNode(s);
                treeView1.Nodes.Add(node);
            }
        }

        private void CheckAuthentication()
        {
            if (username.Length == 0 || api_access_key.Length == 0)
            {
                CredentialsDialog credentialsDialog = new CredentialsDialog();
                if (credentialsDialog.ShowDialog(this) == DialogResult.OK)
                {
                    username = credentialsDialog.Username;
                    api_access_key = credentialsDialog.ApiAccessKey;
                    deleteAllContainersButton.Enabled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearStatusBar();
            CheckAuthentication();

            try
            {
                connection = new Connection(new UserCredentials(username, api_access_key));
                RetrieveContainers();
            }
            catch
            {
                MessageBox.Show("Authentication failed");
                username = "";
            }
        }

        private void RetrieveContainers()
        {
            ClearStatusBar();
            treeView1.Nodes.Clear();
            List<string> containerList = connection.GetContainers();
            PopulateTree(containerList);
        }

        private void PopulateStorageObjectList(List<string> containerItems)
        {
            ClearStatusBar();
            treeViewStorageObjects.Nodes.Clear();

            containerContentsGroup.Text = "Container Contents (" + containerItems.Count + ")";
            foreach (string s in containerItems)
            {
                TreeNode storageItemNode = new TreeNode(s);
                treeViewStorageObjects.Nodes.Add(storageItemNode);
            }
        }

        private void RetrieveContainerItemList()
        {
            ClearStatusBar();
            CheckAuthentication();
            textBoxContainerInfo.Text = "";
            treeViewStorageObjects.Nodes.Clear();

            deleteStorageObjectToolStripMenuItem.Enabled = false;
            deleteContainerToolStripMenuItem.Enabled = false;
            uploadFileToolStripMenuItem.Enabled = false;
            uploadFileAsStreamToolStripMenuItem.Enabled = false;

            if (treeView1.SelectedNode != null)
            {
                try
                {
                    TreeNode selectedNode = treeView1.SelectedNode;

                    Container container =
                        connection.GetContainerInformation(selectedNode.Text);
                    if (container != null)
                    {
                        textBoxContainerInfo.Text =
                            container.Name + Environment.NewLine +
                            "Num objects:" + container.ObjectCount + Environment.NewLine +
                            "Size: " + container.ByteCount + Environment.NewLine;

                        RetrieveContainerInfo();

                        //Enable menu items
                        deleteContainerToolStripMenuItem.Enabled = true;
                        uploadFileToolStripMenuItem.Enabled = true;
                        uploadFileAsStreamToolStripMenuItem.Enabled = true;

                        SetSuccessfulMessageInStatusBar();
                    }
                }
                catch (ContainerNotFoundException notFoundException)
                {
                    MessageBox.Show(notFoundException.Message);
                }
            }
        }

        private void RetrieveContainerInfo()
        {
            ClearStatusBar();
            //Now get the container contents
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode != null)
            {
                List<string> containerList = connection.GetContainerItemList(selectedNode.Text);
                SetSuccessfulMessageInStatusBar();
                PopulateStorageObjectList(containerList);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearStatusBar();
            RetrieveContainerItemList();
        }

        private void RetrieveStorageObjectInfo()
        {
            ClearStatusBar();
            TreeNode selectedContainerNode = treeView1.SelectedNode;
            TreeNode selectedTreeNode = treeViewStorageObjects.SelectedNode;

            deleteStorageObjectToolStripMenuItem.Enabled = false;
            uploadFileToolStripMenuItem.Enabled = false;
            downloadFileToolStripMenuItem.Enabled = false;
            assignMetaToolStripMenuItem.Enabled = false;
            dowToolStripMenuItem.Enabled = false;


            textBoxStorageObjectInformation.Text = "";
            if (selectedContainerNode != null && selectedTreeNode != null)
            {
                StorageItem storageObject = connection.GetStorageItemInformation(selectedContainerNode.Text, selectedTreeNode.Text);
                if (storageObject != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append(
                        "Name:" + selectedTreeNode.Text + Environment.NewLine +
                        "Type:" + storageObject.ContentType + Environment.NewLine +
                        "Size:" + storageObject.FileLength + Environment.NewLine +
                        "Meta:" + Environment.NewLine);

                    foreach (string s in storageObject.MetaTags.Keys)
                    {
                        stringBuilder.Append("\t" + s + " -> " + storageObject.MetaTags[s]);
                    }

                    deleteStorageObjectToolStripMenuItem.Enabled = true;
                    uploadFileToolStripMenuItem.Enabled = true;
                    downloadFileToolStripMenuItem.Enabled = true;
                    assignMetaToolStripMenuItem.Enabled = true;
                    dowToolStripMenuItem.Enabled = true;


                    textBoxStorageObjectInformation.Text = stringBuilder.ToString();
                }
            }
        }

        private void treeViewStorageObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RetrieveStorageObjectInfo();
        }

        private void textBoxStorageObjectInformation_TextChanged(object sender, EventArgs e)
        {
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void deleteStorageObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStatusBar();
            TreeNode selectedContainerNode = treeView1.SelectedNode;
            TreeNode selectedTreeNode = treeViewStorageObjects.SelectedNode;

            if (selectedContainerNode != null && selectedTreeNode != null)
            {
                connection.DeleteStorageItem(selectedContainerNode.Text, selectedTreeNode.Text);
                SetSuccessfulMessageInStatusBar();
                //Now "refresh" the container
                RetrieveContainerInfo();
            }
        }

        private void deleteContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStatusBar();
            TreeNode selectedContainerNode = treeView1.SelectedNode;

            if (selectedContainerNode != null)
            {
                try
                {
                    connection.DeleteContainer(selectedContainerNode.Text);
                    SetSuccessfulMessageInStatusBar();
                    //"Refresh" the container list
                    RetrieveContainers();
                }
                catch
                {
                    if (MessageBox.Show("Container has storage objects inside of it.  Are you sure you want to delete it?", "Confirm Delete Container", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DeleteAllItemsFromContainer(selectedContainerNode.Text);
                        connection.DeleteContainer(selectedContainerNode.Text);
                        //"Refresh" the container list
                        RetrieveContainers();
                    }
                }
            }
        }

        private void DeleteAllItemsFromContainer(string containerName)
        {
            ClearStatusBar();
            List<string> storageObjects = connection.GetContainerItemList(containerName);
            foreach (string storageObjectName in storageObjects)
            {
                try
                {
                    connection.DeleteStorageItem(containerName, storageObjectName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void uploadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStatusBar();
            TreeNode selectedContainerNode = treeView1.SelectedNode;
            openFileDialog1.RestoreDirectory = true;
            if (selectedContainerNode != null && openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                connection.PutStorageItem(selectedContainerNode.Text, openFileDialog1.FileName);
                SetSuccessfulMessageInStatusBar();
                //Refresh the container
                RetrieveContainerItemList();
            }
        }

        private void createContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStatusBar();
            CheckAuthentication();
            ContainerNameEntryDialog containerNameDialog = new ContainerNameEntryDialog();
            if (containerNameDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    connection.CreateContainer(containerNameDialog.ContainerName);
                    SetSuccessfulMessageInStatusBar();
                }
                catch (ContainerAlreadyExistsException caee)
                {
                    MessageBox.Show(caee.ToString());
                }
                RetrieveContainers();
            }
        }

        private void downloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStatusBar();
            TreeNode selectedContainerNode = treeView1.SelectedNode;
            TreeNode selectedTreeNode = treeViewStorageObjects.SelectedNode;

            saveFileDialog1.RestoreDirectory = true;
            if (selectedContainerNode != null && selectedTreeNode != null)
            {
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    connection.GetStorageItem(selectedContainerNode.Text, selectedTreeNode.Text);
                    SetSuccessfulMessageInStatusBar();
                }
            }
        }

        private void assignMetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedContainerNode = treeView1.SelectedNode;
            TreeNode selectedTreeNode = treeViewStorageObjects.SelectedNode;

            if (selectedContainerNode != null && selectedTreeNode != null)
            {
                MetaInformationDialog metaInformationDialog = new MetaInformationDialog();
                if (metaInformationDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        connection.SetStorageItemMetaInformation(selectedContainerNode.Text,
                                                                 selectedTreeNode.Text,
                                                                 metaInformationDialog.MetaKeyCollection);
                        RetrieveStorageObjectInfo();
                        SetSuccessfulMessageInStatusBar();
                    }
                    catch (ContainerNotFoundException cnfe)
                    {
                        MessageBox.Show(cnfe.ToString());
                    }
                    catch (StorageItemNotFoundException sinfe)
                    {
                        MessageBox.Show(sinfe.ToString());
                    }
                }
            }
        }

        private void dowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedContainerNode = treeView1.SelectedNode;
            TreeNode selectedTreeNode = treeViewStorageObjects.SelectedNode;

            saveFileDialog1.RestoreDirectory = true;
            if (selectedContainerNode != null && selectedTreeNode != null)
            {
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    StorageItem storageObject =
                        connection.GetStorageItem(selectedContainerNode.Text, selectedTreeNode.Text);
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);

                    byte[] buffer = new byte[4096];
                    int amt = 0;
                    while ((amt = storageObject.ObjectStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        fs.Write(buffer, 0, amt);
                    }

                    //This is REQUIRED!
                    fs.Flush();
                    fs.Close();
                    storageObject.Dispose();
                }
            }
        }

        private void uploadFileAsStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedContainerNode = treeView1.SelectedNode;
            openFileDialog1.RestoreDirectory = true;
            if (selectedContainerNode != null && openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                FileStream fileStream = new FileStream(openFileDialog1.FileName, FileMode.Open);
                connection.PutStorageItem(selectedContainerNode.Text, fileStream,
                                          openFileDialog1.SafeFileName);

                //Refresh the container
                RetrieveContainerItemList();
            }
        }

        private void SetSuccessfulMessageInStatusBar()
        {
            statusStrip1.Items[0].Text = "Successful!";
        }

        private void SetMessageInStatusBar(string message)
        {
            statusStrip1.Items[0].Text = message;
        }

        private void ClearStatusBar()
        {
            statusStrip1.Items[0].Text = "";
        }

        private void deleteAllContainersButton_Click(object sender, EventArgs e)
        {
            ClearStatusBar();
            try
            {
                List<string> containers = connection.GetContainers();
                foreach (string container in containers)
                {
                    if (container == "aruby123") continue;
                    try
                    {
                        connection.DeleteContainer(container);
                        SetSuccessfulMessageInStatusBar();
                    }
                    catch (ContainerNotEmptyException notEmptyException)
                    {
                        Console.WriteLine(notEmptyException.Message);
                        DeleteAllItemsFromContainer(container);
                        connection.DeleteContainer(container);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                SetSuccessfulMessageInStatusBar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //"Refresh" the container list
                RetrieveContainers();
            }
        }

        private void getAccountInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountInformation accountInformation = connection.GetAccountInformation();
            if (accountInformation != null)
            {
                AccountInformationDialog accountInformationDialog = new AccountInformationDialog(accountInformation.BytesUsed.ToString(), accountInformation.ContainerCount.ToString());
                accountInformationDialog.ShowDialog(this);
            }
            SetSuccessfulMessageInStatusBar();
        }
    }
}