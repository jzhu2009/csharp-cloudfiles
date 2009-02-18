using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.CF.ObjectSpecs
{
    [TestFixture]
    public class When_getting_information_on_an_object
    {
        private IObject @object;

        [SetUp]
        public void Setup()
        {
            @object = new MockCFObject(Constants.STORAGE_ITEM_NAME);
        }

        [Test]
        public void should_have_content_length()
        {
            
            Assert.That(@object.ContentLength, Is.EqualTo(34));
        }

        [Test]
        public void should_have_etag()
        {
            Assert.That(@object.ETag, Is.EqualTo("etag"));
        }

        [Test]
        public void should_have_content_type()
        {
            Assert.That(@object.ContentType, Is.EqualTo("text/plain"));
        }

        [Test]
        public void should_have_metadata()
        {
            Assert.That(@object.Metadata.Count, Is.EqualTo(0));
        }
    }

    [TestFixture]
    public class When_setting_an_objects_meta_data_on_instantiation
    {
        private IObject @object;

        [SetUp]
        public void Setup()
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("key1", "value1");
            metadata.Add("key2", "value2");
            metadata.Add("key3", "value3");
            metadata.Add("key4", "value4");

            @object = new MockCFObject(Constants.STORAGE_ITEM_NAME, metadata);
        }

        [Test]
        public void should_give_count_of_metadata()
        {

            Assert.That(@object.Metadata.Count, Is.EqualTo(4));

        }
    }

    [TestFixture]
    public class When_setting_an_objects_meta_data_via_property
    {
        private IObject @object;
        private Dictionary<string, string> metadata;

        [SetUp]
        public void Setup()
        {
            metadata = new Dictionary<string, string>();
            metadata.Add("key1", "value1");
            metadata.Add("key2", "value2");
            metadata.Add("key3", "value3");
            metadata.Add("key4", "value4");

            @object = new MockCFObject(Constants.STORAGE_ITEM_NAME);
        }

        [Test]
        public void should_give_count_of_metadata()
        {

            Assert.That(@object.Metadata.Count, Is.EqualTo(0));
            @object.Metadata = metadata;
            Assert.That(@object.Metadata.Count, Is.EqualTo(4));
        }
    }

    public class MockCFObject : CF_Object
    {
        public MockCFObject(string objectName) : base(null, objectName, new Dictionary<string, string>()){}
        public MockCFObject(string objectName, Dictionary<string, string> metadata) : base(null, objectName, metadata){}

        protected override void CloudFilesHeadObject()
        {
            contentLength = 34;
            contentType = "text/plain";
            etag = "etag";
        }

        protected override void CloudFilesPostObject()
        {
        }
    }
}