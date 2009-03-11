using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.MarkContainerAsPublicSpecs
{
    [TestFixture]
    public class when_marking_a_container_as_public_and_cdn_management_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new MarkContainerAsPublic(null, "authtoken", "containername");
        }
    }

    [TestFixture]
    public class when_marking_a_container_as_public_and_cdn_management_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new MarkContainerAsPublic("", "authtoken", "containername");
        }
    }

    [TestFixture]
    public class when_marking_a_container_as_public_and_auth_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new MarkContainerAsPublic("http://cdnmanagementurl", null, "containername");
        }
    }

    [TestFixture]
    public class when_marking_a_container_as_public_and_auth_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new MarkContainerAsPublic("http://cdnmanagementurl", "", "containername");
        }
    }

    [TestFixture]
    public class when_marking_a_container_as_public_and_container_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new MarkContainerAsPublic("http://cdnmanagementurl", "authtoken", null);
        }
    }

    [TestFixture]
    public class when_marking_a_container_as_public_and_container_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new MarkContainerAsPublic("http://cdnmanagementurl", "authtoken", "");
        }
    }

    [TestFixture]
    public class when_marking_a_container_as_public
    {
        private MarkContainerAsPublic markContainerAsPublic;

        [SetUp]
        public void setup()
        {
            markContainerAsPublic = new MarkContainerAsPublic("http://cdnmanagementurl", "authtoken", "containername");
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(markContainerAsPublic.Uri.ToString(), Is.EqualTo("http://cdnmanagementurl/containername"));
        }

        [Test]
        public void should_have_a_http_put_method()
        {
            Assert.That(markContainerAsPublic.Method, Is.EqualTo("PUT"));
        }

        [Test]
        public void should_have_an_auth_token_in_the_headers()
        {
            Assert.That(markContainerAsPublic.Headers[cloudfiles.Constants.X_AUTH_TOKEN], Is.EqualTo("authtoken"));
        }
    }
    
}