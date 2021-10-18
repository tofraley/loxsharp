using loxsharp.tool;
using System;

namespace GenerateAst
{
    class Program
    {
        static void Main(string[] args)
        {
            String output = "~/projects/loxsharp/loxsharp/Expr.cs";
            if (args.Length > 0)
            {
                output = args[0];
            }

            AstGenerator gen = new AstGenerator();
            gen.Run(new string[] { "Types.yml", "Expr.hbs", output });
            Console.WriteLine("Done");
        }
    }
}
