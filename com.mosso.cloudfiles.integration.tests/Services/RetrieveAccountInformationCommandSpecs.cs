using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.integration.tests.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.services.RetrieveAccountInformationCommandSpecs
{
    [TestFixture]
    public class When_retrieving_account_information_from_a_container_using_connection : TestBase
    {
        [Test]
        public void Should_return_the_size_and_quantity_of_items_in_the_account()
        {
            string containerName = Guid.NewGuid().ToString();

            try
            {
                connection.CreateContainer(containerName);
                connection.PutStorageItem(containerName, Constants.StorageItemName);

                AccountInformation account = connection.GetAccountInformation();

                Assert.That(account, Is.Not.Null);
                Assert.That(account.BytesUsed, Is.GreaterThan(0));
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemName);
                connection.DeleteContainer(containerName);
            }
        }
    }
}