using System;
using System.Collections.Specialized;
using System.Web;
using ArgumentNullException=System.ArgumentNullException;

namespace com.mosso.cloudfiles.domain.request
{
    public class SetContainerAsPublicRequest : BaseRequest
    {
        private readonly string cdnManagementUrl;
        private readonly string authToken;
        private readonly string ttl;
        private readonly string userAgentAcl;
        private readonly string referrerAcl;

        public SetContainerAsPublicRequest(string cdnManagementUrl, string authToken, string containerName) 
            : this(cdnManagementUrl, authToken, containerName, "", "", ""){}

        public SetContainerAsPublicRequest(string cdnManagementUrl, string authToken, string containerName, string ttl, string userAgentAcl, string referrerAcl)
        {
            if (string.IsNullOrEmpty(cdnManagementUrl)
              || string.IsNullOrEmpty(authToken) 
              || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            this.cdnManagementUrl = cdnManagementUrl;
            this.authToken = authToken;
            this.ttl = ttl;
            this.userAgentAcl = userAgentAcl;
            this.referrerAcl = referrerAcl;

            Method = "PUT";

            Uri = new System.Uri(cdnManagementUrl + "/" + HttpUtility.UrlEncode(containerName));
            Headers.Add(Constants.X_AUTH_TOKEN, HttpUtility.UrlEncode(authToken));

            if (!String.IsNullOrEmpty(ttl))
                Headers.Add(Constants.X_CDN_TTL, ttl);
            if (!String.IsNullOrEmpty(userAgentAcl))
                Headers.Add(Constants.X_USER_AGENT_ACL, userAgentAcl);
            if (!String.IsNullOrEmpty(referrerAcl))
                Headers.Add(Constants.X_REFERRER_ACL, referrerAcl);
        }
    }
}