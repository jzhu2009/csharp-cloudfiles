using System;
using System.Collections.Specialized;
using System.Web;

namespace com.mosso.cloudfiles.domain.request
{
    public class SetPublicContainerDetailsRequest : BaseRequest
    {
        /// <summary>
        /// Assigns various details to containers already publicly available on the CDN
        /// </summary>
        /// <param name="cdnManagementUrl">The CDN URL</param>
        /// <param name="authToken">The authorization token returned from the authorization server</param>
        /// <param name="containerName">The name of the container to update the details for</param>
        /// <param name="isCdnEnabled">Sets whether or not specified container is available on the CDN</param>
        /// <param name="cdnTtl">The TTL of the container on the CDN</param>
        /// <param name="userAgentAcl">Specifies ACL information for user agents.</param>
        /// <param name="referrerAcl">Specifies ACL information for referrers</param>
        public SetPublicContainerDetailsRequest(string cdnManagementUrl, string authToken, string containerName, bool isCdnEnabled, string cdnTtl, string userAgentAcl, string referrerAcl)
        {
            Headers = new NameValueCollection();
            AssignCommonDetails(cdnManagementUrl,authToken,containerName, cdnTtl, userAgentAcl, referrerAcl);
            
            string isCdnEnabledString = isCdnEnabled.ToString();
            isCdnEnabledString = isCdnEnabledString.Substring(0).ToUpper() + isCdnEnabledString.Substring(1);
            Headers.Add(Constants.X_CDN_ENABLED, isCdnEnabledString);
        }

        private void AssignCommonDetails(string cdnManagementUrl, string authToken, string containerName, string cdnTtl, string userAgentAcl, string referrerAcl)
        {
            if (String.IsNullOrEmpty(cdnManagementUrl) ||
                    String.IsNullOrEmpty(authToken) ||
                    String.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Method = "POST";

            this.Uri = new Uri(cdnManagementUrl + "/" + containerName);

            Headers.Add(Constants.X_AUTH_TOKEN, HttpUtility.UrlEncode(authToken));

            if (!String.IsNullOrEmpty(cdnTtl))
                Headers.Add(Constants.X_CDN_TTL, HttpUtility.UrlEncode(userAgentAcl));
            if (!String.IsNullOrEmpty(userAgentAcl))
                Headers.Add(Constants.X_USER_AGENT_ACL, HttpUtility.UrlEncode(userAgentAcl));
            if (!String.IsNullOrEmpty(referrerAcl))
                Headers.Add(Constants.X_REFERRER_ACL, HttpUtility.UrlEncode(referrerAcl));
        }



        /// <summary>
        /// Assigns various details to containers already publicly available on the CDN
        /// </summary>
        /// <param name="cdnManagementUrl">The CDN URL</param>
        /// <param name="authToken">The authorization token returned from the authorization server</param>
        /// <param name="containerName">The name of the container to update the details for</param>
        /// <param name="cdnTtl">The TTL of the container on the CDN</param>
        /// <param name="userAgentAcl">Specifies ACL information for user agents.</param>
        /// <param name="referrerAcl">Specifies ACL information for referrers</param>
        public SetPublicContainerDetailsRequest(string cdnManagementUrl, string authToken, string containerName, string cdnTtl, string userAgentAcl, string referrerAcl)
        {
            AssignCommonDetails(cdnManagementUrl, authToken, containerName,cdnTtl, userAgentAcl, referrerAcl);
        }
    }
}