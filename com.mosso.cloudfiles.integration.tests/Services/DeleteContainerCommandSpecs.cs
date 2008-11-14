using System;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.integration.tests.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services.DeleteContainerCommandSpecs
{
    [TestFixture]
    public class When_deleting_a_container_using_connection : TestBase
    {
        [Test]
        public void Should_return_nothing_when_delete_is_successful()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);
            connection.DeleteContainer(containerName);
        }

        [Test]
        [ExpectedException(typeof (ContainerNotFoundException))]
        public void Should_throw_exception_when_the_container_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.DeleteContainer(containerName);
        }

        [Test]
        public void Should_throw_exception_when_the_container_exists_but_is_not_empty()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);
            connection.PutStorageItem(containerName, Constants.StorageItemName);

            try
            {
                connection.DeleteContainer(containerName);
            }
            catch (Exception ex)
            {
                Assert.That(ex.GetType(), Is.EqualTo(typeof (ContainerNotEmptyException)));
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }
    }
}