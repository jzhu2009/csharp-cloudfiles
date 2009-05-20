using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Collections.Generic;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetPublicContainersSpecs
{
    [TestFixture]
    public class When_requesting_a_list_of_public_containers_and_there_are_public_containers : TestBase
    {

        [Test]
        public void Should_retrieve_a_list_of_public_containers_on_the_cdn()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                Uri cdnUrl = connection.MarkContainerAsPublic(Constants.CONTAINER_NAME);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.ToString().Length, Is.GreaterThan(0));

                List<string> containerList = connection.GetPublicContainers();
                Assert.That(containerList, Is.Not.Null);
                Assert.That(containerList.Count, Is.GreaterThan(0));
                Assert.That(containerList.Contains(Constants.CONTAINER_NAME));
            }
            finally
            {
                connection.MarkContainerAsPrivate(Constants.CONTAINER_NAME);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_requesting_a_list_of_public_containers_and_there_are_no_shared_containers : TestBase
    {

        [Test]
        public void Should_retrieve_a_list_with_count_of_zero()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                var containerList = connection.GetPublicContainers();
                Assert.That(containerList.Count, Is.EqualTo(0));
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}