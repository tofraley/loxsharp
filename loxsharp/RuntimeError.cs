using System;

namespace loxsharp
{
    public class RuntimeError : Exception
    {
        private readonly Token token;

        public RuntimeError(Token token, String message)
            : base(message)
        {
            this.token = token;
        }
    }
}
