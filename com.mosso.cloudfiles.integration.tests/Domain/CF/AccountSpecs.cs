using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.Domain.CF.AccountSpecs
{
    [TestFixture]
    public class AccountIntegrationTestBase
    {
        protected UserCredentials userCredentials;
        protected IAuthentication authentication;
        protected IAccount account;

        [SetUp]
        public void SetUp()
        {
            userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            authentication = new CF_Authentication(userCredentials);

            account = authentication.Authenticate();
        }
    }

    [TestFixture]
    public class When_creating_a_new_container : AccountIntegrationTestBase
    {
        [Test]
        public void should_give_you_container_instance_when_successful()
        {
            var containerName = Guid.NewGuid().ToString();
            var originalContainerCount = account.ContainerCount;
            var originalBytesUsed = account.BytesUsed;
            try
            {
                account.CreateContainer(containerName);
                Assert.That(account.ContainerExists(containerName), Is.True);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed + 0));
            }
            finally
            {
                account.DeleteContainer(containerName);
                Assert.That(account.ContainerExists(containerName), Is.False);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed));
            }
            

            Assert.That(account.StorageUrl.ToString().Contains("https://storage.clouddrive.com/v1/MossoCloudFS_"), Is.True);
            Assert.That(account.StorageToken.Length, Is.EqualTo(36));
            Assert.That(account.AuthToken.Length, Is.EqualTo(36));
            Assert.That(account.CDNManagementUrl.ToString().Contains("https://cdn.clouddrive.com/v1/MossoCloudFS_"), Is.True);

        }

        [Test]
        [ExpectedException(typeof(ContainerAlreadyExistsException))]
        public void should_throw_container_already_exists_exception_if_the_container_already_exists()
        {
            var containerName = Guid.NewGuid().ToString();
            try
            {
                account.CreateContainer(containerName);
                account.CreateContainer(containerName);
            }
            finally
            {
                account.DeleteContainer(containerName);
                Assert.That(account.ContainerExists(containerName), Is.False);
            }
        }
    }

    [TestFixture]
    public class When_deleting_a_container_and_the_container_is_not_empty : AccountIntegrationTestBase
    {
        [Test]
        [ExpectedException(typeof(ContainerNotEmptyException))]
        public void should_throw_container_not_empty_exception_if_the_container_not_empty()
        {
            var containerName = Guid.NewGuid().ToString();
            IContainer container = null;
            var originalContainerCount = account.ContainerCount;
            var originalBytesUsed = account.BytesUsed;
            try
            {
                
                container = account.CreateContainer(containerName);
                container.AddObject(Constants.StorageItemName);
                Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed + 34));

                account.DeleteContainer(containerName);
            }
            finally
            {
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed + 34));

                container.DeleteObject(Constants.StorageItemName);
                Assert.That(container.ObjectExists(Constants.StorageItemName), Is.False);
                account.DeleteContainer(containerName);
                Assert.That(account.ContainerExists(containerName), Is.False);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed));
            }
        }
    }

    [TestFixture]
    public class When_adding_containers_and_objects_to_the_account : AccountIntegrationTestBase
    {
        [Test]
        public void should_keep_count_of_each_container_and_object()
        {
            var containerName = Guid.NewGuid().ToString();
            IContainer container = null;
            var originalContainerCount = account.ContainerCount;
            try
            {
                
                container = account.CreateContainer(containerName);
                container.AddObject(Constants.StorageItemName);
                container.AddObject(Constants.HeadStorageItemName);
                Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(container.ObjectCount, Is.EqualTo(2));
            }
            finally
            {
                container.DeleteObject(Constants.StorageItemName);
                container.DeleteObject(Constants.HeadStorageItemName);
                Assert.That(container.ObjectCount, Is.EqualTo(0));
                Assert.That(container.ObjectExists(Constants.StorageItemName), Is.False);
                Assert.That(container.ObjectExists(Constants.HeadStorageItemName), Is.False);
                account.DeleteContainer(containerName);
                Assert.That(account.ContainerExists(containerName), Is.False);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount));
            }
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_an_account_and_containers_exist : AccountIntegrationTestBase
    {
        [Test]
        public void should_return_json_string_with_container_names_and_item_count_and_bytes_used()
        {
            var containerName = Guid.NewGuid().ToString();
            try
            {
                account.CreateContainer(containerName);
                var expectedJson = "[{\"name\": \"" + containerName + "\", \"count\": 0, \"bytes\": 0}]";

                Assert.That(account.JSON, Is.EqualTo(expectedJson));
            }
            finally
            {
                account.DeleteContainer(containerName);
                Assert.That(account.ContainerExists(containerName), Is.False);
            }
            
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_an_account_and_no_containers_exist : AccountIntegrationTestBase
    {
        [Test]
        public void should_return_json_string_emptry_brackets()
        {
            var expectedJson = "[]";

            Assert.That(account.JSON, Is.EqualTo(expectedJson));
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_an_account_and_containers_exist : AccountIntegrationTestBase
    {
        [Test]
        public void should_return_xml_document_with_container_names_and_item_count_and_bytes_used()
        {
            var containerName = Guid.NewGuid().ToString();
            try
            {
                account.CreateContainer(containerName);
                var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"><container><name>" + containerName + "</name><count>0</count><bytes>0</bytes></container></account>";

                Assert.That(account.XML.InnerXml, Is.EqualTo(expectedXml));
            }
            finally
            {
                account.DeleteContainer(containerName);
                Assert.That(account.ContainerExists(containerName), Is.False);
            }
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_an_account_and_no_containers_exist : AccountIntegrationTestBase
    {
        [Test]
        public void should_return_xml_document_with_account_name()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"></account>";

            Assert.That(account.XML.InnerXml, Is.EqualTo(expectedXml));
        }
    }
}