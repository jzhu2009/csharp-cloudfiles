using System;
using System.Collections.Generic;
using System.IO;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services.ConnectionSpecs
{
    [TestFixture]
    public class When_passing_authentication_to_the_connection
    {
        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_argument_null_exception_with_null_authentication()
        {
            IConnection engine = new Connection(null);
        }

        [Test]
        public void Should_instantiate_engine_without_throwing_exception_when_authentication_passes()
        {

            IConnection engine = new Connection(new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY));
        }
    }

    [TestFixture]
    public class When_requesting_a_list_of_public_containers
    {
        private IConnection connection;
        
        [SetUp]
        public void SetUp()
        {
            UserCredentials userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            connection = new Connection(userCredentials);
        }

        [Test]
        public void Should_retrieve_a_list_of_public_containers_on_the_cdn_when_there_are_shared_containers()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            try
            {
                string cdnUrl = connection.MarkContainerAsPublic(containerName);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.Length, Is.GreaterThan(0));

                List<string> containerList = connection.GetPublicContainers();
                Assert.That(containerList, Is.Not.Null);
                Assert.That(containerList.Count, Is.GreaterThan(0));
                Assert.That(containerList.Contains(containerName));
     
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }           
        }
    }
   
    [TestFixture]
    public class When_marking_a_container_as_public
    {
        private IConnection connection;

        [SetUp]
        public void SetUp()
        {
            UserCredentials userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            connection = new Connection(userCredentials);
        }

        [Test]
        public void Should_return_a_public_uri_when_the_container_is_successfully_marked_public()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);
            try
            {
                string cdnUrl = connection.MarkContainerAsPublic(containerName);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.Length, Is.GreaterThan(0));
            } catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            } finally
            {
                connection.DeleteContainer(containerName);
            }
        }

        private void MarkContainerPublic(IConnection connection, string containerName)
        {
            try
            {
                connection.MarkContainerAsPublic(containerName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        [ExpectedException(typeof(ContainerAlreadyPublicException))]
        [Ignore("Currently not able to test this")]
        public void Should_fail_when_the_container_is_already_marked_public()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);
            MarkContainerPublic(connection, containerName);

            connection.MarkContainerAsPublic(containerName);
        }
    }

    public class TestHelper : IDisposable
    {
        private readonly IConnection connection;
        private readonly bool createContainer;
        private string containerName;


        public TestHelper(IConnection connection, bool createContainer, bool markPublic)
        {
            this.connection = connection;
            this.createContainer = createContainer;
            containerName = Guid.NewGuid().ToString();
            if (createContainer)
            {
                connection.CreateContainer(containerName);
                if (markPublic) connection.MarkContainerAsPublic(containerName);
            }
        }

        public string ContainerName
        {
            get { return containerName; }
        }

        public void Dispose()
        {
            if (createContainer) connection.DeleteContainer(containerName);
        }
    }

    [TestFixture]
    public class When_retrieve_public_container_information
    {
        private IConnection connection;

        [SetUp]
        public void SetUp()
        {
            UserCredentials userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            connection = new Connection(userCredentials);
        }

        [Test]
        public void Should_retrieve_public_container_info_when_the_container_exists_and_is_public()
        {
            using (TestHelper testHelper = new TestHelper(connection, true, true))
            {
                Container container = connection.RetrievePublicContainerInformation(testHelper.ContainerName);
                Assert.That(container.CdnUri, Is.Not.Null);
            }
        }

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            using (TestHelper testHelper = new TestHelper(connection, false, false))
            {
                Container container = connection.RetrievePublicContainerInformation(testHelper.ContainerName);
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_container_exists_and_is_not_public()
        {
            bool excepted = false;
            using (TestHelper testHelper = new TestHelper(connection, true, false))
            {
                try
                {
                    Container container = connection.RetrievePublicContainerInformation(testHelper.ContainerName);
                } catch
                {
                    excepted = true;
                }
            }
            Assert.That(excepted, Is.True);
        }
    }

    [TestFixture]
    public class When_updating_a_public_containers_information
    {
        private IConnection connection;

        [SetUp]
        public void SetUp()
        {
            UserCredentials userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            connection = new Connection(userCredentials);
        }

        [Test]
        public void Should_return_a_cdn_url_when_the_container_is_public()
        {
            using (TestHelper testHelper = new TestHelper(connection, true, true))
            {
                string cdnUrl = connection.SetPublicContainerDetails(testHelper.ContainerName, true);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.Length, Is.GreaterThan(0));
            }
        }

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            using (TestHelper testHelper = new TestHelper(connection, false, false))
            {
                connection.SetPublicContainerDetails(testHelper.ContainerName, true);
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_container_exists_but_is_not_public()
        {
            bool excepted = false;
            using (TestHelper testHelper = new TestHelper(connection, true, false))
            {
                try
                {
                    connection.SetPublicContainerDetails(testHelper.ContainerName, true);
                } catch
                {
                    excepted = true;
                }
            }
            Assert.That(excepted, Is.True);
        }
    }

    [TestFixture]
    public class When_putting_an_object_into_a_container
    {
        private IConnection connection;

        [SetUp]
        public void SetUp()
        {
            UserCredentials userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            connection = new Connection(userCredentials);
        }

        [Test]
        public void Should_upload_the_content_type()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            StorageItem storageItem = null;
            try
            {
                connection.PutStorageItem(containerName, Constants.StorageItemNameJpg);
                storageItem = connection.GetStorageItem(containerName, Constants.StorageItemNameJpg);
                Assert.That(storageItem.ContentType, Is.EqualTo("image/jpeg"));

            }
            finally
            {
                if(storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteStorageItem(containerName, Constants.StorageItemNameJpg);
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        public void Should_upload_the_content_type_when_using_dotnet_fileinfo_type()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            StorageItem storageItem = null;
            try
            {
                var file = new FileInfo(Constants.StorageItemNamePdf);
                var metadata = new Dictionary<string, string>();
                metadata.Add("Source", "1");
                metadata.Add("Note", "2");

                connection.PutStorageItem(containerName, file.Open(FileMode.Open), file.Name, metadata);
                storageItem = connection.GetStorageItem(containerName, Constants.StorageItemNamePdf);
                Assert.That(storageItem.ContentType, Is.EqualTo("application/pdf"));
            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteStorageItem(containerName, Constants.StorageItemNamePdf);
                connection.DeleteContainer(containerName);
            }
        }

    }
}