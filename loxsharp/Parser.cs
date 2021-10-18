using System;
using System.Collections.Generic;

namespace loxsharp
{
    public class Parser
    {
        public class ParseException : Exception { }
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        #region Helpers

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);

        }

        private Exception Error(Token token, string message)
        {
            Lox.Error(token, message);
            return new ParseException();
        }

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
            if (Match(new TokenType[2] { TokenType.BANG, TokenType.MINUS }))
            {
                Token op = Previous();
                Expr right = Unary();
                return new Expr.Unary(op, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(TokenType.FALSE)) return new Expr.Literal(false);
            if (Match(TokenType.TRUE)) return new Expr.Literal(true);
            if (Match(TokenType.NIL)) return new Expr.Literal(null);

            if (Match(new TokenType[2] { TokenType.NUMBER, TokenType.STRING }))
            {
                return new Expr.Literal(Previous().Literal);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }

            return null;
        }
        #endregion
    }
}
