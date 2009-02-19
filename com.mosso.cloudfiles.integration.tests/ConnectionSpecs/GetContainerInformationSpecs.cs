using System;
using System.Xml;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetContainerInformationSpecs
{
    [TestFixture]
    public class When_requesting_information_on_a_container_using_connection : TestBase
    {
        [Test]
        public void Should_return_container_information_when_the_container_exists()
        {
            
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                Container containerInformation = connection.GetContainerInformation(Constants.CONTAINER_NAME);

                Assert.That(containerInformation.Name, Is.EqualTo(Constants.CONTAINER_NAME));
                Assert.That(containerInformation.ByteCount, Is.EqualTo(0));
                Assert.That(containerInformation.ObjectCount, Is.EqualTo(0));
            }
            finally
            {
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            
            connection.GetContainerInformation(Constants.CONTAINER_NAME);
        }
    }

    [TestFixture]
    public class When_getting_serialized_container_information_for_a_container_in_json_format_and_objects_exist : TestBase
    {
        [Test]
        public void Should_get_serialized_json_format()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);

            try
            {
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                string jsonReturnValue = connection.GetContainerInformationJson(Constants.CONTAINER_NAME);
                var expectedSubString = "[{\"name\": \"" + Constants.StorageItemNameJpg + "\", \"hash\": \"b44a59383b3123a747d139bd0e71d2df\", \"bytes\": 105542, \"content_type\": \"image\\u002fjpeg\", \"last_modified\": \"" + String.Format("{0:yyyy-MM}", DateTime.Now);

                Assert.That(jsonReturnValue.IndexOf(expectedSubString) == 0, Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }

    [TestFixture]
    public class When_getting_serialized_container_information_for_a_container_in_xml_format_and_objects_exist : TestBase
    {
        [Test]
        public void Should_get_serialized_xml_format()
        {
            
            connection.CreateContainer(Constants.CONTAINER_NAME);

            try
            {
                connection.PutStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                XmlDocument xmlReturnValue = connection.GetContainerInformationXml(Constants.CONTAINER_NAME);

                var expectedSubString = "<container name=\"" + Constants.CONTAINER_NAME + "\"><object><name>" + Constants.StorageItemNameJpg + "</name><hash>b44a59383b3123a747d139bd0e71d2df</hash><bytes>105542</bytes><content_type>image/jpeg</content_type><last_modified>" + String.Format("{0:yyyy-MM}", DateTime.Now);

                Assert.That(xmlReturnValue.InnerXml.IndexOf(expectedSubString) > -1, Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(Constants.CONTAINER_NAME, Constants.StorageItemNameJpg);
                connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}