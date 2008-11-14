using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain
{
    public class TestBase
    {
        protected string storageUrl;
        protected string storageToken;
        protected IConnection connection;

        [SetUp]
        public void SetUpBase()
        {
            GetAuthentication request =
                new GetAuthentication(new UserCredentials(Constants.MOSSO_ACCOUNT, Constants.MOSSO_PASSWORD));

            IResponse response = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request));

            storageUrl = response.Headers[Constants.XStorageUrl];
            storageToken = response.Headers[Constants.XStorageToken];
            Assert.That(storageToken.Length, Is.EqualTo(36));
            connection = new Connection(new UserCredentials(Constants.MOSSO_ACCOUNT, Constants.MOSSO_PASSWORD));
            SetUp();
        }

        protected virtual void SetUp()
        {
        }
    }
}