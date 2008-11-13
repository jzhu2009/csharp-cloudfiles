///
/// See COPYING file for licensing information
///

using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// This class wraps the response sent back from cloudfiles when a container is deleted
    /// </summary>
    public class DeleteContainerResponse : IResponse
    {
        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the delete container request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }
    }
}