using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.Domain.CF.ContainerSpecs
{
    [TestFixture]
    public class ContainerIntegrationTestBase
    {
        protected IAccount account;
        protected IContainer container;

        [SetUp]
        public void Setup()
        {
            var userCredentials = new UserCredentials(Credentials.USERNAME, Credentials.API_KEY);
            var connection = new Connection(userCredentials);

            account = connection.Account;
            container = account.CreateContainer(Constants.CONTAINER_NAME);
        }

        [TearDown]
        public void TearDown()
        {
            if (container.ObjectExists(Constants.StorageItemName))
                container.DeleteObject(Constants.StorageItemName);

            if (container.ObjectExists(Constants.HeadStorageItemName))
                container.DeleteObject(Constants.HeadStorageItemName);

            if (account.ContainerExists(Constants.CONTAINER_NAME))
                account.DeleteContainer(Constants.CONTAINER_NAME);
        } 
    }

    [TestFixture]
    public class When_making_a_container_public : ContainerIntegrationTestBase
    {
        [Test]
        public void Should_obtain_a_public_url()
        {
            container.MarkAsPublic();

            Assert.That(container.PublicUrl.ToString().Contains("http://cdn.cloudfiles.mosso.com/"), Is.True);
        }
    }

    [TestFixture]
    public class When_adding_an_object_to_the_container_via_file_path_successfully_without_metadata : ContainerIntegrationTestBase
    {
        [Test]
        public void should_add_the_object()
        {
            var @object = container.AddObject(Constants.StorageItemName);

            Assert.That(@object.Name, Is.EqualTo(Constants.StorageItemName));
            Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
            Assert.That(container.ObjectCount, Is.EqualTo(1));
            Assert.That(container.BytesUsed, Is.EqualTo(34));
        }
    }

    [TestFixture]
    public class When_adding_an_object_to_the_container_via_file_path_successfully_with_metadata : ContainerIntegrationTestBase
    {
        [Test]
        public void should_add_the_object()
        {
            var metadata = new Dictionary<string, string>
                               {
                                   { "key1", "value1" }, 
                                   { "key2", "value2" }, 
                                   { "key3", "value3" }, 
                                   { "key4", "value4" }, 
                                   { "key5", "value5" }
                               };
            var @object = container.AddObject(Constants.StorageItemName, metadata);

            Assert.That(@object.Name, Is.EqualTo(Constants.StorageItemName));
            Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
        }

        [Test]
        public void should_give_object_count_and_bytes_used()
        {
            var metadata = new Dictionary<string, string>
                               {
                                   { "key1", "value1" }, 
                                   { "key2", "value2" }, 
                                   { "key3", "value3" }, 
                                   { "key4", "value4" }, 
                                   { "key5", "value5" }
                               };
            var @object = container.AddObject(Constants.StorageItemName, metadata);

            Assert.That(@object.Name, Is.EqualTo(Constants.StorageItemName));
            Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);

            Assert.That(container.ObjectCount, Is.EqualTo(1));
            Assert.That(container.BytesUsed, Is.EqualTo(34));
        }
    }

    [TestFixture]
    public class When_getting_an_object_list_from_the_container_with_the_limit_query_parameter : ContainerIntegrationTestBase
    {
        [Test]
        public void should_return_only_the_specified_number_of_objects()
        {
            container.AddObject(Constants.StorageItemName);
            Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
            container.AddObject(Constants.HeadStorageItemName);
            Assert.That(container.ObjectExists(Constants.HeadStorageItemName), Is.True);

            var objectNames = container.GetObjectNames();
            Assert.That(objectNames.Length, Is.EqualTo(2));
            Assert.That(objectNames[0], Is.EqualTo(Constants.HeadStorageItemName));
            Assert.That(objectNames[1], Is.EqualTo(Constants.StorageItemName));

            var parameters = new Dictionary<GetItemListParameters, string>{{GetItemListParameters.Limit, "1"}};
            objectNames = container.GetObjectNames(parameters);
            Assert.That(objectNames.Length, Is.EqualTo(1));
            Assert.That(objectNames[0], Is.EqualTo(Constants.HeadStorageItemName));
        }
    }

    [TestFixture]
    public class When_getting_an_object_list_from_the_container_with_the_marker_query_parameter : ContainerIntegrationTestBase
    {
        [Test]
        public void should_return_only_objects_greater_than_the_marker()
        {
            container.AddObject(Constants.StorageItemName);
            Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
            container.AddObject(Constants.HeadStorageItemName);
            Assert.That(container.ObjectExists(Constants.HeadStorageItemName), Is.True);

            var objectNames = container.GetObjectNames();
            Assert.That(objectNames.Length, Is.EqualTo(2));
            Assert.That(objectNames[0], Is.EqualTo(Constants.HeadStorageItemName));
            Assert.That(objectNames[1], Is.EqualTo(Constants.StorageItemName));

            var parameters = new Dictionary<GetItemListParameters, string> { { GetItemListParameters.Marker, "HeadStorageItem.txt" } };
            objectNames = container.GetObjectNames(parameters);
            Assert.That(objectNames.Length, Is.EqualTo(1));
            Assert.That(objectNames[0], Is.EqualTo(Constants.StorageItemName));
        }
    }

    [TestFixture]
    public class When_getting_an_object_list_from_the_container_with_the_prefix_query_parameter : ContainerIntegrationTestBase
    {
        [Test]
        public void should_return_only_objects_beginning_with_the_provided_substring()
        {
            container.AddObject(Constants.StorageItemName);
            Assert.That(container.ObjectExists(Constants.StorageItemName), Is.True);
            container.AddObject(Constants.HeadStorageItemName);
            Assert.That(container.ObjectExists(Constants.HeadStorageItemName), Is.True);

            var objectNames = container.GetObjectNames();
            Assert.That(objectNames.Length, Is.EqualTo(2));
            Assert.That(objectNames[0], Is.EqualTo(Constants.HeadStorageItemName));
            Assert.That(objectNames[1], Is.EqualTo(Constants.StorageItemName));

            var parameters = new Dictionary<GetItemListParameters, string> { { GetItemListParameters.Prefix, "H" } };
            objectNames = container.GetObjectNames(parameters);
            Assert.That(objectNames.Length, Is.EqualTo(1));
            Assert.That(objectNames[0], Is.EqualTo(Constants.HeadStorageItemName));

            parameters.Clear();
            parameters.Add(GetItemListParameters.Prefix, "T");
            objectNames = container.GetObjectNames(parameters);
            Assert.That(objectNames.Length, Is.EqualTo(1));
            Assert.That(objectNames[0], Is.EqualTo(Constants.StorageItemName));
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_a_container_and_objects_exist : ContainerIntegrationTestBase
    {
        [Test]
        public void should_return_json_string_with_object_names_and_hash_and_bytes_and_content_type_and_last_modified_date()
        {
            container.AddObject(Constants.StorageItemName);
            var expectedJson = "[{\"name\": \"" + Constants.StorageItemName + "\", \"hash\": \"5c66108b7543c6f16145e25df9849f7f\", \"bytes\": 34, \"content_type\": \"text\\u002fplain\", \"last_modified\": \"" + String.Format("{0:yyyy-MM}", DateTime.Now);

            Assert.That(container.JSON.IndexOf(expectedJson) == 0, Is.True);
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_a_container_and_no_objects_exist : ContainerIntegrationTestBase
    {
        [Test]
        public void should_return_json_string_emptry_brackets()
        {
            var expectedJson = "[]";

            Assert.That(container.JSON, Is.EqualTo(expectedJson));
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_a_container_and_objects_exist : ContainerIntegrationTestBase
    {
        [Test]
        public void should_return_xml_document_with_objects_names_and_hash_and_bytes_and_content_type_and_last_modified_date()
        {
            container.AddObject(Constants.StorageItemName);
            var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><container name=\"" + Constants.CONTAINER_NAME + "\"><object><name>" + Constants.StorageItemName + "</name><hash>5c66108b7543c6f16145e25df9849f7f</hash><bytes>34</bytes><content_type>text/plain</content_type><last_modified>" + String.Format("{0:yyyy-MM}", DateTime.Now);

            Assert.That(container.XML.InnerXml.IndexOf(expectedXml) > -1, Is.True);
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_a_container_and_no_objects_exist : ContainerIntegrationTestBase
    {
        [Test]
        public void should_return_xml_document_with_xxxxx()
        {
            var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><container name=\"" + Constants.CONTAINER_NAME + "\"></container>";

            Assert.That(container.XML.InnerXml, Is.EqualTo(expectedXml));
        }
    }
}