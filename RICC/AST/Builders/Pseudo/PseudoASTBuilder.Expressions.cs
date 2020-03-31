using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;
using RICC.Extensions;
using static RICC.AST.Builders.Pseudo.PseudoParser;

namespace RICC.AST.Builders.Pseudo
{
    public sealed partial class PseudoASTBuilder : PseudoBaseVisitor<ASTNode>, IASTBuilder<PseudoParser>
    {
        public override ASTNode VisitExp([NotNull] ExpContext ctx)
        {
            if (ctx.literal() is { } || ctx.var() is { } || ctx.cexp() is { }) 
                return this.Visit(ctx.children.Single());

            if (ctx.aop() is { })
                return this.VisitArithmeticExpression(ctx.Start.Line, ctx.exp()[0], ctx.aop(), ctx.exp()[1]);

            if (ctx.rop() is { })
                return this.VisitRelationalExpression(ctx.Start.Line, ctx.exp()[0], ctx.rop(), ctx.exp()[1]);

            if (ctx.lop() is { })
                return this.VisitLogicExpression(ctx.Start.Line, ctx.exp()[0], ctx.lop(), ctx.exp()[1]);

            if (ctx.uop() is { })
                return this.VisitUnaryExpression(ctx.Start.Line, ctx.uop(), ctx.exp().Single());

            return this.Visit(ctx.exp().Single());
        }

        public override ASTNode VisitVar([NotNull] VarContext ctx)
        {
            var v = new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
            if (ctx.iexp() is null)
                return v;

            ExpressionNode arrIndex = this.Visit(ctx.iexp()).As<ExpressionNode>();
            return new ArrayAccessExpressionNode(ctx.Start.Line, v, arrIndex);
        }

        public override ASTNode VisitIexp([NotNull] IexpContext ctx)
            => this.Visit(ctx.children.First());

        public override ASTNode VisitLiteral([NotNull] LiteralContext ctx)
            => LiteralNode.FromString(ctx.Start.Line, ctx.children.Single().GetText());

        public override ASTNode VisitAexp([NotNull] AexpContext ctx)
            => this.VisitArithmeticExpression(ctx.Start.Line, ctx.exp()[0], ctx.aop(), ctx.exp()[1]);

        public override ASTNode VisitCexp([NotNull] CexpContext ctx)
        {
            var fname = new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
            if (ctx.explist() is null)
                return new FunctionCallExpressionNode(ctx.Start.Line, fname);

            ExpressionListNode args = this.Visit(ctx.explist()).As<ExpressionListNode>();
            return new FunctionCallExpressionNode(ctx.Start.Line, fname, args);
        }

        public override ASTNode VisitExplist([NotNull] ExplistContext ctx)
            => new ExpressionListNode(ctx.Start.Line, ctx.exp().Select(e => this.Visit(e).As<ExpressionNode>()));


        private ExpressionNode VisitArithmeticExpression(int line, ExpContext lexp, AopContext aop, ExpContext rexp)
        {
            ExpressionNode left = this.Visit(lexp).As<ExpressionNode>();
            var op = ArithmeticOperatorNode.FromSymbol(line, aop.GetText());
            ExpressionNode right = this.Visit(rexp).As<ExpressionNode>();
            return new ArithmeticExpressionNode(line, left, op, right);
        }

        private ExpressionNode VisitRelationalExpression(int line, ExpContext lexp, RopContext rop, ExpContext rexp)
        {
            ExpressionNode left = this.Visit(lexp).As<ExpressionNode>();
            var op = RelationalOperatorNode.FromSymbol(line, rop.GetText());
            ExpressionNode right = this.Visit(rexp).As<ExpressionNode>();
            return new RelationalExpressionNode(line, left, op, right);
        }

        private ExpressionNode VisitLogicExpression(int line, ExpContext lexp, LopContext lop, ExpContext rexp)
        {
            ExpressionNode left = this.Visit(lexp).As<ExpressionNode>();
            var op = BinaryLogicOperatorNode.FromSymbol(line, lop.GetText());
            ExpressionNode right = this.Visit(rexp).As<ExpressionNode>();
            return new LogicExpressionNode(line, left, op, right);
        }

        private ExpressionNode VisitUnaryExpression(int line, UopContext uop, ExpContext exp)
        {
            var op = UnaryOperatorNode.FromSymbol(line, uop.GetText());
            ExpressionNode operand = this.Visit(exp).As<ExpressionNode>();
            return new UnaryExpressionNode(line, op, operand);
        }
    }
}
