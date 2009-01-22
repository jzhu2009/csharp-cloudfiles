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
            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                testHelper.PutItemInContainer(Constants.StorageItemName, Constants.StorageItemName);
                GetAccountInformation getAccountInformation = new GetAccountInformation(storageUrl, storageToken);
                GetAccountInformationResponse response = new ResponseFactory<GetAccountInformationResponse>().Create(new CloudFilesRequest(getAccountInformation));
                Assert.That(response.Headers[Constants.XAccountBytesUsed], Is.Not.Null);
                Assert.That(response.Headers[Constants.XAccountContainerCount], Is.Not.Null);
                testHelper.DeleteItemFromContainer(Constants.StorageItemName);
            }
        }

        [Test]
        public void should_return_401_when_the_account_does_not_exist()
        {
            bool exceptionThrown = false;

            try
            {
                GetAccountInformation getAccountInformation = new GetAccountInformation(storageUrl.Replace("_", "FAIL"), storageToken);
                GetAccountInformationResponse response = new ResponseFactory<GetAccountInformationResponse>().Create(new CloudFilesRequest(getAccountInformation));
            }
            catch (WebException we)
            {
                exceptionThrown = true;
                HttpWebResponse response = (HttpWebResponse) we.Response;
                if (response.StatusCode != HttpStatusCode.Unauthorized) Assert.Fail("Should be a 401 error");
            }

            Assert.That(exceptionThrown, Is.True);
        }

        [Test]
        public void should_return_204_no_content_when_the_account_has_no_containers()
        {
            //Delete all containers, items in the containers first
            //Run GetAccountInformation request
        }
    }

    [TestFixture]
    public class When_querying_for_account_in_json_format : TestBase
    {
        [Test]
        public void should_return_account_information_in_json_format_including_name_count_and_size()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemName);

                    GetAccountInformationSerialized getAccountInformationJson = new GetAccountInformationSerialized(storageUrl, storageToken, Format.JSON);
                    GetAccountInformationSerializedResponse getAccountInformationJsonResponse = new ResponseFactoryWithContentBody<GetAccountInformationSerializedResponse>().Create(new CloudFilesRequest(getAccountInformationJson));

                    if(getAccountInformationJsonResponse.ContentBody.Count == 0)
                        Assert.Fail("No content body returned in response");

//                    foreach (string s in getAccountInformationJsonResponse.ContentBody)
//                    {
//                        Console.WriteLine(s);
//                    }

                    
                    string expectedSubString = "{\"name\":\"" + containerName + "\",\"count\":1,\"size\":34}";
                    System.Collections.Generic.List<string> contentBody = getAccountInformationJsonResponse.ContentBody;
                    getAccountInformationJsonResponse.Dispose();
                    foreach (string s in contentBody)
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
    public class When_querying_for_account_in_xml_format : TestBase
    {
        [Test]
        public void should_return_account_information_in_xml_format_including_name_count_and_size()
        {
            string containerName = Guid.NewGuid().ToString();
            using (TestHelper testHelper = new TestHelper(storageToken, storageUrl, containerName))
            {
                try
                {
                    testHelper.PutItemInContainer(Constants.StorageItemName);

                    GetAccountInformationSerialized accountInformationXml = new GetAccountInformationSerialized(storageUrl, storageToken, Format.XML);
                    GetAccountInformationSerializedResponse getAccountInformationXmlResponse = new ResponseFactoryWithContentBody<GetAccountInformationSerializedResponse>().Create(new CloudFilesRequest(accountInformationXml));

                    if (getAccountInformationXmlResponse.ContentBody.Count == 0)
                        Assert.Fail("No content body returned in response");

                    string contentBody = "";
                    foreach (string s in getAccountInformationXmlResponse.ContentBody)
                    {
                        contentBody += s;
                    }

                    getAccountInformationXmlResponse.Dispose();
                    XmlDocument xmlDocument = new XmlDocument();
                    try
                    {
                        xmlDocument.LoadXml(contentBody);
                    }
                    catch(XmlException e)
                    {
                        Console.WriteLine(e.Message);
                    }

//                    Console.WriteLine(xmlDocument.InnerXml);
                    string expectedSubString = "<container><name>"+ containerName +"</name><count>1</count><size>34</size></container>";
                    Assert.That(contentBody.IndexOf(expectedSubString) > 0, Is.True);
                }
                finally
                {
                    testHelper.DeleteItemFromContainer();
                }
            }
        } 
    }
}