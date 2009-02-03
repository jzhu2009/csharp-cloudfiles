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
        private readonly string storageToken;
        private readonly string storageUrl;
        private readonly string containerName;

        public TestHelper(string storageToken, string storageUrl, string containerName)
        {
            this.storageToken = storageToken;
            this.storageUrl = storageUrl;
            this.containerName = containerName;

            CreateContainer();
        }

        public void DeleteItemFromContainer()
        {
            DeleteItemFromContainer(Constants.StorageItemName);
        }

        public void DeleteItemFromContainer(string storageItemName)
        {
            DeleteStorageItem deleteStorageItem = new DeleteStorageItem(storageUrl, containerName,
                                                                        storageItemName, storageToken);
            IResponse deleteStorageItemResponse =
                new ResponseFactory<DeleteStorageItemResponse>().Create(new CloudFilesRequest(deleteStorageItem));

            Assert.That(deleteStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        public void AddMetadataToItem(string storageItemName)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string> {{"Test", "test"}, {"Test2", "test2"}};
            SetStorageItemMetaInformation setStorageItemMetaInformation = new SetStorageItemMetaInformation(storageUrl, containerName, storageItemName,
                                                                                                            metadata, storageToken);
            IResponse postStorageItemResponse =
                new ResponseFactory<SetStorageItemMetaInformationResponse>().Create(new CloudFilesRequest(setStorageItemMetaInformation));
            Assert.That(postStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.Accepted));
            Assert.That(postStorageItemResponse.Headers["Content-Type"], Is.EqualTo("text/plain; charset=UTF-8"));
            Assert.That(postStorageItemResponse.Headers["Content-Length"], Is.EqualTo("0"));
        }

        public void AddMetadataToItem()
        {
            AddMetadataToItem(Constants.StorageItemName);
        }

        public void PutItemInContainer(string storageItemName, string remoteName)
        {
            PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, remoteName,
                                                               storageItemName, storageToken);
            IResponse putStorageItemResponse =
                new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem));
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
            CreateContainer createContainer = new CreateContainer(storageUrl, storageToken, containerName);

            IResponse putContainerResponse =
                new ResponseFactory<CreateContainerResponse>().Create(new CloudFilesRequest(createContainer));
            Assert.That(putContainerResponse.Status, Is.EqualTo(HttpStatusCode.Created));
        }

        private void DeleteContainer()
        {
            var deleteContainer = new DeleteContainer(storageUrl, containerName, storageToken);

            IResponse deleteContainerResponse =
                new ResponseFactory<DeleteContainerResponse>().Create(new CloudFilesRequest(deleteContainer));
            Assert.That(deleteContainerResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        public void Dispose()
        {
            DeleteContainer();
        }
    }
}