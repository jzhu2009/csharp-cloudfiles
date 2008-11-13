///
/// See COPYING file for licensing information
///

using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// This class wraps the response from the get authentication request
    /// </summary>
    public class GetAuthenticationResponse : IResponse
    {
        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the get authentication request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// A property containing the storage token used to authentication against cloudfiles
        /// </summary>
        public string StorageToken
        {
            get { return Headers[Constants.X_STORAGE_TOKEN]; }
        }

        /// <summary>
        /// The storage endpoint for a given account on cloudfiles
        /// </summary>
        public string StorageUrl
        {
            get { return Headers[Constants.X_STORAGE_URL]; }
        }
    }
}