
using System;
using System.Collections.Generic;

namespace loxsharp
{
    public class Nothing { }

    public class Interpreter : Expr.Visitor<Object>, Stmt.Visitor<Nothing>
    {
        private Environment environment = new Environment();

        public void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeError error)
            {
                Lox.RuntimeError(error);
            }
        }

        #region Stmt.Visitor

        public Nothing VisitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(environment));
            return null;
        }

        public Nothing VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.Expr);
            return null;
        }

        public Nothing VisitPrintStmt(Stmt.Print stmt)
        {
            object value = Evaluate(stmt.Expr);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public Nothing VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }

            environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        #endregion Stmt.Visitor

        #region Expr.Visitor

        public object VisitAssignExpr(Expr.Assign expr)
        {
            object value = Evaluate(expr.Value);
            environment.Assign(expr.Name, value);
            return value;
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            object left = Evaluate(expr.Left);
            object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.BANG_EQUAL:
                    return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL:
                    return IsEqual(left, right);
                case TokenType.GREATER:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                case TokenType.MINUS:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is Double && right is Double)
                    {
                        return (double)left + (double)right;
                    }

                    if (left is String && right is String)
                    {
                        return (String)left + (String)right;
                    }

                    throw new RuntimeError(expr.Operator,
                            "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    CheckNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
            }

            return null;
        }

        private void CheckNumberOperands(Token oper8r, object left, object right)
        {
            if (left is Double && right is Double) return;
            throw new RuntimeError(oper8r, "Operands must be numbers.");
        }

        private void CheckNumberOperand(Token oper8r, object operand)
        {
            if (operand is Double) return;
            throw new RuntimeError(oper8r, "Operand must be a number.");
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
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
                    CheckNumberOperand(expr.Operator, right);
                    return -(double)right;
            }

            return null;
        }

        public object VisitVariableExpr(Expr.Variable expr)
        {
            return environment.Get(expr.Name);
        }

        #endregion Expr.Visitor

        #region Helpers

        public void ExecuteBlock(List<Stmt> statements, Environment environment)
        {
            Environment previous = this.environment;
            try
            {
                this.environment = environment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
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

        private String Stringify(object obj)
        {
            if (obj == null) return "nil";

            if (obj is Double)
            {
                String text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }
            return obj.ToString();
        }

        #endregion Helpers
    }
}
