using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfs.docs.tests
{
    [TestFixture]
    public class Sect2Tests
    {
        [Test]
        public void Should_generate_open_title_para_and_closing_correctly()
        {
            Sect2 sect2 = new Sect2();
            string actual = sect2.Open() + sect2.Title() + sect2.Para() + sect2.Close();
            string expected = "<sect2 id=\".NET_client_api\"><title>.NET Client API</title><para>The .NET Client API codenamed Alektorphobic is available for download from the <ulink url=\"https://beta.mosso.com\">Mosso beta control panel</ulink>.  To access it log into the <ulink url=\"https://beta.mosso.com\">Mosso beta control panel</ulink>, then click the \"Support\" tab at the top, then the \"Developer Resources\" tab on the left.  You will then see the download links on the right under \"Client Code\".  This will require .NET 3.5</para></sect2><para><xref linkend=\"language_apis\"/></para>";

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}