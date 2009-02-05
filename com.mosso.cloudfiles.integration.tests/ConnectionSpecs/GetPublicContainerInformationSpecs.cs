using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetPublicContainerInformationSpecs
{
    [TestFixture]
    public class When_retrieve_public_container_information : TestBase
    {
        [Test]
        public void Should_retrieve_public_container_info_when_the_container_exists_and_is_public()
        {
            using (TestHelper testHelper = new TestHelper(connection, true, true))
            {
                Container container = connection.GetPublicContainerInformation(testHelper.ContainerName);
                Assert.That(container.CdnUri, Is.Not.Null);
            }
        }

        [Test]
        [ExpectedException(typeof(ContainerNotFoundException))]
        public void Should_throw_an_exception_when_the_container_does_not_exist()
        {
            using (TestHelper testHelper = new TestHelper(connection, false, false))
            {
                Container container = connection.GetPublicContainerInformation(testHelper.ContainerName);
            }
        }

        [Test]
        public void Should_throw_an_exception_when_the_container_exists_and_is_not_public()
        {
            bool excepted = false;
            using (TestHelper testHelper = new TestHelper(connection, true, false))
            {
                try
                {
                    Container container = connection.GetPublicContainerInformation(testHelper.ContainerName);
                }
                catch
                {
                    excepted = true;
                }
            }
            Assert.That(excepted, Is.True);
        }
    }
}