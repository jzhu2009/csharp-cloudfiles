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
            
            Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.SetStorageItemMetaInformation(Constants.CONTAINER_NAME, Constants.StorageItemName, metadata);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_setting_meta_information_on_an_object_and_the_object_does_not_exist : TestBase
    {
        [Test]
        public void Should_throw_an_exception_when_the_storage_object_does_not_exist()
        {
            
            Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            connection.CreateContainer(Constants.CONTAINER_NAME);
            try
            {
                connection.SetStorageItemMetaInformation(Constants.CONTAINER_NAME, Constants.StorageItemName, metadata);
            }
            catch (Exception ex)
            {
                Assert.That(ex.GetType(), Is.EqualTo(typeof (StorageItemNotFoundException)));
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }


}