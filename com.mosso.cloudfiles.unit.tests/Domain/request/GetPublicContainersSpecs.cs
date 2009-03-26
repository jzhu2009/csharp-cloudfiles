using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetPublicContainersSpecs
{
    [TestFixture]
    public class when_getting_list_of_public_containers_and_cdn_management_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetPublicContainers(null, "authtoken");
        }
    }

    [TestFixture]
    public class when_getting_list_of_public_containers_and_cdn_management_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetPublicContainers("", "authtoken");
        }
    }

    [TestFixture]
    public class when_getting_list_of_public_containers_and_auth_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetPublicContainers("http://cdnmanagementurl", null);
        }
    }

    [TestFixture]
    public class when_getting_list_of_public_containers_and_auth_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetPublicContainers("http://cdnmanagementurl", "");
        }
    }

    [TestFixture]
    public class when_getting_list_of_public_containers
    {
        private GetPublicContainers getPublicContainers;

        [SetUp]
        public void setup()
        {
            getPublicContainers = new GetPublicContainers("http://cdnmanagementurl", "authtoken");
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getPublicContainers.Uri.ToString(), Is.EqualTo("http://cdnmanagementurl/?enabled_only=true"));
        }

        [Test]
        public void should_have_a_http_get_method()
        {
            Assert.That(getPublicContainers.Method, Is.EqualTo("GET"));
        }

        [Test]
        public void should_have_an_auth_token_in_the_headers()
        {
            Assert.That(getPublicContainers.Headers[utils.Constants.X_AUTH_TOKEN], Is.EqualTo("authtoken"));
        }
    }
}