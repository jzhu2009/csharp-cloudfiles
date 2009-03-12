///
/// See COPYING file for licensing information
///

using System;
using System.ComponentModel;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// GetAccountInformation
    /// </summary>
    public class GetAccountInformation : BaseRequest
    {
        /// <summary>
        /// GetAccountInformation constructor
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null</exception>
        public GetAccountInformation(string storageUrl, string authToken)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(authToken))
                throw new ArgumentNullException();
            Method = "HEAD";
            Uri = new Uri(storageUrl + "/");

            AddStorageOrAuthTokenToHeaders(Constants.X_STORAGE_TOKEN, authToken);
        }
    }

    public enum Format
    {
        [Description("json")]
        JSON,
        [Description("xml")]
        XML
    }

    public class GetAccountInformationSerialized : BaseRequest
    {
        /// <summary>
        /// GetAccountInformationSerialized constructor
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null</exception>
        public GetAccountInformationSerialized(string storageUrl, string authToken, Format format)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(authToken))
                throw new ArgumentNullException();
            Method = "GET";
            Uri = new Uri(storageUrl + "?format=" + EnumHelper.GetDescription(format));

            AddStorageOrAuthTokenToHeaders(Constants.X_STORAGE_TOKEN, authToken);
        }
    }
}