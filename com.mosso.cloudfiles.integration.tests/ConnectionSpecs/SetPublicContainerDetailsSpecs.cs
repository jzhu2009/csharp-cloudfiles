using System;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.SetPublicContainerDetailsSpecs
{
    [TestFixture]
    public class When_marking_a_container_as_private_and_the_container_does_not_exist : TestBase
    {
        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_return_a_public_cdn_uri()
        {
            Assert.Ignore("the get public containers method is still returning the private container, talk to lowell");
            connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);

            Assert.Fail("Container should not exist");
        }
    }

    [TestFixture]
    public class When_marking_a_container_as_private_and_it_is_public_already : TestBase
    {
        //todo: BECAUSE I SAID SO
        [Test]
        [Ignore("the get public containers method is still returning the private container, talk to lowell")]
        public void should_remove_it_from_the_public_containers_list()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                Uri cdnUrl = connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));

                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);

                var publicContainers = connection.GetPublicContainers();
                Assert.That(publicContainers.Contains(Constants.CONTAINER_NAME), Is.False);
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}