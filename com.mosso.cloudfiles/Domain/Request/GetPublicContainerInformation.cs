using System;
using System.Collections.Specialized;
using System.Web;

namespace com.mosso.cloudfiles.domain.request
{
    public class GetPublicContainerInformation : BaseRequest
    {
        public GetPublicContainerInformation(string cdnManagementUrl, string authToken, string containerName)
        {
            if (String.IsNullOrEmpty(cdnManagementUrl) ||
                String.IsNullOrEmpty(authToken) ||
                String.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            this.Uri = new Uri(cdnManagementUrl+"/"+containerName);
            Method = "HEAD";
            Headers.Add(Constants.X_AUTH_TOKEN, HttpUtility.UrlEncode(authToken));
        }
        
    }
}