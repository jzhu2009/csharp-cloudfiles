///
/// See COPYING file for licensing information
///

using System;
using com.mosso.cloudfiles.utils;

namespace com.mosso.cloudfiles.domain.request
{
    public class MarkContainerAsPublic : BaseRequest
    {
        private readonly string cdnManagementUrl;
        private readonly string authToken;

        /// <summary>
        /// Adds this container to the list of containers to be served up publicly
        /// </summary>
        /// <param name="cdnManagementUrl">The URL that will be used for accessing content from CloudFS</param>
        /// <param name="authToken">The authentication token received from the auth server</param>
        /// <param name="containerName">The name of the container to make public on the CDN</param>
        public MarkContainerAsPublic(string cdnManagementUrl, string authToken, string containerName)
        {
            if (string.IsNullOrEmpty(cdnManagementUrl)
              || string.IsNullOrEmpty(authToken) 
              || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            this.cdnManagementUrl = cdnManagementUrl;
            this.authToken = authToken;

            Method = "PUT";

            Uri = new Uri(cdnManagementUrl + "/" + containerName.Encode());
            AddAuthTokenToHeaders(authToken);
        }

        

        /// <summary>
        /// Adds this container to the list of containers to be served up publicly
        /// </summary>
        /// <param name="cdnManagementUrl">The URL that will be used for accessing content from CloudFS</param>
        /// <param name="authToken">The authentication token received from the auth server</param>
        /// <param name="containerName">The name of the container to make public on the CDN</param>
        /// <param name="timeToLiveInSeconds">The maximum time (in seconds) content should be kept alive on the CDN before it checks for freshness.</param>
        public MarkContainerAsPublic(string cdnManagementUrl, string authToken, string containerName, int timeToLiveInSeconds)
        {
            if (string.IsNullOrEmpty(cdnManagementUrl)
              || string.IsNullOrEmpty(authToken) 
              || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            this.cdnManagementUrl = cdnManagementUrl;
            this.authToken = authToken;

            Method = "PUT";

            Uri = new Uri(cdnManagementUrl + "/" + containerName.Encode());
            Headers.Add(Constants.X_CDN_ENABLED, "true".Capitalize());
            if (timeToLiveInSeconds > -1){Headers.Add(Constants.X_CDN_TTL, timeToLiveInSeconds.ToString());}
            AddAuthTokenToHeaders(authToken);
        }
    }
}