using System;

namespace loxsharp
{
    public class Token
    {
        public readonly TokenType Type;
        public readonly String Lexeme;
        public readonly Object Literal;
        public readonly int Line;

        public Token(TokenType type, String lexeme, Object literal, int line)
        {
            this.Type = type;
            this.Lexeme = lexeme;
            this.Literal = literal;
            this.Line = line;
        }

        public override String ToString()
        {
            return Type + " " + Lexeme + " " + Literal;
        }

        public override bool Equals(object obj)
        {
            Token token = (Token)obj;
            bool isLexeme = token.Lexeme == this.Lexeme;
            bool isLine = token.Line == this.Line;
            bool isLiteral = (this.Literal == null && token.Literal == null) || token.Literal.Equals(this.Literal);
            bool isType = token.Type == this.Type;

            return isLexeme && isLine && isLiteral && isType;
                   
                   
        }
    }
}