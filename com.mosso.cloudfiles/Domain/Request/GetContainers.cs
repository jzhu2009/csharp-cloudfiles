///
/// See COPYING file for licensing information
///

using System;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// GetContainers
    /// </summary>
    public class GetContainers : BaseRequest
    {
        /// <summary>
        /// GetContainers constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="authToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public GetContainers(string storageUrl, string authToken)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(authToken))
                throw new ArgumentNullException();

            Uri = new Uri(storageUrl);
            Method = "GET";
            AddAuthTokenToHeaders(authToken);
        }
    }
}