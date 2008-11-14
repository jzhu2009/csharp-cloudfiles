using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.integration.tests.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services.SetStorageObjectMetaInformationCommandSpecs
{
    [TestFixture]
    public class When_setting_meta_information_using_connection : TestBase
    {
        [Test]
        public void Should_return_nothing_when_the_command_succeeds()
        {
            string containerName = Guid.NewGuid().ToString();
            Dictionary<string, string> metaTags = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
                connection.SetStorageItemMetaInformation(containerName, Constants.StorageItemName, metaTags);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_storage_object_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            Dictionary<string, string> metaTags = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            connection.CreateContainer(containerName);
            try
            {
                connection.SetStorageItemMetaInformation(containerName, Constants.StorageItemName, metaTags);
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