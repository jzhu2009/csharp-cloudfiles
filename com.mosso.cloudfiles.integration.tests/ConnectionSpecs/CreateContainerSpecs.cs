using System;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.CreateContainerSpecs
{
    [TestFixture]
    public class When_creating_a_new_container_with_connection : TestBase
    {
        [Test]
        public void Should_return_nothing_when_a_new_container_is_created()
        {
            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_container_already_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            bool exceptionWasThrown = false;

            try
            {
                connection.CreateContainer(containerName);
                connection.CreateContainer(containerName);
            }
            catch (Exception exception)
            {
                Assert.That(exception.GetType(), Is.EqualTo(typeof (ContainerAlreadyExistsException)));
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