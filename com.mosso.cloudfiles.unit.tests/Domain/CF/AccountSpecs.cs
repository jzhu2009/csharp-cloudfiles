using System;
using System.Xml;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Domain.CF.AccountSpecs
{
    [TestFixture]
    public class When_creating_a_new_container
    {
        [Test]
        public void should_give_you_container_instance_when_successful()
        {
            var account = new MockCFAccount();
            Assert.That(account.ContainerExists("testcontainername"), Is.False);
            IContainer container = account.CreateContainer("testcontainername");

            Assert.That(account.ContainerExists("testcontainername"), Is.True);
            Assert.That(container.Name, Is.EqualTo("testcontainername"));
            Assert.That(account.ContainerCount, Is.EqualTo(1));
            Assert.That(account.BytesUsed, Is.EqualTo(34));
        }
    }

    [TestFixture]
    public class When_creating_a_new_container_and_container_already_exists
    {

        [Test]
        [ExpectedException(typeof(ContainerAlreadyExistsException))]
        public void should_throw_container_already_exists_exception()
        {
            var account = new MockCFAccount();
            Assert.That(account.ContainerExists("alreadyexistingcontainer"), Is.False);
            account.CreateContainer("alreadyexistingcontainer");
            Assert.That(account.ContainerExists("alreadyexistingcontainer"), Is.True);
            account.CreateContainer("alreadyexistingcontainer");

            Assert.Fail("Allowed CreateContainer to be called when container already existed");
        }
    }

    [TestFixture]
    public class When_creating_a_new_container_and_container_name_is_null_or_empty_string
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception_if_the_container_name_is_empty_string()
        {
            var account = new MockCFAccount();
            account.ContainerExists("");

            Assert.Fail("Allowed ContainerExists to be called with empty string parameter");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void should_throw_argument_null_exception_if_the_container_name_is_null()
        {
            var account = new MockCFAccount();
            account.ContainerExists(null);

            Assert.Fail("Allowed ContainerExists to be called with null parameter");
        }

    }

    [TestFixture]
    public class When_deleting_a_container
    {
        [Test]
        public void should_do_nothing_when_successful()
        {
            var account = new MockCFAccount();
            account.CreateContainer("testcontainername");
            Assert.That(account.ContainerExists("testcontainername"), Is.True);
            account.DeleteContainer("testcontainername");

            Assert.That(account.ContainerExists("testcontainername"), Is.False);
        }
    }

    [TestFixture]
    public class When_deleting_a_container_and_container_does_not_exist
    {
        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void should_throw_container_not_empty_exception_if_the_container_not_empty()
        {
            var account = new MockCFAccount();
            account.DeleteContainer("testcontainername");

            Assert.Fail("Allowed deletion of non-existant container");
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_an_account_and_containers_exist
    {
        [Test]
        public void should_return_json_string_with_container_names_and_item_count_and_bytes_used()
        {
            var account = new MockCFAccount();
            account.CreateContainer("container");
            var expectedJson = "[{\"name\": \"container\", \"count\": 0, \"bytes\": 0}]";

            Assert.That(account.JSON, Is.EqualTo(expectedJson));
        }
    }

    [TestFixture]
    public class When_getting_a_json_serialized_version_of_an_account_and_no_containers_exist
    {
        [Test]
        public void should_return_json_string_emptry_brackets()
        {
            var account = new MockCFAccount();
            var expectedJson = "[]";

            Assert.That(account.JSON, Is.EqualTo(expectedJson));
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_an_account_and_containers_exist
    {
        [Test]
        public void should_return_xml_document_with_container_names_and_item_count_and_bytes_used()
        {
            var account = new MockCFAccount();
            account.CreateContainer("container");
            var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"><container><name>container</name><count>0</count><bytes>0</bytes></container></account>";

            Assert.That(account.XML.InnerXml, Is.EqualTo(expectedXml));
        }
    }

    [TestFixture]
    public class When_getting_a_xml_serialized_version_of_an_account_and_no_containers_exist
    {
        [Test]
        public void should_return_xml_document_with_account_name()
        {
            var account = new MockCFAccount();
            var expectedXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"></account>";

            Assert.That(account.XML.InnerXml, Is.EqualTo(expectedXml));
        }
    }

    public class MockCFAccount : CF_Account
    {
        public MockCFAccount() : base(null){}

        protected override void CloudFileCreateContainer(string containerName)
        {
            if (containers.Contains(containers.Find(x => x.Name == containerName))) throw new ContainerAlreadyExistsException();
        }

        protected override void CloudFilesDeleteContainer(string containerName)
        {
            if (containers.Find(x => x.Name == containerName) == null)
                throw new ContainerNotFoundException();
        }

        protected override void CloudFilesGetContainer(string containerName)
        {
            IContainer container = containers.Find(x => x.Name == containerName);
            if (container != null)
            {
                containers.Remove(container);
                containers.Add(new CF_Container(null, containerName));                
            }
        }

        public bool ValidatingExistingContainer { get; set; }

        protected override bool CloudFilesHeadContainer(string containerName)
        {
            return containers.Contains(containers.Find(x => x.Name == containerName));
        }

        protected override void CloudFilesHeadAccount()
        {
            containerCount = containers.Count;
            bytesUsed = containers.Count * 34;
        }

        protected override string CloudFileAccountInformationJson()
        {
            if(containers.Count > 0) 
                return "[{\"name\": \"container\", \"count\": 0, \"bytes\": 0}]";

            return "[]";
        }

        protected override XmlDocument CloudFileAccountInformationXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            if (containers.Count > 0)
            {
                xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"><container><name>container</name><count>0</count><bytes>0</bytes></container></account>");
                return xmlDocument;
            }

            xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><account name=\"MossoCloudFS_5d8f3dca-7eb9-4453-aa79-2eea1b980353\"></account>");
            return xmlDocument;

        }
    }
}