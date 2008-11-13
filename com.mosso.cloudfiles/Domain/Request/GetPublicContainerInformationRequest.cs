using System;
using System.Collections.Specialized;
using System.Web;

namespace com.mosso.cloudfiles.domain.request
{
    public class GetPublicContainerInformationRequest : BaseRequest
    {
        public GetPublicContainerInformationRequest(string cdnManagementUrl, string authToken, string containerName)
        {
            if (String.IsNullOrEmpty(cdnManagementUrl) ||
                String.IsNullOrEmpty(authToken) ||
                String.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();
            Headers = new NameValueCollection();
            this.Uri = new Uri(cdnManagementUrl+"/"+containerName);
            Method = "HEAD";
            Headers.Add(Constants.X_AUTH_TOKEN, HttpUtility.UrlEncode(authToken));
        }
        
    }
}