using System;
using System.Xml;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetAccountInformationSpecs
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

    [TestFixture]
    public class When_getting_serialized_account_information_for_an_account_in_json_format_and_container_exists : TestBase
    {
        [Test]
        public void Should_upload_the_content_type()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            try
            {
                connection.PutStorageItem(containerName, Constants.StorageItemNameJpg);
                string jsonReturnValue = connection.GetAccountInformationJson();
                string expectedSubString = "[{\"name\": \"" + containerName + "\", \"count\": 1, \"bytes\": 105542}]";

                Assert.That(jsonReturnValue, Is.EqualTo(expectedSubString));
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemNameJpg);
                connection.DeleteContainer(containerName);
            }
        }
    }

    [TestFixture]
    public class When_getting_serialized_account_information_for_an_account_in_xml_format_and_container_exists : TestBase
    {
        [Test]
        public void Should_upload_the_content_type()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            try
            {
                connection.PutStorageItem(containerName, Constants.StorageItemNameJpg);
                XmlDocument xmlReturnValue = connection.GetAccountInformationXml();

                string expectedSubString = "<container><name>" + containerName + "</name><count>1</count><bytes>105542</bytes></container>";
                Assert.That(xmlReturnValue.InnerXml.IndexOf(expectedSubString) > 0, Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemNameJpg);
                connection.DeleteContainer(containerName);
            }
        }
    }
}