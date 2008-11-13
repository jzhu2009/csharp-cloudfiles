using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetStorageItemSpecs
{
    [TestFixture]
    public class GetAuthenticationSpecs
    {
        [Test]
        public void Should_replace_plus_sign_with_percent_20_on_account_name_username_and_password()
        {
            UserCredentials userCredentials = new UserCredentials(new Uri("http://tempuri"), "user name", "pass word", "v 1", "account name");
            GetAuthentication getAuthentication = new GetAuthentication(userCredentials);

            Assert.That(getAuthentication.Uri.AbsoluteUri, Is.EqualTo("http://tempuri//v%201/account%20name/auth"));
            Assert.That(getAuthentication.Headers[Constants.X_AUTH_USER], Is.EqualTo("user%20name"));
            Assert.That(getAuthentication.Headers[Constants.X_AUTH_KEY], Is.EqualTo("pass%20word"));
        }
    }
}