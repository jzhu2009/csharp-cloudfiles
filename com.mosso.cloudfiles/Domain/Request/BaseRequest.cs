///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Specialized;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// BaseRequest
    /// </summary>
    public abstract class BaseRequest : IRequest
    {
        protected NameValueCollection headers = new NameValueCollection();

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

        public string ContentType { get; set; }
        public Uri Uri { get; protected set; }
        public string UserAgent { get; set; }
        public string Method { get; protected set; }
    }
}