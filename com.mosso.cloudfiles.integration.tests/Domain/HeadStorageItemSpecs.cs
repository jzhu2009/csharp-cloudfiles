using System;
using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.HeadStorageItemSpecs
{
    [TestFixture]
    public class When_getting_information_on_a_storage_item : TestBase
    {
        [Test]
        public void Should_get_204_No_Content_when_item_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                testHelper.PutItemInContainer(Constants.HeadStorageItemName);
                testHelper.AddMetaTagsToItem(Constants.HeadStorageItemName);

                GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation(storageUrl, containerName, Constants.HeadStorageItemName, storageToken);
                GetStorageItemInformationResponse getStorageItemInformationResponse = new ResponseFactory<GetStorageItemInformationResponse>().Create(new CloudFilesRequest(getStorageItemInformation));
                Assert.That(getStorageItemInformationResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));

                Dictionary<string, string> metaTags = getStorageItemInformationResponse.MetaTags;

                testHelper.DeleteItemFromContainer(Constants.HeadStorageItemName);

                Assert.That(metaTags["Test"], Is.EqualTo("test"));
                Assert.That(metaTags["Test2"], Is.EqualTo("test2"));
            }
        }

        [Test]
        public void Should_get_404_when_item_does_not_existt()
        {
            string containerName = Guid.NewGuid().ToString();

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation(storageUrl, containerName, Constants.StorageItemName, storageToken);
                try
                {
                    IResponse headStorageItemResponse = new ResponseFactory<GetStorageItemInformationResponse>().Create(new CloudFilesRequest(getStorageItemInformation));
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (WebException)));
                }
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNameLengthException))]
        public void Should_throw_an_exception_when_the_length_of_the_container_name_exceeds_the_maximum_allowed_length()
        {
            GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation("a", new string('a', Constants.MaximumContainerNameLength + 1), "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_url_is_null()
        {
            GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation(null, "a", "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_container_name_is_null()
        {
            GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation("a", null, "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_object_name_is_null()
        {
            GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation("a", "a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_token_is_null()
        {
            GetStorageItemInformation getStorageItemInformation = new GetStorageItemInformation("a", "a", "a", null);
        }
    }
}