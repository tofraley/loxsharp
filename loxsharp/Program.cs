using System;
using System.Collections.Generic;
using System.IO;

namespace loxsharp
{
    class Program
    {
        static Boolean hadError = false;
        static int sysExit = 0;
        static int Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: lox [script]");
            }
            else if (args.Length == 1)
            {
                RunFile(args[0]);
            }
            else
            {
                RunPrompt();
            }

            return sysExit;
        }

        private static void RunPrompt()
        {
            for (;;)
            {
                Console.Write("> ");
                String line = Console.ReadLine();
                if (line == null) break;
                Run(line);
                hadError = false;
            }
        }

        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            // For now, just print the tokens.
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        private static void RunFile(string path)
        {
            String text = File.ReadAllText(Path.GetFullPath(path));
            Run(text); 
            if (hadError) sysExit = 65;
        }

        static void Error(int line, String message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, String where,
                                   String message)
        {
            Console.Error.WriteLine(
                "[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }
    }
}
