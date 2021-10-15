using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using loxsharp;
using System.Linq;

namespace loxsharp.Tests
{
    public class Scanner_UT
    {
        private Scanner testObject;

        [Theory]
        [InlineData("1", TokenType.NUMBER, "1", 1.0)]
        [InlineData("1234", TokenType.NUMBER, "1234", 1234.0)]
        [InlineData("123.1033", TokenType.NUMBER, "123.1033", 123.1033)]
        [InlineData("(", TokenType.LEFT_PAREN)]
        [InlineData(")", TokenType.RIGHT_PAREN)]
        [InlineData("{", TokenType.LEFT_BRACE)]
        [InlineData("}", TokenType.RIGHT_BRACE)]
        [InlineData(",", TokenType.COMMA)]
        [InlineData(".", TokenType.DOT)]
        [InlineData("-", TokenType.MINUS)]
        [InlineData("+", TokenType.PLUS)]
        [InlineData(";", TokenType.SEMICOLON)]
        [InlineData("*", TokenType.STAR)]
        [InlineData("!", TokenType.BANG)]
        [InlineData("!=", TokenType.BANG_EQUAL)]
        [InlineData("=", TokenType.EQUAL)]
        [InlineData("==", TokenType.EQUAL_EQUAL)]
        [InlineData("<", TokenType.LESS)]
        [InlineData("<=", TokenType.LESS_EQUAL)]
        [InlineData(">", TokenType.GREATER)]
        [InlineData(">=", TokenType.GREATER_EQUAL)]
        [InlineData("/", TokenType.SLASH)]
        [InlineData("// this is a comment\n", TokenType.EOF, "", null, 2)]
        [InlineData("// this is a comment\n=", TokenType.EQUAL, "=", null, 2)]
        public void ScanTokens_Ignores_Comment(
            string input, TokenType type, string lexeme = null, 
            object literal = null, int line = 1)
        {
            lexeme ??= input;
            testObject = new Scanner(input);

            Token actual = testObject.ScanTokens().First();
            Token expected = new Token(type, lexeme,
                literal, line);

            AssertTokensAreEqual(actual, expected);
        }

        private static void AssertTokensAreEqual(Token actual, Token expected)
        {
            Assert.Equal(expected.Lexeme, actual.Lexeme);
            Assert.Equal(expected.Line, actual.Line);
            Assert.Equal(expected.Literal, actual.Literal);
            Assert.Equal(expected.Type, actual.Type);
        }
    }
}
