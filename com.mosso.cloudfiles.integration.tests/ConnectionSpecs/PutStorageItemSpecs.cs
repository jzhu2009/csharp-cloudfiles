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
            
            connection.CreateContainer(Constants.CONTAINER_NAME);
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
            connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
            connection.DeleteContainer(Constants.CONTAINER_NAME);
        }

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
        }

        [Test]
        public void Should_upload_file_with_the_file_name_minus_the_file_path_in_uri_format()
        {
            
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);

                var executingPath = Assembly.GetExecutingAssembly().CodeBase.Replace(@"com.mosso.cloudfiles.integration.tests.DLL", "") + Constants.StorageItemName;
                connection.PutStorageItem(Constants.CONTAINER_NAME, executingPath);

                var containerList = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                Assert.That(containerList.Contains(Constants.StorageItemName), Is.True);
            }
            finally
            {
                var storageItems = connection.GetContainerItemList(Constants.CONTAINER_NAME);

                if(storageItems.Contains(Constants.StorageItemName))
                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);

                if (connection.GetContainerInformation(Constants.CONTAINER_NAME) != null)
                    connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        public void Should_upload_file_with_the_file_name_minus_the_file_path_in_windows_path_format()
        {
            

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Path.GetFullPath(Constants.StorageItemName));

                var containerList = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                Assert.That(containerList.Contains(Constants.StorageItemName), Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_putting_an_object_into_a_container : TestBase
    {

        [Test]
        public void Should_upload_the_content_type()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);

            StorageItem storageItem = null;
            try
            {
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                storageItem = connection.GetStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                Assert.That(storageItem.ContentType, Is.EqualTo("image/jpeg"));

            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        public void Should_upload_the_content_type_when_using_dotnet_fileinfo_type()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);

            StorageItem storageItem = null;
            try
            {
                var file = new FileInfo(Constants.StorageItemNamePdf);
                var metadata = new Dictionary<string, string>();
                metadata.Add("Source", "1");
                metadata.Add("Note", "2");

                connection.PutStorageItem(Constants.CONTAINER_NAME, file.Open(FileMode.Open), file.Name, metadata);
                storageItem = connection.GetStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNamePdf);
                Assert.That(storageItem.ContentType, Is.EqualTo("application/pdf"));
            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNamePdf);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

    }

    [TestFixture]
    public class When_putting_an_object_into_a_container_with_meta_information : TestBase
    {

        [Test]
        public void Should_upload_the_meta_information()
        {

            connection.CreateContainer(Constants.CONTAINER_NAME);

            StorageItem storageItem = null;
            try
            {
                Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg, metadata);
                storageItem = connection.GetStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                Assert.That(storageItem.Metadata[Constants.XMetaKeyHeader + Constants.MetadataKey], Is.EqualTo(Constants.MetadataValue));
                Assert.That(storageItem.ContentType, Is.EqualTo("image/jpeg"));

            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        public void Should_upload_the_meta_information_with_file_stream()
        {

            connection.CreateContainer(Constants.CONTAINER_NAME);

            StorageItem storageItem = null;
            try
            {
                var file = new FileInfo(Constants.StorageItemNameJpg);
                Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
                connection.PutStorageItem(Constants.CONTAINER_NAME, file.Open(FileMode.Open), Constants.StorageItemNameJpg, metadata);
                storageItem = connection.GetStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                Assert.That(storageItem.Metadata[Constants.XMetaKeyHeader + Constants.MetadataKey], Is.EqualTo(Constants.MetadataValue));
                Assert.That(storageItem.ContentType, Is.EqualTo("image/jpeg"));

            }
            finally
            {
                if (storageItem != null && storageItem.ObjectStream.CanRead) storageItem.ObjectStream.Close();
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

//    [TestFixture]
//    public class When_putting_a_object_greater_than_2_GB_into_cloud_files : TestBase
//    {
//        [Test]
//        public void Should_upload_the_file_successfully()
//        {
//            
//            connection.CreateContainer(Constants.CONTAINER_NAME);
//
//            try
//            {
//                connection.PutStorageItem(Constants.CONTAINER_NAME, @"C:\TestStorageItem.iso");
//
//                var items = connection.GetContainerItemList(Constants.CONTAINER_NAME);
//                Assert.That(items.Contains("TestStorageItem.iso"), Is.True);
//            }
//            finally
//            {
//                if(connection.GetContainerItemList(Constants.CONTAINER_NAME).Contains("TestStorageItem.iso"))
//                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "TestStorageItem.iso");
//                connection.DeleteContainer(Constants.CONTAINER_NAME);
//            }
//        }
//    }
}