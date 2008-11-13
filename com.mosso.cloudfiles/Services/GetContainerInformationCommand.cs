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
   
    internal class GetContainerInformationCommand : BaseCommand
    {
        private readonly string containerName;

        
        public GetContainerInformationCommand(string containerName)
        {
            this.containerName = containerName;
        }

        
        protected override object ExecuteCommand()
        {
            Container container = null;
            GetContainerInformation getContainerInformation = new GetContainerInformation(storageUrl, containerName, storageToken);

            try
            {
                GetContainerInformationResponse getContainerInformationResponse = new ResponseFactory<GetContainerInformationResponse>().Create(new CloudFilesRequest(getContainerInformation, userCredentials.ProxyCredentials));
                container = new Container(containerName);
                container.ByteCount = long.Parse(getContainerInformationResponse.Headers[Constants.X_CONTAINER_BYTES_USED]);
                container.ObjectCount = long.Parse(getContainerInformationResponse.Headers[Constants.X_CONTAINER_STORAGE_OBJECT_COUNT]);
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse) we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");

                throw;
            }
            return container;
        }
    }
}