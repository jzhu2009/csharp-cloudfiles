using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.MarkContainerAsPublicSpecs
{
    [TestFixture]
    public class When_marking_a_container_as_public_and_it_is_not_public_already : TestBase
    {
        [Test]
        public void Should_return_a_public_cdn_uri()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                Uri cdnUrl = connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_marking_a_container_as_public_and_it_is_public_already : TestBase
    {
        [Test]
        public void should_allow_redundant_setting_of_public_status()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                Uri cdnUrl = connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));

                connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);

                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}