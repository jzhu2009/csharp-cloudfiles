///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Specialized;
using System.Web;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// GetAccountInformation
    /// </summary>
    public class GetAccountInformation : BaseRequest
    {
        /// <summary>
        /// GetAccountInformation constructor
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null</exception>
        public GetAccountInformation(string storageUrl, string storageToken)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken))
                throw new ArgumentNullException();
            Method = "HEAD";
            Uri = new Uri(storageUrl + "/");

            Headers = new NameValueCollection();
            Headers.Add(Constants.X_STORAGE_TOKEN, HttpUtility.UrlEncode(storageToken));
        }
    }
}