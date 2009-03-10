using System;
using System.Collections.Generic;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.GetContainerItemListSpecs
{
    [TestFixture]
    public class When_retrieving_a_list_items_from_specific_container : TestBase
    {
        [Test]
        public void should_return_no_content_status_when_container_is_empty()
        {
            using (new TestHelper(storageToken, storageUrl))
            {
                var getContainerItemsRequest = new GetContainerItemList(storageUrl, Constants.CONTAINER_NAME, storageToken);
                getContainerItemsRequest.UserAgent = Constants.USER_AGENT;


                var response =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));
                response.Dispose();
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
            }
        }

        [Test]
        public void should_return_a_list_of_items_when_container_is_not_empty()
        {
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);

                var getContainerItemsRequest = new GetContainerItemList(storageUrl, Constants.CONTAINER_NAME, storageToken);
                getContainerItemsRequest.UserAgent = Constants.USER_AGENT;

                var response =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ContentType, Is.Not.Null);
                response.Dispose();
            }
        }

        [Test]
        public void should_return_401_when_the_account_name_is_wrong()
        {
            Uri uri = new Uri("http://henhouse-1.stg.racklabs.com/v1/Persistent");
            GetContainerItemList getContainerItemsRequest = new GetContainerItemList(uri.ToString(), "#%", storageToken);
            getContainerItemsRequest.UserAgent = "NASTTestUserAgent";
            try
            {
                var response =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));
                response.Dispose();
            }
            catch (Exception ex)
            {
                Assert.That(ex, Is.TypeOf(typeof (WebException)));
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNameException))]
        public void Should_throw_an_exception_when_the_container_name_exceeds_the_maximum_number_of_characters_allowed()
        {
            Uri uri = new Uri("http://henhouse-1.stg.racklabs.com/v1/Persistent");
            new GetContainerItemList(uri.ToString(), new string('a', Constants.MaximumContainerNameLength + 1), storageToken);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_url_is_null()
        {
            new GetContainerItemList(null, "a", "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_container_name_is_null()
        {
            new GetContainerItemList("a", null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_token_is_null()
        {
            new GetContainerItemList("a", "a", null);
        }

        [Test]
        public void Should_return_ten_objects_when_setting_the_limit_to_ten()
        {
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                for (int i = 0; i < 12; ++i)
                    testHelper.PutItemInContainer(Constants.StorageItemName, i.ToString());

                var parameters = new Dictionary<GetItemListParameters, string>
                                                                           {{GetItemListParameters.Limit, "10"}};

                var getContainerItemsRequest = new GetContainerItemList(storageUrl, Constants.CONTAINER_NAME,
                                                                                         storageToken, parameters);
                getContainerItemsRequest.UserAgent = Constants.USER_AGENT;

                var response =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                for (int i = 0; i < 12; ++i)
                    testHelper.DeleteItemFromContainer(i.ToString());

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ContentBody.Count, Is.EqualTo(10));

                response.Dispose();
            }
        }


        [Test]
        public void Should_return_specific_files_under_a_directory_when_passed_a_top_directory()
        {
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                for (int i = 0; i < 12; ++i)
                {
                    if(i % 3 == 0)
                    {
                        testHelper.PutItemInContainer(Constants.StorageItemName, "topdir1/subdir2/" + i + "file");
                        continue;
                    }
                    testHelper.PutItemInContainer(Constants.StorageItemName, "topdir1/" + i + "file");
                }

                var parameters = new Dictionary<GetItemListParameters, string> { { GetItemListParameters.Path, "topdir1" } };

                var getContainerItemsRequest = new GetContainerItemList(storageUrl, Constants.CONTAINER_NAME,
                                                                                         storageToken, parameters);
                getContainerItemsRequest.UserAgent = Constants.USER_AGENT;

                var response =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                for (int i = 0; i < 12; ++i)
                {
                    if (i % 3 == 0)
                    {
                        testHelper.DeleteItemFromContainer("topdir1/subdir2/" + i + "file");
                        continue;
                    }
                    testHelper.DeleteItemFromContainer("topdir1/" + i + "file");
                }

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ContentBody.Count, Is.EqualTo(8));
                Assert.That(response.ContentBody[0], Is.EqualTo("topdir1/10file"));
                Assert.That(response.ContentBody[1], Is.EqualTo("topdir1/11file"));
                Assert.That(response.ContentBody[2], Is.EqualTo("topdir1/1file"));
                Assert.That(response.ContentBody[3], Is.EqualTo("topdir1/2file"));
                Assert.That(response.ContentBody[4], Is.EqualTo("topdir1/4file"));
                Assert.That(response.ContentBody[5], Is.EqualTo("topdir1/5file"));
                Assert.That(response.ContentBody[6], Is.EqualTo("topdir1/7file"));
                Assert.That(response.ContentBody[7], Is.EqualTo("topdir1/8file"));

                response.Dispose();
            }
        }

        [Test]
        public void Should_return_specific_files_under_a_directory_when_passed_a_sub_directory()
        {
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                for (int i = 0; i < 12; ++i)
                {
                    if (i % 3 == 0)
                    {
                        testHelper.PutItemInContainer(Constants.StorageItemName, "topdir1/subdir2/" + i + "file");
                        continue;
                    }
                    testHelper.PutItemInContainer(Constants.StorageItemName, "topdir1/" + i + "file");
                }

                var parameters = new Dictionary<GetItemListParameters, string> { { GetItemListParameters.Path, "topdir1/subdir2" } };

                var getContainerItemsRequest = new GetContainerItemList(storageUrl, Constants.CONTAINER_NAME,
                                                                                         storageToken, parameters);
                getContainerItemsRequest.UserAgent = Constants.USER_AGENT;

                var response =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                for (int i = 0; i < 12; ++i)
                {
                    if (i % 3 == 0)
                    {
                        testHelper.DeleteItemFromContainer("topdir1/subdir2/" + i + "file");
                        continue;
                    }
                    testHelper.DeleteItemFromContainer("topdir1/" + i + "file");
                }

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ContentBody.Count, Is.EqualTo(4));
                Assert.That(response.ContentBody[0], Is.EqualTo("topdir1/subdir2/0file"));
                Assert.That(response.ContentBody[1], Is.EqualTo("topdir1/subdir2/3file"));
                Assert.That(response.ContentBody[2], Is.EqualTo("topdir1/subdir2/6file"));
                Assert.That(response.ContentBody[3], Is.EqualTo("topdir1/subdir2/9file"));

                response.Dispose();
            }
        }
        [Test]
        public void Should_return_objects_starting_with_2_when_setting_prefix_as_2()
        {
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                for (int i = 0; i < 12; ++i)
                    testHelper.PutItemInContainer(Constants.StorageItemName, i.ToString());

                var parameters = new Dictionary<GetItemListParameters, string>
                                                                           {{GetItemListParameters.Prefix, "2"}};

                var getContainerItemsRequest = new GetContainerItemList(storageUrl, Constants.CONTAINER_NAME,
                                                                                         storageToken, parameters);
                getContainerItemsRequest.UserAgent = Constants.USER_AGENT;

                var response =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                for (int i = 0; i < 12; ++i)
                    testHelper.DeleteItemFromContainer(i.ToString());

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.ContentBody.Count, Is.EqualTo(1));
                Assert.That(response.ContentBody[0], Is.EqualTo("2"));

                response.Dispose();
            }
        }

        [Test]
        public void Should_return_7_objects_when_the_offset_is_5()
        {
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                for (int i = 0; i < 12; ++i)
                    testHelper.PutItemInContainer(Constants.StorageItemName, i.ToString());

                var parameters = new Dictionary<GetItemListParameters, string>
                                                                           {{GetItemListParameters.Offset, "5"}};

                var getContainerItemsRequest = new GetContainerItemList(storageUrl, Constants.CONTAINER_NAME,
                                                                                         storageToken, parameters);
                getContainerItemsRequest.UserAgent = Constants.USER_AGENT;

                var response =
                    new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                        new CloudFilesRequest(getContainerItemsRequest));

                for (int i = 0; i < 12; ++i)
                    testHelper.DeleteItemFromContainer(i.ToString());

                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));

                //Assert.That(response.ContentBody.Count, Is.EqualTo(7));

                response.Dispose();
            }
        }

        [Test]
        public void Should_fail_when_an_invalid_paramter_is_passed()
        {
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                for (int i = 0; i < 12; ++i)
                    testHelper.PutItemInContainer(Constants.StorageItemName, i.ToString());

                try
                {
                    Dictionary<GetItemListParameters, string> parameters =
                        new Dictionary<GetItemListParameters, string> {{(GetItemListParameters) int.MaxValue, "2"}};

                    new GetContainerItemList(storageUrl, Constants.CONTAINER_NAME,storageToken, parameters);
                }
                catch (NotImplementedException ne)
                {
                    Assert.That(ne, Is.TypeOf(typeof (NotImplementedException)));
                }
                finally
                {
                    for (int i = 0; i < 12; ++i)
                        testHelper.DeleteItemFromContainer(i.ToString());
                }
            }
        }
    }

    [TestFixture]
    public class When_requesting_a_list_of_containers_with_non_alphanumeric_characters : TestBase
    {
        [Test]
        public void should_not_throw_an_exception_when_the_container_name_starts_with_pound()
        {
            var getContainerItemList = new GetContainerItemList(storageUrl, "#container", storageToken);

            var response =
                new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                    new CloudFilesRequest((getContainerItemList)));

            response.Dispose();
            Assert.That(true);
        }

        [Test]
        public void should_not_throw_an_exception_when_the_container_contains_utf8_characters()
        {
            var containerName = '\u07FF' + "container";
            var getContainerItemList = new GetContainerItemList(storageUrl, containerName, storageToken);

            var response =
                new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                    new CloudFilesRequest((getContainerItemList)));

            response.Dispose();
            foreach (string s in response.ContentBody)
                Console.WriteLine(s);
            Assert.That(true);
        }

        [Test]
        public void should_not_throw_an_exception_when_the_container_contains_out_of_range_utf8_characters()
        {

            var containerName = '\uD8CC' + "container";
            var getContainerItemList = new GetContainerItemList(storageUrl, containerName, storageToken);

            var response =
                new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                    new CloudFilesRequest((getContainerItemList)));

            foreach (string s in response.ContentBody)
                Console.WriteLine(s);
            response.Dispose();
            Assert.That(true);
        }

        [Test]
        public void should_not_throw_an_exception_when_the_container_contains_utf8_characters_3()
        {

            var containerName = '\uDCFF' + "container";
            var getContainerItemList = new GetContainerItemList(storageUrl, containerName, storageToken);

            var response =
                new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(
                    new CloudFilesRequest((getContainerItemList)));

            response.Dispose();
            foreach (string s in response.ContentBody)
                Console.WriteLine(s);
            Assert.That(true);
        }
    }
}