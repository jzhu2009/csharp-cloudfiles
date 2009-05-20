namespace com.mosso.cloudfiles.unit.tests
{
    public static class Constants
    {
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

        public const int MAX_CONTAINER_NAME_LENGTH = 256;
        public const int MAX_OBJECT_NAME_LENGTH = 1024;
    }
}