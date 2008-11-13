///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// SetStorageItemMetaInformation
    /// </summary>
    public class SetStorageItemMetaInformation : BaseRequest
    {
        private readonly Dictionary<string, string> metaTags;

        /// <summary>
        /// SetStorageItemMetaInformation constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <param name="metaTags">dictionary containing the meta tags on the storage item</param>
        /// <param name="storageToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the arguments are null</exception>
        public SetStorageItemMetaInformation(string storageUrl, string containerName, string storageItemName,
                                             Dictionary<string, string> metaTags, string storageToken)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(storageToken)
                || string.IsNullOrEmpty(containerName)
                || string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();


            if (containerName.Length > Constants.MAXIMUM_CONTAINER_NAME_LENGTH)
                throw new ContainerNameLengthException("The container name length exceeds " + Constants.MAXIMUM_CONTAINER_NAME_LENGTH + " characters.s");


            this.metaTags = metaTags;
            Headers = new NameValueCollection();

            Uri =
                new Uri(storageUrl + "/" + HttpUtility.UrlEncode(containerName).Replace("+", "%20") + "/" +
                        HttpUtility.UrlEncode(storageItemName).Replace("+", "%20"));
            Method = "POST";

            Headers.Add(Constants.X_STORAGE_TOKEN, HttpUtility.UrlEncode(storageToken));

            AttachMetaTagsToHeaders();
        }

        private void AttachMetaTagsToHeaders()
        {
            foreach (KeyValuePair<string, string> metaTag in metaTags)
            {
                if (metaTag.Key.Length > Constants.MAXIMUM_META_KEY_LENGTH)
                    throw new MetaKeyLengthException("The meta key length exceeds the maximum length of " +
                                                     Constants.MAXIMUM_META_KEY_LENGTH);
                if (metaTag.Value.Length > Constants.MAXIMUM_META_VALUE_LENGTH)
                    throw new MetaValueLengthException("The meta value length exceeds the maximum length of " +
                                                       Constants.MAXIMUM_META_VALUE_LENGTH);

                Headers.Add(Constants.META_DATA_HEADER + metaTag.Key, metaTag.Value);
            }
        }
    }
}