using System;
using System.Collections.Generic;
using com.mosso.cloudfiles.exceptions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs.GetContainersSpecs
{
    [TestFixture]
    public class When_retrieving_a_list_of_containers_with_connection : TestBase
    {
        [Test]
        public void Should_return_a_list_of_containers()
        {
            
            using (new tests.TestHelper(storageToken, storageUrl))
            {
                List<string> containerList = connection.GetContainers();
                Assert.That(containerList.Count, Is.GreaterThan(0));
            }
        }
    }

    [TestFixture]
    public class When_retrieving_a_list_of_containers_with_connection_and_the_account_has_no_containers : TestBase
    {
        [ExpectedException(typeof (NoContainersFoundException))]
        public void Should_throw_no_containers_found_exception()
        {
            connection.GetContainers();
        }
    }
}