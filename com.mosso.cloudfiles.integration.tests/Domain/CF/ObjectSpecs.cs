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
        protected IAccount account;
        protected IContainer container;
        protected IObject @object;

        [SetUp]
        public void Setup()
        {
            var userCredentials = new UserCredentials(Credentials.USERNAME, Credentials.API_KEY);
            IConnection connection = new Connection(userCredentials);

            account = connection.Account;
            container = account.CreateContainer(Constants.CONTAINER_NAME);
        }

        [TearDown]
        public void TearDown()
        {
            if (container.ObjectExists(Constants.StorageItemName))
                container.DeleteObject(Constants.StorageItemName);

            if (account.ContainerExists(Constants.CONTAINER_NAME))
                account.DeleteContainer(Constants.CONTAINER_NAME);
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
            Assert.That(@object.ContentType.Contains("text/plain"), Is.True);
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