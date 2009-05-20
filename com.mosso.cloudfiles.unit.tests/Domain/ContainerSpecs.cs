using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.domain.StorageObjectSpecs.ContainerSpecs
{
    [TestFixture]
    public class When_creating_a_container
    {
        private Container container;

        [SetUp]
        public void SetUp()
        {
            container = new Container(Constants.REMOTE_CONTAINER_NAME);
        }

        [Test]
        public void Should_have_container_name()
        {
            Assert.That(container.Name, Is.EqualTo(Constants.REMOTE_CONTAINER_NAME));
        }

        [Test]
        public void Should_have_byte_count_when_byte_count_is_set()
        {
            Assert.That(container.ByteCount, Is.EqualTo(0));
            container.ByteCount = Constants.CONTAINER_BYTES_COUNT;
            Assert.AreEqual(container.ByteCount, Constants.CONTAINER_BYTES_COUNT);
        }

        [Test]
        public void Should_have_object_count_when_object_count_is_set()
        {
            container.ObjectCount = Constants.CONTAINER_OBJECT_COUNT;
            Assert.AreEqual(container.ObjectCount, Constants.CONTAINER_OBJECT_COUNT);
        }
    }
}