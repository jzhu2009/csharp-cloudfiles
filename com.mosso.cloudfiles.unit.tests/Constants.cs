namespace com.mosso.cloudfiles.unit.tests
{
    public static class Constants
    {
        public const string X_AUTH_USER = "X-Auth-User";
        public const string X_AUTH_KEY = "X-Auth-Key";
        public const string X_STORAGE_TOKEN = "X-Storage-Token";
        public const string X_STORAGE_URL = "X-Storage-Url";
        public const string X_CONTAINER_OBJECT_COUNT = "X-Container-Object-Count";
        public const string X_CONTAINER_BYTES_USED = "X-Container-Bytes-Used";
        public const string X_META_KEY_HEADER = "X-Object-Meta-";
        public const string X_ACCOUNT_CONTAINER_COUNT = "X-Account-Container-Count";
        public const string X_ACCOUNT_BYTES_USED = "X-Account-Bytes-Used";
        public const string X_CDN_MANAGEMENT_URL = "X-CDN-Management-URL";
        public const string X_CDN_ENABLED = "X-CDN-Enabled";
        public const string X_AUTH_TOKEN = "X-Auth-Token";

        //AUTHENTICATION ITEMS
        public const string STORAGE_TOKEN = "TestToken";
        public const string STORAGE_URL = "http://tempuri";
        public const string CONTAINER_NAME = "TestContainer";

        //StorageItem constants
        public const string STORAGE_ITEM_NAME = "TestStorageItem.txt";
        public const string HEAD_STORAGE_ITEM_NAME = "HeadStorageItem.txt";
        public const string META_KEY1 = "Key1";
        public const string META_VALUE1 = "Value1";
        public const string META_KEY2 = "Key2";
        public const string META_VALUE2 = "Value2";
        public const string STORAGE_OBJECT_FILE_NAME = "StorageObjectTest.txt";
        public const string STORAGE_OBJECT_CONTENT_TYPE = "text/plain";


        //Container constants
        public const string REMOTE_CONTAINER_NAME = "RemoteContainerName";
        public const long CONTAINER_BYTES_COUNT = 1000;
        public const int CONTAINER_OBJECT_COUNT = 20;

        //Credentials constants
        public const string CREDENTIALS_USER_NAME = "testuser";
        public const string CREDENTIALS_PASSWORD = "testpass";
        public const string CREDENTIALS_CLOUD_VERSION = "v1";
        public const string CREDENTIALS_ACCOUNT_NAME = "testaccount";
        public const string PROXY_ADDRESS = "http://proxyaddress.com";
        public const string PROXY_USERNAME = "proxyuser";
        public const string PROXY_PASSWORD = "proxypassword";
        public const string PROXY_DOMAIN = "proxydomain";
        public const string AUTH_URL = "http://authurl.com";
    }
}