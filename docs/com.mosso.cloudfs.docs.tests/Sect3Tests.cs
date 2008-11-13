using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfs.docs.tests
{
    [TestFixture]
    public class Sect3Tests
    {
        [Test]
        public void should_generate_sect3_classes_section()
        {
            Sect3 sect3 = new Sect3();
            string actual = sect3.Open() + sect3.Title("Classes") + sect3.Para("") + sect3.Close();
            string expected = "<sect3><title>Classes</title><para></para></sect3>";

            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}