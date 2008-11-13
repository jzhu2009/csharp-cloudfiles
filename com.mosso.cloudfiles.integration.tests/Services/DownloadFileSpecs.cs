using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Collections.Generic;
using RequestHeaderFields=com.mosso.cloudfiles.domain.request.RequestHeaderFields;

namespace com.mosso.cloudfiles.integration.tests.services.GetFileSpecs
{
    [TestFixture]
    public class When_downloading_a_file_using_connection
    {
        private Connection connection;

        [SetUp]
        public void SetUp()
        {
            connection = new Connection(new UserCredentials(Constants.MOSSO_ACCOUNT, Constants.MOSSO_PASSWORD));
        }

        [Test]
        public void Should_return_nothing_if_a_local_file_name_is_supplied_and_the_download_is_successful()
        {
            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
                connection.GetStorageItem(containerName, Constants.StorageItemName, Constants.StorageItemName);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        public void Should_return_a_non_null_storage_object_when_no_local_file_name_is_supplied_and_the_download_is_successful()
        {
            string containerName = Guid.NewGuid().ToString();
            StorageItem storageItem;
            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
                storageItem = connection.GetStorageItem(containerName, Constants.StorageItemName);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }

            Assert.That(storageItem, Is.Not.Null);
            Assert.That(storageItem.ObjectStream, Is.Not.Null);
            Assert.That(storageItem.ObjectStream.CanRead, Is.EqualTo(true));
            storageItem.Dispose();
        }

        [Test]
        public void Should_throw_an_exception_when_the_file_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            bool exceptionWasThrown = false;

            try
            {
                connection.CreateContainer(containerName);
                connection.GetStorageItem(containerName, Constants.StorageItemName);
            }
            catch (Exception exception)
            {
                Assert.That(exception.GetType(), Is.EqualTo(typeof (StorageItemNotFoundException)));
                exceptionWasThrown = true;
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
            Assert.That(exceptionWasThrown, Is.True);
        }

        [Test]
        public void Should_return_partial_content_when_optional_range_header_is_specified()
        {
            string containerName = Guid.NewGuid().ToString();
            StorageItem si = null;
            try
            {
                Dictionary<RequestHeaderFields,string> requestHeaders = new Dictionary<RequestHeaderFields, string>();
                requestHeaders.Add(RequestHeaderFields.Range, "0-5");
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
                si = connection.GetStorageItem(containerName, Constants.StorageItemName, requestHeaders);
            }
            finally
            {
                if (si != null) si.Dispose();
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }
    }
}