using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CloudFSViewer;
using com.mosso.cloudfiles;

namespace cloudfilesviewer
{
    public partial class ProgressDialog : Form
    {
        private float totalTransferred;
        private delegate void CloseCallback();
        private delegate void FileProgressCallback(int bytesTransferred);

        private long filesize; 
        public ProgressDialog()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void StartFileTransfer(Form owner, Connection connection, string container, string filePath)
        {
            totalTransferred = 0f;
            Text = "Uploading File";
            Cursor = Cursors.WaitCursor;
            var fi = new FileInfo(filePath);
            filesize = fi.Length;
            totalBytesLabel.Text = filesize.ToString();

            connection.AddProgressWatcher(fileTransferProgress);
            connection.OperationComplete += transferComplete;

            connection.PutStorageItemAsync(container, filePath);
            //It's absolutely critical that ShowDialog is called over Show. ShowDialog sets the dialog to modal, blocking input to any
            //other form in the application until the operation is complete. Otherwise, a deadlock occurs if you click the main form,
            //hanging the entire application
            ShowDialog(owner);      
        }

        private void transferComplete()
        {
            if (InvokeRequired)
            {
                Invoke(new CloseCallback(Close), new object[]{});
            }
            else
            {
                if (!IsDisposed)
                    Close();
            }
        }

        private void fileTransferProgress(int bytesTransferred)
        {
            if (InvokeRequired)
            {
                Invoke(new FileProgressCallback(fileTransferProgress), new object[] {bytesTransferred});
            }
            else
            {
                System.Console.WriteLine(totalTransferred.ToString());
                totalTransferred += bytesTransferred;
                bytesTransferredLabel.Text = totalTransferred.ToString();
                var progress = (int) ((totalTransferred/filesize)*100.0f);
                if(progress > 100)
                    progress = 100;
                transferProgressBar.Value = progress ;
            }
        }

        public void StartFileDownload(MainForm owner, Connection connection, long filesize, string container, string storageItemName, string localFileName)
        {
            Text = "Downloading File";
            Cursor = Cursors.WaitCursor;
            this.filesize = filesize;

            totalBytesLabel.Text = filesize.ToString();

            connection.AddProgressWatcher(fileTransferProgress);
            connection.OperationComplete += transferComplete;

            connection.GetStorageItemAsync(container, storageItemName, localFileName);

            //It's absolutely critical that ShowDialog is called over Show. ShowDialog sets the dialog to modal, blocking input to any
            //other form in the application until the operation is complete. Otherwise, a deadlock occurs if you click the main form,
            //hanging the entire application
            ShowDialog(owner);
        }
    }
}
