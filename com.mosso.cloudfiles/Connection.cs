///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles
{
    /// <summary>
    /// enumeration of filters to place on the request url
    /// </summary>
    public enum GetItemListParameters
    {
        Limit,
        Offset,
        Prefix,
        Path
    }

    /// <summary>
    /// This class represents the primary means of interaction between a user and cloudfiles. Methods are provided representing all of the actions
    /// one can take against his/her account, such as creating containers and downloading storage objects. 
    /// </summary>
    public class Connection : IConnection
    {
        private bool retry;

        /// <summary>
        /// A constructor used to create an instance of the Connection class
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// </code>
        /// </example>
        /// <param name="userCredentials">An instance of the UserCredentials class, containing all pertinent authentication information</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public Connection(UserCredentials userCredentials)
        {
            StorageToken = "";
            StorageUrl = "";
            if (userCredentials == null) throw new ArgumentNullException("userCredentials");

            UserCredentials = userCredentials;
            VerifyAuthentication();
        }

        protected virtual void VerifyAuthentication()
        {
            if (!IsAuthenticated())
            {
                Authenticate();
            }
        }

        private void Authenticate()
        {
            var getAuthentication = new GetAuthentication(UserCredentials);
            var getAuthenticationResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getAuthentication, UserCredentials.ProxyCredentials));
            if (getAuthenticationResponse.Status == HttpStatusCode.NoContent)
            {
                StorageToken = getAuthenticationResponse.Headers[Constants.X_STORAGE_TOKEN];
                StorageUrl = getAuthenticationResponse.Headers[Constants.X_STORAGE_URL];
                AuthToken = getAuthenticationResponse.Headers[Constants.X_AUTH_TOKEN];
                CdnManagementUrl = getAuthenticationResponse.Headers[Constants.X_CDN_MANAGEMENT_URL];
                return;
            }

            if(!retry && getAuthenticationResponse.Status == HttpStatusCode.Unauthorized)
            {
                retry = true;
                Authenticate();
                return;
            }

            throw new UnauthorizedAccessException();
        }

        private bool IsAuthenticated()
        {
            return !String.IsNullOrEmpty(StorageToken)
                   && !String.IsNullOrEmpty(StorageUrl)
                   && !String.IsNullOrEmpty(AuthToken)
                   && !String.IsNullOrEmpty(CdnManagementUrl)
                   && UserCredentials != null;
        }

        public IAccount Account
        {
            get
            {
                if (IsAuthenticated())
                    return new CF_Account(this);

                Authenticate();

                return null;
            }
        }

        public UserCredentials UserCredentials { get; private set; }
        public string StorageToken { get; private set; }
        public string StorageUrl { get; private set; }
        public string AuthToken { get; private set; }
        public string CdnManagementUrl { get; private set; }

        /// <summary>
        /// This method returns the number of containers and the size, in bytes, of the specified account
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// AccountInformation accountInformation = connection.GetAccountInformation();
        /// </code>
        /// </example>
        /// <returns>An instance of AccountInformation, containing the byte size and number of containers associated with this account</returns>
        public AccountInformation GetAccountInformation()
        {
            var getAccountInformation = new GetAccountInformation(StorageUrl, StorageToken);
            var getAccountInformationResponse =
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getAccountInformation));
            return new AccountInformation(getAccountInformationResponse.Headers[Constants.X_ACCOUNT_CONTAINER_COUNT],
                                          getAccountInformationResponse.Headers[Constants.X_ACCOUNT_BYTES_USED]);
        }

        /// <summary>
        /// Get account information in json format
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// string jsonReturnValue = connection.GetAccountInformationJson();
        /// </code>
        /// </example>
        /// <returns>JSON serialized format of the account information</returns>
        public string GetAccountInformationJson()
        {
            var getAccountInformationJson = new GetAccountInformationSerialized(StorageUrl, StorageToken, Format.JSON);
            var getAccountInformationJsonResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>()
                .Create(new CloudFilesRequest(getAccountInformationJson));

            if (getAccountInformationJsonResponse.ContentBody.Count == 0) return "";
            string jsonResponse =  String.Join("", getAccountInformationJsonResponse.ContentBody.ToArray());

            getAccountInformationJsonResponse.Dispose();
            return jsonResponse;
        }

        /// <summary>
        /// Get account information in xml format
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// XmlDocument xmlReturnValue = connection.GetAccountInformationXml();
        /// </code>
        /// </example>
        /// <returns>XML serialized format of the account information</returns>
        public XmlDocument GetAccountInformationXml()
        {
            var accountInformationXml = new GetAccountInformationSerialized(StorageUrl, StorageToken, Format.XML);
            var getAccountInformationXmlResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(accountInformationXml));

            if (getAccountInformationXmlResponse.ContentBody.Count == 0) return new XmlDocument();

            string contentBody = String.Join("", getAccountInformationXmlResponse.ContentBody.ToArray());
            getAccountInformationXmlResponse.Dispose();
            
            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(contentBody);
            }
            catch (XmlException)
            {
                return xmlDocument;
            }

            return xmlDocument;
        }

        /// <summary>
        /// This method is used to create a container on cloudfiles with a given name
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.CreateContainer("container name");
        /// </code>
        /// </example>
        /// <param name="containerName">The desired name of the container</param>
        public void CreateContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            var createContainer = new CreateContainer(StorageUrl, StorageToken, containerName);
            var createContainerResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(createContainer, UserCredentials.ProxyCredentials));
            if (createContainerResponse.Status == HttpStatusCode.Accepted)
                throw new ContainerAlreadyExistsException("The container already exists");
        }

        /// <summary>
        /// This method is used to delete a container on cloudfiles
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.DeleteContainer("container name");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to delete</param>
        public void DeleteContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            var deleteContainer = new DeleteContainer(StorageUrl, containerName, StorageToken);
            try
            {
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(deleteContainer, UserCredentials.ProxyCredentials));
            }
            catch (WebException ex)
            {
                var response = ((HttpWebResponse)ex.Response);
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");
                if (response != null && response.StatusCode == HttpStatusCode.Conflict)
                    throw new ContainerNotEmptyException("The container you are trying to delete is not empty");
                throw;
            }
        }

        /// <summary>
        /// This method retrieves a list of containers associated with a given account
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// List{string} containers = connection.GetContainers();
        /// </code>
        /// </example>
        /// <returns>An instance of List, containing the names of the containers this account owns</returns>
        public List<string> GetContainers()
        {
            //TODO: try/catch
            List<string> containerList = null;
            var getContainers = new GetContainers(StorageUrl, StorageToken);
            var getContainersResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getContainers, UserCredentials.ProxyCredentials));
            if (getContainersResponse.Status == HttpStatusCode.OK)
            {
                containerList = getContainersResponse.ContentBody;
            }
            return containerList;
        }

        /// <summary>
        /// This method retrieves the contents of a container
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// List{string} containerItemList = connection.GetContainerItemList("container name");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container</param>
        /// <returns>An instance of List, containing the names of the storage objects in the give container</returns>
        public List<string> GetContainerItemList(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            return GetContainerItemList(containerName, null);
        }

         /// <summary>
        /// This method retrieves the contents of a container
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Dictionary{GetItemListParameters, string} parameters = new Dictionary{GetItemListParameters, string}();
        /// parameters.Add(GetItemListParameters.Limit, 2);
        /// parameters.Add(GetItemListParameters.Offset, 1);
        /// parameters.Add(GetItemListParameters.Prefix, "a");
        /// List{string} containerItemList = connection.GetContainerItemList("container name", parameters);
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container</param>
        /// <param name="parameters">Parameters to feed to the request to filter the returned list</param>
        /// <returns>An instance of List, containing the names of the storage objects in the give container</returns>
        public List<string> GetContainerItemList(string containerName, Dictionary<GetItemListParameters, string> parameters)
         {
             if (string.IsNullOrEmpty(containerName))
                 throw new ArgumentNullException();

             List<string> containerItemList = new List<string>();
             try
             {
                 var getContainerItemList = new GetContainerItemList(StorageUrl, containerName,
                                                                                      StorageToken, parameters);
                 var getContainerItemListResponse =
                     new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                         new CloudFilesRequest(getContainerItemList, UserCredentials.ProxyCredentials));
                 if (getContainerItemListResponse.Status == HttpStatusCode.OK)
                 {
                     containerItemList.AddRange(getContainerItemListResponse.ContentBody);
                 }
             }
             catch (WebException we)
             {
                 var response = (HttpWebResponse)we.Response;
                 if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                     throw new ContainerNotFoundException("The requested container does not exist!");

                 throw;
             }
             return containerItemList;
         }

        /// <summary>
        /// This method retrieves the number of storage objects in a container, and the total size, in bytes, of the container
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Container container = connection.GetContainerInformation("container name");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to query about</param>
        /// <returns>An instance of container, with the number of storage objects contained and total byte allocation</returns>
        public Container GetContainerInformation(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            var getContainerInformation = new GetContainerInformation(StorageUrl, containerName, StorageToken);

            try
            {
                var getContainerInformationResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getContainerInformation, UserCredentials.ProxyCredentials));
                var container = new Container(containerName);
                container.ByteCount = long.Parse(getContainerInformationResponse.Headers[Constants.X_CONTAINER_BYTES_USED]);
                container.ObjectCount = long.Parse(getContainerInformationResponse.Headers[Constants.X_CONTAINER_STORAGE_OBJECT_COUNT]);
                return container;
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new AuthenticationFailedException(we.Message);
                throw;
            }
        }

        /// <summary>
        /// JSON serialized format of the container's objects
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// string jsonResponse = connection.GetContainerInformationJson("container name");
        /// </code>
        /// </example>
        /// <param name="containerName">name of the container to get information</param>
        /// <returns>json string of object information inside the container</returns>
        public string GetContainerInformationJson(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            try
            {
                var getContainerInformation = new GetContainerInformationSerialized(StorageUrl, StorageToken, containerName, Format.JSON);
                var getSerializedResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getContainerInformation, UserCredentials.ProxyCredentials));
                var jsonResponse = String.Join("", getSerializedResponse.ContentBody.ToArray());
                getSerializedResponse.Dispose();
                return jsonResponse;
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");

                throw;
            }
        }

        /// <summary>
        /// XML serialized format of the container's objects
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// XmlDocument xmlResponse = connection.GetContainerInformationXml("container name");
        /// </code>
        /// </example>
        /// <param name="containerName">name of the container to get information</param>
        /// <returns>xml document of object information inside the container</returns>
        public XmlDocument GetContainerInformationXml(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            try
            {
                var getContainerInformation = new GetContainerInformationSerialized(StorageUrl, StorageToken, containerName, Format.XML);
                var getSerializedResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getContainerInformation, UserCredentials.ProxyCredentials));
                var xmlResponse = String.Join("", getSerializedResponse.ContentBody.ToArray());
                getSerializedResponse.Dispose();

                if (xmlResponse == null) return new XmlDocument();

                var xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(xmlResponse);

                }
                catch (XmlException)
                {
                    return xmlDocument;
                }

                return xmlDocument;
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");

                throw;
            }
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles with meta tags
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Dictionary{string, string} metadata = new Dictionary{string, string}();
        /// metadata.Add("key1", "value1");
        /// metadata.Add("key2", "value2");
        /// metadata.Add("key3", "value3");
        /// connection.PutStorageItem("container name", "C:\Local\File\Path\file.txt", metadata);
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="storageItemName">The complete file uri of the storage object to be uploaded</param>
        /// <param name="metadata">An optional parameter containing a dictionary of meta tags to associate with the storage object</param>
        public void PutStorageItem(string containerName, string storageItemName, Dictionary<string, string> metadata)
        {
            if(string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            var remoteName = Path.GetFileName(storageItemName);
            var localName = storageItemName.Replace("/", "\\");
            try
            {
                var putStorageItem = new PutStorageItem(StorageUrl, containerName, remoteName, localName, StorageToken, metadata);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem, UserCredentials.ProxyCredentials));
            }
            catch (WebException webException)
            {
                
                var webResponse = (HttpWebResponse)webException.Response;
                if (webResponse == null) throw;
                if (webResponse.StatusCode == HttpStatusCode.BadRequest)
                    throw new ContainerNotFoundException("The requested container does not exist");
                if (webResponse.StatusCode == HttpStatusCode.PreconditionFailed)
                    throw new PreconditionFailedException(webException.Message);

                throw;
            }
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.PutStorageItem("container name", "C:\Local\File\Path\file.txt");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="storageItemName">The complete file uri of the storage object to be uploaded</param>
        public void PutStorageItem(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            PutStorageItem(containerName, storageItemName, new Dictionary<string, string>());
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles with an alternate name
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// FileInfo file = new FileInfo("C:\Local\File\Path\file.txt");
        /// connection.PutStorageItem("container name", file.Open(FileMode.Open), "RemoteFileName.txt");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="remoteStorageItemName">The alternate name as it will be called on cloudfiles</param>
        /// <param name="storageStream">The stream representing the storage item to upload</param>
        public void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(remoteStorageItemName))
                throw new ArgumentNullException();

            PutStorageItem(containerName, storageStream, remoteStorageItemName, new Dictionary<string, string>());
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles with an alternate name
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Dictionary{string, string} metadata = new Dictionary{string, string}();
        /// metadata.Add("key1", "value1");
        /// metadata.Add("key2", "value2");
        /// metadata.Add("key3", "value3");
        /// FileInfo file = new FileInfo("C:\Local\File\Path\file.txt");
        /// connection.PutStorageItem("container name", file.Open(FileMode.Open), "RemoteFileName.txt", metadata);
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="storageStream">The file stream to upload</param>
        /// <param name="metadata">An optional parameter containing a dictionary of meta tags to associate with the storage object</param>
        /// <param name="remoteStorageItemName">The name of the storage object as it will be called on cloudfiles</param>
        public void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName, Dictionary<string, string> metadata)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(remoteStorageItemName))
                throw new ArgumentNullException();


            string remoteName = Path.GetFileName(remoteStorageItemName);
            try
            {
                var putStorageItem = new PutStorageItem(StorageUrl, containerName, remoteName, storageStream, StorageToken, metadata);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem, UserCredentials.ProxyCredentials));
            }
            catch (WebException webException)
            {
                var webResponse = (HttpWebResponse)webException.Response;
                if (webResponse == null) throw;
                if (webResponse.StatusCode == HttpStatusCode.BadRequest)
                    throw new ContainerNotFoundException("The requested container does not exist");
                if (webResponse.StatusCode == HttpStatusCode.PreconditionFailed)
                    throw new PreconditionFailedException(webException.Message);

                throw;
                //following exception is cause when status code is 422 (unprocessable entity)
                //unfortunately, the HttpStatusCode enum does not have that value
                //throw new InvalidETagException("The ETag supplied in the request does not match the ETag calculated by the server");
            }
        }

        /// <summary>
        /// This method deletes a storage object in a given container
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.DeleteStorageItem("container name", "RemoteStorageItem.txt");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container that contains the storage object</param>
        /// <param name="storageItemName">The name of the storage object to delete</param>
        public void DeleteStorageItem(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            var deleteStorageItem = new DeleteStorageItem(StorageUrl, containerName, storageItemName, StorageToken);
            try
            {
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(deleteStorageItem));
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object for deletion does not exist");

                throw;
            }
        }

        /// <summary>
        /// This method downloads a storage object from cloudfiles
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// StorageItem storageItem = connection.GetStorageItem("container name", "RemoteStorageItem.txt");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container that contains the storage object to retrieve</param>
        /// <param name="storageItemName">The name of the storage object to retrieve</param>
        /// <returns>An instance of StorageItem with the stream containing the bytes representing the desired storage object</returns>
        public StorageItem GetStorageItem(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            return GetStorageItem(containerName, storageItemName, new Dictionary<RequestHeaderFields, string>());
        }

        /// <summary>
        /// An alternate method for downloading storage objects. This one allows specification of special HTTP 1.1 compliant GET headers
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials); 
        /// Dictionary{RequestHeaderFields, string} requestHeaderFields = Dictionary{RequestHeaderFields, string}();
        /// string dummy_etag = "5c66108b7543c6f16145e25df9849f7f";
        /// requestHeaderFields.Add(RequestHeaderFields.IfMatch, dummy_etag);
        /// requestHeaderFields.Add(RequestHeaderFields.IfNoneMatch, dummy_etag);
        /// requestHeaderFields.Add(RequestHeaderFields.IfModifiedSince, DateTime.Now.AddDays(6).ToString());
        /// requestHeaderFields.Add(RequestHeaderFields.IfUnmodifiedSince, DateTime.Now.AddDays(-6).ToString());
        /// requestHeaderFields.Add(RequestHeaderFields.Range, "0-5");
        /// StorageItem storageItem = connection.GetStorageItem("container name", "RemoteStorageItem.txt", requestHeaderFields);
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container that contains the storage object</param>
        /// <param name="storageItemName">The name of the storage object</param>
        /// <param name="requestHeaderFields">A dictionary containing the special headers and their values</param>
        /// <returns>An instance of StorageItem with the stream containing the bytes representing the desired storage object</returns>
        public StorageItem GetStorageItem(string containerName, string storageItemName, Dictionary<RequestHeaderFields, string> requestHeaderFields)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();
            
            try
            {
                var getStorageItem = new GetStorageItem(StorageUrl, containerName, storageItemName, StorageToken, requestHeaderFields);
                var getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem, UserCredentials.ProxyCredentials));
                var storageItem = new StorageItem(storageItemName, null, getStorageItemResponse.ContentType, getStorageItemResponse.ContentStream, long.Parse(getStorageItemResponse.ContentLength));
//                getStorageItemResponse.Dispose();
                return storageItem;
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                response.Close();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
        }



        /// <summary>
        /// An alternate method for downloading storage objects from cloudfiles directly to a file name specified in the method
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// StorageItem storageItem = connection.GetStorageItem("container name", "RemoteStorageItem.txt", "C:\Local\File\Path\file.txt");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container that contains the storage object to retrieve</param>
        /// <param name="storageItemName">The name of the storage object to retrieve</param>
        /// <param name="localFileName">The file name to save the storage object into on disk</param>
        public void GetStorageItem(string containerName, string storageItemName, string localFileName)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName) ||
                string.IsNullOrEmpty(localFileName))
                throw new ArgumentNullException();

            GetStorageItem(containerName, storageItemName, localFileName, new Dictionary<RequestHeaderFields, string>());
        }

        /// <summary>
        /// An alternate method for downloading storage objects from cloudfiles directly to a file name specified in the method
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Dictionary{RequestHeaderFields, string} requestHeaderFields = Dictionary{RequestHeaderFields, string}();
        /// string dummy_etag = "5c66108b7543c6f16145e25df9849f7f";
        /// requestHeaderFields.Add(RequestHeaderFields.IfMatch, dummy_etag);
        /// requestHeaderFields.Add(RequestHeaderFields.IfNoneMatch, dummy_etag);
        /// requestHeaderFields.Add(RequestHeaderFields.IfModifiedSince, DateTime.Now.AddDays(6).ToString());
        /// requestHeaderFields.Add(RequestHeaderFields.IfUnmodifiedSince, DateTime.Now.AddDays(-6).ToString());
        /// requestHeaderFields.Add(RequestHeaderFields.Range, "0-5");
        /// StorageItem storageItem = connection.GetStorageItem("container name", "RemoteFileName.txt", "C:\Local\File\Path\file.txt", requestHeaderFields);
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container that contains the storage object to retrieve</param>
        /// <param name="storageItemName">The name of the storage object to retrieve</param>
        /// <param name="localFileName">The file name to save the storage object into on disk</param>
        /// <param name="requestHeaderFields">A dictionary containing the special headers and their values</param>
        public void GetStorageItem(string containerName, string storageItemName, string localFileName, Dictionary<RequestHeaderFields, string> requestHeaderFields)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName) ||
                string.IsNullOrEmpty(localFileName))
                throw new ArgumentNullException();

            var getStorageItem = new GetStorageItem(StorageUrl, containerName, storageItemName, StorageToken, requestHeaderFields);
            try
            {
                var getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem, UserCredentials.ProxyCredentials));
                getStorageItemResponse.SaveStreamToDisk(localFileName);
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                response.Close();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
        }

        /// <summary>
        /// This method applies meta tags to a storage object on cloudfiles
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Dictionary{string, string} metadata = new Dictionary{string, string}();
        /// metadata.Add("key1", "value1");
        /// metadata.Add("key2", "value2");
        /// metadata.Add("key3", "value3");
        /// connection.SetStorageItemMetaInformation("container name", "C:\Local\File\Path\file.txt", metadata);
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container containing the storage object</param>
        /// <param name="storageItemName">The name of the storage object</param>
        /// <param name="metadata">A dictionary containiner key/value pairs representing the meta data for this storage object</param>
        public void SetStorageItemMetaInformation(string containerName, string storageItemName, Dictionary<string, string> metadata)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            try
            {
                var setStorageItemInformation = new SetStorageItemMetaInformation(StorageUrl, containerName, storageItemName, metadata, StorageToken);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(setStorageItemInformation, UserCredentials.ProxyCredentials));
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
        }

        /// <summary>
        /// This method retrieves meta information and size, in bytes, of a requested storage object
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// StorageItem storageItem = connection.GetStorageItemInformation("container name", "RemoteStorageItem.txt");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container that contains the storage object</param>
        /// <param name="storageItemName">The name of the storage object</param>
        /// <returns>An instance of StorageItem containing the byte size and meta information associated with the container</returns>
        public StorageItemInformation GetStorageItemInformation(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            var getStorageItemInformation = new GetStorageItemInformation(StorageUrl, containerName, storageItemName, StorageToken);
            try
            {
                var getStorageItemInformationResponse = 
                    new ResponseFactory<GetStorageItemInformationResponse>()
                    .Create(new CloudFilesRequest(getStorageItemInformation, UserCredentials.ProxyCredentials));
                var storageItemInformation = new StorageItemInformation(getStorageItemInformationResponse.Headers);

                return storageItemInformation;
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
        }

        /// <summary>
        /// This method retrieves the names of the of the containers made public on the CDN
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// List{string} containers = connection.GetPublicContainers();
        /// </code>
        /// </example>
        /// <returns>A list of the public containers</returns>
        public List<string> GetPublicContainers()
        {
            try
            {
                var getPublicContainers = new GetPublicContainers(CdnManagementUrl, AuthToken);
                var getPublicContainersResponse =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getPublicContainers));
                List<string> containerList = getPublicContainersResponse.ContentBody;
                getPublicContainersResponse.Dispose();

                return containerList;
            }
            catch(WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new AuthenticationFailedException("You do not have permission to request the list of public containers.");
                throw;
            }
        }

        /// <summary>
        /// This method sets a container as public on the CDN
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Uri containerPublicUrl = connection.MarkContainerAsPublic("copntainer name");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to mark public</param>
        /// <returns>A string representing the URL of the public container or null</returns>
        public Uri MarkContainerAsPublic(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            try
            {
                var request = new MarkContainerAsPublic(CdnManagementUrl, AuthToken, containerName);
                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(request));
                
                return response == null ? null : new Uri(response.Headers[Constants.X_CDN_URI]);
            }
            catch(WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new AuthenticationFailedException("You do not have permission to request the list of public containers.");
                throw;
            }
        }

        /// <summary>
        /// This method sets a container as private on the CDN
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Uri containerPublicUrl = connection.MarkContainerAsPublic("copntainer name");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to mark public</param>
        public void MarkContainerAsPrivate(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            try
            {
                var request = new SetPublicContainerDetails(CdnManagementUrl, AuthToken, containerName, false);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(request));
            }
            catch(WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Your access credentials are invalid or have expired. ");
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new PublicContainerNotFoundException("The specified container does not exist.");
                throw;
            }

        }

        /// <summary>
        /// Retrieves a Container object containing the public CDN information
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// Container container = connection.GetPublicContainerInformation("container name")
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to query about</param>
        /// <returns>An instance of Container with appropriate CDN information or null</returns>
        public Container GetPublicContainerInformation(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            try
            {
                var request = new GetPublicContainerInformation(CdnManagementUrl, AuthToken, containerName);
                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(request));
                if (response == null) return null;

                return new Container(containerName) { CdnUri = response.Headers[Constants.X_CDN_URI]};
            }
            catch (WebException ex)
            {
                var webResponse = (HttpWebResponse)ex.Response;
                if (webResponse != null && webResponse.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Your authorization credentials are invalid or have expired.");
                if (webResponse != null && webResponse.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The specified container does not exist.");
                throw;
            }
        }
    }
}