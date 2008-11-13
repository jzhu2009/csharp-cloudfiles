using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ObfuscateConstants
{
    public class Obfuscator
    {
        public void Obfuscate(string filepath, string outputfilename, bool showOutput)
        {
            string file = String.Empty;
            using (StreamReader reader = File.OpenText(filepath))
            {
                Regex expression1 = new Regex("public const string Account = \".*\"");
                Regex expression2 = new Regex("public const string UserName = \".*\"");
                Regex expression3 = new Regex("public const string Password = \".*\"");
                Regex expression4 = new Regex("public const string AuthUrl = \".*\"");
                Regex expression5 = new Regex("public const string WrongPassword = \".*\"");
                Regex expression6 = new Regex("public const string ProxyUserName = \".*\"");
                Regex expression7 = new Regex("public const string ProxyPassword = \".*\"");
                Regex expression8 = new Regex("public const string ProxyAddress = \".*\"");
                Regex expression9 = new Regex("public const string ProxyDomain = \".*\"");
                Regex expression10 = new Regex("public const string AccountStaging2 = \".*\"");
                Regex expression11 = new Regex("public const string UserNameStaging2 = \".*\"");
                Regex expression12 = new Regex("public const string PasswordStaging2 = \".*\"");
                Regex expression13 = new Regex("public const string MOSSO_ACCOUNT = \".*\"");
                Regex expression14 = new Regex("public const string MOSSO_PASSWORD = \".*\"");
                Regex expression15 = new Regex("public const string MOSS_AUTH_URL = \".*\"");
                Regex expression16 = new Regex("public const string HTTPSAccount = \".*\"");
                Regex expression17 = new Regex("public const string HTTPSUserName = \".*\"");
                Regex expression18 = new Regex("public const string HTTPSPassword = \".*\"");
                Regex expression19 = new Regex("public const string HTTPSAuthUrl = \".*\"");

                file = reader.ReadToEnd();

                file = expression1.Replace(file, "public const string Account = \"<INPUT HERE>\"");
                file = expression2.Replace(file, "public const string UserName = \"<INPUT HERE>\"");
                file = expression3.Replace(file, "public const string Password = \"<INPUT HERE>\"");
                file = expression4.Replace(file, "public const string AuthUrl = \"<INPUT HERE>\"");
                file = expression5.Replace(file, "public const string WrongPassword = \"<INPUT HERE>\"");
                file = expression6.Replace(file, "public const string ProxyUserName = \"<INPUT HERE>\"");
                file = expression7.Replace(file, "public const string ProxyPassword = \"<INPUT HERE>\"");
                file = expression8.Replace(file, "public const string ProxyAddress = \"<INPUT HERE>\"");
                file = expression9.Replace(file, "public const string ProxyDomain = \"<INPUT HERE>\"");
                file = expression10.Replace(file, "public const string AccountStaging2 = \"<INPUT HERE>\"");
                file = expression11.Replace(file, "public const string UserNameStaging2 = \"<INPUT HERE>\"");
                file = expression12.Replace(file, "public const string PasswordStaging2 = \"<INPUT HERE>\"");
                file = expression13.Replace(file, "public const string MOSSO_ACCOUNT = \"<INPUT HERE>\"");
                file = expression14.Replace(file, "public const string MOSSO_PASSWORD = \"<INPUT HERE>\"");
                file = expression15.Replace(file, "public const string MOSS_AUTH_URL = \"<INPUT HERE>\"");
                file = expression16.Replace(file, "public const string HTTPSAccount = \"<INPUT HERE>\"");
                file = expression17.Replace(file, "public const string HTTPSUserName = \"<INPUT HERE>\"");
                file = expression18.Replace(file, "public const string HTTPSPassword = \"<INPUT HERE>\"");
                file = expression19.Replace(file, "public const string HTTPSAuthUrl = \"<INPUT HERE>\"");

                if(showOutput) Console.WriteLine(file);
                
                using (TextWriter tw = new StreamWriter(outputfilename))
                {
                    tw.WriteLine(file);
                }

                Console.WriteLine("Obfuscation Complete");
            }
        }
    }
}