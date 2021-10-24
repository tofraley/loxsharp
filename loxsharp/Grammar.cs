using System;

namespace loxsharp
{
    public abstract class Expr
    {

        public interface Visitor<R>
        {
            public R VisitBinaryExpr(Binary expr);
            public R VisitGroupingExpr(Grouping expr);
            public R VisitLiteralExpr(Literal expr);
            public R VisitUnaryExpr(Unary expr);
        }

        public abstract R Accept<R>(Visitor<R> visitor);

        public class Binary : Expr
        {
            public readonly Expr Left;
            public readonly Token Operator;
            public readonly Expr Right;

            public Binary(Expr Left, Token Operator, Expr Right)
            {
                this.Left = Left;
                this.Operator = Operator;
                this.Right = Right;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitBinaryExpr(this);
            }
        }

        public class Grouping : Expr
        {
            public readonly Expr Expression;

            public Grouping(Expr Expression)
            {
                this.Expression = Expression;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitGroupingExpr(this);
            }
        }

        public class Literal : Expr
        {
            public readonly Object Value;

            public Literal(Object Value)
            {
                this.Value = Value;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitLiteralExpr(this);
            }
        }

        public class Unary : Expr
        {
            public readonly Token Operator;
            public readonly Expr Right;

            public Unary(Token Operator, Expr Right)
            {
                this.Operator = Operator;
                this.Right = Right;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitUnaryExpr(this);
            }
        }
    }

    public abstract class Stmt
    {

        public interface Visitor<R>
        {
            public R VisitExpressionStmt(Expression stmt);
            public R VisitPrintStmt(Print stmt);
        }

        public abstract R Accept<R>(Visitor<R> visitor);

        public class Expression : Stmt
        {
            public readonly Expr Expr;

            public Expression(Expr Expr)
            {
                this.Expr = Expr;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitExpressionStmt(this);
            }
        }

        public class Print : Stmt
        {
            public readonly Expr Expr;

            public Print(Expr Expr)
            {
                this.Expr = Expr;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitPrintStmt(this);
            }
        }
    }
}
