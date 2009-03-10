using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.DeleteContainerSpecs
{
    [TestFixture]
    public class When_deleting_a_container : TestBase
    {
        [Test]
        public void Should_return_no_content_when_the_container_exists()
        {
            PutContainer(storageUrl, Constants.CONTAINER_NAME);
            var deleteContainer = new DeleteContainer(storageUrl, storageToken, Constants.CONTAINER_NAME);
            var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(deleteContainer));
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        [ExpectedException(typeof (WebException))]
        public void Should_return_404_when_container_does_not_exist()
        {
            var deleteContainer = new DeleteContainer(storageUrl, storageToken, Guid.NewGuid().ToString());

            new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(deleteContainer));
            Assert.Fail("404 Not found exception expected");
        }

        [Test]
        public void Should_return_conflict_status_when_the_container_exists_and_is_not_empty()
        {
            
            try
            {
                using (new TestHelper(storageToken, storageUrl))
                {
                    var putStorageItem = new PutStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, Constants.StorageItemName, storageToken);
                    Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));

                    var putStorageItemResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                    Assert.That(putStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.Created));
                }
                Assert.Fail("409 conflict expected");
            }
            catch (WebException we)
            {
                var response = (HttpWebResponse)we.Response;
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            }
            finally
            {
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(
                    new DeleteStorageItem(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, storageToken)));
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(
                    new DeleteContainer(storageUrl, storageToken, Constants.CONTAINER_NAME)));
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_container_name_exceeds_the_maximum_length()
        {
            string containerName = new string('a', Constants.MaximumContainerNameLength + 1);
            try
            {
                using (new TestHelper(storageToken, storageUrl, containerName))
                {
                    var putStorageItem = new PutStorageItem(storageUrl, containerName, Constants.StorageItemName, Constants.StorageItemName, storageToken);
                    Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));

                    var putStorageItemResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                    Assert.That(putStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.Created));
                }
                Assert.Fail("ContainerNameException expected");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (ContainerNameException)));
            }
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_url_is_null()
        {
            new DeleteContainer(null, "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_container_name_is_null()
        {
            new DeleteContainer("a", "a", null);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_token_is_null()
        {
            new DeleteContainer("a", null, "a");
        }

        private void PutContainer(string storageUri, String containerName)
        {
            var createContainer = new CreateContainer(storageUri, storageToken, containerName);

            IResponse response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(createContainer));
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
        }
    }
}