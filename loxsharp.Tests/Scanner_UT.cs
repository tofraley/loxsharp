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
        [InlineData(1)]
        [InlineData(1234)]
        [InlineData(123.1033)]
        public void ScanTokens_Recognizes_Number(double number)
        {
            testObject = new Scanner(number.ToString());

            Token actual = testObject.ScanTokens().First();
            Token expected = new Token(TokenType.NUMBER, number.ToString(),
                number, 1);

            AssertTokensAreEqual(actual, expected);
        }

        [Theory]
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
        public void ScanTokens_Recognizes_SingleCharOperators(string input, TokenType type)
        {
            testObject = new Scanner(input);

            Token actual = testObject.ScanTokens().First();
            Token expected = new Token(type, input,
                null, 1);

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
