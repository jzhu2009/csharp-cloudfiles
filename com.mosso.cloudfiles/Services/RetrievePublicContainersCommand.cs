using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{
    internal class RetrievePublicContainersCommand : BaseCommand
    {
        protected override object ExecuteCommand()
        {
            GetPublicContainersRequest request = new GetPublicContainersRequest(cdnManagementUrl, authToken);
            GetPublicContainersResponse response =
                new ResponseFactoryWithContentBody<GetPublicContainersResponse>().Create(new CloudFilesRequest(request));

            if (response.Status == HttpStatusCode.Unauthorized)
                throw new AuthenticationFailedException(
                    "You do not have permission to request the list of public containers.");
            List<string> containerList = response.ContentBody;
            response.Dispose();


            return response.ContentBody;
        }
    }
}