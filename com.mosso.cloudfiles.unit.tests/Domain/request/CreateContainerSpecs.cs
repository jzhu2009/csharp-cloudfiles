using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.CreateContainerSpecs
{
    [TestFixture]
    public class when_creating_a_container_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new CreateContainer(null, "storagetoken", "containername");        
        }
    }

    [TestFixture]
    public class when_creating_a_container_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new CreateContainer("", "storagetoken", "containername");
        }
    }

    [TestFixture]
    public class when_creating_a_container_and_storage_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new CreateContainer("http://storageurl", null, "containername");
        }
    }

    [TestFixture]
    public class when_creating_a_container_and_storage_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new CreateContainer("http://storageurl", "", "containername");
        }
    }

    [TestFixture]
    public class when_creating_a_container_and_container_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new CreateContainer("http://storageurl", "storagetoken", null);
        }
    }

    [TestFixture]
    public class when_creating_a_container_and_container_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new CreateContainer("http://storageUrl", "storagetoken", "");
        }
    }

    [TestFixture]
    public class when_creating_a_container
    {
        private CreateContainer createContainer;

        [SetUp]
        public void setup()
        {
            createContainer = new CreateContainer("http://storageurl", "storagetoken", "containername");    
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(createContainer.Uri.ToString(), Is.EqualTo("http://storageurl/containername"));
        }

        [Test]
        public void should_have_a_http_put_method()
        {
            Assert.That(createContainer.Method, Is.EqualTo("PUT"));
        }

        [Test]
        public void should_have_a_storage_token_in_the_headers()
        {
            Assert.That(createContainer.Headers[Constants.X_STORAGE_TOKEN], Is.EqualTo("storagetoken"));
        }
    }
}