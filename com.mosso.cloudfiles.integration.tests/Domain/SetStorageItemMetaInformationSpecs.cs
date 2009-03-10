using System;
using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.SetStorageItemMetaInformationSpecs
{
    [TestFixture]
    public class When_posting_to_storage_objects : TestBase
    {
        [Test]
        public void Should_throw_exception_when_meta_key_exceeds_128_characters()
        {

            using (new TestHelper(storageToken, storageUrl))
            {
                try
                {
                    Dictionary<string, string> metadata = new Dictionary<string, string> {{new string('a', 129), "test"}};
                    SetStorageItemMetaInformation setStorageItemMetaInformation = new SetStorageItemMetaInformation(storageUrl, Constants.CONTAINER_NAME, Guid.NewGuid().ToString(), metadata, storageToken);
                    new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(setStorageItemMetaInformation));
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (MetaKeyLengthException)));
                }
            }
        }

        [Test]
        public void Should_throw_exception_when_meta_value_exceeds_256_characters()
        {
            
            using (new TestHelper(storageToken, storageUrl))
            {
                try
                {
                    Dictionary<string, string> metadata = new Dictionary<string, string> {{new string('a', 10), new string('f', 257)}};
                    SetStorageItemMetaInformation setStorageItemMetaInformation = new SetStorageItemMetaInformation(storageUrl, Constants.CONTAINER_NAME, Guid.NewGuid().ToString(), metadata, storageToken);
                    new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(setStorageItemMetaInformation));
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (MetaValueLengthException)));
                }
            }
        }

        [Test]
        public void Should_return_accepted_when_meta_information_is_supplied()
        {
            

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                testHelper.PutItemInContainer();

                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("Test", "test");
                metadata.Add("Test2", "test2");

                SetStorageItemMetaInformation setStorageItemMetaInformation = new SetStorageItemMetaInformation(storageUrl, Constants.CONTAINER_NAME, Constants.StorageItemName, metadata, storageToken);

                var metaInformationResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(setStorageItemMetaInformation));

                Assert.That(metaInformationResponse.Status, Is.EqualTo(HttpStatusCode.Accepted));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_return_404_not_found_when_requested_object_does_not_exist()
        {
            
            using (new TestHelper(storageToken, storageUrl))
            {
                try
                {
                    Dictionary<string, string> metadata = new Dictionary<string, string>();
                    SetStorageItemMetaInformation setStorageItemMetaInformation = new SetStorageItemMetaInformation(storageUrl, Constants.CONTAINER_NAME, Guid.NewGuid().ToString(), metadata, storageToken);
                    new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(setStorageItemMetaInformation));
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (WebException)));
                }
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNameException))]
        public void Should_throw_an_exception_when_the_container_name_length_exceeds_the_maximum_characters_allowed()
        {
            new SetStorageItemMetaInformation("a", new string('a', Constants.MaximumContainerNameLength + 1), "a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_storage_url_is_null()
        {
            new SetStorageItemMetaInformation(null, "a", "a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_container_name_is_null()
        {
            new SetStorageItemMetaInformation("a", null, "a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_storage_object_name_is_null()
        {
            new SetStorageItemMetaInformation("a", "a", null, null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_storage_token_is_null()
        {
            new SetStorageItemMetaInformation("a", "a", "a", null, null);
        }
    }
}