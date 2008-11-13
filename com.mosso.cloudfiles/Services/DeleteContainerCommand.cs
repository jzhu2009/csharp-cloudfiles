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
    
    internal class DeleteContainerCommand : BaseCommand
    {
        private readonly string containerName;

        
        public DeleteContainerCommand(string containerName)
        {
            this.containerName = containerName;
        }

        
        protected override object ExecuteCommand()
        {
            DeleteContainer deleteContainer = new DeleteContainer(storageUrl, containerName, storageToken);
            try
            {
                DeleteContainerResponse deleteContainerResponse = new ResponseFactory<DeleteContainerResponse>().Create(new CloudFilesRequest(deleteContainer, userCredentials.ProxyCredentials));
            }
            catch (WebException ex)
            {
                HttpWebResponse response = ((HttpWebResponse) ex.Response);
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");

                if (response != null && response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new ContainerNotEmptyException("The container you are trying to delete is not empty");
                }
            }

            return null;
        }
    }
}