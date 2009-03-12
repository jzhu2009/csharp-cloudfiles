using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetAccountInformationSpecs
{
    [TestFixture]
    public class when_getting_account_information_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformation(null, "authtoken");
        }
    }

    [TestFixture]
    public class when_getting_account_information_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformation("", "authtoken");
        }
    }

    [TestFixture]
    public class when_getting_account_information_and_auth_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformation("http://storageurl", null);
        }
    }

    [TestFixture]
    public class when_getting_account_information_and_auth_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformation("http://storageurl", "");
        }
    }

    [TestFixture]
    public class when_getting_account_information
    {
        private GetAccountInformation getAccountInformation;

        [SetUp]
        public void setup()
        {
            getAccountInformation = new GetAccountInformation("http://storageurl", "authtoken");
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getAccountInformation.Uri.ToString(), Is.EqualTo("http://storageurl/"));
        }

        [Test]
        public void should_have_a_http_head_method()
        {
            Assert.That(getAccountInformation.Method, Is.EqualTo("HEAD"));
        }

        [Test]
        public void should_have_a_auth_token_in_the_headers()
        {
            Assert.That(getAccountInformation.Headers[cloudfiles.Constants.X_STORAGE_TOKEN], Is.EqualTo("authtoken"));
        }
    }
}