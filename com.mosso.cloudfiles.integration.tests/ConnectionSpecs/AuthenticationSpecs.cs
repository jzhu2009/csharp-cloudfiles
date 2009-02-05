using System;
using com.mosso.cloudfiles.domain;
using com.mosso.cloudfiles.services;
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
            IConnection engine = new Connection(null);
        }

        [Test]
        public void Should_instantiate_engine_without_throwing_exception_when_authentication_passes()
        {

            IConnection engine = new Connection(new UserCredentials(Constants.MOSSO_USERNAME, Constants.MOSSO_API_KEY));
        }
    }
}