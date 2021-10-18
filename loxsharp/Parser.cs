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

        #region Helpers
        private bool Match(TokenType type)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
            return false;
        }

        private bool Match(IEnumerable<TokenType> types)
        {
            foreach (TokenType type in types)
            {
                Match(type);
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
            return tokens[current - 1];
        }

        private Expr ParseLeftBinaryOperators(TokenType[] types, Func<Expr> Left, Func<Expr> Right)
        {
            Expr expr = Left.Invoke();

            while (Match(types))
            {
                Token op = Previous();
                Expr right = Right.Invoke();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        #endregion 

        #region Rules

        private Expr Expression() => Equality();

        private Expr Equality()
        {
            /*             Expr expr = Comparison(); */

            /*             while (Match()) */
            /*             { */
            /*                 Token op = Previous(); */
            /*                 Expr right = Comparison(); */
            /*                 expr = new Expr.Binary(expr, op, right); */
            /*             } */

            return ParseLeftBinaryOperators(
                new TokenType[2] { TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL },
                Comparison,
                Comparison
                );
        }


        private Expr Comparison()
        {
            /*             Expr expr = Term(); */

            /*             while (Match()) */
            /*             { */
            /*                 Token op = Previous(); */
            /*                 Expr right = Term(); */
            /*                 expr = new Expr.Binary(expr, op, right); */
            /*             } */
            return ParseLeftBinaryOperators(
                new TokenType[4]
                {
                  TokenType.GREATER,
                  TokenType.GREATER_EQUAL,
                  TokenType.LESS,
                  TokenType.LESS_EQUAL
                },
                Term,
                Term
                );
        }

        private Expr Term()
        {
            /*             Expr expr = Factor(); */

            /*             while (Match()) */
            /*             { */
            /*                 Token op = Previous(); */
            /*                 Expr right = Factor(); */
            /*                 expr = new Expr.Binary(expr, op, right); */
            /*             } */

            return ParseLeftBinaryOperators(
                new TokenType[2] { TokenType.MINUS, TokenType.PLUS },
                Factor,
                Factor
                );
        }

        private Expr Factor()
        {

            /* while (Match()) */
            /* { */
            /*     Token op = Previous(); */
            /*     Expr right = Unary(); */
            /*     expr = new Expr.Binary(expr, op, right); */
            /* } */

            return ParseLeftBinaryOperators(
                new TokenType[2] { TokenType.SLASH, TokenType.STAR },
                Factor,
                Unary
                );

        }

        private Expr Unary()
        {
            throw new NotImplementedException();
        }



    }
    #endregion 
}
