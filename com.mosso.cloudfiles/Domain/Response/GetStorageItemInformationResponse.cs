///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// This class wraps the response from the get storage item information request
    /// </summary>
    public class GetStorageItemInformationResponse : IResponse
    {
        private WebHeaderCollection headers;
        private StorageItemInformation storageItemInformation;

        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the get storage item information request
        /// </summary>
        public WebHeaderCollection Headers
        {
            get { return headers; }
            set
            {
                headers = value;
                storageItemInformation = new StorageItemInformation(headers);
            }
        }

        /// <summary>
        /// A property representing the MD5 entity tag for this particular storage item. 
        /// </summary>
        public string ETag
        {
            get { return storageItemInformation.ETag; }
        }

        /// <summary>
        /// A property representing the MIME type of the storage item
        /// </summary>
        public string ContentType
        {
            get { return storageItemInformation.ContentType; }
        }

        /// <summary>
        /// A property representing the size, in bytes, of the storage item
        /// </summary>
        public string ContentLength
        {
            get { return storageItemInformation.ContentLength; }
        }

        /// <summary>
        /// A property representing the meta information associated with the storage item
        /// </summary>
        public Dictionary<string, string> MetaTags
        {
            get { return storageItemInformation.MetaTags; }
        }
    }
}