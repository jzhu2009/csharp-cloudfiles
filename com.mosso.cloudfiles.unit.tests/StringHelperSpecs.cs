using com.mosso.cloudfiles.utils;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfiles.unit.tests.StringHelperSpecs
{
    [TestFixture]
    public class when_capitalizing_a_string
    {
        [Test]
        public void should_capitalize_the_first_letter_and_lower_case_the_remainder_of_the_string()
        {
            var howdy = "howdy";
            Assert.That(howdy.Capitalize(), Is.EqualTo("Howdy"));
        }
    }

    [TestFixture]
    public class when_converting_true_to_string_for_x_cdn_enabled_header
    {
        [Test]
        public void should_result_in_True()
        {
            Assert.That(true.Capitalize(), Is.EqualTo("True"));
        }
    }

    [TestFixture]
    public class when_converting_false_to_string_for_x_cdn_enabled_header
    {
        [Test]
        public void should_result_in_False()
        {
            Assert.That(false.Capitalize(), Is.EqualTo("False"));
        }
    }

    [TestFixture]
    public class when_encoding_a_container_name_or_storage_item_for_a_url
    {
        [Test]
        public void should_replace_plus_signs_with_percent_two_b()
        {
            Assert.That("this+is+my+example+test".Encode(), Is.EqualTo("this%2bis%2bmy%2bexample%2btest"));
        }
    }

    [TestFixture]
    public class when_stripping_a_slash_prefix_from_a_string
    {
        [Test]
        public void should_remove_the_slash_prefix()
        {
            Assert.That("/dir1/dir2/dir3/file.txt".StripSlashPrefix(), Is.EqualTo("dir1/dir2/dir3/file.txt"));
            Assert.That("dira/dirb/dirc/file.txt".StripSlashPrefix(), Is.EqualTo("dira/dirb/dirc/file.txt"));
            Assert.That("/abcdefg".StripSlashPrefix(), Is.EqualTo("abcdefg"));
        }
    }
}