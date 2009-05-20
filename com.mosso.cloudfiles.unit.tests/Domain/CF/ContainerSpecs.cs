using System;
using System.Xml;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using System.Collections.Generic;

namespace com.mosso.cloudfiles.unit.tests.Domain.CF.ContainerSpecs
{
    [TestFixture]
    public class When_making_a_container_public
    {
        [Test]
        public void Should_obtain_a_public_url()
        {
            var container = new MockCFContainer("testcontainername");
            container.MarkAsPublic();

            Assert.That(container.PublicUrl.ToString().Contains("http://tempuri.org"), Is.True);
        }
    }

    [TestFixture]
    public class When_making_a_container_public_and_adding_an_object
    {
        [Test]
        public void Should_obtain_a_public_url()
        {
            var container = new MockCFContainer("testcontainername");
            container.MarkAsPublic();

            Assert.That(container.PublicUrl.ToString().Contains("http://tempuri.org"), Is.True);
        }

        [Test]
        public void should_give_the_object_public_url()
        {
            var container = new MockCFContainer("testcontainername");
            container.MarkAsPublic();
            var @object = container.AddObject(Constants.STORAGE_ITEM_NAME);

            Assert.That(@object.PublicUrl.ToString(), Is.EqualTo("http://tempuri.org/" + Constants.STORAGE_ITEM_NAME));
        }
    }

    [TestFixture]
    public class When_adding_an_object_to_the_container_via_file_path_successfully_without_metadata
    {
        [Test]
        public void should_add_the_object()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject(Constants.STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.STORAGE_ITEM_NAME), Is.True);
            Assert.That(container.ObjectCount, Is.EqualTo(1));
            Assert.That(container.BytesUsed, Is.EqualTo(34));
        }
    }

    [TestFixture]
    public class When_adding_an_object_to_the_container_for_the_second_time_via_file_path_successfully_without_metadata
    {
        [Test]
        public void should_add_the_object()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject(Constants.STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.STORAGE_ITEM_NAME), Is.True);
        }
    }

    [TestFixture]
    public class When_getting_an_object_list_from_the_container_without_query_parameters
    {
        [Test]
        public void should_add_the_object()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject(Constants.STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.STORAGE_ITEM_NAME), Is.True);
            string[] objectNames = container.GetObjectNames();
            Assert.That(objectNames[0], Is.EqualTo(Constants.STORAGE_ITEM_NAME));
        }
    }

    [TestFixture]
    public class When_getting_an_object_list_from_the_container_with_the_limit_query_parameter
    {
        [Test]
        public void should_return_only_the_specified_number_of_objects()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject(Constants.STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.STORAGE_ITEM_NAME), Is.True);
            container.AddObject(Constants.HEAD_STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.HEAD_STORAGE_ITEM_NAME), Is.True);

            string[] objectNames = container.GetObjectNames();
            Assert.That(objectNames.Length, Is.EqualTo(2));
            Assert.That(objectNames[0], Is.EqualTo(Constants.STORAGE_ITEM_NAME));
            Assert.That(objectNames[1], Is.EqualTo(Constants.HEAD_STORAGE_ITEM_NAME));

            Dictionary<GetItemListParameters, string> parameters = new Dictionary<GetItemListParameters, string>();
            parameters.Add(GetItemListParameters.Limit, "1");
            objectNames = container.GetObjectNames(parameters);
            Assert.That(objectNames.Length, Is.EqualTo(1));
            Assert.That(objectNames[0], Is.EqualTo(Constants.STORAGE_ITEM_NAME));
        }
    }

    [TestFixture]
    public class When_getting_an_object_list_from_the_container_with_the_marker_query_parameter
    {
        [Test]
        public void should_return_only_objects_greater_than_the_marker_value()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject(Constants.STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.STORAGE_ITEM_NAME), Is.True);
            container.AddObject(Constants.HEAD_STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.HEAD_STORAGE_ITEM_NAME), Is.True);

            string[] objectNames = container.GetObjectNames();
            Assert.That(objectNames.Length, Is.EqualTo(2));
            Assert.That(objectNames[0], Is.EqualTo(Constants.STORAGE_ITEM_NAME));
            Assert.That(objectNames[1], Is.EqualTo(Constants.HEAD_STORAGE_ITEM_NAME));

            Dictionary<GetItemListParameters, string> parameters = new Dictionary<GetItemListParameters, string>();
            parameters.Add(GetItemListParameters.Marker, "1");
            objectNames = container.GetObjectNames(parameters);
            Assert.That(objectNames.Length, Is.EqualTo(1));
            Assert.That(objectNames[0], Is.EqualTo(Constants.HEAD_STORAGE_ITEM_NAME));
        }
    }

    [TestFixture]
    public class When_getting_an_object_list_from_the_container_with_the_prefix_query_parameter
    {
        [Test]
        public void should_return_only_objects_beginning_with_the_provided_substring()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject(Constants.STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.STORAGE_ITEM_NAME), Is.True);
            container.AddObject(Constants.HEAD_STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.HEAD_STORAGE_ITEM_NAME), Is.True);

            string[] objectNames = container.GetObjectNames();
            Assert.That(objectNames.Length, Is.EqualTo(2));
            Assert.That(objectNames[0], Is.EqualTo(Constants.STORAGE_ITEM_NAME));
            Assert.That(objectNames[1], Is.EqualTo(Constants.HEAD_STORAGE_ITEM_NAME));

            Dictionary<GetItemListParameters, string> parameters = new Dictionary<GetItemListParameters, string>();
            parameters.Add(GetItemListParameters.Prefix, "h");
            objectNames = container.GetObjectNames(parameters);
            Assert.That(objectNames.Length, Is.EqualTo(1));
            Assert.That(objectNames[0], Is.EqualTo(Constants.HEAD_STORAGE_ITEM_NAME));

            parameters.Clear();
            parameters.Add(GetItemListParameters.Prefix, "t");
            objectNames = container.GetObjectNames(parameters);
            Assert.That(objectNames.Length, Is.EqualTo(1));
            Assert.That(objectNames[0], Is.EqualTo(Constants.STORAGE_ITEM_NAME));
        }
    }

    [TestFixture]
    public class When_deleting_an_object_from_the_container_and_the_object_exists
    {
        [Test]
        public void should_delete_the_object()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject(Constants.STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.STORAGE_ITEM_NAME), Is.True);
            container.DeleteObject(Constants.STORAGE_ITEM_NAME);
            Assert.That(container.ObjectExists(Constants.STORAGE_ITEM_NAME), Is.False);
        }
    }

    [TestFixture]
    public class When_deleting_an_object_from_the_container_and_the_object_does_not_exist
    {
        [Test]
        [ExpectedException(typeof(StorageItemNotFoundException))]
        public void should_throw_storage_item_not_found_exception()
        {
            var container = new MockCFContainer("testcontainername");
            container.DeleteObject(Constants.STORAGE_ITEM_NAME);

            Assert.Fail("Allowed deletion of non-existant object");
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_a_container_and_objects_exist
    {
        [Test]
        public void should_return_json_string_with_object_names_and_hash_and_bytes_and_content_type_and_last_modified_date()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject("test_object_1");
            var expectedJson = "[{\"name\":\"test_object_1\",\"hash\":\"4281c348eaf83e70ddce0e07221c3d28\",\"bytes\":14,\"content_type\":\"application\\/octet-stream\",\"last_modified\":\"2009-02-03T05:26:32.612278\"}]";

            Assert.That(container.JSON, Is.EqualTo(expectedJson));
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_a_container_and_no_objects_exist
    {
        [Test]
        public void should_return_json_string_emptry_brackets()
        {
            var container  = new MockCFContainer("testcontainername");
            var expectedJson = "[]";

            Assert.That(container.JSON, Is.EqualTo(expectedJson));
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_a_container_and_objects_exist
    {
        [Test]
        public void should_return_xml_document_with_objects_names_and_hash_and_bytes_and_content_type_and_last_modified_date()
        {
            var container = new MockCFContainer("testcontainername");
            container.AddObject("object");
            var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><container name=\"testcontainername\"><object><name>object</name><hash>4281c348eaf83e70ddce0e07221c3d28</hash><bytes>14</bytes><content_type>application/octet-stream</content_type><last_modified>2009-02-03T05:26:32.612278</last_modified></object></container>";

            Assert.That(container.XML.InnerXml, Is.EqualTo(expectedXml));
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_a_container_and_no_objects_exist
    {
        [Test]
        public void should_return_xml_document_with_xxxxx()
        {
            var container = new MockCFContainer("testcontainername");
            var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><container name=\"testcontainername\"></container>";

            Assert.That(container.XML.InnerXml, Is.EqualTo(expectedXml));
        }
    }

    public class MockCFContainer : CF_Container
    {

        public MockCFContainer(string containerName) : base(null, containerName)
        {
        }

        protected override bool CloudFilesHeadObject(string objectName)
        {
            return objects.Contains(objects.Find(x => x.Name == objectName));
        }

        protected override void CloudFilesDeleteObject(string objectName)
        {
            if(objects.Find(x => x.Name == objectName) == null)
                throw new StorageItemNotFoundException();
        }

        protected override void CloudFilesMarkContainerPublic()
        {
            PublicUrl = new Uri("http://tempuri.org");
        }

        protected override void CloudFilesPutObject(string objectName, Dictionary<string,string> metadata)
        {
            return;
        }

        protected override void  CloudFilesPutObject(System.IO.Stream localObjectStream, string remoteObjectName, Dictionary<string,string> metadata)
        {
            return;
        }

        protected override void CloudFilesHeadContainer()
        {
            objectCount = objects.Count;
            bytesUsed = objects.Count * 34;
        }

        protected override string[] CloudFilesGetContainer(Dictionary<GetItemListParameters, string> parameters)
        {
            List<string> objectNames = new List<string>();
            string limit = parameters.ContainsKey(GetItemListParameters.Limit) ? parameters[GetItemListParameters.Limit] : null;
            string offset = parameters.ContainsKey(GetItemListParameters.Marker) ? parameters[GetItemListParameters.Marker] : null;
            string prefix = parameters.ContainsKey(GetItemListParameters.Prefix) ? parameters[GetItemListParameters.Prefix] : null;

            int count = 0;
            foreach(IObject @object in objects)
            {
                if (offset != null && count < int.Parse(offset))
                {
                    count++;
                    continue;
                }
                if (prefix != null && !@object.Name.ToLower().StartsWith(prefix.ToLower())) continue;

                objectNames.Add(@object.Name);
                count++;

                if (limit != null && count == int.Parse(limit))
                    return objectNames.ToArray();
            }

            return objectNames.ToArray();
        }

        protected override string CloudFileContainerInformationJson()
        {
            if (objects.Count > 0)
                return "[{\"name\":\"test_object_1\",\"hash\":\"4281c348eaf83e70ddce0e07221c3d28\",\"bytes\":14,\"content_type\":\"application\\/octet-stream\",\"last_modified\":\"2009-02-03T05:26:32.612278\"}]";

            return "[]";
        }

        protected override XmlDocument CloudFileContainerInformationXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            if (objects.Count > 0)
            {
                xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><container name=\"testcontainername\"><object><name>object</name><hash>4281c348eaf83e70ddce0e07221c3d28</hash><bytes>14</bytes><content_type>application/octet-stream</content_type><last_modified>2009-02-03T05:26:32.612278</last_modified></object></container>");
                return xmlDocument;
            }

            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><container name=\"testcontainername\"></container>");
            return xmlDocument;

        }
    }
}