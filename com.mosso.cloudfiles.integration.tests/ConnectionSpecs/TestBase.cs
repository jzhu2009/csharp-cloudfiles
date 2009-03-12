using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests
{
    public class TestBase
    {
        protected string storageUrl;
        protected string authToken;
        protected IConnection connection;

        [SetUp]
        public void SetUpBase()
        {
            var request = new GetAuthentication(new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY));
            var response = new ResponseFactory<CloudFilesResponse>().Create(new CloudFilesRequest(request));

            storageUrl = response.Headers[Constants.XStorageUrl];
            authToken = response.Headers[Constants.XAuthToken];
            Assert.That(authToken.Length, Is.EqualTo(36));
            connection = new Connection(new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY));
            SetUp();
        }

        protected virtual void SetUp()
        {
        }
    }
}