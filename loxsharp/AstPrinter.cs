using System;
using System.Collections.Generic;
using System.Text;

namespace loxsharp
{
    public class AstPrinter : Expr.Visitor<String>
    {
        public String Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public String VisitAssignExpr(Expr.Assign expr)
        {
            throw new NotImplementedException();
        }

        public String VisitBinaryExpr(Expr.Binary expr)
        {
            return Parenthesize(expr.Operator.Lexeme,
                                new List<Expr> { expr.Left, expr.Right });
        }

        public String VisitGroupingExpr(Expr.Grouping expr)
        {
            return Parenthesize("group", new List<Expr> { expr.Expression });
        }


        public String VisitLiteralExpr(Expr.Literal expr)
        {
            if (expr.Value == null) return "nil";
            return expr.Value.ToString();
        }


        public String VisitUnaryExpr(Expr.Unary expr)
        {
            return Parenthesize(expr.Operator.Lexeme, new List<Expr> { expr.Right });
        }

        public String VisitVariableExpr(Expr.Variable expr)
        {
            throw new NotImplementedException();
        }

        private String Parenthesize(String name, IEnumerable<Expr> exprs)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach (Expr expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}
