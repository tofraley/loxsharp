
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
            throw new NotImplementedException();
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }
    }
}
