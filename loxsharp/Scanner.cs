﻿using System;
using System.Collections.Generic;

namespace loxsharp
{
    public class Scanner
    {
        private readonly String source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        private readonly Dictionary<String, TokenType> keywords =
            new Dictionary<String, TokenType>() {
                { "and", TokenType.AND },
                { "class", TokenType.CLASS },
                { "else", TokenType.ELSE },
                { "false", TokenType.FALSE },
                { "for", TokenType.FOR },
                { "fun", TokenType.FUN },
                { "if", TokenType.IF },
                { "nil", TokenType.NIL },
                { "or", TokenType.OR },
                { "print", TokenType.PRINT },
                { "return", TokenType.RETURN },
                { "super", TokenType.SUPER },
                { "this", TokenType.THIS },
                { "true", TokenType.TRUE },
                { "var", TokenType.VAR },
                { "while", TokenType.WHILE }
            };


        public Scanner(String source)
        {
            this.source = source;
        }

        public List<Token> ScanTokens()
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
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACE); break;
                case '}': AddToken(TokenType.RIGHT_BRACE); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '*': AddToken(TokenType.STAR); break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;

                case '\n':
                    line++;
                    break;
                case '"': String(); break;
                default:
                    if (IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        Lox.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            String text = source.Substring(start, current - start);
            keywords.TryGetValue(text, out TokenType type);
            AddToken(type);
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                    c == '_';
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();

            // Look for a fractional part.
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Consume the "."
                Advance();

                while (IsDigit(Peek())) Advance();
            }

            AddToken(TokenType.NUMBER,
                Double.Parse(source.Substring(start, current - start)));
        }

        private char PeekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Lox.Error(line, "Unterminated string.");
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            String value = source.Substring(start, current - start).Trim('"');
            AddToken(TokenType.STRING, value);
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[current];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;
        }

        private char Advance()
        {
            return source[current++];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, Object literal)
        {
            String text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }
    }
}
