using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetAccountInformationSerializedSpecs
{
    [TestFixture]
    public class when_getting_account_information_in_json_format_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformationSerialized(null, "authtoken", Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_json_format_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformationSerialized("", "authtoken", Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_json_format_and_auth_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformationSerialized("http://storageurl", null, Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_json_format_and_auth_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformationSerialized("http://storageurl", "", Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_json_format
    {
        private GetAccountInformationSerialized getAccountInformationSerialized;

        [SetUp]
        public void setup()
        {
            getAccountInformationSerialized = new GetAccountInformationSerialized("http://storageurl", "authtoken", Format.JSON);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getAccountInformationSerialized.Uri.ToString(), Is.EqualTo("http://storageurl/?format=json"));
        }

        [Test]
        public void should_have_a_http_get_method()
        {
            Assert.That(getAccountInformationSerialized.Method, Is.EqualTo("GET"));
        }

        [Test]
        public void should_have_a_auth_token_in_the_headers()
        {
            Assert.That(getAccountInformationSerialized.Headers[cloudfiles.Constants.X_AUTH_TOKEN], Is.EqualTo("authtoken"));
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_xml_format_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformationSerialized(null, "authtoken", Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_xml_format_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformationSerialized("", "authtoken", Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_xml_format_and_auth_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformationSerialized("http://storageurl", null, Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_xml_format_and_auth_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetAccountInformationSerialized("http://storageurl", "", Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_account_information_in_xml_format
    {
        private GetAccountInformationSerialized getAccountInformationSerialized;

        [SetUp]
        public void setup()
        {
            getAccountInformationSerialized = new GetAccountInformationSerialized("http://storageurl", "authtoken", Format.XML);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getAccountInformationSerialized.Uri.ToString(), Is.EqualTo("http://storageurl/?format=xml"));
        }

        [Test]
        public void should_have_a_http_get_method()
        {
            Assert.That(getAccountInformationSerialized.Method, Is.EqualTo("GET"));
        }

        [Test]
        public void should_have_a_auth_token_in_the_headers()
        {
            Assert.That(getAccountInformationSerialized.Headers[cloudfiles.Constants.X_AUTH_TOKEN], Is.EqualTo("authtoken"));
        }
    }
}