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
            string containerName = Guid.NewGuid().ToString();
            try
            {
                connection.CreateContainer(containerName);
                Container containerInformation = connection.GetContainerInformation(containerName);

                Assert.That(containerInformation.Name, Is.EqualTo(containerName));
                Assert.That(containerInformation.ByteCount, Is.EqualTo(0));
                Assert.That(containerInformation.ObjectCount, Is.EqualTo(0));
            }
            finally
            {
                connection.DeleteContainer(containerName);
            }
        }

        [Test]
        [ExpectedException(typeof (ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.GetContainerInformation(containerName);
        }
    }

    [TestFixture]
    public class When_getting_serialized_container_information_for_a_container_in_json_format_and_objects_exist : TestBase
    {
        [Test]
        public void Should_get_serialized_json_format()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            try
            {
                connection.PutStorageItem(containerName, Constants.StorageItemNameJpg);
                string jsonReturnValue = connection.GetContainerInformationJson(containerName);
                var expectedSubString = "[{\"name\": \"" + Constants.StorageItemNameJpg + "\", \"hash\": \"b44a59383b3123a747d139bd0e71d2df\", \"bytes\": 105542, \"content_type\": \"image\\u002fjpeg\", \"last_modified\": \"" + String.Format("{0:yyyy-MM}", DateTime.Now);

                Assert.That(jsonReturnValue.IndexOf(expectedSubString) == 0, Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemNameJpg);
                connection.DeleteContainer(containerName);
            }
        }
    }

    [TestFixture]
    public class When_getting_serialized_container_information_for_a_container_in_xml_format_and_objects_exist : TestBase
    {
        [Test]
        public void Should_get_serialized_xml_format()
        {
            string containerName = Guid.NewGuid().ToString();
            connection.CreateContainer(containerName);

            try
            {
                connection.PutStorageItem(containerName, Constants.StorageItemNameJpg);
                XmlDocument xmlReturnValue = connection.GetContainerInformationXml(containerName);

                var expectedSubString = "<container name=\"" + containerName + "\"><object><name>" + Constants.StorageItemNameJpg + "</name><hash>b44a59383b3123a747d139bd0e71d2df</hash><bytes>105542</bytes><content_type>image/jpeg</content_type><last_modified>" + String.Format("{0:yyyy-MM}", DateTime.Now);

                Assert.That(xmlReturnValue.InnerXml.IndexOf(expectedSubString) > -1, Is.True);
            }
            finally
            {
                connection.DeleteStorageItem(containerName, Constants.StorageItemNameJpg);
                connection.DeleteContainer(containerName);
            }
        }
    }
}