using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.services;
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
}