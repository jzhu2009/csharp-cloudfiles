///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Specialized;
using System.Web;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// GetAuthentication
    /// </summary>
    public class GetAuthentication : BaseRequest
    {
        /// <summary>
        /// GetAuthentication constructor
        /// </summary>
        /// <param name="userCredentials">the UserCredentials instace to use when attempting authentication</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        public GetAuthentication(UserCredentials userCredentials)
        {
            if (userCredentials == null) throw new ArgumentNullException();
            if (string.IsNullOrEmpty(userCredentials.AccountName))
            {
                Uri = userCredentials.AuthUrl;
            }
            else
            {
                Uri =
                    new Uri(userCredentials.AuthUrl + "/" + EncodeStringProperlyAccordingToCloudFiles(userCredentials.Cloudversion) + "/" +
                            EncodeStringProperlyAccordingToCloudFiles(userCredentials.AccountName) + "/auth");
            }
            Method = "GET";
            Headers.Add(Constants.X_AUTH_USER, EncodeStringProperlyAccordingToCloudFiles(userCredentials.Username));
            Headers.Add(Constants.X_AUTH_KEY, EncodeStringProperlyAccordingToCloudFiles(userCredentials.Api_access_key));
        }

        private string EncodeStringProperlyAccordingToCloudFiles(string itemToEncode)
        {
            return EncodePlusesIntoPercent20BecauseCloudFilesRequiresIt(HttpUtility.UrlEncode(itemToEncode));
        }

        private string EncodePlusesIntoPercent20BecauseCloudFilesRequiresIt(string itemToEncode)
        {
            return itemToEncode.Replace("+", "%20");
        }
    }
}