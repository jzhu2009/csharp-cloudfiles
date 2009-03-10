///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.Net;

namespace com.mosso.cloudfiles.domain.response
{
    /// <summary>
    /// Represents the response information from a CloudFiles request
    /// </summary>
    public class CloudFilesResponse : IResponse
    {
        /// <summary>
        /// A property representing the HTTP Status code returned from cloudfiles
        /// </summary>
        public HttpStatusCode Status { get; set; }

        /// <summary>
        /// A collection of key-value pairs representing the headers returned from the create container request
        /// </summary>
        public WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// dictionary of meta tags assigned to this storage item
        /// </summary>
        public Dictionary<string, string> Metadata
        {
            get
            {
                Dictionary<string, string> tags = new Dictionary<string, string>();
                foreach (string s in Headers.Keys)
                {
                    if (s.IndexOf(Constants.META_DATA_HEADER) == -1) continue;
                    var metaKeyStart = s.LastIndexOf("-");
                    tags.Add(s.Substring(metaKeyStart + 1), Headers[s]);
                }
                return tags;
            }
        }
    }
}