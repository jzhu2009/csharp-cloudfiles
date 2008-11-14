using System;
using System.Net;
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
    }
}