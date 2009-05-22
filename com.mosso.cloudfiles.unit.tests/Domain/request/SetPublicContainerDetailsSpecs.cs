using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.SetPublicContainerDetailsSpecs
{
    [TestFixture]
    public class when_setting_public_container_details_and_cdn_management_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetPublicContainerDetails(null, "authtoken", "containername", true, -1);
        }
    }

    [TestFixture]
    public class when_setting_public_container_details_and_cdn_management_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetPublicContainerDetails("", "authtoken", "containername", true, -1);
        }
    }

    [TestFixture]
    public class when_setting_public_container_details_and_auth_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetPublicContainerDetails("http://cdnmanagementurl", null, "containername", true, -1);
        }
    }

    [TestFixture]
    public class when_setting_public_container_details_and_auth_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetPublicContainerDetails("http://cdnmanagementurl", "", "containername", true, -1);
        }
    }

    [TestFixture]
    public class when_setting_public_container_details_and_container_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetPublicContainerDetails("http://cdnmanagementurl", "authtoken", null, true, -1);
        }
    }

    [TestFixture]
    public class when_setting_public_container_details_and_container_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetPublicContainerDetails("http://cdnmanagementurl", "authtoken", "", true, -1);
        }
    }

    [TestFixture]
    public class when_setting_public_container_details_and_container_ttl_is_less_than_zero
    {
        [Test]
        public void should_throw_argument_null_exception()
        {
            var setPublicContainerDetails = new SetPublicContainerDetails("http://cdnmanagementurl", "authtoken", "containername", true, -1);
            Assert.That(setPublicContainerDetails.Headers[utils.Constants.X_CDN_TTL], Is.Null);
        }
    }

    [TestFixture]
    public class when_setting_public_container_details
    {
        private SetPublicContainerDetails setPublicContainerDetails;

        [SetUp]
        public void setup()
        {
            setPublicContainerDetails = new SetPublicContainerDetails("http://cdnmanagementurl", "authtoken", "containername", true, 12345);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(setPublicContainerDetails.Uri.ToString(), Is.EqualTo("http://cdnmanagementurl/containername"));
        }

        [Test]
        public void should_have_a_http_post_method()
        {
            Assert.That(setPublicContainerDetails.Method, Is.EqualTo("POST"));
        }

        [Test]
        public void should_have_an_auth_token_in_the_headers()
        {
            Assert.That(setPublicContainerDetails.Headers[utils.Constants.X_AUTH_TOKEN], Is.EqualTo("authtoken"));
        }

        [Test]
        public void should_have_cdn_enabled_in_the_headers()
        {
            Assert.That(setPublicContainerDetails.Headers[utils.Constants.X_CDN_ENABLED], Is.EqualTo("True"));
        }

        [Test]
        public void should_have_time_to_live_aka_ttl_in_the_headers()
        {
            Assert.That(setPublicContainerDetails.Headers[utils.Constants.X_CDN_TTL], Is.EqualTo("12345"));
        }
    }
}