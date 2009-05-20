using System.IO;
using com.mosso.cloudfiles.domain.request;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;

namespace com.mosso.cloudfiles.unit.tests.Domain.request.PutStorageItemSpecs
{
    [TestFixture]
    public class when_putting_a_storage_item_via_local_file_path_and_the_local_file_does_not_exist
    {
        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void should_throw_file_not_found_exception()
        {
            new PutStorageItem("a", "a", "a", "a", "a");
        }

    }

    [TestFixture]
    public class when_putting_a_storage_item_via_local_file_path_and_the_container_name_exceeds_the_maximum_length
    {
        [Test]
        [ExpectedException(typeof(ContainerNameException))]
        public void should_throw_container_name_exception()
        {
            new PutStorageItem("a", "a", new string('a', Constants.MAX_CONTAINER_NAME_LENGTH + 1), "a", "a");
        }

    }

    [TestFixture]
    public class when_putting_a_storage_item_via_stream_and_the_container_name_exceeds_the_maximum_length
    {
        [Test]
        [ExpectedException(typeof(ContainerNameException))]
        public void should_throw_container_name_exception()
        {
            var s = new MemoryStream(new byte[0]);
            new PutStorageItem("a", "a", new string('a', Constants.MAX_CONTAINER_NAME_LENGTH + 1), "a", s);
        }
    }

    [TestFixture]
    public class when_putting_a_storage_item_via_local_file_path_and_the_storage_item_name_exceeds_the_maximum_length
    {
        [Test]
        [ExpectedException(typeof(StorageItemNameException))]
        public void should_throw_container_name_exception()
        {
            new PutStorageItem("a", "a", "a", new string('a', Constants.MAX_OBJECT_NAME_LENGTH + 1), "a");
        }

    }

    [TestFixture]
    public class when_putting_a_storage_item_via_stream_and_the_storage_item_name_exceeds_the_maximum_length
    {
        [Test]
        [ExpectedException(typeof(StorageItemNameException))]
        public void should_throw_container_name_exception()
        {
            var s = new MemoryStream(new byte[0]);
            new PutStorageItem("a", "a", "a", new string('a', Constants.MAX_OBJECT_NAME_LENGTH + 1), s);
        }
    }

    
}