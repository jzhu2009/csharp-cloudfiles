using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.PutStorageItemSpecs
{
    [TestFixture]
    public class When_uploading_a_file : TestBase
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
        [ExpectedException(typeof(ContainerNotFoundException))]
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

    [TestFixture]
    public class When_putting_an_object_into_a_container : TestBase
    {

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
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
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

//    [TestFixture]
//    public class When_putting_a_object_greater_than_2_GB_into_cloud_files : TestBase
//    {
//        [Test]
//        public void Should_upload_the_file_successfully()
//        {
//            string containerName = Guid.NewGuid().ToString();
//            connection.CreateContainer(containerName);
//
//            try
//            {
//                connection.PutStorageItem(containerName, @"C:\TestStorageItem.iso");
//
//                var items = connection.GetContainerItemList(containerName);
//                Assert.That(items.Contains("TestStorageItem.iso"), Is.True);
//            }
//            finally
//            {
//                if(connection.GetContainerItemList(containerName).Contains("TestStorageItem.iso"))
//                    connection.DeleteStorageItem(containerName, "TestStorageItem.iso");
//                connection.DeleteContainer(containerName);
//            }
//        }
//    }
}