using System;

namespace loxsharp
{
    public abstract class Expr
    {
        public class Binary : Expr
        {
            public readonly Expr Left;
            public readonly Expr Right;
            public readonly Token Operator;

            public Binary(Expr Left, Expr Right, Token Operator)
            {
                    this.Left = Left;
                    this.Right = Right;
                    this.Operator = Operator;
            }
        }

        public class Grouping : Expr
        {
            public readonly Expr Expression;

            public Grouping(Expr Expression)
            {
                    this.Expression = Expression;
            }
        }

        public class Literal : Expr
        {
            public readonly Object Value;

            public Literal(Object Value)
            {
                    this.Value = Value;
            }
        }

        public class Unary : Expr
        {
            public readonly Expr Right;
            public readonly Token Operator;

            public Unary(Expr Right, Token Operator)
            {
                    this.Right = Right;
                    this.Operator = Operator;
            }
        }
    }
}