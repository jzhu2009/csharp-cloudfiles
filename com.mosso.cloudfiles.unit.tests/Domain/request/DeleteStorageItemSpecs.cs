using System;
using com.mosso.cloudfiles.domain.request;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.DeleteStorageItemSpecs
{
    [TestFixture]
    public class when_deleting_a_storage_item_and_storage_url_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteStorageItem(null, "containername", "storageitemname", "storagetoken");
        }
    }

    [TestFixture]
    public class when_deleting_a_storage_item_and_storage_url_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteStorageItem("", "containername", "storageitemname", "storagetoken");
        }
    }

    [TestFixture]
    public class when_deleting_a_storage_item_and_storage_token_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteStorageItem("http://storageurl", "containername", "storageitemname", null);
        }
    }

    [TestFixture]
    public class when_deleting_a_storage_item_and_storage_token_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteStorageItem("http://storageurl", "containername", "storageitemname", "");
        }
    }

    [TestFixture]
    public class when_deleting_a_storage_item_and_storage_item_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteStorageItem("http://storageurl", "containername", null, "storagetoken");
        }
    }

    [TestFixture]
    public class when_deleting_a_storage_item_and_storage_item_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteStorageItem("http://storageurl", "containername", "", "storagetoken");
        }
    }

    [TestFixture]
    public class when_deleting_a_storage_item_and_container_name_is_null
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteStorageItem("http://storageurl", null, "storageitemname", "storagetoken");
        }
    }

    [TestFixture]
    public class when_deleting_a_storage_item_and_container_name_is_emptry_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception()
        {
            new DeleteStorageItem("http://storageurl", "", "storageitemname", "storagetoken");
        }
    }

    [TestFixture]
    public class when_deleting_a_storage_item
    {
        private DeleteStorageItem deleteStorageItem;

        [SetUp]
        public void setup()
        {
            deleteStorageItem = new DeleteStorageItem("http://storageurl", "containername", "storageitemname", "storagetoken");
        }

        [Test]
        public void should_have_properly_formmated_request_url()
        {
            Assert.That(deleteStorageItem.Uri.ToString(), Is.EqualTo("http://storageurl/containername/storageitemname"));
        }

        [Test]
        public void should_have_a_http_delete_method()
        {
            Assert.That(deleteStorageItem.Method, Is.EqualTo("DELETE"));
        }

        [Test]
        public void should_have_a_storage_token_in_the_headers()
        {
            Assert.That(deleteStorageItem.Headers[cloudfiles.Constants.X_STORAGE_TOKEN], Is.EqualTo("storagetoken"));
        }
    }
}