using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetStorageItemInformationSpecs
{
    [TestFixture]
    public class When_getting_information_about_a_storage_item_and_the_storage_does_not_exist : TestBase
    {
        [Test]
        [ExpectedException(typeof(StorageItemNotFoundException))]
        public void should_get_storage_item_not_found_exception()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.GetStorageItemInformation(Constants.CONTAINER_NAME, Constants.StorageItemName);

                Assert.Fail("Should have thrown StorageItemNotFoundException");
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_getting_information_about_a_storage_item_and_the_storage_item_exists_with_metadata : TestBase
    {
        [Test]
        public void should_get_name_and_metadata_and_content_type_and_content_length_and_etag()
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName, metadata);
                var storageItemInformation = connection.GetStorageItemInformation(Constants.CONTAINER_NAME, Constants.StorageItemName);

                Assert.That(storageItemInformation.ContentLength, Is.EqualTo("34"));
                Assert.That(storageItemInformation.ContentType.Contains("text/plain"), Is.True);
                Assert.That(storageItemInformation.ETag, Is.EqualTo("5c66108b7543c6f16145e25df9849f7f"));
                Assert.That(storageItemInformation.Metadata.Count, Is.EqualTo(1));
                Assert.That(storageItemInformation.Metadata[Constants.MetadataKey], Is.EqualTo(Constants.MetadataValue));
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_getting_information_about_a_storage_item_and_the_storage_item_exists_without_metadata : TestBase
    {
        [Test]
        public void should_get_name_and_metadata_and_content_type_and_content_length_and_etag()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                var storageItemInformation = connection.GetStorageItemInformation(Constants.CONTAINER_NAME, Constants.StorageItemName);

                Assert.That(storageItemInformation.ContentLength, Is.EqualTo("34"));
                Assert.That(storageItemInformation.ContentType.Contains("text/plain"), Is.True);
                Assert.That(storageItemInformation.ETag, Is.EqualTo("5c66108b7543c6f16145e25df9849f7f"));
                Assert.That(storageItemInformation.Metadata.Count, Is.EqualTo(0));
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}