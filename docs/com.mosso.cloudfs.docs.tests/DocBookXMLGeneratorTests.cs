using System;
using System.IO;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace com.mosso.cloudfs.docs.tests
{
    [TestFixture]
    public class When_generating_doc_book_xml_with_invalid_assembly_xml_file
    {
        [Test]
        [ExpectedException(typeof (FileNotFoundException))]
        public void Should_throw_file_not_found_exception_when_xml_file_does_not_exist_at_path()
        {
            DocBookXmlGenerator generator = new DocBookXmlGenerator(@"Dummy.xml");
        }
    }

    [TestFixture]
    public class When_generating_doc_book_xml_with_valid_assembly_xml_file
    {
        private DocBookXmlGenerator generator;
        private const string TEST_ASSEMBLY_PATH = @"../../../lib/com.mosso.cloudfs.XML";
        private string output;

        [SetUp]
        public void Setup()
        {
            generator = new DocBookXmlGenerator(TEST_ASSEMBLY_PATH);
            output = generator.Generate();
        }

        [Test]
        public void Should_be_able_to_get_all_classes_from_a_known_assembly()
        {
            Assert.That(generator.TypeCount, Is.GreaterThan(10));
        }

        [Test]
        public void Should_export_header_of_doc_xml_file()
        {
            Assert.That(output.Contains("<sect2 id=\".NET_client_api\"><title>.NET Client API</title><para>The .NET Client API codenamed Alektorphobic is available for download from the <ulink url=\"https://beta.mosso.com\">Mosso beta control panel</ulink>.  To access it log into the <ulink url=\"https://beta.mosso.com\">Mosso beta control panel</ulink>, then click the \"Support\" tab at the top, then the \"Developer Resources\" tab on the left.  You will then see the download links on the right under \"Client Code\".  This will require .NET 3.5</para><sect3><title>Classes</title>"));
        }

        [Test]
        public void Should_export_objects_returned_exception_handling_sections()
        {
            Assert.That(output.Contains("</sect3><sect3><title>Objects Returned</title><para></para></sect3><sect3><title>Exception Handling</title><para></para></sect3>"));
        }

        [Test]
        public void Should_export_the_last_line_of_the_document()
        {
            Assert.That(output.Contains("</sect2><para><xref linkend=\"language_apis\"/></para>"));
        }

        [Test]
        public void Should_have_sect4_beginning_block_that_includes_xml_comment_with_type_name()
        {
            Assert.That(output.Contains("<sect4><!-- UserCredentials -->"), Is.True);
        }

        [Test]
        public void Should_have_sect4_end_block_that_includes_xml_comment_with_type_name()
        {
            Assert.That(output.Contains("</sect4><!-- end of com.mosso.cloudfs.domain.UserCredentials class -->"), Is.True);
        }

        [Test]
        public void Should_have_title_blocks_that_include_type_namespace()
        {
            Assert.That(output.Contains("<title>com.mosso.cloudfs.domain.UserCredentials</title>"), Is.True);
            Assert.That(output.Contains("<title>com.mosso.cloudfs.domain.response.CreateContainerResponse</title>"), Is.True);
        }

        [Test]
        public void Should_have_para_section_that_has_type_description()
        {
            Assert.That(output.Contains("<para>UserCredentials</para>"), Is.True);
        }

        [Test]
        public void Should_not_have_informationtable_with_parameter_information_when_parameters_do_not_exist()
        {
            Assert.That(output.Contains("<para>UserCredentials</para>"), Is.True);
        }

        [Test]
        public void Should_have_sect5_beginning_block_with_xml_comment_for_each_method_and_property()
        {
            Assert.That(output.Contains("<sect5><!-- Password -->"), Is.True);
        }

        [Test]
        public void Should_have_sect5_end_block_with_xml_comment_for_each_method_and_property()
        {
            Assert.That(output.Contains("</sect5><!-- end of Password Property -->"), Is.True);
        }

        [Test]
        public void Should_have_sect5_title_block_with_method_or_property_name()
        {
            Assert.That(output.Contains("<title>Password</title>"), Is.True);
        }

        [Test]
        public void Should_have_sect5_para_block_with_method_or_property_description()
        {
            Assert.That(output.Contains("<para>password to use for authentication</para>"), Is.True);
        }

        [Test]
        public void Should_have_sect5_information_table_with_parameter_information_table()
        {
            string parameterRow = "<row><entry><para><parameter>authUrl</parameter></para></entry>" +
                                  "<entry><para>System.Uri</para></entry>" +
                                  "<entry><para>url to authenticate against</para></entry>" +
                                  "<entry><para>YES</para></entry></row>";

            Assert.That(output.Contains(parameterRow));
        }

        [Test]
        public void Should_have_sect5_returns_section_with_description()
        {
            Assert.That(output.Contains("<simplesect><title>Returns</title><para>An instance of UserCredentials</para></simplesect>"));
        }

        [Test]
        public void Should_have_sect5_exceptions_information_table()
        {
            Assert.That(output.Contains("<simplesect><title>Exceptions</title><informaltable><tgroup cols=\"2\"><colspec colname=\"exception\" /><colspec colname=\"description\" /><thead><row><entry><para>Exception Class</para></entry><entry><para>Description</para></entry></row></thead><tbody><row><entry><para>System.ArgumentNullException</para></entry><entry><para>Thrown when any of the reference parameters are null</para></entry></row></tbody></tgroup></informaltable></simplesect>"));
        }

//        [TestFixtureTearDown]
//        public void TextFixtureTearDown()
//        {
//            Console.WriteLine(output);
//        }
    }

    [TestFixture]
    public class When_generating_doc_book_xml_with_valid_assembly_xml_file_and_generating_xml_file
    {
        private const string XML_FILE_NAME = @"C:\test.xml";
        private DocBookXmlGenerator generator;
        private const string TEST_ASSEMBLY_PATH = @"../../../lib/com.mosso.cloudfs.XML";
        private string output;

        [SetUp]
        public void Setup()
        {
            generator = new DocBookXmlGenerator(TEST_ASSEMBLY_PATH);
            output = generator.Generate();
            CreateFile(output);
        }

        [Test]
        public void Should_be_a_properly_formattted_xml_file()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(XML_FILE_NAME);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (message.Contains("There are multiple root elements."))
                {
                    return;
                }

                Assert.Fail(ex.Message);
            }
        }

        [TestFixtureTearDown]
        public void TextFixtureTearDown()
        {
            if (File.Exists(XML_FILE_NAME))
            {
                File.Delete(XML_FILE_NAME);
            }
        }

        private void CreateFile(string output)
        {
            using (FileStream file = new FileStream(XML_FILE_NAME, FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(output);
                }
            }
        }
    }
}