///
/// See COPYING file for licensing information
///

using System;
using com.mosso.cloudfiles.utils;

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
            Uri = string.IsNullOrEmpty(userCredentials.AccountName) 
                ? userCredentials.AuthUrl
                : new Uri(userCredentials.AuthUrl + "/" 
                    + userCredentials.Cloudversion.Encode() + "/" 
                    + userCredentials.AccountName.Encode() + "/auth");
            Method = "GET";
            Headers.Add(Constants.X_AUTH_USER, userCredentials.Username.Encode());
            Headers.Add(Constants.X_AUTH_KEY, userCredentials.Api_access_key.Encode());
        }
    }
}