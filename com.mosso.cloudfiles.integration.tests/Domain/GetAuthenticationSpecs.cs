using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain.AuthenticationRequestSpecs
{
    [TestFixture]
    public class When_requesting_client_authentication
    {
        private Uri uri;
        private GetAuthentication request;
        private const string STORAGE_TOKEN = "5d8f3dca-7eb9-4453-aa79-2eea1b980353";

        [SetUp]
        public void Setup()
        {
            uri = new Uri(Constants.MOSSO_AUTH_URL);
            request =
                new GetAuthentication(new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY));
        }

        [Test]
        public void Should_get_204_response_when_authenticated_correctly()
        {
            IResponse response = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request, null));
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void Should_get_storage_url_when_authenticated_correctly()
        {
            GetAuthenticationResponse response = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request, null));
            Assert.That(response.Headers[Constants.XStorageUrl].Length, Is.GreaterThan(0));
            Uri storageUri = new Uri(response.StorageUrl);
            Assert.That(storageUri.AbsolutePath, Is.EqualTo("/v1/MossoCloudFS_" + STORAGE_TOKEN));
        }

        [Test]
        public void Should_get_storage_token_when_authenticated_correctly()
        {
            //IResponse response = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request, null));
            GetAuthenticationResponse response = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request));
            string storageToken = response.StorageToken;
            Assert.That(storageToken.Length, Is.GreaterThan(0));
            Assert.That(storageToken.Length, Is.EqualTo(STORAGE_TOKEN.Length));
        }

        [Test]
        public void Should_get_content_when_authenticated_correctly()
        {
            IResponse response = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request, null));
            Assert.That(response.Headers["Content-Length"], Is.EqualTo("0"));
        }

        [Test]
        public void Should_return_a_cdn_management_url_header()
        {
            IResponse response =
                new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request, null));
            Assert.That(response.Headers[Constants.XCdnManagementUrl], Is.Not.Null);
        }

        [Test]
        public void Should_return_an_authorization_token_header()
        {
            IResponse response =
                new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request, null));
            Assert.That(response.Headers[Constants.XAuthToken], Is.Not.Null);
        }

        [Test]
        public void Should_get_401_response_when_authenticated_incorrectly()
        {
            request =
                new GetAuthentication(new UserCredentials("EPIC", "FAIL"));

            try
            {
                IResponse response = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request, null));
                Assert.Fail("Should throw WebException");
            }
            catch (WebException we)
            {
                //It's a protocol error that is usually a result of a 401 (Unauthorized)
                //Still trying to figure way to get specific httpstatuscode
                Assert.That(((HttpWebResponse) we.Response).StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            }
        }

    }
}