///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// enumeration of filters to place on the request url
    /// </summary>
    public enum GetItemListParameters
    {
        Limit,
        Offset,
        Prefix
    }

    /// <summary>
    /// GetContainerItemList
    /// </summary>
    public class GetContainerItemList : BaseRequest
    {
        /// <summary>
        /// GetContainerItemList constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="requestParameters">dictionary of parameter filters to place on the request url</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameLengthException">Thrown when the container name length exceeds the maximum container length allowed</exception>
        public GetContainerItemList(string storageUrl, string containerName, string storageToken,
                                    Dictionary<GetItemListParameters, string> requestParameters)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken)
                || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            if (containerName.Length > Constants.MAXIMUM_CONTAINER_NAME_LENGTH)
                throw new ContainerNameLengthException("Container name " + containerName + " exceeds " +
                                                       Constants.MAXIMUM_CONTAINER_NAME_LENGTH + " character limit");

            StringBuilder stringBuilder = new StringBuilder();

            if (requestParameters != null && requestParameters.Count > 0)
            {
                foreach (GetItemListParameters param in requestParameters.Keys)
                {
                    string paramName = param.ToString().ToLower();
                    if (param == GetItemListParameters.Limit)
                        int.Parse(requestParameters[param]);

                    if (stringBuilder.Length > 0)
                        stringBuilder.Append("&");
                    else
                        stringBuilder.AppendFormat("?");
                    stringBuilder.Append(paramName + "=" + HttpUtility.UrlEncode(requestParameters[param]).Replace("+", "%20"));
                }
            }
            Uri = new Uri(storageUrl + "/" + HttpUtility.UrlEncode(containerName).Replace("+", "%20") + stringBuilder);
            Method = "GET";
            Headers = new NameValueCollection();
            Headers.Add(Constants.X_STORAGE_TOKEN, HttpUtility.UrlEncode(storageToken));
        }

        /// <summary>
        /// GetContainerItemList constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        public GetContainerItemList(string storageUrl, string containerName, string storageToken)
            : this(storageUrl, containerName, storageToken, null)
        {
        }
    }
}