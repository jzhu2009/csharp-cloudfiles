///
/// See COPYING file for licensing information
///

using System;

namespace com.mosso.cloudfiles.domain.request
{
    public class GetPublicContainers : BaseRequest
    {
        public GetPublicContainers(string cdnManagementUrl, string authToken)
        {
            if (string.IsNullOrEmpty(cdnManagementUrl)
               || string.IsNullOrEmpty(authToken))
                throw new ArgumentNullException();

            Method = "GET";

            AddAuthTokenToHeaders(authToken);
            Uri = new Uri(cdnManagementUrl + "?enabled_only=true");
        }
    }
}