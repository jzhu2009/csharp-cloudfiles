///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;

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
            }
        }

        private object Execute(BaseCommand command)
        {
            VerifyAuthentication();

            command.StorageToken = storageToken;
            command.StorageUrl = storageUrl;
            command.CdnManagementUrl = cdnManagementUrl;
            command.AuthToken = authToken;

            command.UserCredentials = userCredentials;
            int retryCount = 0;
            bool retry = false;
            do
            {
                try
                {
                    return command.Execute();
                }
                catch (WebException we)
                {
                    HttpWebResponse response = ((HttpWebResponse) we.Response);
                    if (response != null && response.StatusCode == HttpStatusCode.Unauthorized && retryCount == 0)
                    {
                        ++retryCount;
                        Authenticate();
                        retry = true;
                    }
                    else
                        throw;
                }
            } while (retry);
            return null;
        }

        /// <summary>
        /// This method returns the number of containers and the size, in bytes, of the specified account
        /// </summary>
        /// <returns>An instance of AccountInformation, containing the byte size and number of containers associated with this account</returns>
        public AccountInformation GetAccountInformation()
        {
            return Execute(new GetAccountInformationCommand()) as AccountInformation;
        }

        /// <summary>
        /// This method is used to create a container on cloudfiles with a given name
        /// </summary>
        /// <param name="containerName">The desired name of the container</param>
        public void CreateContainer(string containerName)
        {
            Execute(new CreateContainerCommand(containerName));
        }

        /// <summary>
        /// This method is used to delete a container on cloudfiles
        /// </summary>
        /// <param name="containerName">The name of the container to delete</param>
        public void DeleteContainer(string containerName)
        {
            Execute(new DeleteContainerCommand(containerName));
        }

        /// <summary>
        /// This method retrieves a list of containers associated with a given account
        /// </summary>
        /// <returns>An instance of List, containing the names of the containers this account owns</returns>
        public List<string> GetContainers()
        {
            return Execute(new GetContainerListCommand()) as List<string>;
        }

        /// <summary>
        /// This method retrieves the contents of a container
        /// </summary>
        /// <param name="containerName">The name of the container</param>
        /// <returns>An instance of List, containing the names of the storage objects in the give container</returns>
        public List<string> GetContainerItemList(string containerName)
        {
            return Execute(new GetContainerItemsListCommand(containerName)) as List<string>;
        }

        /// <summary>
        /// This method retrieves the number of storage objects in a container, and the total size, in bytes, of the container
        /// </summary>
        /// <param name="containerName">The name of the container to query about</param>
        /// <returns>An instance of container, with the number of storage objects contained and total byte allocation</returns>
        public Container GetContainerInformation(string containerName)
        {
            return Execute(new GetContainerInformationCommand(containerName)) as Container;
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles with meta tags
        /// </summary>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="storageItemName">The complete file uri of the storage object to be uploaded</param>
        /// <param name="metaTags">An optional parameter containing a dictionary of meta tags to associate with the storage object</param>
        public void PutStorageItem(string containerName, string storageItemName, Dictionary<string, string> metaTags)
        {
            Execute(new PutStorageItemCommand(containerName, storageItemName, storageItemName, metaTags));
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles
        /// </summary>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="storageItemName">The complete file uri of the storage object to be uploaded</param>
        public void PutStorageItem(string containerName, string storageItemName)
        {
            Execute(new PutStorageItemCommand(containerName, storageItemName, storageItemName));
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles with an alternate name
        /// </summary>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="remoteStorageItemName">The alternate name as it will be called on cloudfiles</param>
        /// <param name="storageStream">The stream representing the storage item to upload</param>
        public void PutStorageItem(string containerName, FileStream storageStream, string remoteStorageItemName)
        {
            Execute(new PutStorageItemCommand(containerName, storageStream, remoteStorageItemName));
        }

        /// <summary>
        /// This method uploads a storage object to cloudfiles with an alternate name
        /// </summary>
        /// <param name="containerName">The name of the container to put the storage object in</param>
        /// <param name="storageStream">The file stream to upload</param>
        /// <param name="metaTags">An optional parameter containing a dictionary of meta tags to associate with the storage object</param>
        /// <param name="remoteStorageItemName">The name of the storage object as it will be called on cloudfiles</param>
        public void PutStorageItem(string containerName, FileStream storageStream, string remoteStorageItemName, Dictionary<string, string> metaTags)
        {
            Execute(new PutStorageItemCommand(containerName, storageStream, remoteStorageItemName, metaTags));
        }

        /// <summary>
        /// This method deletes a storage object in a given container
        /// </summary>
        /// <param name="containerName">The name of the container that contains the storage object</param>
        /// <param name="storageItemName">The name of the storage object to delete</param>
        public void DeleteStorageItem(string containerName, string storageItemName)
        {
            Execute(new DeleteStorageItemCommand(containerName, storageItemName));
        }

        /// <summary>
        /// This method downloads a storage object from cloudfiles
        /// </summary>
        /// <param name="containerName">The name of the container that contains the storage object to retrieve</param>
        /// <param name="storageItemName">The name of the storage object to retrieve</param>
        /// <returns>An instance of StorageItem with the stream containing the bytes representing the desired storage object</returns>
        public StorageItem GetStorageItem(string containerName, string storageItemName)
        {
            return Execute(new GetStorageItemCommand(containerName, storageItemName)) as StorageItem;
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
            return Execute(new GetStorageItemCommand(containerName, storageItemName, requestHeaderFields)) as StorageItem;
        }



        /// <summary>
        /// An alternate method for downloading storage objects from cloudfiles directly to a file name specified in the method
        /// </summary>
        /// <param name="containerName">The name of the container that contains the storage object to retrieve</param>
        /// <param name="storageItemName">The name of the storage object to retrieve</param>
        /// <param name="localFileName">The file name to save the storage object into on disk</param>
        public void GetStorageItem(string containerName, string storageItemName, string localFileName)
        {
            Execute(new GetStorageItemCommand(containerName, storageItemName, localFileName));
        }

        /// <summary>
        /// This method applies meta tags to a storage object on cloudfiles
        /// </summary>
        /// <param name="containerName">The name of the container containing the storage object</param>
        /// <param name="storageItemName">The name of the storage object</param>
        /// <param name="metaTags">A dictionary containiner key/value pairs representing the meta data for this storage object</param>
        public void SetStorageItemMetaInformation(string containerName, string storageItemName, Dictionary<string, string> metaTags)
        {
            Execute(new SetStorageItemMetaInformationCommand(containerName, storageItemName, metaTags));
        }

        /// <summary>
        /// This method retrieves meta information and size, in bytes, of a requested storage object
        /// </summary>
        /// <param name="containerName">The name of the container that contains the storage object</param>
        /// <param name="storageItemName">The name of the storage object</param>
        /// <returns>An instance of StorageItem containing the byte size and meta information associated with the container</returns>
        public StorageItem GetStorageItemInformation(string containerName, string storageItemName)
        {
            return Execute(new RetrieveStorageItemInformationCommand(containerName, storageItemName)) as StorageItem;
        }

        /// <summary>
        /// This method retrieves the names of the of the containers made public on the CDN
        /// </summary>
        /// <returns>A list of the public containers</returns>
        public List<string> GetPublicContainers()
        {
            return Execute(new RetrievePublicContainersCommand()) as List<string>;
        }

        /// <summary>
        /// This method sets a container as public on the CDN
        /// </summary>
        /// <param name="containerName">The name of the container to mark public</param>
        /// <returns>A string representing the URL of the public container</returns>
        public string MarkContainerAsPublic(string containerName)
        {
            return Execute(new MarkContainerAsPublicCommand(containerName)) as string;
        }

        /// <summary>
        /// This method sets a container as public on the CDN
        /// </summary>
        /// <param name="containerName">The name of the container to mark public</param>
        /// <param name="cdnTtl">The TTL of the container on the CDN</param>
        /// <param name="userAgentAcl">Access List information for the user agent</param>
        /// <param name="referralAcl">Access List information for the referrer</param>
        /// <returns>A string representing the URL of the public container</returns>
        public string MarkContainerAsPublic(string containerName, string cdnTtl, string userAgentAcl, string referralAcl)
        {
            return Execute(new MarkContainerAsPublicCommand(containerName, cdnTtl, userAgentAcl, referralAcl)) as string;
        }

        /// <summary>
        /// Updates the details associated with the container on the cdn
        /// </summary>
        /// <param name="containerName">The name of the container to update</param>
        /// <param name="isCdnEnabled">Enables/Disables the public status of the container</param>
        /// <returns>A string containing the CDN URI for this container</returns>
        public string SetPublicContainerDetails(string containerName, bool isCdnEnabled)
        {
            return Execute(new SetPublicContainerDetailsCommand(containerName, isCdnEnabled, "", "", "")) as string;
        }

        /// <summary>
        /// Updates the details associated with the container on the cdn
        /// </summary>
        /// <param name="containerName">The name of the container to update</param>
        /// <param name="cdnTtl">The TTL of the container on the CDN</param>
        /// <param name="userAgentAcl">Associates ACL information for specific user agents</param>
        /// <param name="referrerAcl">Associates ACL information for specific referrers</param>
        /// <returns>A string containing the CDN URI for this container</returns>
        public string SetPublicContainerDetails(string containerName, string cdnTtl, string userAgentAcl, string referrerAcl)
        {
            return Execute(new SetPublicContainerDetailsCommand(containerName, cdnTtl, userAgentAcl, referrerAcl)) as string;
        }

        /// <summary>
        /// Updates the details associated with the container on the cdn
        /// </summary>
        /// <param name="containerName">The name of the container to update</param>
        /// <param name="isCdnEnabled">Enables/Disables the public status of the container</param>
        /// <param name="cdnTtl">The TTL of the container on the CDN</param>
        /// <param name="userAgentAcl">Associates ACL information for specific user agents</param>
        /// <param name="referrerAcl">Associates ACL information for specific referrers</param>
        /// <returns>A string containing the CDN URI for this container</returns>
        public string SetPublicContainerDetails(string containerName, bool isCdnEnabled, string cdnTtl, string userAgentAcl, string referrerAcl)
        {
            return Execute(new SetPublicContainerDetailsCommand(containerName, isCdnEnabled, cdnTtl, userAgentAcl, referrerAcl)) as string;
        }

        /// <summary>
        /// Retrieves a Container object containing the public CDN information
        /// </summary>
        /// <param name="containerName">The name of the container to query about</param>
        /// <returns>An instance of Container with appropriate CDN information</returns>
        public Container RetrievePublicContainerInformation(string containerName)
        {
            return Execute(new RetrievePublicContainerInformationCommand(containerName)) as Container;
        }
    }
}