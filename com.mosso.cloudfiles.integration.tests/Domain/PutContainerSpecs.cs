using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.PutContainerSpecs
{
    [TestFixture]
    public class When_creating_a_container : TestBase
    {
        [Test]
        [ExpectedException(typeof (ContainerNameLengthException))]
        public void Should_throw_exception_if_container_name_greater_than_64_characters()
        {
            CreateContainer createContainer = new CreateContainer(storageUrl, storageToken, Constants.BadContainerName);
            Assert.Fail("Should fail due to container name exceeding 64 characters");
        }

        [Test]
        public void Should_return_created_status_when_the_container_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            CreateContainer createContainer = new CreateContainer(storageUrl, storageToken, containerName);

            IResponse response = new ResponseFactory<CreateContainerResponse>().Create(new CloudFilesRequest(createContainer));
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));

            DeleteContainer(storageUrl, containerName);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_url_is_null()
        {
            CreateContainer createContainer = new CreateContainer(null, "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_container_name_is_null()
        {
            CreateContainer createContainer = new CreateContainer("a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_token_is_null()
        {
            CreateContainer createContainer = new CreateContainer("a", "a", null);
        }


        [Test]
        public void Should_return_accepted_status_when_the_container_already_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            CreateContainer createContainer = new CreateContainer(storageUrl, storageToken, containerName);

            IResponse response = new ResponseFactory<CreateContainerResponse>().Create(new CloudFilesRequest(createContainer));
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));

            createContainer = new CreateContainer(storageUrl, storageToken, containerName);

            response = new ResponseFactory<CreateContainerResponse>().Create(new CloudFilesRequest(createContainer, null));
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Accepted));

            DeleteContainer(storageUrl, containerName);
        }

        private void DeleteContainer(string storageUrl, string containerName)
        {
            cloudfiles.domain.request.DeleteContainer deleteContainer = new DeleteContainer(storageUrl, containerName, storageToken);

            IResponse response = new ResponseFactory<DeleteContainerResponse>().Create(new CloudFilesRequest(deleteContainer));
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}