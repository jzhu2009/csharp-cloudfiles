///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.IO;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;

namespace com.mosso.cloudfiles.services
{
    /// <summary>
    /// The interface dictating the required methods for all implementing classes
    /// </summary>
    public interface IConnection
    {
        AccountInformation GetAccountInformation();

        void CreateContainer(string containerName);
        void DeleteContainer(string continerName);
        List<string> GetContainers();
        List<string> GetContainerItemList(string containerName);
        Container GetContainerInformation(string containerName);

        void PutStorageItem(string containerName, string storageItemName, Dictionary<string, string> metaTags);
        void PutStorageItem(string containerName, string storageItemName);
        void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName);
        void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName, Dictionary<string, string> metaTags);
        void DeleteStorageItem(string containerName, string storageItemname);
        StorageItem GetStorageItem(string containerName, string storageItemName);
        StorageItem GetStorageItem(string containerName, string storageItemName, Dictionary<RequestHeaderFields, string> requestHeaderFields);
        void SetStorageItemMetaInformation(string containerName, string storageItemName, Dictionary<string, string> metaTags);
        StorageItem GetStorageItemInformation(string containerName, string storageItemName);
        List<string> GetPublicContainers();
    }
}