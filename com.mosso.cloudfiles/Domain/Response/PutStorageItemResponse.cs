///
/// See COPYING file for licensing information
///

using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// This class wraps the response from the put storage item request
    /// </summary>
    public class PutStorageItemResponse : IResponse
    {
        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the put storage item request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// A property representing the MD5 entity tag for this particular storage item. 
        /// </summary>
        public string ETag
        {
            get { return Headers[Constants.ETAG]; }
        }
    }
}