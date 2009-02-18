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
        protected IConnection connection;
        protected IAccount account;

        [SetUp]
        public void SetUp()
        {
            userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            connection = new Connection(userCredentials);

            account = connection.Account;
        }
    }

    [TestFixture]
    public class When_creating_a_new_container : AccountIntegrationTestBase
    {
        [Test]
        public void should_give_you_container_instance_when_successful()
        {

            var originalContainerCount = account.ContainerCount;
            var originalBytesUsed = account.BytesUsed;
            try
            {
                account.CreateContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.True);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed + 0));
            }
            finally
            {
                if(account.ContainerExists(Constants.CONTAINER_NAME))
                    account.DeleteContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed));
            }
        }

        [Test]
        [ExpectedException(typeof(ContainerAlreadyExistsException))]
        public void should_throw_container_already_exists_exception_if_the_container_already_exists()
        {
            try
            {
                account.CreateContainer(Constants.CONTAINER_NAME);
                account.CreateContainer(Constants.CONTAINER_NAME);
            }
            finally
            {
                account.DeleteContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
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
            IContainer container = null;
            var originalContainerCount = account.ContainerCount;
            var originalBytesUsed = account.BytesUsed;
            try
            {

                container = account.CreateContainer(Constants.CONTAINER_NAME);
                container.AddObject(Constants.StorageItemName);
                Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed + 34));

                account.DeleteContainer(Constants.CONTAINER_NAME);
            }
            finally
            {
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(account.BytesUsed, Is.EqualTo(originalBytesUsed + 34));

                if(container != null && account.ContainerExists(Constants.CONTAINER_NAME))
                {
                    container.DeleteObject(Constants.StorageItemName);
                    Assert.That(container.ObjectExists(Constants.StorageItemName), Is.False);
                    account.DeleteContainer(Constants.CONTAINER_NAME);    
                }
                
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
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
            IContainer container = null;
            var originalContainerCount = account.ContainerCount;
            try
            {

                container = account.CreateContainer(Constants.CONTAINER_NAME);
                container.AddObject(Constants.StorageItemName);
                container.AddObject(Constants.HeadStorageItemName);
                Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
                Assert.That(account.ContainerCount, Is.EqualTo(originalContainerCount + 1));
                Assert.That(container.ObjectCount, Is.EqualTo(2));
            }
            finally
            {
                if (container != null && account.ContainerExists(Constants.CONTAINER_NAME))
                {
                    container.DeleteObject(Constants.StorageItemName);
                    container.DeleteObject(Constants.HeadStorageItemName);
                    Assert.That(container.ObjectCount, Is.EqualTo(0));
                    Assert.That(container.ObjectExists(Constants.StorageItemName), Is.False);
                    Assert.That(container.ObjectExists(Constants.HeadStorageItemName), Is.False);
                    account.DeleteContainer(Constants.CONTAINER_NAME);
                }

                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
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
            try
            {
                account.CreateContainer(Constants.CONTAINER_NAME);
                var expectedJson = "[{\"name\": \"" + Constants.CONTAINER_NAME + "\", \"count\": 0, \"bytes\": 0}]";

                Assert.That(account.JSON, Is.EqualTo(expectedJson));
            }
            finally
            {
                account.DeleteContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
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
            try
            {
                account.CreateContainer(Constants.CONTAINER_NAME);
                var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"><container><name>" + Constants.CONTAINER_NAME + "</name><count>0</count><bytes>0</bytes></container></account>";

                Assert.That(account.XML.InnerXml, Is.EqualTo(expectedXml));
            }
            finally
            {
                account.DeleteContainer(Constants.CONTAINER_NAME);
                Assert.That(account.ContainerExists(Constants.CONTAINER_NAME), Is.False);
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