using System;
using System.Collections.Generic;

namespace loxsharp
{
    public abstract class Expr
    {

        public interface Visitor<R>
        {
            public R VisitAssignExpr(Assign expr);
            public R VisitBinaryExpr(Binary expr);
            public R VisitGroupingExpr(Grouping expr);
            public R VisitLiteralExpr(Literal expr);
            public R VisitLogicalExpr(Logical expr);
            public R VisitUnaryExpr(Unary expr);
            public R VisitVariableExpr(Variable expr);
        }

        public abstract R Accept<R>(Visitor<R> visitor);

        public class Assign : Expr
        {
            public readonly Token Name;
            public readonly Expr Value;

            public Assign(Token Name, Expr Value)
            {
                this.Name = Name;
                this.Value = Value;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitAssignExpr(this);
            }
        }

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

        public class Logical : Expr
        {
            public readonly Expr Left;
            public readonly Token Operator;
            public readonly Expr Right;

            public Logical(Expr Left, Token Operator, Expr Right)
            {
                this.Left = Left;
                this.Operator = Operator;
                this.Right = Right;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitLogicalExpr(this);
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

        public class Variable : Expr
        {
            public readonly Token Name;

            public Variable(Token Name)
            {
                this.Name = Name;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitVariableExpr(this);
            }
        }
    }

    public abstract class Stmt
    {

        public interface Visitor<R>
        {
            public R VisitBlockStmt(Block stmt);
            public R VisitExpressionStmt(Expression stmt);
            public R VisitIfStmt(If stmt);
            public R VisitPrintStmt(Print stmt);
            public R VisitVarStmt(Var stmt);
        }

        public abstract R Accept<R>(Visitor<R> visitor);

        public class Block : Stmt
        {
            public readonly List<Stmt> Statements;

            public Block(List<Stmt> Statements)
            {
                this.Statements = Statements;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitBlockStmt(this);
            }
        }

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

        public class If : Stmt
        {
            public readonly Expr Condition;
            public readonly Stmt ThenBranch;
            public readonly Stmt ElseBranch;

            public If(Expr Condition, Stmt ThenBranch, Stmt ElseBranch)
            {
                this.Condition = Condition;
                this.ThenBranch = ThenBranch;
                this.ElseBranch = ElseBranch;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitIfStmt(this);
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

        public class Var : Stmt
        {
            public readonly Token Name;
            public readonly Expr Initializer;

            public Var(Token Name, Expr Initializer)
            {
                this.Name = Name;
                this.Initializer = Initializer;
            }

            public override R Accept<R>(Visitor<R> visitor) {
                return visitor.VisitVarStmt(this);
            }
        }
    }
}
