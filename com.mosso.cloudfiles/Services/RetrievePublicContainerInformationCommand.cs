using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{
    public class RetrievePublicContainerInformationCommand : BaseCommand
    {
        private readonly string containerName;

        public RetrievePublicContainerInformationCommand(string containerName)
        {
            this.containerName = containerName;
        }

        protected override object ExecuteCommand()
        {
            GetPublicContainerInformationRequest request = new GetPublicContainerInformationRequest(cdnManagementUrl, authToken, containerName);
            GetPublicContainerInformationResponse response = null;
            try
            {
                response = new ResponseFactory<GetPublicContainerInformationResponse>().Create(new CloudFilesRequest(request));
            } catch (WebException ex)
            {
                HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                if (webResponse.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Your authorization credentials are invalid or have expired.");
                if (webResponse.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The specified container does not exist.");
            }
            
            Container container = new Container(containerName);
            if (response.Headers[Constants.X_CDN_TTL] != null)
                container.TTL = response.Headers[Constants.X_CDN_TTL];
            if (response.Headers[Constants.X_USER_AGENT_ACL] != null)
                container.UserAgentAcl = response.Headers[Constants.X_USER_AGENT_ACL];
            if (response.Headers[Constants.X_REFERRER_ACL] != null)
                container.ReferrerAcl = response.Headers[Constants.X_REFERRER_ACL];
            container.CdnUri = response.Headers[Constants.X_CDN_URI];
            return container;
        }

    }
}