using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.GetContainerInformationSpecs
{
    [TestFixture]
    public class When_retrieving_the_statistics_on_a_container : TestBase
    {
        [Test]
        public void Should_return_no_content_when_the_container_exists()
        {
            
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);
                GetContainerInformation getContainerInformation = new GetContainerInformation(storageUrl, Constants.CONTAINER_NAME, storageToken);

                var informationResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getContainerInformation));
                Assert.That(informationResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));
                Assert.That(informationResponse.Headers[Constants.XContainerObjectCount], Is.EqualTo("1"));
                Assert.That(informationResponse.Headers[Constants.XContainerBytesUsed], (Is.Not.Null));
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);
            }
        }

        [Test]
        //TODO:
        public void Should_return_no_content_when_the_container_exists_and_the_name_contains_spaces()
        {
            const string containerName = "I am making a funky container";
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);
                var getContainerInformation = new GetContainerInformation(storageUrl, containerName, storageToken);

                var informationResponse = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getContainerInformation));
                Assert.That(informationResponse.Status, Is.EqualTo(HttpStatusCode.NoContent));
                Assert.That(informationResponse.Headers[Constants.XContainerObjectCount], Is.EqualTo("1"));
                Assert.That(informationResponse.Headers[Constants.XContainerBytesUsed], (Is.Not.Null));
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);
            }
        }

        [Test]
        [ExpectedException(typeof (WebException))]
        public void Should_return_not_found_when_the_container_does_not_exist()
        {
            var getContainerInformation = new GetContainerInformation(storageUrl, "Idonthasacontainer", storageToken);

            new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getContainerInformation));
            Assert.Fail("Expecting a 404 error when trying to retrieve data about a non-existent container");
        }

        [Test]
        [ExpectedException(typeof (ContainerNameException))]
        public void Should_throw_an_exception_when_the_container_name_exceeds_the_maximum_allowed_length()
        {
            var getContainerInformation = new GetContainerInformation(storageUrl, new string('a', Constants.MaximumContainerNameLength + 1), storageToken);

            new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getContainerInformation));
            Assert.Fail("Expecting a ContainerNameException");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_storage_url_is_null()
        {
            new GetContainerInformation(null, "a", "whatever");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_container_name_is_null()
        {
            new GetContainerInformation("a", null, "whatever");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_if_the_storage_token_is_null()
        {
            new GetContainerInformation("a", "a", null);
        }
    }

    [TestFixture]
    public class When_getting_serialized_container_information_for_a_container_in_json_format_and_objects_exist : TestBase
    {
        [Test]
        public void Should_get_serialized_json_format()
        {

            
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                testHelper.PutItemInContainer(Constants.StorageItemNameJpg);
                var getContainerInformation = new GetContainerInformationSerialized(storageUrl, storageToken, Constants.CONTAINER_NAME, Format.JSON);

                var jsonResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getContainerInformation));
                Assert.That(jsonResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                var jsonReturnValue = String.Join("", jsonResponse.ContentBody.ToArray());
                jsonResponse.Dispose();
                var expectedSubString = "[{\"name\": \"" + Constants.StorageItemNameJpg + "\", \"hash\": \"b44a59383b3123a747d139bd0e71d2df\", \"bytes\": 105542, \"content_type\": \"image\\u002fjpeg\", \"last_modified\": \"" + String.Format("{0:yyyy-MM}", DateTime.Now);

                Assert.That(jsonReturnValue.IndexOf(expectedSubString) == 0, Is.True);
                testHelper.DeleteItemFromContainer(Constants.StorageItemNameJpg);
            }
        }
    }

    [TestFixture]
    public class When_getting_serialized_account_information_for_an_account_in_xml_format_and_container_exists : TestBase
    {
        [Test]
        public void Should_get_serialized_xml_format()
        {
            
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                testHelper.PutItemInContainer(Constants.StorageItemNameJpg);
                var getContainerInformation = new GetContainerInformationSerialized(storageUrl, storageToken, Constants.CONTAINER_NAME, Format.XML);

                var xmlResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getContainerInformation));
                Assert.That(xmlResponse.Status, Is.EqualTo(HttpStatusCode.OK));
                var xmlReturnValue = String.Join("", xmlResponse.ContentBody.ToArray());
                xmlResponse.Dispose();
                var expectedSubString = "<container name=\"" + Constants.CONTAINER_NAME + "\"><object><name>" + Constants.StorageItemNameJpg + "</name><hash>b44a59383b3123a747d139bd0e71d2df</hash><bytes>105542</bytes><content_type>image/jpeg</content_type><last_modified>" + String.Format("{0:yyyy-MM}", DateTime.Now);

                Assert.That(xmlReturnValue.IndexOf(expectedSubString) > -1, Is.True);
                testHelper.DeleteItemFromContainer(Constants.StorageItemNameJpg);
            }
        }
    }
}