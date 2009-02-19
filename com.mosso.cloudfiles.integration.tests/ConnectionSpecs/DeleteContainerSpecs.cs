using System;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.DeleteContainerSpecs
{
    [TestFixture]
    public class When_deleting_a_container_using_connection : TestBase
    {
        [Test]
        public void Should_return_nothing_when_delete_is_successful()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);
            connection.DeleteContainer(Constants.CONTAINER_NAME);
        }

        [Test]
        [ExpectedException(typeof (ContainerNotFoundException))]
        public void Should_throw_exception_when_the_container_does_not_exist()
        {
            
            connection.DeleteContainer(Constants.CONTAINER_NAME);
        }

        [Test]
        public void Should_throw_exception_when_the_container_exists_but_is_not_empty()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);
            connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);

            try
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
            catch (Exception ex)
            {
                Assert.That(ex.GetType(), Is.EqualTo(typeof (ContainerNotEmptyException)));
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}