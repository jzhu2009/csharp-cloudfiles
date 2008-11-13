///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Specialized;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// IRequest
    /// </summary>
    public interface IRequest
    {
        Uri Uri { get; }
        string Method { get; }
        NameValueCollection Headers { get; }
        string ContentType { get; }
        string UserAgent { get; set; }
    }
}