///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{

    internal class GetContainerItemsListCommand : BaseCommand
    {
        private readonly Dictionary<GetItemListParameters, string> parameters;
        private readonly string containerName;

        
        public GetContainerItemsListCommand(string containerName) : this(containerName, null)
        {
        }

      
        public GetContainerItemsListCommand(string containerName,
                                            Dictionary<GetItemListParameters, string> parameters)
        {
            this.containerName = containerName;
            this.parameters = parameters;
        }

        
        protected override object ExecuteCommand()
        {
            List<string> containerItemList = new List<string>();
            try
            {
                GetContainerItemList getContainerItemList = new GetContainerItemList(storageUrl, containerName,
                                                                                     storageToken, parameters);
                IResponseWithContentBody getContainerItemListResponse =
                    new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                        new CloudFilesRequest(getContainerItemList, userCredentials.ProxyCredentials));
                if (getContainerItemListResponse.Status == HttpStatusCode.OK)
                {
                    containerItemList.AddRange(getContainerItemListResponse.ContentBody);
                }
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse) we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist!");

                throw;
            }
            return containerItemList;
        }
    }
}