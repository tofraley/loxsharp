using System;
using System.Collections.Generic;
using System.IO;

namespace loxsharp
{
    static public class Lox
    {
        private static readonly Interpreter interpreter = new Interpreter();
        static Boolean hadError = false;
        static Boolean hadRuntimeError = false;

        public static void RunPrompt()
        {
            for (; ; )
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
            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.Parse();

            if (hadError) return;

            interpreter.Interpret(statements);
        }

        public static void RuntimeError(RuntimeError error)
        {
            Console.WriteLine(error.Message);
            Console.WriteLine($"[line {error.Token.Line}]");
            hadRuntimeError = true;
        }

        public static void RunFile(string path)
        {
            String text = File.ReadAllText(Path.GetFullPath(path));
            Run(text);
            if (hadError) System.Environment.Exit(65);
            if (hadRuntimeError) System.Environment.Exit(70);
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
            hadError = true;
        }

        public static void Error(Token token, String message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, " at end", message);
            }
            else
            {
                Report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }
    }
}
