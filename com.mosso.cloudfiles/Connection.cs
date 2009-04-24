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
using com.mosso.cloudfiles.utils;

/// <example>
/// <code>
/// UserCredentials userCredentials = new UserCredentials("username", "api key");
/// IConnection connection = new Connection(userCredentials);
/// </code>
/// </example>
namespace com.mosso.cloudfiles
{
    /// <summary>
    /// enumeration of filters to place on the request url
    /// </summary>
    public enum GetItemListParameters
    {
        Limit,
        Marker,
        Prefix,
        Path
    }

    /// <summary>
    /// This class represents the primary means of interaction between a user and cloudfiles. Methods are provided representing all of the actions
    /// one can take against his/her account, such as creating containers and downloading storage objects. 
    /// </summary>
    /// <example>
    /// <code>
    /// UserCredentials userCredentials = new UserCredentials("username", "api key");
    /// IConnection connection = new Connection(userCredentials);
    /// </code>
    /// </example>
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
            Log.EnsureInitialized();
            AuthToken = "";
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
            Log.Info(this, "Authenticating user " + UserCredentials.Username);
            try
            {
                var getAuthentication = new GetAuthentication(UserCredentials);
                var getAuthenticationResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getAuthentication, UserCredentials.ProxyCredentials));
                if (getAuthenticationResponse.Status == HttpStatusCode.NoContent)
                {
                    StorageUrl = getAuthenticationResponse.Headers[Constants.X_STORAGE_URL];
                    AuthToken = getAuthenticationResponse.Headers[Constants.X_AUTH_TOKEN];
                    CdnManagementUrl = getAuthenticationResponse.Headers[Constants.X_CDN_MANAGEMENT_URL];
                    return;
                }

                if (!retry && getAuthenticationResponse.Status == HttpStatusCode.Unauthorized)
                {
                    retry = true;
                    Authenticate();
                    return;
                }    
            }
            catch(Exception ex)
            {
                Log.Error(this, "Error authenticating user " + UserCredentials.Username, ex);
                throw;
            }
        }

        private bool IsAuthenticated()
        {
            return !String.IsNullOrEmpty(AuthToken)
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

        /// <summary>
        /// The user credentials used to authenticate against cloud files
        /// </summary>
        public UserCredentials UserCredentials { get; private set; }

        /// <summary>
        /// The storage url used to interact with cloud files
        /// </summary>
        public string StorageUrl { get; private set; }

        /// <summary>
        /// the session based token used to ensure the user was authenticated
        /// </summary>
        public string AuthToken { get; private set; }

        /// <summary>
        /// the public cdn url for the authenticated user
        /// </summary>
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
            Log.Info(this, "Getting account information for user " + UserCredentials.Username);
            try
            {
                var getAccountInformation = new GetAccountInformation(StorageUrl, AuthToken);
                var getAccountInformationResponse =
                    new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getAccountInformation));
                return new AccountInformation(getAccountInformationResponse.Headers[Constants.X_ACCOUNT_CONTAINER_COUNT],
                                              getAccountInformationResponse.Headers[Constants.X_ACCOUNT_BYTES_USED]);
            }
            catch (Exception ex)
            {
                Log.Error(this, "Error getting account information for user " 
                        + UserCredentials.Username, ex);
                throw;
            }
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
            try
            {
                Log.Info(this, "Getting account information (JSON format) for user " + UserCredentials.Username);
                var getAccountInformationJson = new GetAccountInformationSerialized(StorageUrl, AuthToken, Format.JSON);
                var getAccountInformationJsonResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>()
                    .Create(new CloudFilesRequest(getAccountInformationJson));

                if (getAccountInformationJsonResponse.ContentBody.Count == 0) return "";
                var jsonResponse = String.Join("", getAccountInformationJsonResponse.ContentBody.ToArray());

                getAccountInformationJsonResponse.Dispose();
                return jsonResponse;    
            }
            catch (Exception ex)
            {
                Log.Error(this, "Error getting account information (JSON format) for user "
                                + UserCredentials.Username, ex);
                throw;
            }
            
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
            try
            {
                Log.Info(this, "Getting account information (XML format) for user " + UserCredentials.Username);
                var accountInformationXml = new GetAccountInformationSerialized(StorageUrl, AuthToken, Format.XML);
                var getAccountInformationXmlResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(accountInformationXml));

                if (getAccountInformationXmlResponse.ContentBody.Count == 0) return new XmlDocument();

                var contentBody = String.Join("", getAccountInformationXmlResponse.ContentBody.ToArray());
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
            catch(Exception ex)
            {
                Log.Error(this, "Error getting account information (XML format) for user "
                    + UserCredentials.Username, ex);
                throw;
            }
            
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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void CreateContainer(string containerName)
        {
            try
            {
                if (string.IsNullOrEmpty(containerName))
                    throw new ArgumentNullException();

                Log.Info(this, "Creating container " + containerName + " for user " + UserCredentials.Username);

                var createContainer = new CreateContainer(StorageUrl, AuthToken, containerName);
                var createContainerResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(createContainer, UserCredentials.ProxyCredentials));
                if (createContainerResponse.Status == HttpStatusCode.Accepted)
                    throw new ContainerAlreadyExistsException("The container already exists");    
            }
            catch(Exception ex)
            {
                Log.Error(this, "Error creating container "
                    + containerName + " for user "
                    + UserCredentials.Username, ex);
                throw;
            }
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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void DeleteContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Log.Info(this, "Deleting container " + containerName + " for user " + UserCredentials.Username);

            try
            {
                var deleteContainer = new DeleteContainer(StorageUrl, AuthToken, containerName);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(deleteContainer, UserCredentials.ProxyCredentials));
            }
            catch (WebException ex)
            {
                Log.Error(this, "Error deleting container "
                    + containerName + " for user "
                    + UserCredentials.Username, ex);

                var response = ((HttpWebResponse)ex.Response);
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container " + containerName + " does not exist");
                if (response != null && response.StatusCode == HttpStatusCode.Conflict)
                    throw new ContainerNotEmptyException("The container you are trying to delete " + containerName +"is not empty");
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
            Log.Info(this, "Getting containers for user " + UserCredentials.Username);
            try
            {
                List<string> containerList = null;
                var getContainers = new GetContainers(StorageUrl, AuthToken);
                var getContainersResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getContainers, UserCredentials.ProxyCredentials));
                if (getContainersResponse.Status == HttpStatusCode.OK)
                {
                    containerList = getContainersResponse.ContentBody;
                }
                return containerList;    
            }
            catch(Exception ex)
            {
                Log.Error(this, "Error getting containers for user " + UserCredentials.Username, ex);
                throw;
            }
            
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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public List<string> GetContainerItemList(string containerName)
        {
            try
            {
                if (string.IsNullOrEmpty(containerName))
                    throw new ArgumentNullException();

                Log.Info(this, "Getting container item list for container "
                    + containerName + " for user "
                    + UserCredentials.Username);

                return GetContainerItemList(containerName, null);    
            }
            catch (Exception ex)
            {

                Log.Error(this, "Error getting item list for container "
                    + containerName + " for user "
                    + UserCredentials.Username, ex);
                throw;
            }
        }

        /// <summary>
        /// This method ensures directory objects created for the entire path
        /// </summary>
        /// <example>
        /// <code>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.MakePath("/dir1/dir2/dir3/dir4/file.txt");
        /// </code>
        /// </example>
        /// <param name="containerName">The container to create the directory objects in</param>
        /// <param name="path">The path of directory objects to create</param>
        /// <returns>An instance of List, containing the names of the storage objects in the give container</returns>
        public void MakePath(string containerName, string path)
        {
            try
            {
                Log.Info(this, "Make path "
                + path + " for container "
                + containerName + " for user "
                + UserCredentials.Username);

                var directories = path.StripSlashPrefix().Split('/');
                var directory = "";
                var firstItem = true;
                foreach (var item in directories)
                {
                    if (string.IsNullOrEmpty(item)) continue;
                    if (item.IndexOf('.') > 0) continue;
                    if (!firstItem) directory += "/";
                    directory += item.Encode();
                    PutStorageItem(containerName, new MemoryStream(new byte[0]), directory);
                    firstItem = false;
                }    
            }
            catch(Exception ex)
            {
                Log.Error(this, "Error making path "
                    + path + " in container "
                    + containerName + " for user "
                    + UserCredentials.Username, ex);
                throw;
            }
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
        /// parameters.Add(GetItemListParameters.Marker, 1);
        /// parameters.Add(GetItemListParameters.Prefix, "a");
        /// List{string} containerItemList = connection.GetContainerItemList("container name", parameters);
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container</param>
        /// <param name="parameters">Parameters to feed to the request to filter the returned list</param>
        /// <returns>An instance of List, containing the names of the storage objects in the give container</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public List<string> GetContainerItemList(string containerName, Dictionary<GetItemListParameters, string> parameters)
         {
             if (string.IsNullOrEmpty(containerName))
                 throw new ArgumentNullException();
            
             Log.Info(this, "Getting container item list for container " 
                 + containerName + " for user " 
                 + UserCredentials.Username);

            var containerItemList = new List<string>();
            
             try
             {
                 var getContainerItemList = new GetContainerItemList(StorageUrl, AuthToken, containerName, parameters);
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
                 Log.Error(this, "Error getting containers item list for container "
                    + containerName + " for user "
                    + UserCredentials.Username, we);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public Container GetContainerInformation(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting container information for container " 
                + containerName + " for user " 
                + UserCredentials.Username);

            try
            {
                var getContainerInformation = new GetContainerInformation(StorageUrl, AuthToken, containerName);
                var getContainerInformationResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getContainerInformation, UserCredentials.ProxyCredentials));
                var container = new Container(containerName);
                container.ByteCount = long.Parse(getContainerInformationResponse.Headers[Constants.X_CONTAINER_BYTES_USED]);
                container.ObjectCount = long.Parse(getContainerInformationResponse.Headers[Constants.X_CONTAINER_STORAGE_OBJECT_COUNT]);
                return container;
            }
            catch (WebException we)
            {
                Log.Error(this, "Error getting container information for container "
                    + containerName + " for user "
                    + UserCredentials.Username, we);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public string GetContainerInformationJson(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting container information (JSON format) for container " 
                + containerName + " for user " 
                + UserCredentials.Username);

            try
            {
                var getContainerInformation = new GetContainerInformationSerialized(StorageUrl, AuthToken, containerName, Format.JSON);
                var getSerializedResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getContainerInformation, UserCredentials.ProxyCredentials));
                var jsonResponse = String.Join("", getSerializedResponse.ContentBody.ToArray());
                getSerializedResponse.Dispose();
                return jsonResponse;
            }
            catch (WebException we)
            {
                Log.Error(this, "Error getting container information (JSON format) for container "
                    + containerName + " for user "
                    + UserCredentials.Username, we);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public XmlDocument GetContainerInformationXml(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting container information (XML format) for container " 
                + containerName + " for user " 
                + UserCredentials.Username);
            
            try
            {
                var getContainerInformation = new GetContainerInformationSerialized(StorageUrl, AuthToken, containerName, Format.XML);
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
                Log.Error(this, "Error getting container information (XML format) for container"
                    + containerName + " for user "
                    + UserCredentials.Username, we);

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
        /// <param name="localFilePath">The complete file path of the storage object to be uploaded</param>
        /// <param name="metadata">An optional parameter containing a dictionary of meta tags to associate with the storage object</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void PutStorageItem(string containerName, string localFilePath, Dictionary<string, string> metadata)
        {
            if(string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(localFilePath))
                throw new ArgumentNullException();

            Log.Info(this, "Putting storage item " 
                + localFilePath + " with metadata into container " 
                + containerName + " for user " 
                + UserCredentials.Username);

            try
            {
                var remoteName = Path.GetFileName(localFilePath);
                var localName = localFilePath.Replace("/", "\\");
                var putStorageItem = new PutStorageItem(StorageUrl, AuthToken, containerName, remoteName, localName, metadata);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem, UserCredentials.ProxyCredentials));
            }
            catch (WebException webException)
            {
                Log.Error(this, "Error putting storage item "
                    + localFilePath + " with metadata into container "
                    + containerName + " for user "
                    + UserCredentials.Username, webException);
                
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
        /// <param name="localFilePath">The complete file path of the storage object to be uploaded</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void PutStorageItem(string containerName, string localFilePath)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(localFilePath))
                throw new ArgumentNullException();

            Log.Info(this, "Putting storage item " 
                + localFilePath + " into container " 
                + containerName + " for user " 
                + UserCredentials.Username);

            PutStorageItem(containerName, localFilePath, new Dictionary<string, string>());
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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(remoteStorageItemName))
                throw new ArgumentNullException();

            Log.Info(this, "Putting storage item stream into container " 
                + containerName + " named " 
                + remoteStorageItemName + "for user " 
                + UserCredentials.Username);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName, Dictionary<string, string> metadata)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(remoteStorageItemName))
                throw new ArgumentNullException();

            Log.Info(this, "Putting storage item stream with metadata into container " 
                + containerName + " named " 
                + remoteStorageItemName + " for user " 
                + UserCredentials.Username);

            try
            {
                var putStorageItem = new PutStorageItem(StorageUrl, AuthToken, containerName, remoteStorageItemName, storageStream, metadata);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem, UserCredentials.ProxyCredentials));
            }
            catch (WebException webException)
            {
                Log.Error(this, "Error putting storage item stream with metadata into container "
                    + containerName + " named "
                    + remoteStorageItemName + " for user "
                    + UserCredentials.Username, webException);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void DeleteStorageItem(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            Log.Info(this, "Deleting storage item "
                + storageItemName + " in container "
                + containerName + " for user "
                + UserCredentials.Username);

            try
            {
                var deleteStorageItem = new DeleteStorageItem(StorageUrl, AuthToken, containerName, storageItemName);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(deleteStorageItem));
            }
            catch (WebException we)
            {
                Log.Error(this, "Error deleting storage item "
                    + storageItemName + " in container "
                    + containerName + " for user "
                    + UserCredentials.Username, we);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public StorageItem GetStorageItem(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting storage item "
                + storageItemName + " in container "
                + containerName + " for user "
                + UserCredentials.Username);


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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public StorageItem GetStorageItem(string containerName, string storageItemName, Dictionary<RequestHeaderFields, string> requestHeaderFields)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting storage item "
                + storageItemName + " with request Header fields in container "
                + containerName + " for user "
                + UserCredentials.Username);

            try
            {
                var getStorageItem = new GetStorageItem(StorageUrl, AuthToken, containerName, storageItemName, requestHeaderFields);
                var getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem, UserCredentials.ProxyCredentials));
                var metadata = GetMetadata(getStorageItemResponse);
                var storageItem = new StorageItem(storageItemName, metadata, getStorageItemResponse.ContentType, getStorageItemResponse.ContentStream, long.Parse(getStorageItemResponse.ContentLength));
//                getStorageItemResponse.Dispose();
                return storageItem;
            }
            catch (WebException we)
            {
                Log.Error(this, "Error getting storage item "
                    + storageItemName + " with request Header fields in container "
                    + containerName + " for user "
                    + UserCredentials.Username, we);

                var response = (HttpWebResponse)we.Response;
                response.Close();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
        }

        private Dictionary<string, string> GetMetadata(GetStorageItemResponse getStorageItemResponse)
        {
            var metadata = new Dictionary<string, string>();
            var headers = getStorageItemResponse.Headers;
            foreach (var key in headers.AllKeys)
            {
                if (key.IndexOf(Constants.META_DATA_HEADER) > -1)
                    metadata.Add(key, headers[key]);
            }
            return metadata;
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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void GetStorageItem(string containerName, string storageItemName, string localFileName)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName) ||
                string.IsNullOrEmpty(localFileName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting storage item "
                + storageItemName + " in container "
                + containerName + " for user "
                + UserCredentials.Username + " and name it " 
                + localFileName + " locally");

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void GetStorageItem(string containerName, string storageItemName, string localFileName, Dictionary<RequestHeaderFields, string> requestHeaderFields)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName) ||
                string.IsNullOrEmpty(localFileName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting storage item "
                + storageItemName + " with request Header fields in container "
                + containerName + " for user "
                + UserCredentials.Username + " and name it "
                + localFileName + " locally");

            var getStorageItem = new GetStorageItem(StorageUrl, AuthToken, containerName, storageItemName, requestHeaderFields);
            try
            {
                var getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem, UserCredentials.ProxyCredentials));
                getStorageItemResponse.SaveStreamToDisk(localFileName);
            }
            catch (WebException we)
            {
                Log.Error(this, "Error getting storage item "
                    + storageItemName + " with request Header fields in container "
                    + containerName + " for user "
                    + UserCredentials.Username, we);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void SetStorageItemMetaInformation(string containerName, string storageItemName, Dictionary<string, string> metadata)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            Log.Info(this, "Setting storage item "
                + storageItemName + " meta information for container "
                + containerName + " for user");

            try
            {
                var setStorageItemInformation = new SetStorageItemMetaInformation(StorageUrl, AuthToken, containerName, storageItemName, metadata);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(setStorageItemInformation, UserCredentials.ProxyCredentials));
            }
            catch (WebException we)
            {
                Log.Error(this, "Error setting metainformation for storage item "
                    + storageItemName + " in container "
                    + containerName + " for user "
                    + UserCredentials.Username, we);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public StorageItemInformation GetStorageItemInformation(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting storage item "
                + storageItemName + " information in container "
                + containerName + " for user");

            try
            {
                var getStorageItemInformation = new GetStorageItemInformation(StorageUrl, AuthToken, containerName, storageItemName);
                var getStorageItemInformationResponse = 
                    new ResponseFactory<CloudFilesResponse>()
                    .Create(new CloudFilesRequest(getStorageItemInformation, UserCredentials.ProxyCredentials));
                var storageItemInformation = new StorageItemInformation(getStorageItemInformationResponse.Headers);

                return storageItemInformation;
            }
            catch (WebException we)
            {
                Log.Error(this, "Error getting storage item "
                    + storageItemName + " information in container "
                    + containerName + " for user "
                    + UserCredentials.Username, we);
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
            Log.Info(this, "Getting public containers for user " + UserCredentials.Username);

            try
            {
                var getPublicContainers = new GetPublicContainers(CdnManagementUrl, AuthToken);
                var getPublicContainersResponse =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getPublicContainers));
                var containerList = getPublicContainersResponse.ContentBody;
                getPublicContainersResponse.Dispose();

                return containerList;
            }
            catch(WebException we)
            {
                Log.Error(this, "Error getting public containers for user " + UserCredentials.Username, we);
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
        /// Uri containerPublicUrl = connection.MarkContainerAsPublic("container name");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to mark public</param>
        /// <returns>A string representing the URL of the public container or null</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public Uri MarkContainerAsPublic(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Log.Info(this, "Marking container "
                + containerName + " as public for user "
                + UserCredentials.Username);

            try
            {
                var request = new MarkContainerAsPublic(CdnManagementUrl, AuthToken, containerName);
                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(request));
                
                return response == null ? null : new Uri(response.Headers[Constants.X_CDN_URI]);
            }
            catch(WebException we)
            {
                Log.Error(this, "Error marking container " 
                    + containerName + " as public for user "
                    + UserCredentials.Username, we);
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
        /// connection.MarkContainerAsPrivate("container name");
        /// </code>
        /// </example>
        /// <param name="containerName">The name of the container to mark public</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public void MarkContainerAsPrivate(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Log.Info(this, "Marking container "
                + containerName + " as private for user "
                + UserCredentials.Username);

            try
            {
                var request = new SetPublicContainerDetails(CdnManagementUrl, AuthToken, containerName, false);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(request));
            }
            catch(WebException we)
            {
                Log.Error(this, "Error marking container "
                    + containerName + " as private for user "
                    + UserCredentials.Username, we);

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
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public Container GetPublicContainerInformation(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Log.Info(this, "Getting public container "
                    + containerName + " information for user "
                    + UserCredentials.Username);

            try
            {
                var request = new GetPublicContainerInformation(CdnManagementUrl, AuthToken, containerName);
                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(request));
                return response == null ? null : new Container(containerName) { CdnUri = response.Headers[Constants.X_CDN_URI]};
            }
            catch (WebException ex)
            {
                Log.Error(this, "Error getting public container "
                    + containerName + " information for user "
                    + UserCredentials.Username, ex);

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