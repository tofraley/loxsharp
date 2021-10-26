using System;
using System.Collections.Generic;

namespace loxsharp
{
    public class Environment
    {
        private readonly Dictionary<String, object> values = new Dictionary<String, object>();

        public void Define(String name, object value)
        {
            if (values.ContainsKey(name))
            {
                values.Remove(name);
            }
            values.Add(name, value);
        }

        public object Get(Token name)
        {
            object value = null;
            if (values.TryGetValue(name.Lexeme, out value))
            {
                return value;
            }

            throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }
    }
}
