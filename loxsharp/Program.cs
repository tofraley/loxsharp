using System;
using System.Collections.Generic;
using System.IO;

namespace loxsharp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test();
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: lox [script]");
            }
            else if (args.Length == 1)
            {
                Lox.RunFile(args[0]);
            }
            else
            {
                Lox.RunPrompt();
            }
        }

        static void Test()
        {
            Expr expression =
                new Expr.Binary(
                    new Expr.Unary(
                        new Expr.Literal(123),
                        new Token(TokenType.MINUS, "-", null, 1)
                        ),

                    new Expr.Grouping(
                            new Expr.Literal(45.67)),
                    new Token(TokenType.STAR, "*", null, 1));

            Console.WriteLine(new AstPrinter().Print(expression));
        }
    }
}
