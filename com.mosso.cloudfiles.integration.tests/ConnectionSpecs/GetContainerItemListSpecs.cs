using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetContainerItemListSpecs
{
    [TestFixture]
    public class When_retrieving_a_list_of_items_from_a_container_using_connection : TestBase
    {
        [Test]
        public void Should_return_a_list_of_items_in_the_container()
        {
            List<string> containerItems;

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
            List<string> containerItems;

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