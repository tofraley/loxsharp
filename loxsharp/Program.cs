using System;
using System.Collections.Generic;
using System.IO;

namespace loxsharp
{
    class Program
    {
        static int Main(string[] args)
        {
            Lox lox = new Lox();
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: lox [script]");
            }
            else if (args.Length == 1)
            {
                lox.RunFile(args[0]);
            }
            else
            {
                lox.RunPrompt();
            }

            return lox.GetSysExit();
        }

    }
}
