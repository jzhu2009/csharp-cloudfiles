using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.Domain.CF.ObjectSpecs
{
    [TestFixture]
    public class ObjectIntegrationTestBase
    {
        protected string containerName;
        protected IAccount account;
        protected IContainer container;
        protected IObject @object;

        [SetUp]
        public void Setup()
        {
            containerName = Guid.NewGuid().ToString();

            var userCredentials = new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY);
            var authentication = new CF_Authentication(userCredentials);

            account = authentication.Authenticate();
            container = account.CreateContainer(containerName);
        }

        [TearDown]
        public void TearDown()
        {
            if (container.ObjectExists(Constants.StorageItemName))
                container.DeleteObject(Constants.StorageItemName);

            if (containerName != null && container != null)
                account.DeleteContainer(containerName);
        }
    }

    [TestFixture]
    public class When_getting_information_on_an_object : ObjectIntegrationTestBase
    {
        [Test]
        public void should_have_content_length()
        {
            @object = container.AddObject(Constants.StorageItemName);
            Assert.That(@object.ContentLength, Is.EqualTo(34));
        }

        [Test]
        public void should_have_etag()
        {
            @object = container.AddObject(Constants.StorageItemName);
            Assert.That(@object.ETag.Length, Is.EqualTo(32));
        }

        [Test]
        public void should_have_content_type()
        {
            @object = container.AddObject(Constants.StorageItemName);
            Assert.That(@object.ContentType, Is.EqualTo("text/plain; charset=UTF-8"));
        }

        [Test]
        public void should_have_metadata()
        {
            @object = container.AddObject(Constants.StorageItemName);
            Assert.That(@object.Metadata.Count, Is.EqualTo(0));
        }
    }

    [TestFixture]
    public class When_setting_an_objects_meta_data_on_instantiation : ObjectIntegrationTestBase
    {
        [Test]
        public void should_give_count_of_metadata()
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("key1", "value1");
            metadata.Add("key2", "value2");
            metadata.Add("key3", "value3");
            metadata.Add("key4", "value4");

            @object = container.AddObject(Constants.StorageItemName, metadata);

            Assert.That(@object.Metadata.Count, Is.EqualTo(4));

        }
    }

    [TestFixture]
    public class When_setting_an_objects_meta_data_via_property : ObjectIntegrationTestBase
    {
        [Test]
        public void should_give_count_of_metadata()
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("key1", "value1");
            metadata.Add("key2", "value2");
            metadata.Add("key3", "value3");
            metadata.Add("key4", "value4");

            @object = container.AddObject(Constants.StorageItemName);

            Assert.That(@object.Metadata.Count, Is.EqualTo(0));
            @object.Metadata = metadata;
            Assert.That(@object.Metadata.Count, Is.EqualTo(4));
        }
    }
}