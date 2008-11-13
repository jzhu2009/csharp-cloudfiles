///
/// See COPYING file for licensing information
///

using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{
    
    internal class RetrieveStorageItemInformationCommand : BaseCommand
    {
        private readonly string containerName;
        private readonly string storageObjectName;

       
        public RetrieveStorageItemInformationCommand(string containerName, string storageObjectName)
        {
            this.containerName = containerName;
            this.storageObjectName = storageObjectName;
        }

        
        protected override object ExecuteCommand()
        {
            StorageItem storageItem = null;
            GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation(storageUrl, containerName, storageObjectName, storageToken);
            try
            {
                GetStorageItemInformationResponse getStorageItemResponse = new ResponseFactory<GetStorageItemInformationResponse>().Create(new CloudFilesRequest(getStorageItemInformation, userCredentials.ProxyCredentials));
                storageItem = new StorageItem(storageObjectName, getStorageItemResponse.MetaTags, getStorageItemResponse.ContentType, long.Parse(getStorageItemResponse.ContentLength));
            }
            catch (WebException)
            {
                throw new StorageItemNotFoundException("The requested storage object does not exist");
            }
            return storageItem;
        }
    }
}