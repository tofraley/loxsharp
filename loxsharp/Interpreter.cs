
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
            return evaluate(expr.Expression);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.Value;
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            throw new NotImplementedException();
        }
    }
}
