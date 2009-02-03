///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Specialized;
using System.Web;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// CreateContainer
    /// </summary>
    public class CreateContainer : BaseRequest
    {
        /// <summary>
        /// CreateContainer constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the reference arguments are null</exception>
        /// <exception cref="ContainerNameLengthException">Thrown when the container name length exceeds the maximum container length allowed</exception>
        public CreateContainer(string storageUrl, string storageToken, string containerName)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken)
                || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            if (containerName.Length > Constants.MAXIMUM_CONTAINER_NAME_LENGTH)
                throw new ContainerNameLengthException("Container name " + containerName + " exceeds " +
                                                       Constants.MAXIMUM_CONTAINER_NAME_LENGTH + " character limit");

            if(containerName.IndexOf("/") > -1)
                throw new ContainerNameBadlyFormedException("Container name " + containerName + " has an invalid character (currently only the slash '/' character)");

            Uri = new Uri(storageUrl + "/" + HttpUtility.UrlEncode(containerName).Replace("+", "%20"));
            Method = "PUT";

            Headers.Add(Constants.X_STORAGE_TOKEN, HttpUtility.UrlEncode(storageToken));
        }
    }
}