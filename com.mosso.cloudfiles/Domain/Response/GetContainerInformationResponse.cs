///
/// See COPYING file for licensing information
///

using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// This class wraps the resposne from the get container information request
    /// </summary>
    public class GetContainerInformationResponse : IResponse
    {
        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the get container information request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// A property containing the number of storage objects in the requested container
        /// </summary>
        public string ObjectCount
        {
            get { return Headers[Constants.X_CONTAINER_STORAGE_OBJECT_COUNT]; }
        }

        /// <summary>
        /// A property representing the number of bytes used by the requested container
        /// </summary>
        public string BytesUsed
        {
            get { return Headers[Constants.X_CONTAINER_BYTES_USED]; }
        }
    }
}