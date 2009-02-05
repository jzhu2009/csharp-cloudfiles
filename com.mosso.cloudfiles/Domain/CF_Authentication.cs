///
/// See COPYING file for licensing information
///

using System;
using System.Net;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;

namespace com.mosso.cloudfiles.domain
{
    public interface IAuthentication
    {
        IAccount Authenticate();
    }

    public class CF_Authentication : IAuthentication
    {
        protected UserCredentials userCredentials;
        protected bool retry;

        public CF_Authentication(UserCredentials userCredentials)
        {
            this.userCredentials = userCredentials;
            retry = false;
        }

        public IAccount Authenticate()
        {
            GetAuthenticationResponse getAuthenticationResponse = null;

            GetAuthentication getAuthentication = new GetAuthentication(userCredentials);
            getAuthenticationResponse = AuthenticateWithCloudFiles(getAuthentication);

            if (AuthenticationPassed(getAuthenticationResponse))
            {
                IAccount account = new CF_Account(getAuthenticationResponse.StorageToken, new Uri(getAuthenticationResponse.StorageUrl));
                account.AuthToken = getAuthenticationResponse.Headers[Constants.X_AUTH_TOKEN];
                account.CDNManagementUrl = new Uri(getAuthenticationResponse.Headers[Constants.X_CDN_MANAGEMENT_URL]);
                account.UserCredentials = userCredentials;
                return account;
            }

            throw new UnauthorizedAccessException();
        }

        protected virtual GetAuthenticationResponse AuthenticateWithCloudFiles(GetAuthentication getAuthentication)
        {
            try
            {
                return new ResponseFactory<GetAuthenticationResponse>()
                    .Create(new CloudFilesRequest(getAuthentication, userCredentials.ProxyCredentials));
            }
            catch(WebException we)
            {
                RetryAuthenticationSecondTime(we);
            }

            return null;
        }

        protected virtual bool AuthenticationPassed(GetAuthenticationResponse getAuthenticationResponse)
        {
            return getAuthenticationResponse != null && getAuthenticationResponse.Status == HttpStatusCode.NoContent;
        }

        protected virtual void RetryAuthenticationSecondTime(WebException we)
        {
            HttpStatusCode code = ((HttpWebResponse)we.Response).StatusCode;
            if (!retry && code == HttpStatusCode.Unauthorized)
            {
                retry = true;
                Authenticate();
            }
        }
    }
}