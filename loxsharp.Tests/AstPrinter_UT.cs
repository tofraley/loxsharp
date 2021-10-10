using System;
using Xunit;

namespace loxsharp.Tests
{
    public class AstPrinter_UT
    {
        [Fact]
        public void AstPrinter_Works()
        {
            Expr expression =
                new Expr.Binary(
                    new Expr.Unary(
                        new Expr.Literal(123),
                        new Token(TokenType.MINUS, "-", null, 1)
                        ),

                    new Expr.Grouping(
                            new Expr.Literal(45.67)),
                    new Token(TokenType.STAR, "*", null, 1));

            var actual = new AstPrinter().Print(expression);
            var expected = "(* (- 123) (group 45.67))";
            
            Assert.Equal(expected, actual);
        }
    }
}
