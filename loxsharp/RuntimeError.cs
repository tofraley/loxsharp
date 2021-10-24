using System;

namespace loxsharp
{
    public class RuntimeError : Exception
    {
        public readonly Token Token;

        public RuntimeError(Token token, String message)
            : base(message)
        {
            this.Token = token;
        }
    }
}
