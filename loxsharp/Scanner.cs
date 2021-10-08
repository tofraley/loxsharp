using System;
using System.Collections.Generic;

namespace loxsharp
{
    internal class Scanner
    {
        private readonly String source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(String source)
        {
            this.source = source;
        }

        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                // We are at the beginning of the next lexeme.
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private Boolean IsAtEnd()
        {
            return current >= source.Length;
        }

        private void ScanToken()
        {
            throw new NotImplementedException();
        }
    }
}