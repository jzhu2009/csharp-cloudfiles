using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Collections.Generic;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetPublicContainersSpecs
{
    [TestFixture]
    public class When_requesting_a_list_of_public_containers : TestBase
    {

        [Test]
        public void Should_retrieve_a_list_of_public_containers_on_the_cdn_when_there_are_shared_containers()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            try
            {
                string cdnUrl = connection.MarkContainerAsPublic(containerName);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.Length, Is.GreaterThan(0));

                List<string> containerList = connection.GetPublicContainers();
                Assert.That(containerList, Is.Not.Null);
                Assert.That(containerList.Count, Is.GreaterThan(0));
                Assert.That(containerList.Contains(containerName));

            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
        }
    }
}