using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfs.docs.tests
{
    [TestFixture]
    public class ProgramListingTests
    {
        [Test]
        public void Should_generate_a_program_listing_wrapped_in_necessary_CDATA_tag()
        {
            ProgramListing programListing = new ProgramListing("x = new x();");
            string expected = "<![CDATA[\nx = new x();\n]]>";
            Assert.That(programListing.Code, Is.EqualTo(expected));
        }

        [Test]
        public void Should_generate_a_complete_program_listing_when_using_tostring()
        {
            ProgramListing programListing = new ProgramListing("x = new x();");
            string expected = "<programlisting>\n<![CDATA[\nx = new x();\n]]>\n</programlisting>\n";
            Assert.That(programListing.ToString(), Is.EqualTo(expected));
        }
    }
}