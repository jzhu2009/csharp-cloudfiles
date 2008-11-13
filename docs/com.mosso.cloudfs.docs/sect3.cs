namespace com.mosso.cloudfs.docs
{
    public class Sect3
    {
        public string Open()
        {
            return "<sect3>";
        }

        public string Title(string title)
        {
            return "<title>" + title + "</title>";
        }

        public string Para(string para)
        {
            return "<para>" + para + "</para>";
        }

        public string Close()
        {
            return "</sect3>";
        }
    }
}