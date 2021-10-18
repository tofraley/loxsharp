using System;
using System.Collections.Generic;

namespace loxsharp
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private Expr Expression() => Equality();


        private Expr Equality()
        {
            Expr expr = Comparison();

            while (Match(new TokenType[2] { TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL }))
            {
                Token op = Previous();
                Expr right = Comparison();
                expr = new Expr.Binary(expr, right, op);
            }

            return expr;
        }

        private bool Match(IEnumerable<TokenType> types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        private Token Advance()
        {
            if (!IsAtEnd()) current++;
            return Previous();
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Previous()
        {
            return tokens[current - 1);
        }

        private Expr Comparison()
        {
            throw new NotImplementedException();
        }
    }
}
