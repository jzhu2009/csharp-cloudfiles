using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.SetStorageItemMetaInformationSpecs
{
    [TestFixture]
    public class When_setting_meta_information_on_an_object : TestBase
    {
        [Test]
        public void Should_return_nothing_when_the_command_succeeds()
        {
            string containerName = Guid.NewGuid().ToString();
            Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
                connection.SetStorageItemMetaInformation(containerName, Constants.StorageItemName, metadata);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }
    }

    [TestFixture]
    public class When_setting_meta_information_on_an_object_and_the_object_does_not_exist : TestBase
    {
        [Test]
        public void Should_throw_an_exception_when_the_storage_object_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            connection.CreateContainer(containerName);
            try
            {
                connection.SetStorageItemMetaInformation(containerName, Constants.StorageItemName, metadata);
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