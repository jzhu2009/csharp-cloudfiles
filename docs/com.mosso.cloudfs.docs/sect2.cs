using System.Text;

namespace com.mosso.cloudfs.docs
{
    public class Sect2
    {
        public string Open()
        {
            return "<sect2 id=\".NET_client_api\">";
        }

        public string Title()
        {
            return "<title>.NET Client API</title>";
        }

        public string Para()
        {
            StringBuilder output = new StringBuilder();
            output.Append("<para>");
            output.Append("The .NET Client API codenamed Alektorphobic is available ");
            output.Append("for download from the ");
            output.Append("<ulink url=\"https://beta.mosso.com\">Mosso beta control panel</ulink>.  ");
            output.Append("To access it log into the ");
            output.Append("<ulink url=\"https://beta.mosso.com\">Mosso beta control panel</ulink>, ");
            output.Append("then click the \"Support\" tab at the top, then the \"Developer Resources\" ");
            output.Append("tab on the left.  You will then see the download links on the right under ");
            output.Append("\"Client Code\".  This will require .NET 3.5");
            output.Append("</para>");
            return output.ToString();
        }

        public string Close()
        {
            return "</sect2><para><xref linkend=\"language_apis\"/></para>";
        }
    }
}