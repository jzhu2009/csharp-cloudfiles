///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.IO;
using System.Net;
using com.mosso.cloudfiles.domain.response;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// This class wraps the response from the get account information request
    /// </summary>
    public class GetAccountInformationResponse : IResponse
    {
        /// <summary>
        /// A property containing the HTTP status code from the transaction
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the get account information request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }
    }

    
}
    
