using System;
using System.Collections.Generic;
using System.Drawing;
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

        public MainForm()
        {
            InitializeComponent();
            ClearStatusBar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetUsernameAndApiKey();
            ClearStatusBar();
            CheckAuthentication();
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
                System.Windows.Forms.DialogResult dialogResult = credentialsDialog.ShowDialog(this);

                if (dialogResult != DialogResult.OK) return;

                if (dialogResult == DialogResult.OK)
                {
                    username = credentialsDialog.Username;
                    api_access_key = credentialsDialog.ApiAccessKey;
                    deleteAllContainersButton.Enabled = true;
                }

                try
                {
                    Connection = new Connection(new UserCredentials(username, api_access_key));
                    RetrieveContainers();
                }
                catch
                {
                    MessageBox.Show("Authentication failed");
                    Form1_Load(this, new EventArgs());
                }
            }
        }

        private void ResetUsernameAndApiKey()
        {
            username = "";
            api_access_key = "";
        }

        private void RetrieveContainers()
        {
            ClearStatusBar();
            treeView1.Nodes.Clear();
            treeViewStorageObjects.Nodes.Clear();
            List<string> containerList = Connection.GetContainers();
            if (containerList != null && containerList.Count > 0)
            {
                PopulateTree(containerList);
            }
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

                    PopulateContainerInfo(selectedNode.Text);

                    RetrieveContainerInfo();

                    //Enable menu items
                    deleteContainerToolStripMenuItem.Enabled = true;
                    uploadFileToolStripMenuItem.Enabled = true;
                    uploadFileAsStreamToolStripMenuItem.Enabled = true;

                    SetSuccessfulMessageInStatusBar();
                }
                catch (ContainerNotFoundException notFoundException)
                {
                    MessageBox.Show(notFoundException.Message);
                }
            }
        }

        private void PopulateContainerInfo(string containerName)
        {
            Container container = Connection.GetContainerInformation(containerName); 
            if (container != null)
            {
                textBoxContainerInfo.Text =
                    container.Name + Environment.NewLine +
                    "Num objects:" + container.ObjectCount + Environment.NewLine +
                    "Size: " + container.ByteCount + Environment.NewLine;
            }
        }

        private void RetrieveContainerInfo()
        {
            ClearStatusBar();
            //Now get the container contents
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode != null)
            {
                List<string> containerList = Connection.GetContainerItemList(selectedNode.Text);
                SetSuccessfulMessageInStatusBar();
                PopulateStorageObjectList(containerList);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ClearStatusBar();
            RetrieveContainerItemList();
            textBoxStorageObjectInformation.Text = "";
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
                PopulateObjectInfo(selectedContainerNode.Text, selectedTreeNode.Text);
            }
        }


        private void PopulateObjectInfo(string containerName, string objectName)
        {
            StorageItem storageObject = Connection.GetStorageItemInformation(containerName, objectName);
            if (storageObject != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(
                    "Name:" + objectName + Environment.NewLine +
                    "Type:" + storageObject.ContentType + Environment.NewLine +
                    "Size:" + storageObject.FileLength + Environment.NewLine +
                    "Meta:" + Environment.NewLine);

                foreach (string s in storageObject.Metadata.Keys)
                {
                    stringBuilder.Append("\t" + s + " -> " + storageObject.Metadata[s]);
                }

                deleteStorageObjectToolStripMenuItem.Enabled = true;
                uploadFileToolStripMenuItem.Enabled = true;
                downloadFileToolStripMenuItem.Enabled = true;
                assignMetaToolStripMenuItem.Enabled = true;
                dowToolStripMenuItem.Enabled = true;


                textBoxStorageObjectInformation.Text = stringBuilder.ToString();
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
                Connection.DeleteStorageItem(selectedContainerNode.Text, selectedTreeNode.Text);
                PopulateContainerInfo(selectedContainerNode.Text);
                textBoxStorageObjectInformation.Text = "";
                SetSuccessfulMessageInStatusBar();
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
                    Connection.DeleteContainer(selectedContainerNode.Text);
                    SetSuccessfulMessageInStatusBar();
                    textBoxContainerInfo.Text = "";
                    //"Refresh" the container list
                    RetrieveContainers();
                }
                catch
                {
                    if (MessageBox.Show("Container has storage objects inside of it.  Are you sure you want to delete it?", "Confirm Delete Container", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DeleteAllItemsFromContainer(selectedContainerNode.Text);
                        Connection.DeleteContainer(selectedContainerNode.Text);
                        //"Refresh" the container list
                        RetrieveContainers();
                    }
                }
            }
        }

        private void DeleteAllItemsFromContainer(string containerName)
        {
            ClearStatusBar();
            List<string> storageObjects = Connection.GetContainerItemList(containerName);
            foreach (string storageObjectName in storageObjects)
            {
                try
                {
                    Connection.DeleteStorageItem(containerName, storageObjectName);
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
                EnableControls = false;
                HandleControlsAndShowUploadingFileInfo();
                Connection.PutStorageItem(selectedContainerNode.Text, openFileDialog1.FileName);
                EnableControls = true;
                HandleControlsAndShowUploadingFileInfo();
                SetSuccessfulMessageInStatusBar();
                //Refresh the container
                RetrieveContainerItemList();
            }
        }

        private void HandleControlsAndShowUploadingFileInfo()
        {
            foreach(Control control in Controls)
            {
                control.Enabled = EnableControls;
            }

            var imgUploadingControl = Controls["imgUploading"];
            var lblUploadingControl = Controls["lblUploading"];

            imgUploadingControl.Enabled = imgUploadingControl.Visible = !EnableControls;
            lblUploadingControl.Enabled = lblUploadingControl.Visible = !EnableControls;
        }


        public bool EnableControls { get; private set; }
        public Connection Connection { get; private set; }

        private void createContainerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStatusBar();
            CheckAuthentication();
            ContainerNameEntryDialog containerNameDialog = new ContainerNameEntryDialog();
            if (containerNameDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    Connection.CreateContainer(containerNameDialog.ContainerName);
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
                    Connection.GetStorageItem(selectedContainerNode.Text, selectedTreeNode.Text);
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
                        Connection.SetStorageItemMetaInformation(selectedContainerNode.Text,
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
            var selectedContainerNode = treeView1.SelectedNode;
            var selectedTreeNode = treeViewStorageObjects.SelectedNode;

            saveFileDialog1.RestoreDirectory = true;
            if (selectedContainerNode == null || selectedTreeNode == null) return;
            if (saveFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            
            StorageItem storageObject =
                Connection.GetStorageItem(selectedContainerNode.Text, selectedTreeNode.Text);
            FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);

            byte[] buffer = new byte[4096];
            int amt;
            while ((amt = storageObject.ObjectStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, amt);
            }

            //This is REQUIRED!
            fs.Flush();
            fs.Close();
            storageObject.Dispose();
        }

        private void uploadFileAsStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedContainerNode = treeView1.SelectedNode;
            openFileDialog1.RestoreDirectory = true;
            if (selectedContainerNode == null || openFileDialog1.ShowDialog(this) != DialogResult.OK) return;
            
            FileStream fileStream = new FileStream(openFileDialog1.FileName, FileMode.Open);
            EnableControls = false;
            HandleControlsAndShowUploadingFileInfo();
            Connection.PutStorageItem(selectedContainerNode.Text, fileStream,openFileDialog1.SafeFileName);
            EnableControls = true;
            HandleControlsAndShowUploadingFileInfo();

            //Refresh the container
            RetrieveContainerItemList();
        }

        private void SetSuccessfulMessageInStatusBar()
        {
            statusStrip1.Items[0].Text = "Successful!";
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
                List<string> containers = Connection.GetContainers();
                foreach (string container in containers)
                {
                    if (container == "aruby123") continue;
                    try
                    {
                        Connection.DeleteContainer(container);
                        SetSuccessfulMessageInStatusBar();
                    }
                    catch (ContainerNotEmptyException notEmptyException)
                    {
                        Console.WriteLine(notEmptyException.Message);
                        DeleteAllItemsFromContainer(container);
                        Connection.DeleteContainer(container);
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
            AccountInformation accountInformation = Connection.GetAccountInformation();
            if (accountInformation != null)
            {
                AccountInformationDialog accountInformationDialog = new AccountInformationDialog(accountInformation.BytesUsed.ToString(), accountInformation.ContainerCount.ToString());
                accountInformationDialog.ShowDialog(this);
            }
            SetSuccessfulMessageInStatusBar();
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
                if (node == null)
                {
                    MenuItem[] items = new MenuItem[2];
                    items[0] = new MenuItem("Get Information");
                    items[0].Click += getAccountInformationToolStripMenuItem_Click;
                    items[1] = new MenuItem("Create Container");
                    items[1].Click += createContainerToolStripMenuItem_Click;

                    ContextMenu cmMenu = new ContextMenu(items);
                    cmMenu.Show(this, new Point(e.X, e.Y));
                }
                else
                {
                    MenuItem[] items = new MenuItem[4];
                    items[0] = new MenuItem("Get Information");
                    items[0].Click += getAccountInformationToolStripMenuItem_Click;
                    items[1] = new MenuItem("Upload Local File");
                    items[1].Click += uploadFileToolStripMenuItem_Click;
                    items[2] = new MenuItem("Upload File Stream");
                    items[2].Click += uploadFileAsStreamToolStripMenuItem_Click;
                    items[3] = new MenuItem("Delete");
                    items[3].Click += deleteContainerToolStripMenuItem_Click;

                    ContextMenu cmMenu = new ContextMenu(items);
                    cmMenu.Show(this, new Point(e.X, e.Y));
                }
            }
        }

        private void treeViewStorageObjects_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
                if (node == null) return;

                MenuItem[] items = new MenuItem[4];
                items[0] = new MenuItem("Delete");
                items[0].Click += deleteStorageObjectToolStripMenuItem_Click;
                items[1] = new MenuItem("Download");
                items[1].Click += downloadFileToolStripMenuItem_Click;
                items[2] = new MenuItem("Download As Stream");
                items[2].Click += dowToolStripMenuItem_Click;
                items[3] = new MenuItem("Assign Metadata");
                items[3].Click += assignMetaToolStripMenuItem_Click;

                ContextMenu cmMenu = new ContextMenu(items);
                cmMenu.Show(this, new Point(e.X, e.Y));
            }
        }
    }
}