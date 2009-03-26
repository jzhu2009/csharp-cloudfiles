///
/// See COPYING file for licensing information
///

using System;
using com.mosso.cloudfiles.utils;

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

            Uri = new Uri(cdnManagementUrl + "/" + containerName.Encode() + "?enabled_only=true");
            Method = "HEAD";
            AddAuthTokenToHeaders(authToken);
        }
        
    }
}