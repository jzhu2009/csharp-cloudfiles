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
    
    internal class DeleteStorageItemCommand : BaseCommand
    {
        private readonly string containerName;
        private readonly string storageObjectName;

       
        public DeleteStorageItemCommand(string containerName, string storageObjectName)
        {
            this.containerName = containerName;
            this.storageObjectName = storageObjectName;
        }

        
        protected override object ExecuteCommand()
        {
            DeleteStorageItem deleteStorageItem = new DeleteStorageItem(storageUrl, containerName, storageObjectName, storageToken);
            try
            {
                DeleteStorageItemResponse deleteStorageItemResponse = new ResponseFactory<DeleteStorageItemResponse>().Create(new CloudFilesRequest(deleteStorageItem));
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse) we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object for deletion does not exist");

                throw;
            }
            return null;
        }
    }
}