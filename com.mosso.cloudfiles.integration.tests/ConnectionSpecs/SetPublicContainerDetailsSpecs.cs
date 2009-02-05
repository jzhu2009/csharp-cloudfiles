using System;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs
{
    [TestFixture]
    public class When_updating_a_public_containers_information : TestBase
    {
        [Test]
        public void Should_return_a_cdn_url_when_the_container_is_public()
        {
            using (TestHelper testHelper = new TestHelper(connection, true, true))
            {
                string cdnUrl = connection.SetPublicContainerDetails(testHelper.ContainerName, true);
                Assert.That(cdnUrl, Is.Not.Null);
                Assert.That(cdnUrl.Length, Is.GreaterThan(0));
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            using (TestHelper testHelper = new TestHelper(connection, false, false))
            {
                connection.SetPublicContainerDetails(testHelper.ContainerName, true);
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_container_exists_but_is_not_public()
        {
            bool excepted = false;
            using (TestHelper testHelper = new TestHelper(connection, true, false))
            {
                try
                {
                    connection.SetPublicContainerDetails(testHelper.ContainerName, true);
                }
                catch
                {
                    excepted = true;
                }
            }
            Assert.That(excepted, Is.True);
        }
    }

    public class TestHelper : IDisposable
    {
        private readonly IConnection connection;
        private readonly bool createContainer;
        private string containerName;


        public TestHelper(IConnection connection, bool createContainer, bool markPublic)
        {
            this.connection = connection;
            this.createContainer = createContainer;
            containerName = Guid.NewGuid().ToString();
            if (createContainer)
            {
                connection.CreateContainer(containerName);
                if (markPublic) connection.MarkContainerAsPublic(containerName);
            }
        }

        public string ContainerName
        {
            get { return containerName; }
        }

        public void Dispose()
        {
            if (createContainer) connection.DeleteContainer(containerName);
        }
    }

}