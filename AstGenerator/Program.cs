using loxsharp.tool;
using System;

namespace GenerateAst
{
    class Program
    {
        static void Main(string[] args)
        {
            AstGenerator gen = new AstGenerator();
            gen.Run(new string[] { "Types.yml", "Expr.hbs", "C:\\clearent\\projects\\loxsharp\\loxsharp\\Expr.cs" });
            Console.WriteLine("Done");
        }
    }
}
