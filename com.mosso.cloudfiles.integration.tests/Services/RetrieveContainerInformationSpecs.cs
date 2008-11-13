using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services.RetrieveContainerInformationSpecs
{
    [TestFixture]
    public class When_requesting_information_on_a_container_using_connection
    {
        private Connection connection;

        [SetUp]
        public void SetUp()
        {
            connection = new Connection(new UserCredentials(Constants.MOSSO_ACCOUNT, Constants.MOSSO_PASSWORD));
        }

        [Test]
        public void Should_return_container_information_when_the_container_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);
                Container containerInformation = connection.GetContainerInformation(containerName);

                Assert.That(containerInformation.Name, Is.EqualTo(containerName));
                Assert.That(containerInformation.ByteCount, Is.EqualTo(0));
                Assert.That(containerInformation.ObjectCount, Is.EqualTo(0));
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            Container container = connection.GetContainerInformation(containerName);
        }
    }
}