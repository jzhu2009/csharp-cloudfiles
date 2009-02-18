namespace com.mosso.cloudfiles
{
    public class StringHelper
    {
        public static string Capitalize(string wordToCapitalize)
        {
            return char.ToUpper(wordToCapitalize[0]) + wordToCapitalize.Substring(1);
        }

        public static string Capitalize(bool booleanValue)
        {
            
            return booleanValue ? "True" : "False";
        }
    }
}