///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{
    
    internal class PutStorageItemCommand : BaseCommand
    {
        private readonly string containerName;
        private readonly string localItemName;
        private readonly FileStream fileStream;
        private string remoteItemName;
        private readonly Dictionary<string, string> metaInformation;

       
        public PutStorageItemCommand(string containerName, string localItemName, string remoteItemName, Dictionary<string, string> metaInformation)
        {
            this.containerName = containerName;
            this.localItemName = localItemName;
            this.remoteItemName = remoteItemName;
            this.metaInformation = metaInformation;
        }

       
        public PutStorageItemCommand(string containerName, FileStream fileStream, string remoteItemName, Dictionary<string, string> metaInformation)
        {
            this.containerName = containerName;
            this.fileStream = fileStream;
            this.remoteItemName = remoteItemName;
            this.metaInformation = metaInformation;
        }

      
        public PutStorageItemCommand(string containerName, string localItemName, string remoteItemName) : this(containerName, localItemName, remoteItemName, null)
        {
        }

       
        public PutStorageItemCommand(string containerName, FileStream fileStream, string remoteItemName)
            : this(containerName, fileStream, remoteItemName, null)
        {
        }

       
        protected override object ExecuteCommand()
        {
            string remoteName = Path.GetFileName(remoteItemName);
            string localName = localItemName.Replace("/", "\\");
            try
            {
                PutStorageItem putStorageItem;
                if (fileStream == null)
                {
                    putStorageItem = new PutStorageItem(storageUrl, containerName, remoteName, localName, storageToken, metaInformation);
                }
                else
                {
                    putStorageItem = new PutStorageItem(storageUrl, containerName, remoteName, fileStream, storageToken, metaInformation);
                }

                PutStorageItemResponse putStorageItemResponse = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem, userCredentials.ProxyCredentials));
            }
            catch (WebException webException)
            {
                HttpWebResponse webResponse = (HttpWebResponse) webException.Response;
                if (webResponse.StatusCode == HttpStatusCode.BadRequest)
                    throw new ContainerNotFoundException("The requested container does not exist");
                throw new InvalidETagException("The ETag supplied in the request does not match the ETag calculated by the server");
            }
            return null;
        }

        private string ExtractFileNameFromPath(string fileName)
        {

            int lastSlash = fileName.LastIndexOf(@"\");
            if (lastSlash > -1) return fileName.Substring(lastSlash + 1);
            return fileName;
        }
    }
}