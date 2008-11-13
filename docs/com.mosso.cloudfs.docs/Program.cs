using System;
using System.IO;
using System.Text;

namespace com.mosso.cloudfs.docs
{
    public class Program
    {
        private static void Main(string[] args)
        {
            if(args.Length < 1)
            {
                ShowUsage();
                return;
            }

            DocBookXmlGenerator generator = new DocBookXmlGenerator(args[0]);
            try
            {
                CreateFile(generator.Generate());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed creating docbook xml file for CloudFS.NET\n\n" + ex.Message);
            }
        }

        private static void ShowUsage()
        {
            StringBuilder usage = new StringBuilder();

            usage.Append("CloudFSDotNetDocBookGenerator.exe <Visual Studio Xml Documentation File Path>");

            Console.WriteLine(usage.ToString());
        }

        private static void CreateFile(string output)
        {
            using (FileStream file = new FileStream("5.4_net_api.xml", FileMode.Create, FileAccess.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(output);
                }
            }
        }
    }
}