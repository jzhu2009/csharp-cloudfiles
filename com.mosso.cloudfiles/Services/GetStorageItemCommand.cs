///
/// See COPYING file for licensing information
///

using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using System.Collections.Generic;

namespace com.mosso.cloudfiles.services
{
 
    internal class GetStorageItemCommand : BaseCommand
    {
        private readonly string containerName;
        private readonly string storageObjectName;
        private readonly string localFileName;
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;

       
        public GetStorageItemCommand(string containerName, string storageObjectName) : this(containerName, storageObjectName, "", null)
        {
        }

    
        public GetStorageItemCommand(string containerName, string storageObjectName, Dictionary<RequestHeaderFields, string> requestHeaderFields): this(containerName, storageObjectName, "", requestHeaderFields)
        {
           
        }


      
        public GetStorageItemCommand(string containerName, string storageObjectName, string localFileName) : this(containerName, storageObjectName, localFileName, null)
        {
            this.containerName = containerName;
            this.storageObjectName = storageObjectName;
            this.localFileName = localFileName;
        }

        
        public GetStorageItemCommand(string containerName, string storageObjectName, string localFileName, Dictionary<RequestHeaderFields, string> requestHeaderFields)
        {
            this.containerName = containerName;
            this.storageObjectName = storageObjectName;
            this.localFileName = localFileName;
            if (requestHeaderFields != null)
                this.requestHeaderFields = requestHeaderFields;
        }

        
        protected override object ExecuteCommand()
        {
            StorageItem storageItem = null;
            GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, storageObjectName, storageToken, requestHeaderFields);
            try
            {
                GetStorageItemResponse getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem, userCredentials.ProxyCredentials));

                if (localFileName.Length > 0)
                {
                    getStorageItemResponse.SaveStreamToDisk(localFileName);
                }
                else
                {
                    storageItem = new StorageItem(storageObjectName, null, getStorageItemResponse.ContentType, getStorageItemResponse.ContentStream, long.Parse(getStorageItemResponse.ContentLength));
                }
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse) we.Response;
                response.Close();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
            return storageItem;
        }
    }
}