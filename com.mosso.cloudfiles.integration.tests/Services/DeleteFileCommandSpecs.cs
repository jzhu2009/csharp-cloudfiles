using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services.DeleteFileCommandSpecs
{
    [TestFixture]
    public class When_deleting_a_storage_object_using_connection
    {
        private Connection connection;

        [SetUp]
        public void SetUp()
        {
            connection = new Connection(new UserCredentials(Constants.MOSSO_ACCOUNT, Constants.MOSSO_PASSWORD));
        }

        [Test]
        public void Should_return_nothing_when_successful()
        {
            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_file_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            bool exceptionWasThrown = false;

            try
            {
                connection.CreateContainer(containerName);
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
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
    }
}