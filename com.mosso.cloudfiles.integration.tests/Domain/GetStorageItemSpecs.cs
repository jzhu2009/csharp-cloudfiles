using System;
using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.GetStorageItemSpecs
{
    [TestFixture]
    public class When_getting_a_storage_object : TestBase
    {
        [Test]
        public void Should_return_ok_if_the_object_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        public void Should_return_ok_if_the_object_exists_and_valid_content_type_of_image_gif()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemNameGif);
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemNameGif, storageToken);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(getStorageItemResponse.Headers["Content-Type"], Is.EqualTo("image/gif"));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer(Constants.StorageItemNameGif);
                }
            }
        }

        [Test]
        public void Should_return_ok_if_the_object_exists_and_valid_content_type_of_image_jpeg()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemNameJpg);
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemNameJpg, storageToken);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(getStorageItemResponse.Headers["Content-Type"], Is.EqualTo("image/jpeg"));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer(Constants.StorageItemNameJpg);
                }
            }
        }

        [Test]
        public void Should_return_not_found_if_the_object_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            using (new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName,Constants.StorageItemName, storageToken);
                bool exceptionWasThrown = false;

                IResponseWithContentBody getStorageItemResponse = null;
                try
                {
                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                }
                catch (WebException ex)
                {
                    exceptionWasThrown = true;
                    HttpStatusCode code = ((HttpWebResponse) ex.Response).StatusCode;
                    Assert.That(code, Is.EqualTo(HttpStatusCode.NotFound));
                    if (getStorageItemResponse != null)getStorageItemResponse.Dispose();
                }

                Assert.That(exceptionWasThrown, Is.True);
            }
        }
    }

    [TestFixture]
    public class When_getting_a_storage_item_and_providing_a_if_match_request_header : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        private const string DUMMY_ETAG = "5c66108b7543c6f16145e25df9849f7f";

        protected override void SetUp()
        {
            requestHeaderFields = new Dictionary<RequestHeaderFields, string>();
            requestHeaderFields.Add(RequestHeaderFields.IfMatch, DUMMY_ETAG);
        }

        [Test]
        public void Should_return_ok_if_the_item_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }
    }

    [TestFixture]
    public class When_getting_a_storage_item_and_providing_a_if_none_match_request_header : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        private const string DUMMY_ETAG = "5c66108b7543c6f16145e25df9849f7fTest";

        protected override void SetUp()
        {
            requestHeaderFields = new Dictionary<RequestHeaderFields, string>();
            requestHeaderFields.Add(RequestHeaderFields.IfNoneMatch, DUMMY_ETAG);
        }

        [Test]
        public void Should_return_ok_if_the_item_exists()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }
    }

    [TestFixture]
    public class When_getting_a_storage_item_and_providing_a_if_modified_since_request_header : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        private readonly DateTime pastDateTime = DateTime.Now.AddDays(-6);
        private readonly DateTime futureDateTime = DateTime.Now.AddDays(6);

        [Test]
        public void Should_return_not_modified_if_the_item_exists_and_it_hasnt_been_modified()
        {
            string containerName = Guid.NewGuid().ToString();

            requestHeaderFields = new Dictionary<RequestHeaderFields, string>();
            requestHeaderFields.Add(RequestHeaderFields.IfModifiedSince, futureDateTime.ToString());

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                bool exceptionWasThrown = false;
                try
                {
                    testHelper.PutItemInContainer();
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);
                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                } 
                catch(WebException ex)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotModified));
                    exceptionWasThrown = true;
                }
                finally
                {
                    if(getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }

                Assert.That(exceptionWasThrown, Is.True);
            }
        }

        [Test]
        public void Should_return_item_if_the_item_exists_and_has_been_modified_since_designated_time()
        {
            string containerName = Guid.NewGuid().ToString();

            requestHeaderFields = new Dictionary<RequestHeaderFields, string>();
            requestHeaderFields.Add(RequestHeaderFields.IfModifiedSince, pastDateTime.ToString());

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }
    }

    [TestFixture]
    public class When_getting_a_storage_item_and_providing_a_if_unmodified_since_request_header : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        private readonly DateTime pastDateTime = DateTime.Now.AddDays(-6);
        private readonly DateTime futureDateTime = DateTime.Now.AddDays(6);

        [Test]
        public void Should_return_item_if_the_item_exists_and_it_hasnt_been_modified()
        {
            string containerName = Guid.NewGuid().ToString();

            requestHeaderFields = new Dictionary<RequestHeaderFields, string>();
            requestHeaderFields.Add(RequestHeaderFields.IfUnmodifiedSince, futureDateTime.ToString());

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);
                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        [Ignore("Not throwing 412 when file has been modified")]
        public void Should_return_412_precondition_failed_if_the_item_exists_and_has_been_modified_since_designated_time()
        {
            string containerName = Guid.NewGuid().ToString();

            requestHeaderFields = new Dictionary<RequestHeaderFields, string>();
            requestHeaderFields.Add(RequestHeaderFields.IfUnmodifiedSince, pastDateTime.ToString());

            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                bool exceptionWasThrown = false;
                try
                {
                    testHelper.PutItemInContainer();
                    testHelper.PutItemInContainer();
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                }
                catch (WebException ex)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.PreconditionFailed));
                    exceptionWasThrown = true;
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }

                Assert.That(exceptionWasThrown, Is.True);
            }
        }
    }

    [TestFixture]
    public class When_including_a_range_header_when_retrieving_a_storage_item : TestBase
    {
        private Dictionary<RequestHeaderFields, string> requestHeaderFields;
        

        protected override void SetUp()
        {
            requestHeaderFields = new Dictionary<RequestHeaderFields, string>();
        }

        [Test]
        public void Should_return_partial_content_if_the_item_exists_and_both_range_fields_are_set()
        {
            string containerName = Guid.NewGuid().ToString();
            requestHeaderFields.Add(RequestHeaderFields.Range, "0-5");
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();
         
                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.PartialContent));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        public void Should_return_partial_content_if_the_item_exists_and_only_range_from_is_set()
        {
            string containerName = Guid.NewGuid().ToString();
            requestHeaderFields.Add(RequestHeaderFields.Range, "10-");
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();

                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.PartialContent));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }

        [Test]
        public void Should_return_partial_content_if_the_item_exists_and_only_range_to_is_set()
        {
            string containerName = Guid.NewGuid().ToString();
            requestHeaderFields.Add(RequestHeaderFields.Range, "-8");
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                GetStorageItemResponse getStorageItemResponse = null;
                try
                {
                    testHelper.PutItemInContainer();

                    GetStorageItem getStorageItem = new GetStorageItem(storageUrl, containerName, Constants.StorageItemName, storageToken, requestHeaderFields);

                    getStorageItemResponse = new ResponseFactoryWithContentBody<GetStorageItemResponse>().Create(new CloudFilesRequest(getStorageItem));
                    Assert.That(getStorageItemResponse.Status, Is.EqualTo(HttpStatusCode.PartialContent));
                }
                finally
                {
                    if (getStorageItemResponse != null) getStorageItemResponse.Dispose();
                    testHelper.DeleteItemFromContainer();
                }
            }
        }
    }
}

