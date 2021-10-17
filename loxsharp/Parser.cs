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

        private Token Previous()
        {
            throw new NotImplementedException();
        }

        private Expr Comparison()
        {
            throw new NotImplementedException();
        }
    }
}
