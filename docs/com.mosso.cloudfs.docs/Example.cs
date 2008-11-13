namespace com.mosso.cloudfs.docs
{
    public class Example
    {
        public string Content { get; set; }

        public override string ToString()
        {
            return "<para>\n" + Content + "</para>\n";
        }
    }
}