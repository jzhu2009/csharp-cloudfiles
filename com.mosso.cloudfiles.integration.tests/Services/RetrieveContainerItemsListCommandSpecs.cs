using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.integration.tests.domain;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services.RetrieveContainerItemsListCommandSpecs
{
    [TestFixture]
    public class When_retrieving_a_list_of_items_from_a_container_using_connection : TestBase
    {
        private Connection connection;

        protected override void SetUp()
        {
            connection = new Connection(new UserCredentials(Constants.MOSSO_ACCOUNT, Constants.MOSSO_PASSWORD));
        }

        [Test]
        public void Should_return_a_list_of_items_in_the_container()
        {
            List<string> containerItems = new List<string>();

            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);
                containerItems = connection.GetContainerItemList(containerName);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }

            Assert.That(containerItems.Count, Is.EqualTo(1));
        }

        [Test]
        public void Should_return_no_items_when_the_container_has_no_items()
        {
            List<string> containerItems = new List<string>();

            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);
                containerItems = connection.GetContainerItemList(containerName);
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }

            Assert.That(containerItems.Count, Is.EqualTo(0));
        }
    }
}