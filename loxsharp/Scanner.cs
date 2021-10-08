using System;
using System.Collections.Generic;

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