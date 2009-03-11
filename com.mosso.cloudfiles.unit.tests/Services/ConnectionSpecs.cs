using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.Services.ConnectionSpecs
{
    [TestFixture]
    public class When_instantiating_a_connection_object
    {
        [Test]
        public void Should_instantiate_engine_without_throwing_exception_when_authentication_passes()
        {
            UserCredentials userCredentials = new UserCredentials(new Uri(Constants.AUTH_URL), Constants.CREDENTIALS_USER_NAME, Constants.CREDENTIALS_PASSWORD, Constants.CREDENTIALS_CLOUD_VERSION, Constants.CREDENTIALS_ACCOUNT_NAME);

            MockConnection conection = new MockConnection(userCredentials);

            Assert.That(conection.AuthenticationSuccessful, Is.True);
        }
    }

    [TestFixture]
    public class when_querying_a_list_of_containers_with_a_path_query_item_and_path_objects_enforced
    {
        [Test]
        public void should_create_0_byte_objects_of_content_type_application_directory_at_each_of_the_directories()
        {
            UserCredentials userCredentials = new UserCredentials(new Uri(Constants.AUTH_URL), Constants.CREDENTIALS_USER_NAME, Constants.CREDENTIALS_PASSWORD, Constants.CREDENTIALS_CLOUD_VERSION, Constants.CREDENTIALS_ACCOUNT_NAME);

            MockConnection connection = new MockConnection(userCredentials);

            var @objects = connection.GetContainerItemList("containername", new Dictionary<GetItemListParameters, string> {{GetItemListParameters.Path, "/dir/subdir/testfile.txt"}});


        }
    }

    internal class MockConnection : Connection
    {
        public MockConnection(UserCredentials userCredentials) : base(userCredentials){}

        public bool AuthenticationSuccessful { get; private set; }

        protected override void VerifyAuthentication()
        {
            AuthenticationSuccessful = true;
        }
    }
}