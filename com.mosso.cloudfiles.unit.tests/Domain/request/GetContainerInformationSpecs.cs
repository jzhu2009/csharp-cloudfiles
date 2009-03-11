using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetContainerInformationSpecs
{
    [TestFixture]
    public class when_getting_container_information_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformation(null, "storagetoken", "containername");
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformation("", "storagetoken", "containername");
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformation("http://storageurl", null, "containername");
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformation("http://storageurl", "", "containername");
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_container_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformation("http://storageurl", "storagetoken", null);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_container_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformation("http://storageurl", "storagetoken", "");
        }
    }

    [TestFixture]
    public class when_getting_container_information
    {
        private GetContainerInformation getContainerInformation;

        [SetUp]
        public void setup()
        {
            getContainerInformation = new GetContainerInformation("http://storageurl", "storagetoken", "containername");
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(getContainerInformation.Uri.ToString(), Is.EqualTo("http://storageurl/containername"));
        }

        [Test]
        public void should_have_a_http_head_method()
        {
            Assert.That(getContainerInformation.Method, Is.EqualTo("HEAD"));
        }

        [Test]
        public void should_have_a_storage_token_in_the_headers()
        {
            Assert.That(getContainerInformation.Headers[cloudfiles.Constants.X_STORAGE_TOKEN], Is.EqualTo("storagetoken"));
        }
    }
}