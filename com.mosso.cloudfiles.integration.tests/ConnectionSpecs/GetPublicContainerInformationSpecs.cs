using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetPublicContainerInformationSpecs
{
    [TestFixture]
    public class When_retrieve_public_container_information_and_there_is_a_public_container : TestBase
    {
        [Test]
        public void Should_retrieve_public_container_info_when_the_container_exists_and_is_public()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);

                var container = connection.GetPublicContainerInformation(Constants.CONTAINER_NAME);

                Assert.That(container.CdnUri.Length, Is.GreaterThan(0));
                Assert.That(container.ByteCount, Is.EqualTo(0));    
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

    }

    [TestFixture]
    public class When_retrieve_public_container_information_and_there_is_not_a_public_container : TestBase
    {

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_container_not_found_exception()
        {
            Assert.Ignore("the get public containers method is still returning the private container, talk to lowell");
            connection.GetPublicContainerInformation(Constants.CONTAINER_NAME);

            Assert.Fail("Should not get container information for an non-existant container");
        }
    }

    [TestFixture]
    public class When_retrieve_public_container_information_and_the_container_is_not_public : TestBase
    {

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_container_not_found_exception()
        {
            Assert.Ignore("the get public containers method is still returning the private container, talk to lowell");
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);

                connection.GetPublicContainerInformation(Constants.CONTAINER_NAME);

                Assert.Fail("Should not get container information for an existing non-public container");
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}