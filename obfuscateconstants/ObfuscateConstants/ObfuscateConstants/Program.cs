using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObfuscateConstants
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3 || (args[2].ToLower() != "y" && args[2].ToLower() != "n"))
            {
                ShowUsage();
                return;
            }

            Obfuscator obfuscator = new Obfuscator();
            
            obfuscator.Obfuscate(args[0], args[1], args[2].ToLower() == "y");
        }

        private static void ShowUsage()
        {
            StringBuilder usage = new StringBuilder();

            usage.Append("obfuscateconstants.exe <original file path> <output file name> <show output: y or n>");

            Console.WriteLine(usage.ToString());
        }
    }
}
