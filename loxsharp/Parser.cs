using System;
using System.Collections.Generic;

namespace loxsharp
{
    public class Parser
    {
        public class ParseError : Exception { }
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public List<Stmt> Parse()
        {
            List<Stmt> statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }

            return statements;
        }

        #region Helpers

        private List<Stmt> Block()
        {
            List<Stmt> statements = new List<Stmt>();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }

            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            return statements;
        }

        private Stmt Declaration()
        {
            try
            {
                if (Match(TokenType.VAR)) return VarDeclaration();
                return Statement();
            }
            catch (ParseError)
            {
                Synchronize();
                return null;
            }
        }

        private Stmt VarDeclaration()
        {
            Token name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

            Expr initializer = null;
            if (Match(TokenType.EQUAL))
            {
                initializer = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
            return new Stmt.Var(name, initializer);
        }

        private Stmt Statement()
        {
            if (Match(TokenType.IF)) return IfStatement();
            if (Match(TokenType.PRINT)) return PrintStatement();
            if (Match(TokenType.LEFT_BRACE)) return new Stmt.Block(Block());

            return ExpressionStatement();
        }

        private Stmt IfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
            Expr condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after 'if' condition.");

            Stmt thenBranch = Statement();
            Stmt elseBranch = null;
            if (Match(TokenType.ELSE))
            {
                elseBranch = Statement();
            }

            return new Stmt.If(condition, thenBranch, elseBranch);
        }

        private Stmt PrintStatement()
        {
            Expr value = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new Stmt.Print(value);
        }

        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
            return new Stmt.Expression(expr);
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.SEMICOLON) return;

                switch (Peek().Type)
                {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }

                Advance();
            }

        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            throw Error(Peek(), message);

        }

        private Exception Error(Token token, string message)
        {
            Lox.Error(token, message);
            return new ParseError();
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
                if (Match(type)) return true;
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

        private Expr Expression() => Assignment();

        private Expr Assignment()
        {
            Expr expr = Or();

            if (Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr is Expr.Variable)
                {
                    Token name = ((Expr.Variable)expr).Name;
                    return new Expr.Assign(name, value);
                }

                Error(equals, "Invalid assignment target.");
            }

            return expr;
        }

        private Expr Or()
        {
            Expr expr = And();

            while (Match(TokenType.OR))
            {
                Token oper8r = Previous();
                Expr right = And();
                expr = new Expr.Logical(expr, oper8r, right);
            }

            return expr;
        }

        private Expr And()
        {
            Expr expr = Equality();

            while (Match(TokenType.AND))
            {
                Token oper8r = Previous();
                Expr right = Equality();
                expr = new Expr.Logical(expr, oper8r, right);
            }

            return expr;
        }

        private Expr Equality()
        {
            return ParseLeftBinaryOperators(
                new TokenType[2] { TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL },
                Comparison,
                Comparison
                );
        }


        private Expr Comparison()
        {
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
            return ParseLeftBinaryOperators(
                new TokenType[2] { TokenType.MINUS, TokenType.PLUS },
                Factor,
                Factor
                );
        }

        private Expr Factor()
        {
            return ParseLeftBinaryOperators(
                new TokenType[2] { TokenType.SLASH, TokenType.STAR },
                Unary,
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

            if (Match(TokenType.IDENTIFIER))
            {
                return new Expr.Variable(Previous());
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expr expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }

            throw Error(Peek(), "Expect expression.");
        }
        #endregion
    }
}
