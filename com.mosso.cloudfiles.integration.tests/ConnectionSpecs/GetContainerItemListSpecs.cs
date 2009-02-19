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

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                containerItems = connection.GetContainerItemList(Constants.CONTAINER_NAME);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(containerItems.Count, Is.EqualTo(1));
        }

        [Test]
        public void Should_return_no_items_when_the_container_has_no_items()
        {
            List<string> containerItems;

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                containerItems = connection.GetContainerItemList(Constants.CONTAINER_NAME);
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }

            Assert.That(containerItems.Count, Is.EqualTo(0));
        }
    }
}