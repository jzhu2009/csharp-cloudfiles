///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.IO;
using System.Xml;
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
        string GetAccountInformationJson();
        XmlDocument GetAccountInformationXml();

        void CreateContainer(string containerName);
        
        void DeleteContainer(string continerName);
        
        List<string> GetContainers();
        
        List<string> GetContainerItemList(string containerName);
        List<string> GetContainerItemList(string containerName, Dictionary<GetItemListParameters, string> parameters);
        
        Container GetContainerInformation(string containerName);
        
        void PutStorageItem(string containerName, string storageItemName, Dictionary<string, string> metadata);
        void PutStorageItem(string containerName, string storageItemName);
        void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName);
        void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName, Dictionary<string, string> metadata);
        
        void DeleteStorageItem(string containerName, string storageItemname);

        StorageItem GetStorageItem(string containerName, string storageItemName);
        void GetStorageItem(string containerName, string storageItemName, string localFileName);
        StorageItem GetStorageItem(string containerName, string storageItemName, Dictionary<RequestHeaderFields, string> requestHeaderFields);
        void GetStorageItem(string containerName, string storageItemName, string localFileName, Dictionary<RequestHeaderFields, string> requestHeaderFields);
        
        
        StorageItem GetStorageItemInformation(string containerName, string storageItemName);
        
        void SetStorageItemMetaInformation(string containerName, string storageItemName, Dictionary<string, string> metadata);
        
        List<string> GetPublicContainers();
        
        string MarkContainerAsPublic(string containerName);
        string SetPublicContainerDetails(string containerName, bool isCdnEnabled);
        
        Container GetPublicContainerInformation(string containerName);
    }
}