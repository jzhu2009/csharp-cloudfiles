using System.Text;

namespace com.mosso.cloudfs.docs
{
    public class ProgramListing
    {
        private string [] code;

        public ProgramListing(params string [] code)
        {
            this.code = code;
           
        }

        public string Code
        {
            get
            {

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append( "<![CDATA[\n");
                foreach (string s in code)
                {
                    stringBuilder.Append(s+"\n");
                }
                stringBuilder.Append("]]>");
                return stringBuilder.ToString();
            }  
        }

        public override string ToString()
        {
            return "<programlisting>\n" + Code + "\n</programlisting>\n";
        }
    }
}