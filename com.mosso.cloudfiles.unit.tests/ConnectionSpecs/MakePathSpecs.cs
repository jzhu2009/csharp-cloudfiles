//using NUnit.Framework;
//using NUnit.Framework.SyntaxHelpers;
//using Rhino.Mocks;
//
//namespace com.mosso.cloudfiles.unit.tests.ConnectionSpecs.MakePathSpecs
//{
//    [TestFixture]
//    public class when_making_a_directory_structure
//    {
//        [Test]
//        public void should_create_zero_byte_objects_with_content_type_application_directory()
//        {
//            var connection = MockRepository.GenerateStub<IConnection>();
//
//            connection.MakePath("containername", "/dir1/dir2/dir3/file.txt");
//
//            connection.AssertWasCalled(conn => conn.PutStorageItem("containername", null, "directory"), options => options.IgnoreArguments());
//
////            var dir1 = connection.GetStorageItem("containername", "dir1");
////            Assert.That(dir1.ContentType, Is.EqualTo("application/octet-stream"));
////            Assert.That(dir1.FileLength, Is.EqualTo(0));
////
////            var dir2 = connection.GetStorageItem("containername", "dir1/dir2");
////            Assert.That(dir2.ContentType, Is.EqualTo("application/octet-stream"));
////            Assert.That(dir2.FileLength, Is.EqualTo(0));
////
////            var dir3 = connection.GetStorageItem("containername", "dir1/dir2/dir3");
////            Assert.That(dir3.ContentType, Is.EqualTo("application/octet-stream"));
////            Assert.That(dir3.FileLength, Is.EqualTo(0));
//         
//        }
//
////        [Test]
////        public void should_be_able_to_query_a_container_and_get_directory_like_results_with_the_path_query()
////        {
////            try
////            {
////                connection.CreateContainer(Constants.CONTAINER_NAME);
////                connection.MakePath(Constants.CONTAINER_NAME, "/dir1/dir2/dir3/file.txt");
////
////                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/" + Constants.StorageItemName);
////                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/" + Constants.StorageItemName);
////                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/" + Constants.StorageItemNameGif);
////                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/dir3/" + Constants.StorageItemName);
////                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/dir3/" + Constants.StorageItemNameGif);
////                connection.PutStorageItem(Constants.CONTAINER_NAME, new MemoryStream(new byte[0]), "/dir1/dir2/dir3/" + Constants.StorageItemNameJpg);
////
////                var items = connection.GetContainerItemList(Constants.CONTAINER_NAME, new Dictionary<GetItemListParameters, string> { { GetItemListParameters.Path, "dir1" } });
////                Assert.That(items.Count, Is.EqualTo(2));
////            }
////            finally
////            {
////                var storageItems = connection.GetContainerItemList(Constants.CONTAINER_NAME);
////
////                if (storageItems.Contains("dir1"))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir1");
////                if (storageItems.Contains("dir1/" + Constants.StorageItemName))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir1/" + Constants.StorageItemName);
////                if (storageItems.Contains("dir2"))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir2");
////                if (storageItems.Contains("dir2/" + Constants.StorageItemName))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir1/dir2/" + Constants.StorageItemName);
////                if (storageItems.Contains("dir2/" + Constants.StorageItemNameGif))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir1/dir2/" + Constants.StorageItemNameGif);
////                if (storageItems.Contains("dir3"))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir3");
////                if (storageItems.Contains("dir3/" + Constants.StorageItemName))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir1/dir2/dir3/" + Constants.StorageItemName);
////                if (storageItems.Contains("dir3/" + Constants.StorageItemNameGif))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir1/dir2/dir3/" + Constants.StorageItemNameGif);
////                if (storageItems.Contains("dir3/" + Constants.StorageItemNameJpg))
////                    connection.DeleteStorageItem(Constants.CONTAINER_NAME, "dir1/dir2/dir3/" + Constants.StorageItemNameJpg);
////
////                if (connection.GetContainerInformation(Constants.CONTAINER_NAME) != null)
////                    connection.DeleteContainer(Constants.CONTAINER_NAME);
////            }
////        }
//    }
//}