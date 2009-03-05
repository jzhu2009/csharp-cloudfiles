using System;

namespace com.mosso.cloudfiles
{
    public static class StringHelper
    {
        public static string Capitalize(this String wordToCapitalize)
        {
            return char.ToUpper(wordToCapitalize[0]) + wordToCapitalize.Substring(1);
        }

        public static string Capitalize(this bool booleanValue)
        {
            
            return booleanValue ? "True" : "False";
        }
    }
}