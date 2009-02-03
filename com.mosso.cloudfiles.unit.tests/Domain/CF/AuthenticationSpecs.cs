using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.domain.request;

namespace com.mosso.cloudfiles.unit.tests.Domain.CF.AuthenticationSpecs
{
    [TestFixture]
    public class When_authenticating_with_valid_user_credentials
    {
        [Test]
        public void should_return_account()
        {
            UserCredentials user = new UserCredentials("testuser", "testapikey");
            MockCFAuthentication auth = new MockCFAuthentication(user);

            IAccount account = auth.Authenticate();

            Assert.That(account.StorageUrl.ToString(), Is.EqualTo("http://tempuri/"));
            Assert.That(account.StorageToken, Is.EqualTo("test storage token"));
            Assert.That(account.AuthToken, Is.EqualTo("test auth token"));
            Assert.That(account.CDNManagementUrl.ToString(), Is.EqualTo("http://tempuri/"));
        }
    }

    [TestFixture]
    public class When_authenticating_with_invalid_user_credentials
    {
        [Test]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void should_throw_unauthorized_exception()
        {
            UserCredentials user = new UserCredentials("invalidtestuser", "invalidtestapikey");
            MockCFAuthentication auth = new MockCFAuthentication(user);

            auth.Authenticate();

            Assert.Fail("Allowed unauthorized user credentials past Authorize method");
        }
    }

    public class MockCFAuthentication : CF_Authentication
    {
        public MockCFAuthentication(UserCredentials userCredentials) : base(userCredentials){}

        protected override bool AuthenticationPassed(GetAuthenticationResponse getAuthenticationResponse)
        {
            return userCredentials.Username == "testuser" && userCredentials.Api_access_key == "testapikey";
        }

        protected override GetAuthenticationResponse AuthenticateWithCloudFiles(GetAuthentication getAuthentication)
        {

            WebHeaderCollection headers = new WebHeaderCollection();
            headers.Add(Constants.X_STORAGE_TOKEN, "test storage token");
            headers.Add(Constants.X_STORAGE_URL, "http://tempuri");
            headers.Add(Constants.X_AUTH_TOKEN, "test auth token");
            headers.Add(Constants.X_CDN_MANAGEMENT_URL, "http://tempuri");

            GetAuthenticationResponse response = new GetAuthenticationResponse();
            response.Status = HttpStatusCode.NoContent;
            response.Headers = headers;
            return response;
        }
    }
}