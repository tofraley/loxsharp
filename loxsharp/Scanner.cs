using System;

namespace loxsharp
{
    internal class Scanner
    {
        private String source;

        public Scanner(String source)
        {
            this.source = source;
        }

        internal List<Token> ScanTokens()
        {
            throw new NotImplementedException();
        }
    }
}