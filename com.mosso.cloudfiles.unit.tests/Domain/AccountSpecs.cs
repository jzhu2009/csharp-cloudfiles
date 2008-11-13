using System;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.domain
{
    [TestFixture]
    public class When_checking_account
    {
        private AccountInformation accountInformation;

        [SetUp]
        public void SetUp()
        {
            accountInformation = new AccountInformation("3", "1024");
        }

        [Test]
        public void should_return_the_number_of_containers()
        {
            Assert.That(accountInformation.ContainerCount, Is.EqualTo(3));
        }

        [Test]
        public void should_return_the_bytes_used()
        {
            Assert.That(accountInformation.BytesUsed, Is.EqualTo(1024));
        }
    }

    [TestFixture]
    public class When_creating_account_with_invalid_arguments
    {
        private AccountInformation accountInformation;

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void should_throw_an_exception_when_the_container_name_is_empty()
        {
            accountInformation = new AccountInformation("", "");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void should_throw_an_exception_when_the_container_name_is_null()
        {
            accountInformation = new AccountInformation(null, "");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void should_throw_an_exception_when_the_bytes_used_is_empty()
        {
            accountInformation = new AccountInformation("3", "");
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void should_throw_an_exception_when_the_bytes_used_is_null()
        {
            accountInformation = new AccountInformation("3", null);
        }

        [Test]
        [ExpectedException(typeof (FormatException))]
        public void should_throw_an_exception_when_the_container_name_is_invalid()
        {
            accountInformation = new AccountInformation("hello", "1231");
        }

        [Test]
        [ExpectedException(typeof (FormatException))]
        public void should_throw_an_exception_when_the_bytes_used_is_invalid()
        {
            accountInformation = new AccountInformation("1231", "hello");
        }
    }
}