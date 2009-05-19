using System;
using System.Net;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs
{
    [TestFixture]
    public class When_passing_authentication_to_the_connection
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Should_throw_argument_null_exception_with_null_authentication()
        {
            new Connection(null);
        }

        [Test]
        public void Should_instantiate_engine_without_throwing_exception_when_authentication_passes()
        {

            new Connection(new UserCredentials(Credentials.USERNAME, Credentials.API_KEY));
        }

        [Test]
        [ExpectedException(typeof(WebException), ExpectedMessage = "The remote server returned an error: (401) Unauthorized.")]
        public void Should_throw_not_authorized_exception_when_credentials_are_invalid()
        {
            new Connection(new UserCredentials("invalid_username", "invalid_api_key"));
        }
    }
}