///
/// See COPYING file for licensing information
///

using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;

namespace com.mosso.cloudfiles.domain
{
    public interface IAccount
    {
        int ContainerCount { get; }
        long BytesUsed { get; }
        Uri StorageUrl { get; }
        string StorageToken { get; }
        string AuthToken { get; set; }
        Uri CDNManagementUrl { get; set; }
        IContainer CreateContainer(string containerName);
        void DeleteContainer(string containerName);
        IContainer GetContainer(string containerName);
        bool ContainerExists(string containerName);
        UserCredentials UserCredentials { get; set; }
        string JSON { get; }
        XmlDocument XML { get; }
    }

    public class CF_Account : IAccount
    {
        protected List<IContainer> containers;
        protected string storageToken;
        protected Uri storageUrl;
        protected int containerCount;
        protected long bytesUsed;

        public CF_Account(string storageToken, Uri storageUrl)
        {
            this.storageToken = storageToken;
            this.storageUrl = storageUrl;
            containers = new List<IContainer>();
        }

        public Uri StorageUrl
        {
            get { return storageUrl; }
        }

        public string StorageToken
        {
            get { return storageToken; }
        }

        public int ContainerCount
        {
            get
            {
                CloudFilesHeadAccount();
                return containerCount;
            }
        }

        public long BytesUsed
        {
            get
            {
                CloudFilesHeadAccount();
                return bytesUsed;
            }
        }

        public string AuthToken { get; set; }

        public Uri CDNManagementUrl { get; set; }

        public UserCredentials UserCredentials { get; set; }

        public string JSON
        {
            get
            {
                return CloudFileAccountInformationJson();
            }
        }

        public XmlDocument XML
        {
            get
            {
                return CloudFileAccountInformationXml();
            }
        }

        public IContainer CreateContainer(string containerName)
        {
            CloudFileCreateContainer(containerName);

            IContainer container = new CF_Container(containerName);
            container.CDNManagementUrl = CDNManagementUrl;
            container.AuthToken = AuthToken;
            container.StorageToken = StorageToken;
            container.StorageUrl = StorageUrl;
            container.UserCredentials = UserCredentials;
            containers.Add(container);

            return container;
        }

        public bool ContainerExists(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            return CloudFilesHeadContainer(containerName)
                   && containers.Contains(containers.Find(x => x.Name == containerName));
        }

        public void DeleteContainer(string containerName)
        {
            CloudFilesDeleteContainer(containerName);
            if (containers.Find(x => x.Name == containerName) == null)
                throw new ContainerNotFoundException();
            containers.Remove(containers.Find(x => x.Name == containerName));
        }

        public IContainer GetContainer(string containerName)
        {
            CloudFilesGetContainer(containerName);
            return containers.Find(x => x.Name == containerName);
        }

        protected virtual string CloudFileAccountInformationJson()
        {
            throw new NotImplementedException();
        }

        protected virtual XmlDocument CloudFileAccountInformationXml()
        {
            throw new NotImplementedException();
        }

        protected virtual void CloudFileCreateContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            CreateContainer createContainer = new CreateContainer(StorageUrl.ToString(), StorageToken, containerName);
            CreateContainerResponse createContainerResponse =
                new ResponseFactory<CreateContainerResponse>().Create(new CloudFilesRequest(createContainer));
            if (createContainerResponse.Status == HttpStatusCode.Accepted)
                throw new ContainerAlreadyExistsException("The container already exists");
        }

        protected virtual void CloudFilesHeadAccount()
        {
            GetAccountInformation getAccountInformation = new GetAccountInformation(StorageUrl.ToString(), StorageToken);
            GetAccountInformationResponse getAccountInformationResponse =
                new ResponseFactory<GetAccountInformationResponse>().Create(new CloudFilesRequest(getAccountInformation));
            containerCount = Int32.Parse(getAccountInformationResponse.Headers[Constants.X_ACCOUNT_CONTAINER_COUNT]);
            bytesUsed = Convert.ToInt64(getAccountInformationResponse.Headers[Constants.X_ACCOUNT_BYTES_USED]);
        }

        protected virtual bool CloudFilesHeadContainer(string containerName)
        {
            GetContainerInformation getContainerInformation = new GetContainerInformation(StorageUrl.ToString(), containerName, StorageToken);

            try
            {
                new ResponseFactory<GetContainerInformationResponse>().Create(new CloudFilesRequest(getContainerInformation, UserCredentials.ProxyCredentials));
            }
            catch (WebException we)
            {
                HttpStatusCode code = ((HttpWebResponse) we.Response).StatusCode;
                if (we.Response != null && code == HttpStatusCode.NotFound)
                    return false;
            }
            return true;
        }

        protected virtual void CloudFilesDeleteContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException();

            DeleteContainer deleteContainer = new DeleteContainer(StorageUrl.ToString(), containerName, StorageToken);
            try
            {
                new ResponseFactory<DeleteContainerResponse>().Create(new CloudFilesRequest(deleteContainer));
            }
            catch (WebException ex)
            {
                HttpWebResponse response = ((HttpWebResponse) ex.Response);
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    throw new ContainerNotFoundException("The requested container does not exist");

                if (response != null && response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new ContainerNotEmptyException("The container you are trying to delete is not empty");
                }
            }
        }

        protected virtual void CloudFilesGetContainer(string containerName)
        {
            throw new NotImplementedException();
        }
    }
}