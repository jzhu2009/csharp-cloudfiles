using System;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.MarkContainerAsPublicSpecs
{
    [TestFixture]
    public class When_marking_a_container_as_public : TestBase
    {
        [Test]
        public void Should_return_a_public_uri_when_the_container_is_successfully_marked_public()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);
            try
            {
                string cdnUrl = connection.MarkContainerAsPublic(containerName);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.Length, Is.GreaterThan(0));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
        }

        private void MarkContainerPublic(IConnection connection, string containerName)
        {
            try
            {
                connection.MarkContainerAsPublic(containerName);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        [ExpectedException(typeof(ContainerAlreadyPublicException))]
        [Ignore("Currently not able to test this")]
        public void Should_fail_when_the_container_is_already_marked_public()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);
            MarkContainerPublic(connection, containerName);

            connection.MarkContainerAsPublic(containerName);
        }
    }
}