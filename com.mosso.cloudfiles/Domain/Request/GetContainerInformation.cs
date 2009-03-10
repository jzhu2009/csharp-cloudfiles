///
/// See COPYING file for licensing information
///

using System;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// GetContainerInformation
    /// </summary>
    public class GetContainerInformation : BaseRequest
    {
        /// <summary>
        /// GetContainerInformation constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        public GetContainerInformation(string storageUrl, string storageToken, string containerName)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken)
                || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            if (!ContainerNameValidator.Validate(containerName)) throw new ContainerNameException();

            Uri = new Uri(storageUrl + "/" + containerName.Encode());
            Method = "HEAD";
            AddStorageOrAuthTokenToHeaders(Constants.X_STORAGE_TOKEN, storageToken);
        }
    }

    public class GetContainerInformationSerialized : BaseRequest
    {
        /// <summary>
        /// GetContainerInformationSerialized constructor
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are null</exception>
        public GetContainerInformationSerialized(string storageUrl, string storageToken, string containerName, Format format)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken)
                || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            if (!ContainerNameValidator.Validate(containerName)) throw new ContainerNameException();

            Uri = new Uri(storageUrl + "/" + containerName.Encode() + "?format=" + EnumHelper.GetDescription(format));
            Method = "GET";
            AddStorageOrAuthTokenToHeaders(Constants.X_STORAGE_TOKEN, storageToken);
        }
    }
}