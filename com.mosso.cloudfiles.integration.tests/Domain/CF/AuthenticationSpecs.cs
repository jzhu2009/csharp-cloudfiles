using System;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.Domain.CF.AuthenticationSpecs
{
    [TestFixture]
    public class When_authenticating_with_valid_user_credentials
    {
        [Test]
        public void should_return_account()
        {
            var userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            var authentication = new CF_Authentication(userCredentials);

            var account = authentication.Authenticate();

            Assert.That(account.StorageUrl.ToString().Length, Is.EqualTo(83));
            Assert.That(account.StorageUrl.ToString().Contains("https://storage.clouddrive.com/v1/MossoCloudFS_"), Is.True);
            Assert.That(account.StorageToken.Length, Is.EqualTo(36));
            Assert.That(account.StorageToken.IndexOf("-"), Is.EqualTo(8));
            Assert.That(account.AuthToken.Length, Is.EqualTo(36));
            Assert.That(account.AuthToken.IndexOf("-"), Is.EqualTo(8));
            Assert.That(account.CDNManagementUrl.ToString().Length, Is.EqualTo(79));
            Assert.That(account.CDNManagementUrl.ToString().Contains("https://cdn.clouddrive.com/v1/MossoCloudFS_"), Is.True);

        }
    }

    [TestFixture]
    public class When_authenticating_with_invalid_user_credentials
    {
        [Test]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void should_throw_unauthorized_access_exception()
        {
            var userCredentials = new UserCredentials("invalid_user", "invalid_auth_key");
            var authentication = new CF_Authentication(userCredentials);

            authentication.Authenticate();
        }
    }
}