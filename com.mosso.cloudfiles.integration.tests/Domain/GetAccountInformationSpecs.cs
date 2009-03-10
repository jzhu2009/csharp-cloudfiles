using System;
using System.Net;
using System.Xml;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.GetAccountSpecs
{
    [TestFixture]
    public class When_querying_for_account : TestBase
    {
        [Test]
        public void should_return_number_of_containers_and_bytes_used()
        {
            
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);
                GetAccountInformation getAccountInformation = new GetAccountInformation(storageUrl, storageToken);
                var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getAccountInformation));
                Assert.That(response.Headers[Constants.XAccountBytesUsed], Is.Not.Null);
                Assert.That(response.Headers[Constants.XAccountContainerCount], Is.Not.Null);
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);
            }
        }
    }

    [TestFixture]
    public class When_querying_for_account_and_the_account_does_not_exist : TestBase
    {
        [Test]
        public void should_return_401_unauthorized()
        {
            bool exceptionThrown = false;

            try
            {
                GetAccountInformation getAccountInformation = new GetAccountInformation(storageUrl.Replace("_", "FAIL"), storageToken);
                new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getAccountInformation));
            }
            catch (WebException we)
            {
                exceptionThrown = true;
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response.StatusCode != HttpStatusCode.Unauthorized) Assert.Fail("Should be a 401 error");
            }

            Assert.That(exceptionThrown, Is.True);
        }
    }

    [TestFixture]
    public class When_querying_for_account_and_account_has_no_containers : TestBase
    {
        //THIS TEST SHOULD NOT HAVE TO GO DELETE ANY EXISTING CONTAINERS & OBJECTS.  IF IT FAILS, IT
        //IS MOST LIKELY THAT ANOTHER TEST DID NOT CLEAN UP CORRECTLY.  FIX THAT OTHER TEST.
        [Test]
        public void should_return_204_no_content_when_the_account_has_no_containers()
        {
            GetAccountInformation getAccountInformation = new GetAccountInformation(storageUrl, storageToken);
            var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(getAccountInformation));
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }

    [TestFixture]
    public class When_querying_for_account_in_json_format_and_container_exists : TestBase
    {
        [Test]
        public void should_return_account_information_in_json_format_including_name_count_and_bytes()
        {
            
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemName);

                    var getAccountInformationJson = new GetAccountInformationSerialized(storageUrl, storageToken, Format.JSON);
                    var getAccountInformationJsonResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getAccountInformationJson));

                    if(getAccountInformationJsonResponse.ContentBody.Count == 0)
                        Assert.Fail("No content body returned in response");

//                    foreach (string s in getAccountInformationJsonResponse.ContentBody)
//                    {
//                        Console.WriteLine(s);
//                    }

                    
                    var expectedSubString = "{\"name\": \"" + Constants.CONTAINER_NAME + "\", \"count\": 1, \"bytes\": 34}";
                    var contentBody = getAccountInformationJsonResponse.ContentBody;
                    getAccountInformationJsonResponse.Dispose();
                    foreach (var s in contentBody)
                    {
                        if (s.IndexOf(expectedSubString) > -1) return;  
                    }

                    Assert.Fail("Expected value: " + expectedSubString + " not found");
               
                }
                finally
                {

                    testHelper.DeleteItemFromContainer();
                }
            }    
        }
    }


    [TestFixture]
    public class When_querying_for_account_in_json_format_and_no_containers_exist : TestBase
    {
        [Test]
        public void should_return_empty_brackets_and_ok_status_200()
        {
            var getAccountInformationJson = new GetAccountInformationSerialized(storageUrl, storageToken, Format.JSON);
            var getAccountInformationJsonResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(getAccountInformationJson));
            Assert.That(getAccountInformationJsonResponse.Status, Is.EqualTo(HttpStatusCode.OK));
            var contentBody = String.Join("",getAccountInformationJsonResponse.ContentBody.ToArray());
            getAccountInformationJsonResponse.Dispose();

            Assert.That(contentBody, Is.EqualTo("[]"));        
        }
    }

    [TestFixture]
    public class When_querying_for_account_in_xml_format_and_container_exists : TestBase
    {
        [Test]
        public void should_return_account_information_in_xml_format_including_name_count_and_size()
        {
            
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl))
            {
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemName);

                    var accountInformationXml = new GetAccountInformationSerialized(storageUrl, storageToken, Format.XML);
                    var getAccountInformationXmlResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(accountInformationXml));

                    if (getAccountInformationXmlResponse.ContentBody.Count == 0)
                        Assert.Fail("No content body returned in response");

                    var contentBody = "";
                    foreach (var s in getAccountInformationXmlResponse.ContentBody)
                    {
                        contentBody += s;
                    }

                    getAccountInformationXmlResponse.Dispose();
                    var xmlDocument = new XmlDocument();
                    try
                    {
                        xmlDocument.LoadXml(contentBody);
                    }
                    catch(XmlException e)
                    {
                        Console.WriteLine(e.Message);
                    }

//                    Console.WriteLine(xmlDocument.InnerXml);
                    var expectedSubString = "<container><name>"+ Constants.CONTAINER_NAME +"</name><count>1</count><bytes>34</bytes></container>";
                    Assert.That(contentBody.IndexOf(expectedSubString) > 0, Is.True);
                }
                finally
                {
                    testHelper.DeleteItemFromContainer();
                }
            }
        } 
    }

    [TestFixture]
    public class When_querying_for_account_in_xml_format_and_no_container_exists : TestBase
    {
        [Test]
        public void should_return_account_name_and_ok_status_200()
        {
            var accountInformationXml = new GetAccountInformationSerialized(storageUrl, storageToken, Format.XML);
            var getAccountInformationXmlResponse = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(accountInformationXml));
            Assert.That(getAccountInformationXmlResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            var contentBody = "";
            foreach (var s in getAccountInformationXmlResponse.ContentBody)
            {
                contentBody += s;
            }

            getAccountInformationXmlResponse.Dispose();
            const string expectedSubString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"></account>";
            Assert.That(contentBody, Is.EqualTo(expectedSubString));
        }
    }
}