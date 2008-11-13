using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.HeadContainerSpecs
{
    [TestFixture]
    public class When_retrieving_the_statistics_on_a_container : TestBase
    {
        [Test]
        public void Should_return_no_content_when_the_container_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);
                GetContainerInformation getContainerInformation = new GetContainerInformation(storageUrl, containerName, storageToken);

                GetContainerInformationResponse informationResponse = new ResponseFactory<GetContainerInformationResponse>().Create(new CloudFilesRequest(getContainerInformation));
                Assert.That(informationResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));
                Assert.That(informationResponse.ObjectCount, Is.EqualTo("1"));
                Assert.That(informationResponse.BytesUsed, (Is.Not.Null));
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);
            }
        }

        [Test]
        public void Should_return_no_content_when_the_container_exists_and_the_name_contains_spaces()
        {
            string containerName = "I am making a funky container";
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);
                GetContainerInformation getContainerInformation = new GetContainerInformation(storageUrl, containerName, storageToken);

                GetContainerInformationResponse informationResponse = new ResponseFactory<GetContainerInformationResponse>().Create(new CloudFilesRequest(getContainerInformation));
                Assert.That(informationResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));
                Assert.That(informationResponse.ObjectCount, Is.EqualTo("1"));
                Assert.That(informationResponse.BytesUsed, (Is.Not.Null));
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);
            }
        }

        [Test]
        [ExpectedException(typeof (WebException))]
        public void Should_return_not_found_when_the_container_does_not_exist()
        {
            GetContainerInformation getContainerInformation = new GetContainerInformation(storageUrl, "Idonthasacontainer", storageToken);

            IResponse response = new ResponseFactory<GetContainerInformationResponse>().Create(new CloudFilesRequest(getContainerInformation));
            Assert.Fail("Expecting a 404 error when trying to retrieve data about a non-existent container");
        }

        [Test]
        [ExpectedException(typeof (ContainerNameLengthException))]
        public void Should_throw_an_exception_when_the_container_name_exceeds_the_maximum_allowed_length()
        {
            GetContainerInformation getContainerInformation = new GetContainerInformation(storageUrl, new string('a', Constants.MaximumContainerNameLength + 1), storageToken);

            IResponse response = new ResponseFactory<GetContainerInformationResponse>().Create(new CloudFilesRequest(getContainerInformation));
            Assert.Fail("Expecting a ContainerNameLengthException");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_storage_url_is_null()
        {
            GetContainerInformation getContainerInformation = new GetContainerInformation(null, "a", "whatever");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_container_name_is_null()
        {
            GetContainerInformation getContainerInformation = new GetContainerInformation("a", null, "whatever");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_storage_token_is_null()
        {
            GetContainerInformation getContainerInformation = new GetContainerInformation("a", "a", null);
        }
    }
}