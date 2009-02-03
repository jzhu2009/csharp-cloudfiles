using System;
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
            Assert.That(container.CDNManagementUrl, Is.EqualTo(account.CDNManagementUrl));
            Assert.That(container.AuthToken, Is.EqualTo(account.AuthToken));
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
    public class When_deleting_a_container_and_container_is_not_empty
    {
        
        [Test]
        [ExpectedException(typeof(ContainerNotEmptyException))]
        public void should_throw_container_not_empty_exception_if_the_container_not_empty()
        {
            Assert.Ignore();
            var account = new MockCFAccount();
            IContainer container = account.CreateContainer("testcontainername");
            Assert.That(account.ContainerExists("testcontainername"), Is.True);

            container.AddObject(Constants.STORAGE_ITEM_NAME);
            container.ObjectExists(Constants.STORAGE_ITEM_NAME);

            account.DeleteContainer("testcontainername");

            Assert.Fail("Allowed deletion of a non-empty container");
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

    public class MockCFAccount : CF_Account
    {
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
                containers.Add(new CF_Container(containerName));                
            }
        }

        public bool ValidatingExistingContainer { get; set; }

        protected override bool CloudFilesHeadContainer(string containerName)
        {
            return containers.Contains(containers.Find(x => x.Name == containerName));
        }
    }
}