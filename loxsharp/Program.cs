﻿using System;
using System.Collections.Generic;
using System.IO;

namespace loxsharp
{
    class Program
    {
        static void Main(string[] args)
        {
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
    }
}
