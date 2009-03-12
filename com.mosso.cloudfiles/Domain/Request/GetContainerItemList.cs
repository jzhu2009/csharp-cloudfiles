///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.Text;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain.request
{
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
        /// <param name="authToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        /// <exception cref="ContainerNameException">Thrown when the container name is invalid</exception>
        public GetContainerItemList(string storageUrl, string authToken, string containerName, Dictionary<GetItemListParameters, string> requestParameters)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(authToken)
                || string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            if (!ContainerNameValidator.Validate(containerName)) throw new ContainerNameException();

            StringBuilder stringBuilder = new StringBuilder();

            if (requestParameters != null && requestParameters.Count > 0)
            {
                foreach (GetItemListParameters param in requestParameters.Keys)
                {
                    var paramName = param.ToString().ToLower();
                    if (param == GetItemListParameters.Limit)
                        int.Parse(requestParameters[param]);

                    if (stringBuilder.Length > 0)
                        stringBuilder.Append("&");
                    else
                        stringBuilder.AppendFormat("?");
                    stringBuilder.Append(paramName + "=" + requestParameters[param].Encode());
                }
            }
            Uri = new Uri(storageUrl + "/" + containerName.Encode() + stringBuilder);
            Method = "GET";
            AddAuthTokenToHeaders(authToken);
        }

        /// <summary>
        /// GetContainerItemList constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="authToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        public GetContainerItemList(string storageUrl, string authToken, string containerName)
            : this(storageUrl, authToken, containerName, null)
        {
        }
    }
}