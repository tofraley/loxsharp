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

        [Fact]
        public void ScanTokens_Recognizes_Numbers()
        {
            double number = (double)1;
            testObject = new Scanner(number.ToString());

            Token actual = testObject.ScanTokens().First();
            Token expected = new Token(TokenType.NUMBER, number.ToString(),
                number, 1);

            Assert.Equal(expected.Lexeme, actual.Lexeme);
            Assert.Equal(expected.Line, actual.Line);
            Assert.Equal(expected.Literal, actual.Literal);
            Assert.Equal(expected.Type, actual.Type);
        }
    }
}
