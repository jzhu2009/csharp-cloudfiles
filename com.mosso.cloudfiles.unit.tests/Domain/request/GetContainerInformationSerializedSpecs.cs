using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.GetContainerInformationSerializedSerializedSpecs
{
    [TestFixture]
    public class when_getting_container_information_and_storage_url_is_null_and_format_is_json
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized(null, "storagetoken", "containername", Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_url_is_emptry_string_and_format_is_json
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("", "storagetoken", "containername", Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_token_is_null_and_format_is_json
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("http://storageurl", null, "containername", Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_token_is_emptry_string_and_format_is_json
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("http://storageurl", "", "containername", Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_container_name_is_null_and_format_is_json
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("http://storageurl", "storagetoken", null, Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_container_name_is_emptry_string_and_format_is_json
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("http://storageurl", "storagetoken", "", Format.JSON);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_format_is_json
    {
        private GetContainerInformationSerialized GetContainerInformationSerialized;

        [SetUp]
        public void setup()
        {
            GetContainerInformationSerialized = new GetContainerInformationSerialized("http://storageurl", "storagetoken", "containername", Format.JSON);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(GetContainerInformationSerialized.Uri.ToString(), Is.EqualTo("http://storageurl/containername?format=json"));
        }

        [Test]
        public void should_have_a_http_get_method()
        {
            Assert.That(GetContainerInformationSerialized.Method, Is.EqualTo("GET"));
        }

        [Test]
        public void should_have_a_storage_token_in_the_headers()
        {
            Assert.That(GetContainerInformationSerialized.Headers[cloudfiles.Constants.X_STORAGE_TOKEN], Is.EqualTo("storagetoken"));
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_url_is_null_and_format_is_xml
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized(null, "storagetoken", "containername", Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_url_is_emptry_string_and_format_is_xml
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("", "storagetoken", "containername", Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_token_is_null_and_format_is_xml
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("http://storageurl", null, "containername", Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_storage_token_is_emptry_string_and_format_is_xml
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("http://storageurl", "", "containername", Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_container_name_is_null_and_format_is_xml
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("http://storageurl", "storagetoken", null, Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_container_name_is_emptry_string_and_format_is_xml
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new GetContainerInformationSerialized("http://storageurl", "storagetoken", "", Format.XML);
        }
    }

    [TestFixture]
    public class when_getting_container_information_and_format_is_xml
    {
        private GetContainerInformationSerialized GetContainerInformationSerialized;

        [SetUp]
        public void setup()
        {
            GetContainerInformationSerialized = new GetContainerInformationSerialized("http://storageurl", "storagetoken", "containername", Format.XML);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(GetContainerInformationSerialized.Uri.ToString(), Is.EqualTo("http://storageurl/containername?format=xml"));
        }

        [Test]
        public void should_have_a_http_get_method()
        {
            Assert.That(GetContainerInformationSerialized.Method, Is.EqualTo("GET"));
        }

        [Test]
        public void should_have_a_storage_token_in_the_headers()
        {
            Assert.That(GetContainerInformationSerialized.Headers[cloudfiles.Constants.X_STORAGE_TOKEN], Is.EqualTo("storagetoken"));
        }
    }

}