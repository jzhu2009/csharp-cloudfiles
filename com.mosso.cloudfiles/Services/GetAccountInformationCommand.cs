///
/// See COPYING file for licensing information
///

using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;

namespace com.mosso.cloudfiles.services
{
    
    internal class GetAccountInformationCommand : BaseCommand
    {
       
        protected override object ExecuteCommand()
        {
            GetAccountInformation getAccountInformation = new GetAccountInformation(storageUrl, storageToken);
            GetAccountInformationResponse getAccountInformationResponse =
                new ResponseFactory<GetAccountInformationResponse>().Create(new CloudFilesRequest(getAccountInformation));
            return new AccountInformation(getAccountInformationResponse.Headers[Constants.X_ACCOUNT_CONTAINER_COUNT],
                                          getAccountInformationResponse.Headers[Constants.X_ACCOUNT_BYTES_USED]);
        }
    }
}