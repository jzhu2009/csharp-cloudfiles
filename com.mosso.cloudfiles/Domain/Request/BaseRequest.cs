///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Specialized;
using System.Web;
using com.mosso.cloudfiles.utils;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// BaseRequest
    /// </summary>
    public abstract class BaseRequest : IRequest
    {
        protected NameValueCollection headers = new NameValueCollection();

        /// <summary>
        /// HTTP Headers collection
        /// </summary>
        public NameValueCollection Headers
        {
            get
            {
                return headers;
            }
            protected set
            {
                headers = value;
            }
        }

        /// <summary>
        /// The content-type of the http request
        /// </summary>
        public string ContentType { get; set; }
        
        /// <summary>
        /// The uri to use in the http request
        /// </summary>
        public Uri Uri { get; protected set; }
        
        /// <summary>
        /// The ACL based on user-agent to be passed in the request
        /// </summary>
        public string UserAgent { get; set; }
        
        /// <summary>
        /// The http method (GET,PUT,HEAD,POST, DELETE) of the request
        /// </summary>
        public string Method { get; protected set; }

        protected void AddAuthTokenToHeaders(string value)
        {
            Headers.Add(Constants.X_AUTH_TOKEN, HttpUtility.UrlEncode(value));
        }
    }
}