using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{
//    public class SetPublicContainerDetailsCommand : BaseCommand
//    {
//        private readonly string containerName;
//        private readonly bool isCdnEnabled;
//        private readonly string cdnTtl;
//        private readonly string userAgentAcl;
//        private readonly string referrerAcl;
//        private bool cdnTtlIsSet = false;
//
//        public SetPublicContainerDetailsCommand(string containerName, bool isCdnEnabled, string cdnTtl, string userAgentAcl, string referrerAcl)
//        {
//            cdnTtlIsSet = true;
//            this.containerName = containerName;
//            this.isCdnEnabled = isCdnEnabled;
//            this.cdnTtl = cdnTtl;
//            this.userAgentAcl = userAgentAcl;
//            this.referrerAcl = referrerAcl;
//        }
//
//        public SetPublicContainerDetailsCommand(string containerName, string cdnTtl, string userAgentAcl, string referrerAcl)
//        {
//            this.containerName = containerName;
//            this.cdnTtl = cdnTtl;
//            this.userAgentAcl = userAgentAcl;
//            this.referrerAcl = referrerAcl;
//        }
//
//
//        protected override object ExecuteCommand()
//        {
//            SetPublicContainerDetailsRequest request;
//            if (cdnTtlIsSet)
//                request = new SetPublicContainerDetailsRequest(cdnManagementUrl, authToken, containerName, isCdnEnabled, cdnTtl, userAgentAcl, referrerAcl);
//            else
//                request = new SetPublicContainerDetailsRequest(cdnManagementUrl, authToken, containerName, cdnTtl, userAgentAcl, referrerAcl);
//            SetPublicContainerDetailsResponse response = null;
//            try
//            {
//                response = new ResponseFactory<SetPublicContainerDetailsResponse>().Create(new CloudFilesRequest(request));
//            } catch (WebException ex)
//            {
//                HttpWebResponse webResponse = (HttpWebResponse) ex.Response;
//                if (webResponse.StatusCode == HttpStatusCode.Unauthorized)
//                    throw new UnauthorizedAccessException("Your access credentials are invalid or have expired. ");
//                if (webResponse.StatusCode == HttpStatusCode.NotFound)
//                    throw new ContainerNotFoundException("The specified container does not exist.");
//            }
//            
//            return response.Headers[Constants.X_CDN_URI];
//        }
//    }
}