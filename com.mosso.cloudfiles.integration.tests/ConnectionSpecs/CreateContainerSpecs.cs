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
            
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_creating_a_new_container_and_the_container_already_exists : TestBase
    {
        [Test]
        public void Should_throw_a_container_already_exists_exception()
        {
            
            var exceptionWasThrown = false;

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.CreateContainer(Constants.CONTAINER_NAME);
            }
            catch (Exception exception)
            {
                Assert.That(exception.GetType(), Is.EqualTo(typeof(ContainerAlreadyExistsException)));
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