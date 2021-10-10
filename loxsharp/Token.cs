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
    }
}