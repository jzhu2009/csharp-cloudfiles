///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{
    
    internal class SetStorageItemMetaInformationCommand : BaseCommand
    {
        private readonly string containerName;
        private readonly string storageObjectName;
        private readonly Dictionary<string, string> metaTags;

        public SetStorageItemMetaInformationCommand(string containerName, string storageObjectName, NameValueCollection meta)
        {
            this.containerName = containerName;
            this.storageObjectName = storageObjectName;
            metaTags = new Dictionary<string, string>();
            foreach (string s in meta.Keys)
            {
                metaTags.Add(s, meta[s]);
            }
        }


        public SetStorageItemMetaInformationCommand(string containerName, string storageObjectName, Dictionary<string, string> metaTags)
        {
            this.containerName = containerName;
            this.storageObjectName = storageObjectName;
            this.metaTags = metaTags;
        }

       
        protected override object ExecuteCommand()
        {
            SetStorageItemMetaInformation setStorageItemInformation = new SetStorageItemMetaInformation(storageUrl, containerName, storageObjectName, metaTags, storageToken);
            try
            {
                SetStorageItemMetaInformationResponse response = new ResponseFactory<SetStorageItemMetaInformationResponse>().Create(new CloudFilesRequest(setStorageItemInformation, userCredentials.ProxyCredentials));
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse) we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
            return null;
        }
    }
}