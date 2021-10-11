using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using HandlebarsDotNet;

namespace loxsharp.tool
{

    class Field
    {
        public String Name { get; set; }
        public String Type { get; set; }
    }

    class Expression
    {
        public String ClassName { get; set; }
        public IEnumerable<Field> Fields { get; set; }
    }

    public class AstGenerator
    {
        public void Run(String[] args)
        {
            if (args.Length != 3) {
              Console.Error.WriteLine("Usage: generate_ast <types path> <template path> <output path>");
              Environment.Exit(64);
            }
            String typesDir = args[0];
            String templateDir = args[1];
            String outputDir = args[2];

            DefineAst(typesDir, templateDir, outputDir);
        }

        private void DefineAst(String typesDir, String templateDir, String outputDir)
        {
            var yml = File.ReadAllText(typesDir);
            var template = Handlebars.Compile(File.ReadAllText(templateDir));
            var deserializer = new DeserializerBuilder()
                .Build();

            var types = deserializer.Deserialize<List<Expression>>(yml);
            var result = template(types);
            File.WriteAllText(outputDir, result);
        }
    }
}
