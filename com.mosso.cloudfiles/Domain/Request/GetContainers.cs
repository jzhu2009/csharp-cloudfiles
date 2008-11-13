///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Specialized;
using System.Web;

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
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public GetContainers(string storageUrl, string storageToken)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken))
                throw new ArgumentNullException();

            Uri = new Uri(storageUrl);
            Headers = new NameValueCollection();
            Method = "GET";
            Headers.Add(Constants.X_STORAGE_TOKEN, HttpUtility.UrlEncode(storageToken));
        }
    }
}