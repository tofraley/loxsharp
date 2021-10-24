using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using HandlebarsDotNet;

namespace loxsharp.tool
{

    class AbstractClassCollection
    {
        public IEnumerable<AbstractClass> AbstractClasses { get; set; }
    }

    class AbstractClass
    {
        public String AbstractClassName { get; set; }
        public String AbstractParam => AbstractClassName.ToLower();
        public IEnumerable<ImplClass> ImplClasses { get; set; }
    }

    class ImplClass
    {
        public String ClassName { get; set; }
        public IEnumerable<Field> Fields { get; set; }
    }

    class Field
    {
        public String Name { get; set; }
        public String Type { get; set; }
    }

    public class AstGenerator
    {
        public void Run(String[] args)
        {
            String typesDir = args[0];
            String templateDir = args[1];
            String outputDir = args[2];

            Console.WriteLine($"{typesDir}, {templateDir}, {outputDir}");
            DefineAst(typesDir, templateDir, outputDir);
        }

        private void DefineAst(String typesDir, String templateDir, String outputDir)
        {
            Console.WriteLine($"Reading {typesDir}");
            var yml = File.ReadAllText(typesDir);
            Console.WriteLine($"Compiling {templateDir}");
            var template = Handlebars.Compile(File.ReadAllText(templateDir));
            Console.WriteLine("Creating deserializer");
            var deserializer = new DeserializerBuilder()
                .Build();

            Console.WriteLine("Deserializing...");
            var types = deserializer.Deserialize<List<AbstractClass>>(yml);
            Console.WriteLine("Done deserializing...");
            var result = template(types);
            File.WriteAllText(outputDir, result);
        }
    }
}
