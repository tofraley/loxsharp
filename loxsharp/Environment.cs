using System;
using System.Collections.Generic;

namespace loxsharp
{
    public class Environment
    {
        private readonly Environment enclosing;
        private readonly Dictionary<String, object> values = new Dictionary<String, object>();

        public Environment()
        {
            enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            this.enclosing = enclosing;
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                Add(name.Lexeme, value);
            }
            else if (enclosing != null)
            {
                enclosing.Assign(name, value);
            }
            else
            {
                throw UndefinedVariable(name);
            }
        }

        public void Define(String name, object value)
        {
            if (!values.TryAdd(name, value))
            {
                Add(name, value);
            }
        }

        public object Get(Token name)
        {
            object value = null;
            if (values.TryGetValue(name.Lexeme, out value))
            {
                return value;
            }

            if (enclosing != null) return enclosing.Get(name);
            throw UndefinedVariable(name);
        }

        private void Add(String name, object value)
        {
            values.Remove(name);
            values.Add(name, value);
        }

        private RuntimeError UndefinedVariable(Token name)
        {
            return new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
        }
    }
}
