using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.domain.response;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests
{
    public class TestBase
    {
        protected string storageUrl;
        protected string authToken;

        [SetUp]
        public void SetUpBase()
        {
            Uri uri = new Uri(Constants.AUTH_URL);

            GetAuthentication request =
                new GetAuthentication(
                    new UserCredentials(
                        uri,
                        Constants.CREDENTIALS_USER_NAME,
                        Constants.CREDENTIALS_PASSWORD,
                        Constants.CREDENTIALS_CLOUD_VERSION,
                        Constants.CREDENTIALS_ACCOUNT_NAME));

            IResponse response =
                new ResponseFactory<CloudFilesResponse>().Create(
                    new CloudFilesRequest(request));

            storageUrl = response.Headers[utils.Constants.X_STORAGE_URL];
            authToken = response.Headers[utils.Constants.X_AUTH_TOKEN];
            Assert.That(authToken.Length, Is.EqualTo(32));
            SetUp();
        }

        protected virtual void SetUp()
        {
        }
    }
}