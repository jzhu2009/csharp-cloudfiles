using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace com.mosso.cloudfs.docs
{
    public class DocBookXmlGenerator
    {
        private readonly string xmlFilePath;
        private readonly XmlDocument xmlDocument;
        private List<string> types;

        public DocBookXmlGenerator(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
            xmlDocument = new XmlDocument();

            if (!File.Exists(xmlFilePath)) throw new FileNotFoundException();

            GetXmlFileContents();
        }

        private void GetXmlFileContents()
        {
            xmlDocument.Load(xmlFilePath);
            types = GetPublicTypes();
        }

        private List<string> GetPublicTypes()
        {
            List<string> publicTypes = new List<string>();

            XmlNodeList memberNodes = xmlDocument.SelectNodes("/doc/members/member[starts-with(@name, 'T:')]");

            if(memberNodes.Count == 0)
                throw new NotSupportedException("No public types available to convert to docbook documentation type");

            foreach (XmlNode node in memberNodes)
            {
                publicTypes.Add(node.Attributes["name"].InnerText.Replace("T:", ""));
            }
            return publicTypes;
        }

        public int TypeCount
        {
            get { return types.Count; }
        }

        public string Generate()
        {
            StringBuilder document = new StringBuilder();
            Sect2 sect2 = new Sect2();
            document.Append(sect2.Open());
            document.Append(sect2.Title());
            document.Append(sect2.Para());

            CreateSect3Section(document, "Classes", GenerateClassDefinitions());
            CreateSect3Section(document, "Objects Returned", "");
            CreateSect3Section(document, "Exception Handling", "");
            CreateExamplesSection(document);


            document.Append(sect2.Close());

            return document.ToString();
        }

        private void CreateExamplesSection(StringBuilder document)
        {
            document.Append(new ExampleGenerator().GenerateExampeles());
        }

        private void CreateSect3Section(StringBuilder document, string title, string content)
        {
            Sect3 sect3 = new Sect3();
            document.Append(sect3.Open());
            document.Append(sect3.Title(title));
            document.Append(sect3.Para(""));
            document.Append(content);
            document.Append(sect3.Close());
        }

        private string GenerateClassDefinitions()
        {
            StringBuilder classDefinitions = new StringBuilder();
            foreach (string typeName in types)
            {
                Sect4 sect4 = new Sect4(xmlDocument);
                classDefinitions.Append(sect4.Open(typeName));
                classDefinitions.Append(sect4.Title(typeName));
                classDefinitions.Append(sect4.Para(typeName));
                classDefinitions.Append(GenerateMethodDefinitions(typeName));
                classDefinitions.Append(sect4.Close(typeName));
            }

            return classDefinitions.ToString();
        }

        private string GenerateMethodDefinitions(string typeName)
        {
            StringBuilder methodDefinitions = new StringBuilder();

            string xpathQuery = "/doc/members/member[starts-with(@name, 'P:" + typeName + "')] | /doc/members/member[starts-with(@name, 'M:" + typeName + "')]";
            XmlNodeList methodsAndProperties = xmlDocument.SelectNodes(xpathQuery);

            foreach (XmlNode node in methodsAndProperties)
            {
                string fullyQualifiedMethodOrPropertyName = node.Attributes["name"].InnerText;

                DocBookType type = fullyQualifiedMethodOrPropertyName.Contains("P:") ? DocBookType.Property : DocBookType.Method;
                string[] splitMethodNameAndArguments = fullyQualifiedMethodOrPropertyName.Split('(');
                string[] splitName = splitMethodNameAndArguments[0].Split('.');
                string name = splitName[splitName.Length - 1];

                if (name == "#ctor") name = "Constructor";

                methodDefinitions.Append(new Sect5().Open(name));
                methodDefinitions.Append(GenerateSect5Title(name));
                methodDefinitions.Append(GenerateSect5Para(splitMethodNameAndArguments[0]));
                if(type == DocBookType.Method)
                {
                    methodDefinitions.Append(GenerateSect5ParameterInformationTable(fullyQualifiedMethodOrPropertyName));
                    methodDefinitions.Append(GenerateSect5ReturnsSection(fullyQualifiedMethodOrPropertyName));
                    methodDefinitions.Append(GenerateSect5ExceptionInformationTable(fullyQualifiedMethodOrPropertyName));
                }
                methodDefinitions.Append(new Sect5().Close(name, type));
            }

            return methodDefinitions.ToString();
        }

        private string GenerateSect5ReturnsSection(string fullyQualifiedMethodOrPropertyName)
        {
            XmlNode node = xmlDocument.SelectSingleNode("//doc/members/member[@name='" + fullyQualifiedMethodOrPropertyName + "']/returns");
            string text = node == null ? "None" : node.InnerText.Replace("\r\n", "").Trim();
            return "<simplesect><title>Returns</title><para>" + text + "</para></simplesect>";
        }

        private string GenerateSect5ExceptionInformationTable(string fullyQualifiedMethodOrPropertyName)
        {
            StringBuilder exceptionInformationTable = new StringBuilder();
            exceptionInformationTable.Append("<simplesect><title>Exceptions</title>");
            
            XmlNodeList nodes = xmlDocument.SelectNodes("//doc/members/member[@name='" + fullyQualifiedMethodOrPropertyName + "']/exception");

            if(nodes.Count == 0)
            {
                exceptionInformationTable.Append("<para>None</para></simplesect>");
                return exceptionInformationTable.ToString();
            }

            exceptionInformationTable.Append("<informaltable><tgroup cols=\"2\"><colspec colname=\"exception\" /><colspec colname=\"description\" /><thead><row><entry><para>Exception Class</para></entry><entry><para>Description</para></entry></row></thead><tbody>");
            AddExceptionsToInformationTable(nodes, exceptionInformationTable);
            exceptionInformationTable.Append("</tbody></tgroup></informaltable></simplesect>");

            return exceptionInformationTable.ToString();
        }

        private void AddExceptionsToInformationTable(XmlNodeList nodes, StringBuilder exceptionInformationTable)
        {
            foreach (XmlNode node in nodes)
            {
                string type = node.Attributes["cref"].InnerText.Replace("T:", "").Trim();
                string description = node.InnerText.Trim();

                exceptionInformationTable.Append("<row><entry><para>" + type + "</para></entry><entry><para>" + description + "</para></entry></row>");
            }
        }

        private string GenerateSect5ParameterInformationTable(string fullyQualifiedMethodNameWithParameterTypes)
        {
            StringBuilder informationTable = new StringBuilder();
            List<Parameter> parameters = new List<Parameter>();

            string[] split = fullyQualifiedMethodNameWithParameterTypes.Split('(');

            if (split.Length < 2) return "";

            string[] types = split[1].Replace("(", "").Replace(")","").Split(',');

            informationTable.Append(GenerateInformationTableHeader());

            XmlNodeList parameterNodes = xmlDocument.SelectNodes("/doc/members/member[@name='"+fullyQualifiedMethodNameWithParameterTypes+"']/param");
            if (parameterNodes.Count == 0) return "";

            string dictionaryType = "";
            for (int i = 0; i < types.Length; i++)
            {
                if(types[i].IndexOf("Dictionary") > 0) 
                {
                    dictionaryType = types[i];
                    continue;
                }

                if (dictionaryType != "")
                {
                    types[i] = dictionaryType + types[i];
                    ShrinkTypesArrayAccordingly(ref types, i);
                    dictionaryType = "";
                    if (i == types.Length) continue;
                }

                Parameter parameter = new Parameter();
                parameter.Name = parameterNodes[i] == null ? "" : parameterNodes[i].Attributes["name"].InnerText;
                parameter.Type = types[i] ?? "";
                parameter.Description = parameterNodes[i] == null ? "" : parameterNodes[i].InnerText;
                parameter.Required = true;
                parameters.Add(parameter);
                informationTable.Append(GenerateInforamtionTableParameterRow(parameter));
            }

            informationTable.Append(GenerateInformationTableFooter());

            return informationTable.ToString();
        }

        private void ShrinkTypesArrayAccordingly(ref string[] types, int index)
        {
            List<string> newTypes = new List<string>();
            for(int i = 0; i < types.Length; i++)
            {
                newTypes.Add(types[i]);
            }

            newTypes.RemoveAt(index-1);
            types = newTypes.ToArray();
        }

        private string GenerateInforamtionTableParameterRow(Parameter parameter)
        {
            return "<row><entry><para><parameter>" + parameter.Name + "</parameter></para></entry>" + 
                "<entry><para>" + parameter.Type + "</para></entry>" + 
                "<entry><para>" + parameter.Description + "</para></entry>" + 
                "<entry><para>" + (parameter.Required ? "YES" : "NO") + "</para></entry></row>";
        }

        private string GenerateInformationTableFooter()
        {
            return "</tbody></tgroup></informaltable>";
        }

        private string GenerateInformationTableHeader()
        {
            return "<informaltable><tgroup cols=\"4\"><colspec colname=\"param\" /><colspec colname=\"type\" /><colspec colname=\"description\" /><colspec colname=\"required\" /><thead><row><entry><para>Parameter</para></entry><entry><para>Type</para></entry><entry><para>Description</para></entry><entry><para>Required</para></entry></row></thead><tbody>";
        }

        private string GenerateSect5Title(string methodOrProperty)
        {
            return "<title>" + methodOrProperty + "</title>";
        }

        private string GenerateSect5Para(string methodOrProperty)
        {
            XmlNode node = xmlDocument.SelectSingleNode("//doc/members/member[starts-with(@name, '" + methodOrProperty + "')]/summary");
            string text = node.InnerText.Replace("\r\n", "").Trim();
            return "<para>" + text + "</para>";
        }

        private string GenerateFooter()
        {
            return "</sect3><sect3><title>Objects Returned</title><para></para></sect3><sect3><title>Exception Handling</title><para></para></sect3><sect3><title>Examples</title><para></para></sect3></sect2><para><xref linkend=\"language_apis\"/></para>";
        }

        private string GenerateHeader()
        {
            return "<sect2 id=\".NET_client_api\"><title>.NET Client API</title><para>A .NET &api; developed in C#, codenamed &alektorphobic; has been created for &productname;.It can be downloaded at [NET_DOWNLOAD_URL]. .NET Framework 3.5, and subsequently Visual Studio2008, is required to utilize the assembly.</para><sect3><title>Classes</title>";
        }
    }

    public enum DocBookType
    {
        Method,
        Property
    }
}