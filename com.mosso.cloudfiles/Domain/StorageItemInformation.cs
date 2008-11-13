///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.Net;

namespace com.mosso.cloudfiles.domain
{
    public class StorageItemInformation
    {
        private readonly WebHeaderCollection headers;

        public StorageItemInformation(WebHeaderCollection headers)
        {
            this.headers = headers;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ETag
        {
            get { return headers[Constants.ETAG]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType
        {
            get { return headers[Constants.CONTENT_TYPE_HEADER]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentLength
        {
            get { return headers[Constants.CONTENT_LENGTH_HEADER]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> MetaTags
        {
            get
            {
                Dictionary<string, string> tags = new Dictionary<string, string>();
                foreach (string s in headers.Keys)
                {
                    if (s.IndexOf(Constants.META_DATA_HEADER) != -1)
                    {
                        int metaKeyStart = s.LastIndexOf("-");
                        tags.Add(s.Substring(metaKeyStart + 1), headers[s]);
                    }
                }
                return tags;
            }
        }
    }
}