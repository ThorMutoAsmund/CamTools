using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamTools
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: CAMTOOLS HELP [DATEFROMEXIF]");
                return;
            }
            var tool = args[0].ToLowerInvariant();
            var showHelp = false;

            if (tool == "help")
            {
                args = args.Skip(1).ToArray();
                if (args.Length == 0)
                {
                    Console.WriteLine("Usage: CAMTOOLS HELP [DATEFROMEXIF]");
                    return;
                }
                showHelp = true;
            }

            switch (args[0].ToLowerInvariant())
            {
                case "datefromexif":
                    if (showHelp) {  DateChanger.ShowHelp(); return; }
                    new DateChanger().Execute(args.Skip(1).ToArray());
                    return;
                default:
                    Console.WriteLine($"Unknown tool '{args[0]}'");
                    return;
            }
        }
    }
}
