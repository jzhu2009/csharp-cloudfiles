using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.DeleteContainerSpecs
{
    [TestFixture]
    public class when_deleting_a_container_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteContainer(null, "authtoken", "containername");
        }
    }

    [TestFixture]
    public class when_deleting_a_container_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteContainer("", "authtoken", "containername");
        }
    }

    [TestFixture]
    public class when_deleting_a_container_and_auth_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteContainer("http://storageurl", null, "containername");
        }
    }

    [TestFixture]
    public class when_deleting_a_container_and_auth_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteContainer("http://storageurl", "", "containername");
        }
    }

    [TestFixture]
    public class when_deleting_a_container_and_container_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteContainer("http://storageurl", "authtoken", null);
        }
    }

    [TestFixture]
    public class when_deleting_a_container_and_container_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteContainer("http://storageUrl", "authtoken", "");
        }
    }

    [TestFixture]
    public class when_deleting_a_container
    {
        private DeleteContainer deleteContainer;

        [SetUp]
        public void setup()
        {
            deleteContainer = new DeleteContainer("http://storageurl", "authtoken", "containername");
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(deleteContainer.Uri.ToString(), Is.EqualTo("http://storageurl/containername"));
        }

        [Test]
        public void should_have_a_http_delete_method()
        {
            Assert.That(deleteContainer.Method, Is.EqualTo("DELETE"));
        }

        [Test]
        public void should_have_a_auth_token_in_the_headers()
        {
            Assert.That(deleteContainer.Headers[utils.Constants.X_AUTH_TOKEN], Is.EqualTo("authtoken"));
        }
    }
}