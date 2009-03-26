using com.mosso.cloudfiles.utils;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.ContainerNameValidatorSpecs
{
    [TestFixture]
    public class When_a_container_name_has_no_invalid_characters
    {
        [Test]
        public void should_be_valid()
        {
            var containerName = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()-+={}[]|\'\":;,.<>~`";
            Assert.That(ContainerNameValidator.Validate(containerName), Is.True);

        }
    }

    [TestFixture]
    public class When_a_container_name_has_a_slash
    {
        [Test]
        public void should_be_invalid()
        {
            var containerName = "containerName/withSlash";
            Assert.That(ContainerNameValidator.Validate(containerName), Is.False);

        }
    }

    [TestFixture]
    public class When_a_container_name_has_a_question_mark
    {
        [Test]
        public void should_be_invalid()
        {
            var containerName = "containerName?withQuestionMark";
            Assert.That(ContainerNameValidator.Validate(containerName), Is.False);

        }
    }

    [TestFixture]
    public class When_a_container_name_is_valid_length
    {
        [Test]
        public void should_be_valid()
        {
            var containerName = new string('a', ContainerNameValidator.MAX_CONTAINER_NAME_LENGTH);
            Assert.That(ContainerNameValidator.Validate(containerName), Is.True);

        }
    }

    [TestFixture]
    public class When_a_container_name_is_invalid_length
    {
        [Test]
        public void should_be_invalid()
        {
            var containerName = new string('a', ContainerNameValidator.MAX_CONTAINER_NAME_LENGTH + 1);
            Assert.That(ContainerNameValidator.Validate(containerName), Is.False);

        }
    }
}