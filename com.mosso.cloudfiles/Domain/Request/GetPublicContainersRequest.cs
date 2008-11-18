using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace com.mosso.cloudfiles.domain.request
{
    public class GetPublicContainersRequest : BaseRequest
    {
        private readonly string cdnManagementUrl;
        private readonly string authToken;

        public GetPublicContainersRequest(string cdnManagementUrl, string authToken)
        {
            if (string.IsNullOrEmpty(cdnManagementUrl)
               || string.IsNullOrEmpty(authToken))
                throw new ArgumentNullException();

            this.cdnManagementUrl = cdnManagementUrl;
            this.authToken = authToken;
            Method = "GET";

            Headers.Add(Constants.X_AUTH_TOKEN, HttpUtility.UrlEncode(authToken));
            Uri = new Uri(cdnManagementUrl);
        }


    }
}