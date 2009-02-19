using System;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.DeleteStorageItemSpecs
{
    [TestFixture]
    public class When_deleting_a_storage_object_using_connection : TestBase
    {
        [Test]
        public void Should_return_nothing_when_successful()
        {
            
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_file_does_not_exist()
        {
            
            bool exceptionWasThrown = false;

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
            }
            catch (Exception exception)
            {
                Assert.That(exception.GetType(), Is.EqualTo(typeof (StorageItemNotFoundException)));
                exceptionWasThrown = true;
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(exceptionWasThrown, Is.True);
        }
    }
}