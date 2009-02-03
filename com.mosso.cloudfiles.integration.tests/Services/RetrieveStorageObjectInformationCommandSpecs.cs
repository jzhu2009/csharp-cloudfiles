using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services
{
    [TestFixture]
    public class RetrieveStorageObjectInformationCommandSpecs
    {
        private IConnection connection;

        [SetUp]
        public void SetUp()
        {
            connection = new Connection(new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY));
        }

        [Test, Ignore]
        public void Should_return_a_storage_item_when_successful()
        {
            string containerName = Guid.NewGuid().ToString();

            Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };

            StorageItem storageItem;
            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
                connection.SetStorageItemMetaInformation(containerName, Constants.StorageItemName, metadata);
                storageItem = connection.GetStorageItem(containerName, Constants.StorageItemName);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }

            Assert.That(storageItem, Is.Not.Null);
            Assert.That(storageItem.Metadata[Constants.MetadataKey], Is.EqualTo(Constants.MetadataValue));
        }

        [Test]
        public void Should_throw_an_exception_when_the_storage_object_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            try
            {
                StorageItem storageItem = connection.GetStorageItem(containerName, Constants.StorageItemName);
            }
            catch (Exception ex)
            {
                Assert.That(ex.GetType(), Is.EqualTo(typeof (StorageItemNotFoundException)));
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
        }
    }
}