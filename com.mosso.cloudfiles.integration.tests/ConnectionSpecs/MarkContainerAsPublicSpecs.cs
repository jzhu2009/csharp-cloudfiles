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
                var cdnUrl = connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        public void Should_be_able_to_get_the_cdn_uri_from_the_container_information()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                var cdnUrl = connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));

                var containerInformation = connection.GetContainerInformation(Constants.CONTAINER_NAME);
                Assert.That(containerInformation.CdnUri, Is.EqualTo(cdnUrl.AbsoluteUri));
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [TestFixture]
        public class and_adding_a_time_to_live_TTL_parameter : TestBase
        {
            // Have to use GUID container name instead of our constant because minimum TTL for a
            // container on the CDN is 1 hour (3600 seconds).  Don't want the integration test
            // to take that long. ;)

            [Test]
            public void should_add_the_ttl_to_the_container()
            {
                string containerName = Guid.NewGuid().ToString();
                try
                {
                    connection.CreateContainer(containerName);
                    Uri cdnUrl = connection.MarkContainerAsPublic(containerName, 10500);
                    Assert.That(cdnUrl, Is.Not.Null);
                    Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));
                    var publicContainerInformation = connection.GetPublicContainerInformation(containerName);
                    Assert.That(publicContainerInformation.TTL, Is.EqualTo(10500));
                }
                finally
                {
                    connection.MarkContainerAsPrivate(containerName);
                    connection.DeleteContainer(containerName);
                }
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