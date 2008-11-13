using System.Net;
using System.Security.Authentication;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{
    public class MarkContainerAsPublicCommand : BaseCommand
    {
        private readonly string containerName;
        private readonly string ttl;
        private readonly string userAgentAcl;
        private readonly string referrerAcl;

        public MarkContainerAsPublicCommand(string containerName)
        {
            this.containerName = containerName;
        }

        public MarkContainerAsPublicCommand(string containerName, string ttl, string userAgentAcl, string referrerAcl)
        {
            this.containerName = containerName;
            this.ttl = ttl;
            this.userAgentAcl = userAgentAcl;
            this.referrerAcl = referrerAcl;
        }

        protected override object ExecuteCommand()
        {
            SetContainerAsPublicRequest request = new SetContainerAsPublicRequest(cdnManagementUrl, authToken, containerName, ttl, userAgentAcl, referrerAcl);
            SetContainerAsPublicResponse response = null;
            try
            {   
                response = new ResponseFactory<SetContainerAsPublicResponse>().Create(new CloudFilesRequest(request));
            }
            catch (WebException we)
            {
                //It's a protocol error that is usually a result of a 401 (Unauthorized)
                //Still trying to figure way to get specific httpstatuscode
                HttpWebResponse webResponse = (HttpWebResponse) we.Response;
                if (webResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new InvalidCredentialException("You do not have permission to mark this container as public.");
                }
                if (webResponse.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new ContainerAlreadyPublicException("The specified container is already marked as public.");
                }
            }


            return response.Headers[Constants.X_CDN_URI];
        }
    }
}