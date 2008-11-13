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
    
    internal class CreateContainerCommand : BaseCommand
    {
        private readonly string containerName;

        
        public CreateContainerCommand(string containerName)
        {
            this.containerName = containerName;
        }

      
        protected override object ExecuteCommand()
        {
            CreateContainer createContainer = new CreateContainer(storageUrl, storageToken, containerName);
            CreateContainerResponse createContainerResponse = new ResponseFactory<CreateContainerResponse>().Create(new CloudFilesRequest(createContainer, userCredentials.ProxyCredentials));
            if (createContainerResponse.Status == HttpStatusCode.Accepted)
                throw new ContainerAlreadyExistsException("The container already exists");
            return null;
        }
    }
}