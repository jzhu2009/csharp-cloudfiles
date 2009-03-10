using System;
using System.Web;

namespace com.mosso.cloudfiles
{
    public static class StringHelper
    {
        public static string Capitalize(this String wordToCapitalize)
        {
            if (String.IsNullOrEmpty(wordToCapitalize))
                throw new ArgumentNullException();

            return char.ToUpper(wordToCapitalize[0]) + wordToCapitalize.Substring(1);
        }

        public static string Capitalize(this bool booleanValue)
        {
            
            return booleanValue ? "True" : "False";
        }

        public static string Encode(this string stringToEncode)
        {
            if (String.IsNullOrEmpty(stringToEncode))  
                throw new ArgumentNullException();

            return HttpUtility.UrlEncode(stringToEncode).Replace("+", "%20");
        }
    }
}