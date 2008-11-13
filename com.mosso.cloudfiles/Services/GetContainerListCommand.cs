///
/// See COPYING file for licensing information
///

using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;

namespace com.mosso.cloudfiles.services
{
   
    internal class GetContainerListCommand : BaseCommand
    {
        
        protected override object ExecuteCommand()
        {
            List<string> containerList = null;
            GetContainers getContainers = new GetContainers(storageUrl, storageToken);
            GetContainersResponse getContainersResponse = new ResponseFactoryWithContentBody<GetContainersResponse>().Create(new CloudFilesRequest(getContainers, userCredentials.ProxyCredentials));
            if (getContainersResponse.Status == HttpStatusCode.OK)
            {
                containerList = getContainersResponse.ContentBody;
            }
            return containerList;
        }
    }
}