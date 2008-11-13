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
        public NameValueCollection Headers { get; protected set; }
        public string ContentType { get; set; }
        public Uri Uri { get; protected set; }
        public string UserAgent { get; set; }
        public string Method { get; protected set; }
    }
}