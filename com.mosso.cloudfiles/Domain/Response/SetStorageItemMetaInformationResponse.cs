///
/// See COPYING file for licensing information
///

using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// This class wraps the response from the set storage item meta information request
    /// </summary>
    public class SetStorageItemMetaInformationResponse : IResponse
    {
        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }


        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the set storage item meta information request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// A property representing the MIME type of the response body
        /// </summary>
        public string ContentType
        {
            get { return Headers[Constants.CONTENT_TYPE_HEADER]; }
        }

        /// <summary>
        /// A property representing the length, in bytes, of the content body
        /// </summary>
        public string ContentLength
        {
            get { return Headers[Constants.CONTENT_LENGTH_HEADER]; }
        }
    }
}