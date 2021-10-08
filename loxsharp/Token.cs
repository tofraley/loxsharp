﻿using System;

namespace loxsharp
{
    internal class Token
    {
        readonly TokenType Type;
        readonly String Lexeme;
        readonly Object Literal;
        readonly int Line;

        Token(TokenType type, String lexeme, Object literal, int line)
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