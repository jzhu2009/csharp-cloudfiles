///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain.request
{
    /// <summary>
    /// SetStorageItemMetaInformation
    /// </summary>
    public class SetStorageItemMetaInformation : BaseRequest
    {
        private readonly Dictionary<string, string> metadata;

        /// <summary>
        /// SetStorageItemMetaInformation constructor
        /// </summary>
        /// <param name="storageUrl">the customer unique url to interact with cloudfiles</param>
        /// <param name="containerName">the name of the container where the storage item is located</param>
        /// <param name="storageItemName">the name of the storage item to add meta information too</param>
        /// <param name="metadata">dictionary containing the meta tags on the storage item</param>
        /// <param name="authToken">the customer unique token obtained after valid authentication necessary for all cloudfiles ReST interaction</param>
        /// <exception cref="System.ArgumentNullException">Thrown when any of the arguments are null</exception>
        public SetStorageItemMetaInformation(string storageUrl, string authToken, string containerName, string storageItemName, Dictionary<string, string> metadata)
        {
            if (string.IsNullOrEmpty(storageUrl)
                || string.IsNullOrEmpty(authToken)
                || string.IsNullOrEmpty(containerName)
                || string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();


            if (!ContainerNameValidator.Validate(containerName)) throw new ContainerNameException();
            if (!ObjectNameValidator.Validate(storageItemName)) throw new StorageItemNameException();

            this.metadata = metadata;

            Uri = new Uri(storageUrl + "/" + containerName.Encode() + "/" + storageItemName.Encode());
            Method = "POST";

            AddStorageOrAuthTokenToHeaders(Constants.X_STORAGE_TOKEN, authToken);

            AttachMetadataToHeaders();
        }

        private void AttachMetadataToHeaders()
        {
            foreach (var pair in metadata)
            {
                if (pair.Key.Length > Constants.MAXIMUM_META_KEY_LENGTH)
                    throw new MetaKeyLengthException("The meta key length exceeds the maximum length of " +
                                                     Constants.MAXIMUM_META_KEY_LENGTH);
                if (pair.Value.Length > Constants.MAXIMUM_META_VALUE_LENGTH)
                    throw new MetaValueLengthException("The meta value length exceeds the maximum length of " +
                                                       Constants.MAXIMUM_META_VALUE_LENGTH);

                Headers.Add(Constants.META_DATA_HEADER + pair.Key, pair.Value);
            }
        }
    }
}