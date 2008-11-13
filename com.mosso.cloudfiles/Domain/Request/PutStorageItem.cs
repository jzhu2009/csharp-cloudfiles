///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// PutStorageItem
    /// </summary>
    public class PutStorageItem : IRequestWithContentBody
    {
        private string fileUri;
        private Stream stream;
        private const int MAXIMUM_FILE_NAME_LENGTH = 128;
        private const string MIME_TYPES_XML_FILE = "mime-types.xml";

        /// <summary>
        /// PutStorageItem constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <param name="fileUri">the path of the file to put into cloudfiles</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        public PutStorageItem(string storageUrl, string containerName, string storageItemName, string fileUri,
                              string storageToken)
            : this(storageUrl, containerName, storageItemName, fileUri, storageToken, null)
        {
        }

        /// <summary>
        /// PutStorageItem constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <param name="filestream">the fiel stream of the file to put into cloudfiles</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        public PutStorageItem(string storageUrl, string containerName, string storageItemName, Stream filestream,
                              string storageToken)
            : this(storageUrl, containerName, storageItemName, filestream, storageToken, null)
        {
        }

        /// <summary>
        /// PutStorageItem constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <param name="stream">the fiel stream of the file to put into cloudfiles</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <param name="metaTags">dictionary of meta tags to apply to the storage item</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameLengthException">Thrown when the container name length exceeds the maximum container length allowed</exception>
        public PutStorageItem(string storageUrl, string containerName, string storageItemName, Stream stream,
                              string storageToken, Dictionary<string, string> metaTags)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken)
                || string.IsNullOrEmpty(containerName)
                || stream == null
                || string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();


            if (containerName.Length > Constants.MAXIMUM_CONTAINER_NAME_LENGTH)
                throw new ContainerNameLengthException("The container name length exceeds " + Constants.MAXIMUM_CONTAINER_NAME_LENGTH + " characters.s");


            this.stream = stream;
            Headers = new NameValueCollection();
            ContentLength = this.stream.Length;

            ETag = StringifyMD5(new MD5CryptoServiceProvider().ComputeHash(this.stream));

            this.stream.Seek(0, 0);

            if (metaTags != null)
            {
                foreach (string s in metaTags.Keys)
                {
                    Headers.Add(Constants.META_DATA_HEADER + s, metaTags[s]);
                }
            }

            if (stream.Position == stream.Length)
                stream.Seek(0, 0);

            Headers.Add(Constants.X_STORAGE_TOKEN, HttpUtility.UrlEncode(storageToken));
            Uri =
                new Uri(storageUrl + "/" + HttpUtility.UrlEncode(containerName).Replace("+", "%20") + "/" +
                        HttpUtility.UrlEncode(storageItemName).Replace("+", "%20"));
            Method = "PUT";
        }

        /// <summary>
        /// PutStorageItem constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <param name="fileUri">the path of the file to put into cloudfiles</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <param name="metaTags">dictionary of meta tags to apply to the storage item</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameLengthException">Thrown when the container name length exceeds the maximum container length allowed</exception>
        /// <exception cref="StorageItemNameLengthException">Thrown when the storage name length exceeds the maximum storage object name length allowed</exception>
        public PutStorageItem(string storageUrl, string containerName, string storageItemName, string fileUri,
                              string storageToken, Dictionary<string, string> metaTags)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken)
                || string.IsNullOrEmpty(containerName)
                || string.IsNullOrEmpty(fileUri)
                || string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();


            if (containerName.Length > Constants.MAXIMUM_CONTAINER_NAME_LENGTH)
                throw new ContainerNameLengthException("The container name length exceeds " + Constants.MAXIMUM_CONTAINER_NAME_LENGTH + " characters.s");


            this.fileUri = CleanUpFileUri(fileUri);

            if (storageItemName.Length > MAXIMUM_FILE_NAME_LENGTH)
                throw new StorageItemNameLengthException("File: " + this.fileUri + " exceeds maximum file length of " +
                                                         MAXIMUM_FILE_NAME_LENGTH);
            Headers = new NameValueCollection();

            
            using (FileStream file = new FileStream(this.fileUri, FileMode.Open))
            {
                ContentLength = file.Length;
                ETag = StringifyMD5(new MD5CryptoServiceProvider().ComputeHash(file));
            }

            if (metaTags != null && metaTags.Count > 0)
            {
                foreach (string s in metaTags.Keys)
                {
                    Headers.Add(Constants.META_DATA_HEADER + s, metaTags[s]);
                }
            }

            Headers.Add(Constants.X_STORAGE_TOKEN, HttpUtility.UrlEncode(storageToken));
            Uri =
                new Uri(storageUrl + "/" + HttpUtility.UrlEncode(containerName).Replace("+", "%20") + "/" +
                        HttpUtility.UrlEncode(storageItemName).Replace("+", "%20"));
            Method = "PUT";
        }

        private string CleanUpFileUri(string fileUri)
        {
            return fileUri.Replace(@"file:\\\", "");
        }

        private void ReadStreamIntoRequest(Stream httpWebRequestFileStream)
        {
            byte[] buffer = new byte[Constants.CHUNK_SIZE];

            int amt = 0;
            while ((amt = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                httpWebRequestFileStream.Write(buffer, 0, amt);
            }

            stream.Close();
            httpWebRequestFileStream.Flush();
            httpWebRequestFileStream.Close();
        }

        /// <summary>
        /// inputs the supplied file stream into the http request
        /// </summary>
        /// <param name="httpWebRequestFileStream">the file stream to input into the http request</param>
        public void ReadFileIntoRequest(Stream httpWebRequestFileStream)
        {
            if (stream == null)
                stream = new FileStream(fileUri, FileMode.Open);

            ReadStreamIntoRequest(httpWebRequestFileStream);
            stream.Close();
        }

        /// <summary>
        /// the entity tag of the storage item
        /// </summary>
        /// <returns>string representation of the entity tag</returns>
        public string ETag
        {
            get { return Headers[Constants.ETAG]; }
            private set { Headers.Add(Constants.ETAG, value); }
        }

        /// <summary>
        /// the content type of the storage item
        /// </summary>
        /// <returns>string representation of the storage item's content type</returns>
        public string ContentType
        {
            get { return "application/octet-stream"; }
        }

        /// <summary>
        /// the http request user agent
        /// </summary>
        /// <returns>useragent as a string</returns>
        public string UserAgent { get; set; }
        
        /// <summary>
        /// the http request url
        /// </summary>
        /// <returns>Uri instance containing the http request url</returns>
        public Uri Uri { get; private set; }
        
        /// <summary>
        /// the http request method
        /// </summary>
        /// <returns>method of the http request</returns>
        public string Method { get; private set; }
        
        /// <summary>
        /// the http headers of the request
        /// </summary>
        /// <returns>a NameValueCollection of the http request's headers</returns>
        public NameValueCollection Headers { get; private set; }
        
        /// <summary>
        /// the content length of the file to upload
        /// </summary>
        /// <returns>long integer content length of the file</returns>
        public long ContentLength { get; private set; }

        private static string StringifyMD5(byte[] bytes)
        {
            StringBuilder result = new StringBuilder();
            foreach (byte b in bytes)
                result.AppendFormat("{0:x2}", b);
            return result.ToString();
        }
    }
}