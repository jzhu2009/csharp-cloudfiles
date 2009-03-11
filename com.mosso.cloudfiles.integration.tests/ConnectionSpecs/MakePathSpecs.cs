using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.MakePathSpecs
{
    [TestFixture]
    public class when_making_a_directory_structure : TestBase
    {
        [Test]
        public void should_create_zero_byte_objects_with_content_type_application_directory()
        {
            try
            {
                connection.CreateContainer(Constants.CONTAINER_NAME);
                connection.MakePath(Constants.CONTAINER_NAME, "/dir1/dir2/dir3/file.txt");

                var dir1 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir1");
                Assert.That(dir1.ContentType, Is.EqualTo("application/octet-stream"));
                Assert.That(dir1.FileLength, Is.EqualTo(0));

                var dir2 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir2");
                Assert.That(dir2.ContentType, Is.EqualTo("application/octet-stream"));
                Assert.That(dir2.FileLength, Is.EqualTo(0));

                var dir3 = connection.GetStorageItem(Constants.CONTAINER_NAME, "dir3");
                Assert.That(dir3.ContentType, Is.EqualTo("application/octet-stream"));
                Assert.That(dir3.FileLength, Is.EqualTo(0));
            }
            finally
            {
                var storageItems = connection.GetContainerItemList(Constants.CONTAINER_NAME);
                if(storageItems.Contains("dir1"))
                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir1");
                if (storageItems.Contains("dir2"))
                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir2");
                if (storageItems.Contains("dir3"))
                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir3");

                if (connection.GetContainerInformation(Constants.CONTAINER_NAME) != null)
                    connection.DeleteContainer(Constants.CONTAINER_NAME);
            }
        }
    }
}