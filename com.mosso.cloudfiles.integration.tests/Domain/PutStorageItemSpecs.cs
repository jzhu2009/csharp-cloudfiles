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

namespace com.mosso.cloudfiles.integration.tests.domain.PutStoragecsSpecs
{
    [TestFixture]
    public class When_putting_storage_objects : TestBase
    {
        [Test]
        public void Should_return_created_as_status_when_the_file_does_not_already_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, Constants.StorageItemName, Constants.StorageItemName, storageToken);

                Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));

                PutStorageItemResponse response = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.ETag, Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_return_created_as_status_when_the_file_does_not_already_exist_and_meta_information_is_supplied()
        {
            string containerName = Guid.NewGuid().ToString();
            Dictionary<string, string> metaTags = new Dictionary<string, string>
                                                      {
                                                          {Constants.MetadataKey, Constants.MetadataValue}
                                                      };
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, Constants.StorageItemName, Constants.StorageItemName, storageToken);

                Assert.That(putStorageItem.ContentLength, Is.GreaterThan(0));

                PutStorageItemResponse response = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.ETag, Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_throw_file_IO_exception_when_file_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                try
                {
                    PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, "#%", "noexists.fail", storageToken);
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (FileNotFoundException)));
                }
            }
        }

        [Test]
        public void Should_not_allow_upload_of_item_if_name_exceeds_128_characters()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                try
                {
                    PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, new string('a', 129), "%#", storageToken);
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (StorageItemNameLengthException)));
                }
            }
        }

        [Test]
        public void Should_return_created_when_etag_is_not_supplied_because_it_is_optional()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, Constants.StorageItemName, Constants.StorageItemName, storageToken);
                putStorageItem.Headers.Remove("ETag");

                IResponse response = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_return_created_when_a_stream_is_passed_instead_of_a_file_name()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                FileStream fs = new FileStream(Constants.StorageItemName, FileMode.Open);
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, Constants.StorageItemName, fs, storageToken, null);
                IResponse response = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem));
                fs.Close();
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_return_created_when_content_length_is_not_supplied_because_it_is_optional()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, Constants.StorageItemName, Constants.StorageItemName, storageToken);
                putStorageItem.Headers.Remove("Content-Length");

                PutStorageItemResponse response = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.ETag, Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer();
            }
        }


        [Test]
        public void Should_return_created_when_content_type_is_not_supplied_because_it_is_optional()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, Constants.StorageItemName, Constants.StorageItemName, storageToken);
                putStorageItem.Headers.Remove("Content-Type");

                PutStorageItemResponse response = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem));
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.ETag, Is.EqualTo(putStorageItem.ETag));
                testHelper.DeleteItemFromContainer();
            }
        }

        [Test]
        public void Should_throw_a_WebException_with_status_code_422_when_the_ETag_passed_does_not_match_MD5_of_the_file()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                PutStorageItem putStorageItem = new PutStorageItem(storageUrl, containerName, Constants.StorageItemName, Constants.StorageItemName, storageToken);
                putStorageItem.Headers.Remove("ETag");
                putStorageItem.Headers.Add("ETag", new string('A', 32));
                try
                {
                    IResponse response = new ResponseFactory<PutStorageItemResponse>().Create(new CloudFilesRequest(putStorageItem));
                }
                catch (Exception ex)
                {
                    Assert.That(ex, Is.TypeOf(typeof (WebException)));
                }
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNameLengthException))]
        public void Should_throw_an_exception_when_a_file_name_is_passed_and_the_container_name_exceeds_the_maximum_length()
        {
            PutStorageItem putStorageItem = new PutStorageItem("a", new string('a', Constants.MaximumContainerNameLength + 1), "a", "a", "a");
        }

        [Test]
        public void Should_throw_an_exception_when_a_stream_is_passed_and_the_container_name_exceeds_the_maximum_length()
        {
            FileStream s = new FileStream(Constants.StorageItemName, FileMode.Open);
            try
            {
                PutStorageItem putStorageItem = new PutStorageItem("a", new string('a', Constants.MaximumContainerNameLength + 1), "a", s, "a");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (ContainerNameLengthException)));
            }
            s.Close();
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_a_file_name_is_passed_and_the_storage_url_is_null()
        {
            PutStorageItem putStorageItem = new PutStorageItem(null, "a", "a", "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_a_file_name_is_passed_and_the_container_name_is_null()
        {
            PutStorageItem putStorageItem = new PutStorageItem("a", null, "a", "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_a_file_name_is_passed_and_the_storage_object_name_is_null()
        {
            PutStorageItem putStorageItem = new PutStorageItem("a", "a", null, "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_a_file_name_is_passed_and_the_file_uri_is_null()
        {
            PutStorageItem putStorageItem = new PutStorageItem("a", "a", "a", "", "a");
        }


        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_a_file_name_is_passed_and_the_storage_token_is_null()
        {
            PutStorageItem putStorageItem = new PutStorageItem("a", "a", "a", "a", null);
        }

        [Test]
        public void Should_throw_an_exception_when_a_stream_is_passed_and_the_storage_url_is_null()
        {
            FileStream s = new FileStream(Constants.StorageItemName, FileMode.Open);
            try
            {
                PutStorageItem putStorageItem = new PutStorageItem(null, "a", "a", s, "a");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (ArgumentNullException)));
            }
            s.Close();
        }

        [Test]
        public void Should_throw_an_exception_when_a_stream_is_passed_and_the_container_name_is_null()
        {
            FileStream s = new FileStream(Constants.StorageItemName, FileMode.Open);
            try
            {
                PutStorageItem putStorageItem = new PutStorageItem("a", null, "a", s, "a");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (ArgumentNullException)));
            }
            s.Close();
        }

        [Test]
        public void Should_throw_an_exception_when_a_stream_is_passed_and_the_storage_object_name_is_null()
        {
            FileStream s = new FileStream(Constants.StorageItemName, FileMode.Open);
            try
            {
                PutStorageItem putStorageItem = new PutStorageItem("a", "a", null, s, "a");
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (ArgumentNullException)));
            }
            s.Close();
        }

        [Test]
        public void Should_throw_an_exception_when_a_stream_is_passed_and_the_storage_token_is_null()
        {
            FileStream s = new FileStream(Constants.StorageItemName, FileMode.Open);
            try
            {
                PutStorageItem putStorageItem = new PutStorageItem("a", "a", "a", s, null);
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (ArgumentNullException)));
            }
            s.Close();
        }

      
    }
}