///
/// See COPYING file for licensing information
///

using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// Represents the response information from creating a container on cloudfiles
    /// </summary>
    public class CreateContainerResponse : IResponse
    {
        /// <summary>
        /// A property representing the HTTP Status code returned from cloudfiles
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the create container request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }
    }
}