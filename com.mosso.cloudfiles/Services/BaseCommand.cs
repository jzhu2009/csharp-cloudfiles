///
/// See COPYING file for licensing information
///

using com.mosso.cloudfiles.domain;

namespace com.mosso.cloudfiles.services
{
    public abstract class BaseCommand
    {
        protected string storageToken;
        protected string storageUrl;
        protected UserCredentials userCredentials;
        protected abstract object ExecuteCommand();
        protected string cdnManagementUrl;
        protected string authToken;
        
        public UserCredentials UserCredentials
        {
            set { userCredentials = value; }
        }

        
        public string StorageToken
        {
            set { storageToken = value; }
        }

        public string CdnManagementUrl
        {
            set { cdnManagementUrl = value; }
        }


        public string AuthToken
        {
            set { authToken = value; }
        }
        
        public string StorageUrl
        {
            set { storageUrl = value; }
        }

       
        public object Execute()
        {
            return ExecuteCommand();
        }
    }
}