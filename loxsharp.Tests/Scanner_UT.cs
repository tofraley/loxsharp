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

            Assert.Equal(actual, expected);
        }
        
        [Fact]
        public void ScanTokens_Recognizes_MultipleTokens()
        {
            List<Token> expectedTokens = new List<Token> {
                new Token(TokenType.NUMBER, "1", 1.0, 1),
                new Token(TokenType.PLUS, "+", null, 1),
                new Token(TokenType.NUMBER, "2", 2.0, 1),
                new Token(TokenType.EOF, "", null, 1)
            };

            testObject = new Scanner("1+2");

            List<Token> actualTokens = testObject.ScanTokens();

            List<(Token, Token)> tokens = expectedTokens.Zip(actualTokens).ToList();

            foreach (var (expected, actual) in tokens)
            {
                Assert.Equal(expected, actual);
            }
        }
    }
}
