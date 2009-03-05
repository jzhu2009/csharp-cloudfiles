using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.GetStorageItemInformationSpecs
{
    [TestFixture]
    public class When_getting_information_on_a_storage_item : TestBase
    {
        [Test]
        public void Should_get_204_No_Content_when_item_exists()
        {
            
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                try
                {
                    testHelper.PutItemInContainer(Constants.HeadStorageItemName);
                    testHelper.AddMetadataToItem(Constants.HeadStorageItemName);

                    var getStorageItemInformation = new GetStorageItemInformation(storageUrl, Constants.CONTAINER_NAME, Constants.HeadStorageItemName, storageToken);
                    var getStorageItemInformationResponse = new ResponseFactory<GetStorageItemInformationResponse>().Create(new CloudFilesRequest(getStorageItemInformation));
                    Assert.That(getStorageItemInformationResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));

                    var metadata = getStorageItemInformationResponse.Metadata;
                    Assert.That(metadata["Test"], Is.EqualTo("test"));
                    Assert.That(metadata["Test2"], Is.EqualTo("test2"));
                }
                finally
                {
                    testHelper.DeleteItemFromContainer(Constants.HeadStorageItemName);
                }
            }
        }

        [Test]
        public void Should_get_404_when_item_does_not_existt()
        {
            

            using (new TestHelper(storageToken, storageUrl))
            {
                var getStorageItemInformation = new GetStorageItemInformation(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, storageToken);
                try
                {
                    new ResponseFactory<GetStorageItemInformationResponse>().Create(new CloudFilesRequest(getStorageItemInformation));
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (WebException)));
                }
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNameException))]
        public void Should_throw_an_exception_when_the_length_of_the_container_name_exceeds_the_maximum_allowed_length()
        {
            new GetStorageItemInformation("a", new string('a', Constants.MaximumContainerNameLength + 1), "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_url_is_null()
        {
            new GetStorageItemInformation(null, "a", "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_container_name_is_null()
        {
            new GetStorageItemInformation("a", null, "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_object_name_is_null()
        {
            new GetStorageItemInformation("a", "a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_token_is_null()
        {
            new GetStorageItemInformation("a", "a", "a", null);
        }
    }
}