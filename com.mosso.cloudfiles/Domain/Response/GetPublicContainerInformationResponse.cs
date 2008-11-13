using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    public class GetPublicContainerInformationResponse : IResponse
    {
        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the get containers request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

    }
}