using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.PutStorageItemSpecs
{
    [TestFixture]
    public class When_putting_storage_objects : TestBase
    {
        [Test]
        public void Should_return_created_as_status_when_the_file_does_not_already_exist()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemName, Constants.StorageItemName);

                Assert.That(putStorageItem.ContentLength, Is.EqualTo(34));

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_set_content_type_of_jpg_for_local_file_upload()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemNameJpg, Constants.StorageItemNameJpg);

                Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));
                Assert.That(putStorageItem.ContentType, Is.EqualTo("image/jpeg"));

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer(Constants.StorageItemNameJpg);
            }
        }

        [Test]
        public void Should_still_come_back_as_pdf_even_when_sent_up_as_octet_stream()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var file = new FileInfo(Constants.StorageItemNamePdf);
                var metadata = new Dictionary<string, string>();
                metadata.Add("Source", "1");
                metadata.Add("Note", "2");
                const string DUMMY_FILE_NAME = "HAHAHA";

                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, DUMMY_FILE_NAME, file.Open(FileMode.Open), metadata);

                Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));
                Assert.That(putStorageItem.ContentType, Is.EqualTo("application/octet-stream"));

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));

                var getStorageItem = new GetStorageItem(storageUrl, Constants.CONTAINER_NAME, DUMMY_FILE_NAME, authToken);
                var getStorageItemResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getStorageItem));
                Assert.That(getStorageItemResponse.ContentType, Is.EqualTo("application/octet-stream"));
                getStorageItemResponse.Dispose();

                testHelper.DeleteItemFromContainer(DUMMY_FILE_NAME);
            }
        }

        [Test]
        public void Should_set_content_type_of_gif_for_local_file_upload()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemNameGif, Constants.StorageItemNameGif);

                Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));
                Assert.That(putStorageItem.ContentType, Is.EqualTo("image/gif"));

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer(Constants.StorageItemNameGif);
            }
        }

        [Test]
        public void Should_set_content_type_of_jpg_for_stream_upload()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var fileStream = new FileStream(Constants.StorageItemNameJpg, FileMode.Open);
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemNameJpg, fileStream);

                Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));
                Assert.That(putStorageItem.ContentType, Is.EqualTo("image/jpeg"));

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer(Constants.StorageItemNameJpg);
            }
        }

        [Test]
        public void Should_set_content_type_of_gif_for_stream_upload()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var fileStream = new FileStream(Constants.StorageItemNameGif, FileMode.Open);
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemNameGif, fileStream);

                Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));
                Assert.That(putStorageItem.ContentType, Is.EqualTo("image/gif"));

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer(Constants.StorageItemNameGif);
            }
        }

        [Test]
        public void Should_return_created_as_status_when_the_file_does_not_already_exist_and_meta_information_is_supplied()
        {
            
            Dictionary<string, string> metadata = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemName, Constants.StorageItemName, metadata);

                Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_return_created_when_etag_is_not_supplied_because_it_is_optional()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemName, Constants.StorageItemName);
                putStorageItem.Headers.Remove("ETag");

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_return_created_when_a_stream_is_passed_instead_of_a_file_name()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var fs = new FileStream(Constants.StorageItemName, FileMode.Open);
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemName, fs, null);
                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                fs.Close();
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_return_created_when_content_length_is_not_supplied_because_it_is_optional()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemName, Constants.StorageItemName);
                putStorageItem.Headers.Remove("Content-Length");

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer();
            }
        }


        [Test]
        public void Should_return_created_when_content_type_is_not_supplied_because_it_is_optional()
        {
            
            using (TestHelper testHelper = new TestHelper(authToken, storageUrl))
            {
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemName, Constants.StorageItemName);
                putStorageItem.Headers.Remove("Content-Type");

                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Headers[Constants.ETAG], Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_throw_a_WebException_with_status_code_422_when_the_ETag_passed_does_not_match_MD5_of_the_file()
        {
            
            using (new TestHelper(authToken, storageUrl))
            {
                var putStorageItem = new PutStorageItem(storageUrl, authToken, Constants.CONTAINER_NAME, Constants.StorageItemName, Constants.StorageItemName);
                putStorageItem.Headers.Remove("ETag");
                putStorageItem.Headers.Add("ETag", new string('A', 32));
                try
                {
                    new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(putStorageItem));
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (WebException)));
                }
            }
        }

        
    }
}