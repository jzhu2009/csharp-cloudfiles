using System;
using System.IO;
using System.Xml;

namespace com.mosso.cloudfiles.integration.tests
{
    internal static class Credentials
    {
        private static readonly CredentialsConfigParser _credentialsConfigParser = new CredentialsConfigParser();

        public static readonly string USERNAME = _credentialsConfigParser.GetUsername();
        public static readonly string API_KEY = _credentialsConfigParser.GetApiKey();
    }

    internal class CredentialsConfigParser
    {
        private readonly XmlDocument _xmlDocument;

        public CredentialsConfigParser()
        {
            var CONFIG_FILE_PATH = Directory.GetCurrentDirectory() + "\\Credentials.config";
            if (!File.Exists(CONFIG_FILE_PATH)) 
                throw new FileNotFoundException("Missing Credentials.config file");
            
            _xmlDocument = new XmlDocument();
            _xmlDocument.Load(CONFIG_FILE_PATH);
        }
        
        public string GetUsername()
        {
            return _xmlDocument.SelectSingleNode("/credentials/username").InnerText;
        }

        public string GetApiKey()
        {
            return _xmlDocument.SelectSingleNode("/credentials/api_key").InnerText;
        }
    }
}