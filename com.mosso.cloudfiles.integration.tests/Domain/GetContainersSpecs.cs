using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.RetrieveContainerRequestSpecs
{
    [TestFixture]
    public class When_requesting_a_list_of_containers_and_containers_are_present : TestBase
    {

        [Test]
        public void Should_return_OK_status()
        {
            
            using(new TestHelper(authToken, storageUrl))
            {
                CloudFilesResponseWithContentBody response = null;
                try
                {
                    GetContainers request = new GetContainers(storageUrl, authToken);
                    request.UserAgent = "NASTTestUserAgent";

                    response = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(request));

                    Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
                    Assert.That(response.ContentBody, Is.Not.Null);
                }
                finally
                {
                    if(response != null)
                        response.Dispose();
                }
            }
            
        }

        [Test]
        public void Should_return_the_list_of_containers()
        {
//            Console.WriteLine("Begin listing containers");

            
            using (new TestHelper(authToken, storageUrl))
            {
                IResponseWithContentBody response = null;
                try
                {
                    GetContainers request = new GetContainers(storageUrl, authToken);
                    response = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(request));
                    Assert.That(response.ContentBody.Count, Is.GreaterThan(0));
//                    foreach (string s in response.ContentBody)
//                        Console.WriteLine(s);
//                    Console.WriteLine("End of listing containers");
                }
                finally
                {
                    if (response != null)
                        response.Dispose();
                }
            }
            
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_storage_url_is_null()
        {
            new GetContainers(null, "a");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Should_throw_an_exception_when_the_auth_token_is_null()
        {
            new GetContainers("a", null);
        }
    }

    [TestFixture]
    public class When_requesting_a_list_of_containers_and_no_containers_are_present : TestBase
    {
        [Test]
        public void Should_return_No_Content_status()
        {
            //Assert.Ignore("Is returning OK instead of NoContent, need to investigate - 2/3/2009");
            GetContainers request = new GetContainers(storageUrl, authToken);
            request.UserAgent = "NASTTestUserAgent";

            var response = new ResponseFactoryWithContentBody<CloudFilesResponseWithContentBody>().Create(new CloudFilesRequest(request));

            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
            if(response.ContentBody != null)
                Assert.That(response.ContentBody.Count, Is.EqualTo(0));
            response.Dispose();
        }

    }
}