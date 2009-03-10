///
/// See COPYING file for licensing information
///

using System.IO;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// This class wraps the response from the get storage item request
    /// </summary>
    public class GetStorageItemResponse : CloudFilesResponseWithContentBody
    {
        private Stream contentStream;
        
        /// <summary>
        /// A property representing the stream returned from the response
        /// </summary>
        public override Stream ContentStream
        {
            get { return contentStream; }
            set { contentStream = value; }
        }

        /// <summary>
        /// This method saves the stream from the response directly to a named file on disk
        /// </summary>
        /// <param name="filename">The file name to save the stream to locally</param>
        public void SaveStreamToDisk(string filename)
        {
            StoreFile(filename);
        }
     
        private void StoreFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);

            byte[] buffer = new byte[4096];

            var amt = 0;
            while ((amt = contentStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, amt);
            }
            fs.Close();
            contentStream.Close();
        }
    }
}