using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.SetStorageItemMetaInformationSpecs
{
    [TestFixture]
    public class when_setting_storage_item_information_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetStorageItemMetaInformation(null, "authtoken", "containername", "storageitemname", null);
        }
    }

    [TestFixture]
    public class when_setting_storage_item_information_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetStorageItemMetaInformation("", "authtoken", "containername", "storageitemname", null);
        }
    }

    [TestFixture]
    public class when_setting_storage_item_information_and_auth_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetStorageItemMetaInformation("http://storageurl", null, "containername", "storageitemname", null);
        }
    }

    [TestFixture]
    public class when_setting_storage_item_information_and_auth_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetStorageItemMetaInformation("http://storageurl", "", "containername", "storageitemname", null);
        }
    }

    [TestFixture]
    public class when_setting_storage_item_information_and_container_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetStorageItemMetaInformation("http://storageurl", "authtoken", null, "storageitemname", null);
        }
    }

    [TestFixture]
    public class when_setting_storage_item_information_and_container_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetStorageItemMetaInformation("http://storageurl", "authtoken", "", "storageitemname", null);
        }
    }

    [TestFixture]
    public class when_setting_storage_item_information_and_storage_item_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetStorageItemMetaInformation("http://storageurl", "authtoken", "containername", null, null);
        }
    }

    [TestFixture]
    public class when_setting_storage_item_information_and_storage_item_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new SetStorageItemMetaInformation("http://storageurl", "authtoken", "containername", "", null);
        }
    }

    [TestFixture]
    public class when_setting_storage_item_information
    {
        private SetStorageItemMetaInformation setStorageItemInformation;

        [SetUp]
        public void setup()
        {
            var metadata = new Dictionary<string, string>{{"key1", "value1"},{"key2", "value2"}};
            setStorageItemInformation = new SetStorageItemMetaInformation("http://storageurl", "authtoken", "containername", "storageitemname", metadata);
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(setStorageItemInformation.Uri.ToString(), Is.EqualTo("http://storageurl/containername/storageitemname"));
        }

        [Test]
        public void should_have_a_http_post_method()
        {
            Assert.That(setStorageItemInformation.Method, Is.EqualTo("POST"));
        }

        [Test]
        public void should_have_a_auth_token_in_the_headers()
        {
            Assert.That(setStorageItemInformation.Headers[utils.Constants.X_AUTH_TOKEN], Is.EqualTo("authtoken"));
        }

        [Test]
        public void should_have_metadata_in_the_headers()
        {
            Assert.That(setStorageItemInformation.Headers[utils.Constants.META_DATA_HEADER + "key1"], Is.EqualTo("value1"));
            Assert.That(setStorageItemInformation.Headers[utils.Constants.META_DATA_HEADER + "key2"], Is.EqualTo("value2"));
        }
    }
}