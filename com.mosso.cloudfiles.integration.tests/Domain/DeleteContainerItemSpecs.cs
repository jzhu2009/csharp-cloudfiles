using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.DeleteStorageObjectSpecs
{
    [TestFixture]
    public class When_deleting_a_storage_item : TestBase
    {
        [Test]
        public void should_return_204_no_content_when_the_item_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                testHelper.PutItemInContainer();

                DeleteStorageItem deleteStorageItem = new DeleteStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken);
                IResponse response = new ResponseFactory<DeleteStorageItemResponse>().Create(new CloudFilesRequest(deleteStorageItem));

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
                Assert.That(response.Headers["Content-Type"], Is.EqualTo("text/plain; charset=UTF-8"));
            }
        }

        [Test]
        public void Shoulds_return_404_when_the_item_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                DeleteStorageItem deleteStorageItem = new DeleteStorageItem(storageUrl, containerName, Guid.NewGuid().ToString(), storageToken);
                try
                {
                    IResponse response = new ResponseFactory<DeleteStorageItemResponse>().Create(new CloudFilesRequest(deleteStorageItem));
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (WebException)));
                }
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_container_name_length_exceeds_the_maximum_allowed_length()
        {
            string containerName = new string('a', Constants.MaximumContainerNameLength + 1);
            try
            {
                using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
                {
                }
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (ContainerNameException)));
                Assert.That(ex, Is.TypeOf(typeof (ContainerNameException)));
            }
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_container_name_is_null()
        {
            DeleteStorageItem deleteStorageItem = new DeleteStorageItem("a", null, "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_url_name_is_null()
        {
            DeleteStorageItem deleteStorageItem = new DeleteStorageItem(null, "a", "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_object_name_is_null()
        {
            DeleteStorageItem deleteStorageItem = new DeleteStorageItem("a", "a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_token_is_null()
        {
            DeleteStorageItem deleteStorageItem = new DeleteStorageItem("a", "a", "a", null);
        }
    }
}