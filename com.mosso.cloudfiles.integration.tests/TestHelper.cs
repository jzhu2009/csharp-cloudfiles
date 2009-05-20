using System;
using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using CreateContainer=com.mosso.cloudfiles.domain.request.CreateContainer;
using DeleteStorageItem=com.mosso.cloudfiles.domain.request.DeleteStorageItem;
using PutStorageItem=com.mosso.cloudfiles.domain.request.PutStorageItem;
using SetStorageItemMetaInformation=com.mosso.cloudfiles.domain.request.SetStorageItemMetaInformation;

namespace com.mosso.cloudfiles.integration.tests
{
    public class TestHelper : IDisposable
    {
        private readonly string authToken;
        private readonly string storageUrl;
        private readonly string containerName;

        public TestHelper(string authToken, string storageUrl, string containerName)
        {
            this.authToken = authToken;
            this.storageUrl = storageUrl;
            this.containerName = containerName;

            CreateContainer();
        }

        public TestHelper(string authToken, string storageUrl) : this(authToken, storageUrl, Constants.CONTAINER_NAME)
        {} 

        public void DeleteItemFromContainer()
        {
            DeleteItemFromContainer(Constants.StorageItemName);
        }

        public void DeleteItemFromContainer(string storageItemName)
        {
            var deleteStorageItem = new DeleteStorageItem(storageUrl, authToken, containerName, storageItemName);
            var deleteStorageItemResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(deleteStorageItem));
            Assert.That(deleteStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        public void AddMetadataToItem(string storageItemName)
        {
            var metadata = new Dictionary<string, string> {{"Test", "test"}, {"Test2", "test2"}};
            var setStorageItemMetaInformation = new SetStorageItemMetaInformation(storageUrl, authToken, containerName, storageItemName, metadata);
            var postStorageItemResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(setStorageItemMetaInformation));
            Assert.That(postStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.Accepted));
            Assert.That(postStorageItemResponse.Headers["Content-Type"].Contains("text/plain"), Is.True);
            Assert.That(postStorageItemResponse.Headers["Content-Length"], Is.EqualTo("0"));
        }

        public void AddMetadataToItem()
        {
            AddMetadataToItem(Constants.StorageItemName);
        }

        public void PutItemInContainer(string storageItemName, string remoteName)
        {
            var putStorageItem = new PutStorageItem(storageUrl, authToken, containerName, remoteName, storageItemName);
            var putStorageItemResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
            Assert.That(putStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.Created));
        }

        public void PutItemInContainer(string storageItemName)
        {
            PutItemInContainer(storageItemName, storageItemName);
        }

        public void PutItemInContainer()
        {
            PutItemInContainer(Constants.StorageItemName);
        }

        private void CreateContainer()
        {
            var createContainer = new CreateContainer(storageUrl, authToken, containerName);
            var putContainerResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(createContainer));
            Assert.That(putContainerResponse.Status, Is.EqualTo(HttpStatusCode.Created));
        }

        private void DeleteContainer()
        {
            var deleteContainer = new DeleteContainer(storageUrl, authToken, containerName);
            var deleteContainerResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(deleteContainer));
            Assert.That(deleteContainerResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        public void Dispose()
        {
            DeleteContainer();
        }
    }
}