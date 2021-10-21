
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
            throw new NotImplementedException();
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            throw new NotImplementedException();
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            throw new NotImplementedException();
        }
    }
}
