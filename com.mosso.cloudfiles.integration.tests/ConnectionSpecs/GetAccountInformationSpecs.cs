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
            

            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);

                AccountInformation account = connection.GetAccountInformation();

                Assert.That(account, Is.Not.Null);
                Assert.That(account.BytesUsed, Is.GreaterThan(0));
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemName);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_getting_serialized_account_information_for_an_account_in_json_format_and_container_exists : TestBase
    {
        [Test]
        public void Should_get_serialized_json_format()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);

            try
            {
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                string jsonReturnValue = connection.GetAccountInformationJson();
                string expectedSubString = "[{\"name\": \"" + Constants.CONTAINER_NAME + "\", \"count\": 1, \"bytes\": 105542}]";

                Assert.That(jsonReturnValue, Is.EqualTo(expectedSubString));
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_getting_serialized_account_information_for_an_account_in_xml_format_and_container_exists : TestBase
    {
        [Test]
        public void Should_get_serialized_xml_format()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);

            try
            {
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                XmlDocument xmlReturnValue = connection.GetAccountInformationXml();

                string expectedSubString = "<container><name>" + Constants.CONTAINER_NAME + "</name><count>1</count><bytes>105542</bytes></container>";
                Assert.That(xmlReturnValue.InnerXml.IndexOf(expectedSubString) > 0, Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}