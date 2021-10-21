
using System;

namespace loxsharp
{
    public class Interpreter : Expr.Visitor<Object>
    {
        public object VisitBinaryExpr(Expr.Binary expr)
        {
            throw new NotImplementedException();
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.Expression);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    return -(double)right;
            }

            return null;
        }

        private bool IsTruthy(object obj)
        {
            if (obj == null) return false;
            if (obj is Boolean)
                return (bool)obj;
            else
                return true;
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }
    }
}
