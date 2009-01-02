using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.services;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace com.mosso.cloudfiles.unit.tests.Services.ConnectionSpecs
{
    [TestFixture]
    public class When_instantiating_a_connection_object
    {
        [Test]
        public void Should_instantiate_engine_without_throwing_exception_when_authentication_passes()
        {
            UserCredentials userCredentials = new UserCredentials(new Uri(Constants.AUTH_URL), Constants.CREDENTIALS_USER_NAME, Constants.CREDENTIALS_PASSWORD, Constants.CREDENTIALS_CLOUD_VERSION, Constants.CREDENTIALS_ACCOUNT_NAME);

            MockConnection engine = new MockConnection(userCredentials);

            Assert.That(engine.AuthenticationSuccessful, Is.True);
        }
    }

    internal class MockConnection : Connection
    {
        private bool authenticationSuccessful;

        public MockConnection(UserCredentials userCredentials) : base(userCredentials)
        {
        }

        public bool AuthenticationSuccessful
        {
            get { return authenticationSuccessful; }
        }

        protected override void VerifyAuthentication()
        {
            authenticationSuccessful = true;
        }
    }

//    [TestFixture]
//    public class When_getting_list_of_containers
//    {
//        [Test]
//        public void should_return_list_of_containers_when_content_is_present()
//        {
//            IConnection connection = MockRepository.GenerateMock<IConnection>();
//
//        }
//    }

    [TestFixture]
    public class When_formatting_account_information_into_json
    {
        [Test]
        public void should_get_container_names()
        {
//            var formatter = MockRepository.GenerateStub<IFormatter>();
//            var userCredentials = new UserCredentials("test_username", "test_api_key");
//
//            System.Collections.Generic.List<Container> containers = new System.Collections.Generic.List<Container>()
//                                             {
//                                                 new Container("container1") {ByteCount = 50, ObjectCount = 1},
//                                                 new Container("container2") {ByteCount = 100, ObjectCount = 2},
//                                                 new Container("container3") {ByteCount = 150, ObjectCount = 3},
//                                                 new Container("container4") {ByteCount = 200, ObjectCount = 4},
//                                                 new Container("container5") {ByteCount = 0, ObjectCount = 0},
//                                             };
//
//
//            string expected = "[\n{\"name\":\"container1\",\"count\":1,\"size\":50,{\"name\":\"container2\",\"count\":2,\"size\":100},{\"name\":\"container3\",\"count\":3,\"size\":150},{\"name\":\"container4\",\"count\":4,\"size\":200},{\"name\":\"container5\",\"count\":0,\"size\":0}]";
//
//            Connection connection = new Connection(userCredentials);
//            connection.GetAccountInformation(formatter);
//
//            formatter.Expect(x => x.Format(containers)).Return(expected);
//
//            formatter.VerifyAllExpectations();
        }
    }
}