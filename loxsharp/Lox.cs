using System;
using System.Collections.Generic;
using System.IO;

namespace loxsharp
{
    public class Lox
    {
        Boolean _hadError = false;
        int _sysExit = 0;

        public int GetSysExit() => _sysExit;

        public void RunPrompt()
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

        private void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            // For now, just print the tokens.
            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        public void RunFile(string path)
        {
            String text = File.ReadAllText(Path.GetFullPath(path));
            Run(text); 
            if (_hadError) _sysExit = 65;
        }

        private void Error(int line, String message)
        {
            Report(line, "", message);
        }

        private void Report(int line, String where,
                                   String message)
        {
            Console.Error.WriteLine(
                "[line " + line + "] Error" + where + ": " + message);
            _hadError = true;
        }
    }
}
