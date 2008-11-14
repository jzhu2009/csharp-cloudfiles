using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.integration.tests.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services.UploadFileCommandSpecs
{
    [TestFixture]
    public class When_uploading_a_file_with_connection : TestBase
    {
        [Test]
        public void Should_return_nothing_when_the_file_is_uploaded_successfully()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);
            connection.PutStorageItem(containerName, Constants.StorageItemName);
            connection.DeleteStorageItem(containerName, Constants.StorageItemName);
            connection.DeleteContainer(containerName);
        }

        [Test]
        [ExpectedException(typeof (ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.PutStorageItem(containerName, Constants.StorageItemName);
        }

        [Test]
        public void Should_upload_file_with_the_file_name_minus_the_file_path_in_uri_format()
        {
            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);

                string executingPath = Assembly.GetExecutingAssembly().CodeBase.Replace(@"com.mosso.cloudfiles.integration.tests.DLL", "") + Constants.StorageItemName;
                connection.PutStorageItem(containerName, executingPath);

                List<string> containerList = connection.GetContainerItemList(containerName);
                Assert.That(containerList.Contains(Constants.StorageItemName), Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        public void Should_upload_file_with_the_file_name_minus_the_file_path_in_windows_path_format()
        {
            string containerName = Guid.NewGuid().ToString();

            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Path.GetFullPath(Constants.StorageItemName));

                List<string> containerList = connection.GetContainerItemList(containerName);
                Assert.That(containerList.Contains(Constants.StorageItemName), Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }
    }
}