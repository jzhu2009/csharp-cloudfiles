using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

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
    public class When_adding_an_object_to_the_container_via_file_path_successfully_without_metatags
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
    public class When_adding_an_object_to_the_container_for_the_second_time_via_file_path_successfully_without_metatags
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

    public class MockCFContainer : CF_Container
    {

        public MockCFContainer(string containerName) : base(containerName)
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

        protected override void CloudFilesPutObject(string objectName, System.Collections.Generic.Dictionary<string,string> metaTags)
        {
            return;
        }

        protected override void  CloudFilesPutObject(System.IO.Stream localObjectStream, string remoteObjectName, System.Collections.Generic.Dictionary<string,string> metaTags)
        {
            return;
        }

        protected override void CloudFilesHeadContainer()
        {
            objectCount = objects.Count;
            bytesUsed = objects.Count * 34;
        }
    }
}