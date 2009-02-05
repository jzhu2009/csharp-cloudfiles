///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Xml;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.services
{
    /// <summary>
    /// This class represents the primary means of interaction between a user and cloudfiles. Methods are provided representing all of the actions
    /// one can take against his/her account, such as creating containers and downloading storage objects. 
    /// </summary>
    public class Connection : IConnection
    {
        private readonly UserCredentials userCredentials;
        private string storageToken;
        private string storageUrl;
        private string authToken;
        private string cdnManagementUrl;
        private bool retry = false;

        /// <summary>
        /// A constructor used to create an instance of the Connection class
        /// </summary>
        /// <param name="userCredentials">An instance of the UserCredentials class, containing all pertinent authentication information</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the reference parameters are null</exception>
        public Connection(UserCredentials userCredentials)
        {
            storageToken = "";
            storageUrl = "";
            if (userCredentials == null) throw new ArgumentNullException("You must provide an instance of UserCredentials");

            this.userCredentials = userCredentials;
            VerifyAuthentication();
        }

        protected virtual void VerifyAuthentication()
        {
            if (storageToken.Length == 0)
            {
                Authenticate();
            }
        }

        private void Authenticate()
        {
            GetAuthentication getAuthentication = new GetAuthentication(userCredentials);
            GetAuthenticationResponse getAuthenticationResponse = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(getAuthentication, userCredentials.ProxyCredentials));
            if (getAuthenticationResponse.Status == HttpStatusCode.NoContent)
            {
                storageToken = getAuthenticationResponse.StorageToken;
                storageUrl = getAuthenticationResponse.StorageUrl;
                authToken = getAuthenticationResponse.Headers[Constants.X_AUTH_TOKEN];
                cdnManagementUrl = getAuthenticationResponse.Headers[Constants.X_CDN_MANAGEMENT_URL];
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

        /// <summary>
        /// This method returns the number of containers and the size, in bytes, of the specified account
        /// </summary>
        /// <returns>An instance of AccountInformation, containing the byte size and number of containers associated with this account</returns>
        public AccountInformation GetAccountInformation()
        {
            GetAccountInformation getAccountInformation = new GetAccountInformation(storageUrl, storageToken);
            GetAccountInformationResponse getAccountInformationResponse =
                new ResponseFactory<GetAccountInformationResponse>().Create(new CloudFilesRequest(getAccountInformation));
            return new AccountInformation(getAccountInformationResponse.Headers[Constants.X_ACCOUNT_CONTAINER_COUNT],
                                          getAccountInformationResponse.Headers[Constants.X_ACCOUNT_BYTES_USED]);
        }

        /// <summary>
        /// Get account information in json format
        /// </summary>
        /// <example>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.PutStorageItem("container name", new FileInfo("file path"));
        /// string jsonReturnValue = connection.GetAccountInformationJson();
        /// </example>
        /// <returns>JSON serialized format of the account information</returns>
        public string GetAccountInformationJson()
        {
            GetAccountInformationSerialized getAccountInformationJson = new GetAccountInformationSerialized(storageUrl, storageToken, Format.JSON);
            GetAccountInformationSerializedResponse getAccountInformationJsonResponse 
                = new ResponseFactoryWithContentBody<GetAccountInformationSerializedResponse>()
                .Create(new CloudFilesRequest(getAccountInformationJson));

            if (getAccountInformationJsonResponse.ContentBody.Count == 0) return "";

            var jsonReturnValue = "";
            foreach (string s in getAccountInformationJsonResponse.ContentBody)
            {
                jsonReturnValue += s;
            }
            getAccountInformationJsonResponse.Dispose();
            return jsonReturnValue;
        }

        /// <summary>
        /// Get account information in xml format
        /// </summary>
        /// <example>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.PutStorageItem("container name", new FileInfo("file path"));
        /// XmlDocument xmlReturnValue = connection.GetAccountInformationXml();
        /// </example>
        /// <returns>XML serialized format of the account information</returns>
        public XmlDocument GetAccountInformationXml()
        {
            GetAccountInformationSerialized accountInformationXml = new GetAccountInformationSerialized(storageUrl, storageToken, Format.XML);
            GetAccountInformationSerializedResponse getAccountInformationXmlResponse = new ResponseFactoryWithContentBody<GetAccountInformationSerializedResponse>().Create(new CloudFilesRequest(accountInformationXml));

            if (getAccountInformationXmlResponse.ContentBody.Count == 0) return null;

            string contentBody = "";
            foreach (string s in getAccountInformationXmlResponse.ContentBody)
            {
                contentBody += s;
            }

            getAccountInformationXmlResponse.Dispose();
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(contentBody);
            }
            catch (XmlException)
            {
                return null;
            }

            return xmlDocument;
        }

        /// <summary>
        /// This method is used to create a container on cloudfiles with a given name
        /// </summary>
        /// <example>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.CreateContainer("container name");
        /// </example>
        /// <param name="containerName">The desired name of the container</param>
        public void CreateContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            CreateContainer createContainer = new CreateContainer(storageUrl, storageToken, containerName);
            CreateContainerResponse createContainerResponse = new ResponseFactory<CreateContainerResponse>().Create(new CloudFilesRequest(createContainer, userCredentials.ProxyCredentials));
            if (createContainerResponse.Status == HttpStatusCode.Accepted)
                throw new ContainerAlreadyExistsException("The container already exists");
        }

        /// <summary>
        /// This method is used to delete a container on cloudfiles
        /// </summary>
        /// <example>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// connection.DeleteContainer("container name");
        /// </example>
        /// <param name="containerName">The name of the container to delete</param>
        public void DeleteContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            DeleteContainer deleteContainer = new DeleteContainer(storageUrl, containerName, storageToken);
            try
            {
                DeleteContainerResponse deleteContainerResponse = new ResponseFactory<DeleteContainerResponse>().Create(new CloudFilesRequest(deleteContainer, userCredentials.ProxyCredentials));
            }
            catch (WebException ex)
            {
                HttpWebResponse response = ((HttpWebResponse)ex.Response);
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");

                if (response != null && response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new ContainerNotEmptyException("The container you are trying to delete is not empty");
                }
            }
        }

        /// <summary>
        /// This method retrieves a list of containers associated with a given account
        /// </summary>
        /// <example>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// List{string} containers = connection.GetContainers();
        /// </example>
        /// <returns>An instance of List, containing the names of the containers this account owns</returns>
        public List<string> GetContainers()
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

        /// <summary>
        /// This method retrieves the contents of a container
        /// </summary>
        /// <example>
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// List{string} containerItemList = connection.GetContainerItemList("container name");
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
        /// UserCredentials userCredentials = new UserCredentials("username", "api key");
        /// IConnection connection = new Connection(userCredentials);
        /// List{string} containerItemList = connection.GetContainerItemList("container name);
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
                 GetContainerItemList getContainerItemList = new GetContainerItemList(storageUrl, containerName,
                                                                                      storageToken, parameters);
                 IResponseWithContentBody getContainerItemListResponse =
                     new ResponseFactoryWithContentBody<GetContainerItemListResponse>().Create(
                         new CloudFilesRequest(getContainerItemList, userCredentials.ProxyCredentials));
                 if (getContainerItemListResponse.Status == HttpStatusCode.OK)
                 {
                     containerItemList.AddRange(getContainerItemListResponse.ContentBody);
                 }
             }
             catch (WebException we)
             {
                 HttpWebResponse response = (HttpWebResponse)we.Response;
                 if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                     throw new ContainerNotFoundException("The requested container does not exist!");

                 throw;
             }
             return containerItemList;
         }

        /// <summary>
        /// This method retrieves the number of storage objects in a container, and the total size, in bytes, of the container
        /// </summary>
        /// <param name="containerName">The name of the container to query about</param>
        /// <returns>An instance of container, with the number of storage objects contained and total byte allocation</returns>
        public Container GetContainerInformation(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            Container container = null;
            GetContainerInformation getContainerInformation = new GetContainerInformation(storageUrl, containerName, storageToken);

            try
            {
                GetContainerInformationResponse getContainerInformationResponse = new ResponseFactory<GetContainerInformationResponse>().Create(new CloudFilesRequest(getContainerInformation, userCredentials.ProxyCredentials));
                container = new Container(containerName);
                container.ByteCount = long.Parse(getContainerInformationResponse.Headers[Constants.X_CONTAINER_BYTES_USED]);
                container.ObjectCount = long.Parse(getContainerInformationResponse.Headers[Constants.X_CONTAINER_STORAGE_OBJECT_COUNT]);
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");

                throw;
            }
            return container;
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles with meta tags
        /// </summary>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="storageItemName">The complete file uri of the storage object to be uploaded</param>
        /// <param name="metadata">An optional parameter containing a dictionary of meta tags to associate with the storage object</param>
        public void PutStorageItem(string containerName, string storageItemName, Dictionary<string, string> metadata)
        {
            if(string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            string remoteName = Path.GetFileName(storageItemName);
            string localName = storageItemName.Replace("/", "\\");
            try
            {
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, remoteName, localName, storageToken, metadata);
                PutStorageItemResponse putStorageItemResponse = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem, userCredentials.ProxyCredentials));
            }
            catch (WebException webException)
            {
                HttpWebResponse webResponse = (HttpWebResponse)webException.Response;
                if (webResponse.StatusCode == HttpStatusCode.BadRequest)
                    throw new ContainerNotFoundException("The requested container does not exist");
                throw new InvalidETagException("The ETag supplied in the request does not match the ETag calculated by the server");
            }
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles
        /// </summary>
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
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, remoteName, storageStream, storageToken, metadata);
                PutStorageItemResponse putStorageItemResponse = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem, userCredentials.ProxyCredentials));
            }
            catch (WebException webException)
            {
                HttpWebResponse webResponse = (HttpWebResponse)webException.Response;
                if (webResponse.StatusCode == HttpStatusCode.BadRequest)
                    throw new ContainerNotFoundException("The requested container does not exist");
                throw new InvalidETagException("The ETag supplied in the request does not match the ETag calculated by the server");
            }
        }

        /// <summary>
        /// This method deletes a storage object in a given container
        /// </summary>
        /// <param name="containerName">The name of the container that contains the storage object</param>
        /// <param name="storageItemName">The name of the storage object to delete</param>
        public void DeleteStorageItem(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
                string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            DeleteStorageItem deleteStorageItem = new DeleteStorageItem(storageUrl, containerName, storageItemName, storageToken);
            try
            {
                DeleteStorageItemResponse deleteStorageItemResponse = new ResponseFactory<DeleteStorageItemResponse>().Create(new CloudFilesRequest(deleteStorageItem));
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object for deletion does not exist");

                throw;
            }
        }

        /// <summary>
        /// This method downloads a storage object from cloudfiles
        /// </summary>
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
        /// <param name="containerName">The name of the container that contains the storage object</param>
        /// <param name="storageItemName">The name of the storage object</param>
        /// <param name="requestHeaderFields">A dictionary containing the special headers and their values</param>
        /// <returns>An instance of StorageItem with the stream containing the bytes representing the desired storage object</returns>
        public StorageItem GetStorageItem(string containerName, string storageItemName, Dictionary<RequestHeaderFields, string> requestHeaderFields)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            StorageItem storageItem = null;
            GetStorageItemResponse getStorageItemResponse = null;
            GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, storageItemName, storageToken, requestHeaderFields);
            try
            {
                 getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem, userCredentials.ProxyCredentials));
                storageItem = new StorageItem(storageItemName, null, getStorageItemResponse.ContentType, getStorageItemResponse.ContentStream, long.Parse(getStorageItemResponse.ContentLength));
                
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                response.Close();
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
//            finally
//            {
//                if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
//            }
            return storageItem;
        }



        /// <summary>
        /// An alternate method for downloading storage objects from cloudfiles directly to a file name specified in the method
        /// </summary>
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

            GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, storageItemName, storageToken, requestHeaderFields);
            try
            {
                GetStorageItemResponse getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem, userCredentials.ProxyCredentials));
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
        /// <param name="containerName">The name of the container containing the storage object</param>
        /// <param name="storageItemName">The name of the storage object</param>
        /// <param name="metadata">A dictionary containiner key/value pairs representing the meta data for this storage object</param>
        public void SetStorageItemMetaInformation(string containerName, string storageItemName, Dictionary<string, string> metadata)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            SetStorageItemMetaInformation setStorageItemInformation = new SetStorageItemMetaInformation(storageUrl, containerName, storageItemName, metadata, storageToken);
            try
            {
                SetStorageItemMetaInformationResponse response = new ResponseFactory<SetStorageItemMetaInformationResponse>().Create(new CloudFilesRequest(setStorageItemInformation, userCredentials.ProxyCredentials));
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new StorageItemNotFoundException("The requested storage object does not exist");

                throw;
            }
        }

        /// <summary>
        /// This method retrieves meta information and size, in bytes, of a requested storage object
        /// </summary>
        /// <param name="containerName">The name of the container that contains the storage object</param>
        /// <param name="storageItemName">The name of the storage object</param>
        /// <returns>An instance of StorageItem containing the byte size and meta information associated with the container</returns>
        public StorageItem GetStorageItemInformation(string containerName, string storageItemName)
        {
            if (string.IsNullOrEmpty(containerName) ||
               string.IsNullOrEmpty(storageItemName))
                throw new ArgumentNullException();

            StorageItem storageItem = null;
            GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation(storageUrl, containerName, storageItemName, storageToken);
            try
            {
                GetStorageItemInformationResponse getStorageItemResponse = new ResponseFactory<GetStorageItemInformationResponse>().Create(new CloudFilesRequest(getStorageItemInformation, userCredentials.ProxyCredentials));
                storageItem = new StorageItem(storageItemName, getStorageItemResponse.Metadata, getStorageItemResponse.ContentType, long.Parse(getStorageItemResponse.ContentLength));
            }
            catch (WebException)
            {
                throw new StorageItemNotFoundException("The requested storage object does not exist");
            }
            return storageItem;
        }

        /// <summary>
        /// This method retrieves the names of the of the containers made public on the CDN
        /// </summary>
        /// <returns>A list of the public containers</returns>
        public List<string> GetPublicContainers()
        {
            GetPublicContainersRequest request = new GetPublicContainersRequest(cdnManagementUrl, authToken);
            GetPublicContainersResponse response =
                new ResponseFactoryWithContentBody<GetPublicContainersResponse>().Create(new CloudFilesRequest(request));

            if (response.Status == HttpStatusCode.Unauthorized)
                throw new AuthenticationFailedException(
                    "You do not have permission to request the list of public containers.");
            List<string> containerList = response.ContentBody;
            response.Dispose();


            return containerList;
        }

        /// <summary>
        /// This method sets a container as public on the CDN
        /// </summary>
        /// <param name="containerName">The name of the container to mark public</param>
        /// <returns>A string representing the URL of the public container</returns>
        public string MarkContainerAsPublic(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            SetContainerAsPublicRequest request = new SetContainerAsPublicRequest(cdnManagementUrl, authToken, containerName);
            return MarkContainerAsPublic(request);
        }

        /// <summary>
        /// Updates the details associated with the container on the cdn
        /// </summary>
        /// <param name="containerName">The name of the container to update</param>
        /// <param name="isCdnEnabled">Enables/Disables the public status of the container</param>
        /// <returns>A string containing the CDN URI for this container</returns>
        public string SetPublicContainerDetails(string containerName, bool isCdnEnabled)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            SetPublicContainerDetailsRequest request = 
                new SetPublicContainerDetailsRequest(cdnManagementUrl, authToken, containerName, isCdnEnabled, "", "", "");
            return SetPublicContainerDetails(request);
        }

        /// <summary>
        /// Retrieves a Container object containing the public CDN information
        /// </summary>
        /// <param name="containerName">The name of the container to query about</param>
        /// <returns>An instance of Container with appropriate CDN information</returns>
        public Container GetPublicContainerInformation(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            GetPublicContainerInformationRequest request = new GetPublicContainerInformationRequest(cdnManagementUrl, authToken, containerName);
            GetPublicContainerInformationResponse response = null;
            try
            {
                response = new ResponseFactory<GetPublicContainerInformationResponse>().Create(new CloudFilesRequest(request));
            }
            catch (WebException ex)
            {
                HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                if (webResponse.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Your authorization credentials are invalid or have expired.");
                if (webResponse.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The specified container does not exist.");
            }

            Container container = new Container(containerName);
            if (response == null) return null;
            container.CdnUri = response.Headers[Constants.X_CDN_URI];
            return container;
        }

        private string SetPublicContainerDetails(SetPublicContainerDetailsRequest request)
        {
            SetPublicContainerDetailsResponse response = null;
            try
            {
                response = new ResponseFactory<SetPublicContainerDetailsResponse>().Create(new CloudFilesRequest(request));
            }
            catch (WebException ex)
            {
                HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                if (webResponse.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException("Your access credentials are invalid or have expired. ");
                if (webResponse.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The specified container does not exist.");
            }

            return response == null ? null : response.Headers[Constants.X_CDN_URI];
        }


        private string MarkContainerAsPublic(SetContainerAsPublicRequest request)
        {
            SetContainerAsPublicResponse response = null;
            try
            {
                response = new ResponseFactory<SetContainerAsPublicResponse>().Create(new CloudFilesRequest(request));
            }
            catch (WebException we)
            {
                //It's a protocol error that is usually a result of a 401 (Unauthorized)
                //Still trying to figure way to get specific httpstatuscode
                HttpWebResponse webResponse = (HttpWebResponse)we.Response;
                if (webResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new InvalidCredentialException("You do not have permission to mark this container as public.");
                }
                if (webResponse.StatusCode == HttpStatusCode.Accepted)
                {
                    throw new ContainerAlreadyPublicException("The specified container is already marked as public.");
                }
            }

            return response == null ? null : response.Headers[Constants.X_CDN_URI];
        }
    }
}