using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.domain
{
    public class TestBase
    {
        protected string storageUrl;
        protected string storageToken;

        [SetUp]
        public void SetUpBase()
        {
            Uri uri = new Uri(Constants.MOSSO_AUTH_URL);
            GetAuthentication request =
                new GetAuthentication(new UserCredentials(Constants.MOSSO_ACCOUNT, Constants.MOSSO_PASSWORD));

            IResponse response = new ResponseFactory<GetAuthenticationResponse>().Create(new CloudFilesRequest(request));

            storageUrl = response.Headers[Constants.XStorageUrl];
            storageToken = response.Headers[Constants.XStorageToken];
            Assert.That(storageToken.Length, Is.EqualTo(36));
            SetUp();
        }

        protected virtual void SetUp()
        {
        }
    }
}