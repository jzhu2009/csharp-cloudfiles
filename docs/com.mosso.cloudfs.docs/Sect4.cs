using System.Xml;

namespace com.mosso.cloudfs.docs
{
    public class Sect4
    {
        private readonly XmlDocument doc;

        public Sect4(XmlDocument doc)
        {
            this.doc = doc;
        }

        public string Open(string typeName)
        {
            string[] splitTypeName = typeName.Split('.');
            return "<sect4><!-- " + splitTypeName[splitTypeName.Length - 1] + " -->";
        }

        public string Title(string typeName)
        {
            return "<title>" + typeName + "</title>";
        }

        public string Para(string typeName)
        {
            XmlNode node = doc.SelectSingleNode("//doc/members/member[@name='T:" + typeName + "']/summary");
            string text = node.InnerText.Replace("\r\n", "").Trim();
            return "<para>" + text + "</para>";
        }

        public string Close(string typeName)
        {
            return "</sect4><!-- end of " + typeName + " class -->";
        }
    }
}