using System;
using System.Collections.Generic;
using System.IO;

namespace loxsharp
{
    static public class Lox
    {
        static Boolean _hadError = false;

        public static void RunPrompt()
        {
            for (;;)
            {
                Console.Write("> ");
                String line = Console.ReadLine();
                if (line == null) break;
                Run(line);
                _hadError = false;
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

        public static void RunFile(string path)
        {
            String text = File.ReadAllText(Path.GetFullPath(path));
            Run(text); 
            if (_hadError) Environment.Exit(65);
        }

        public static void Error(int line, String message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, String where,
                                   String message)
        {
            Console.Error.WriteLine(
                "[line " + line + "] Error" + where + ": " + message);
            _hadError = true;
        }
    }
}
