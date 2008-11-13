namespace com.mosso.cloudfs.docs
{
    public class Sect5
    {
        public string Open(string methodOrProperty)
        {
            return "<sect5><!-- " + methodOrProperty + " -->";
        }

        public string Close(string methodOrProperty, DocBookType type)
        {
            return "</sect5><!-- end of " + methodOrProperty + " " + type + " -->";
        }
    }
}