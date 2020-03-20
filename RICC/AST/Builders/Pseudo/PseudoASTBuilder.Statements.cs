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
        public override ASTNode VisitStatement([NotNull] StatementContext ctx)
        {
            if (ctx.declaration() is { })
                return this.Visit(ctx.declaration());

            if (ctx.assignment() is { })
                return this.Visit(ctx.assignment());

            if (ctx.cexp() is { })
                return this.Visit(ctx.cexp());

            switch (ctx.children.First().GetText()) {
                case "pass": 
                    return new EmptyStatementNode(ctx.Start.Line);
                case "return": 
                    return new JumpStatementNode(ctx.Start.Line, this.Visit(ctx.exp()).As<ExpressionNode>());
                case "error":
                    return new ThrowStatementNode(ctx.Start.Line, new LiteralNode(ctx.Start.Line, ctx.STRING().GetText()));
                case "if":
                    ExpressionNode cond = this.Visit(ctx.exp()).As<ExpressionNode>();
                    BlockStatementNode thenBlock = this.Visit(ctx.block().First()).As<BlockStatementNode>();
                    if (ctx.block().Length > 1) {
                        BlockStatementNode? elseBlock = this.Visit(ctx.block().Last()).As<BlockStatementNode>();
                        return new IfStatementNode(ctx.Start.Line, cond, thenBlock, elseBlock);
                    } else {
                        return new IfStatementNode(ctx.Start.Line, cond, thenBlock);
                    }
                case "while":
                    ExpressionNode whileCond = this.Visit(ctx.exp()).As<ExpressionNode>();
                    BlockStatementNode whileBlock = this.Visit(ctx.block().Single()).As<BlockStatementNode>();
                    return new WhileStatementNode(ctx.Start.Line, whileCond, whileBlock);
                case "repeat":
                    ExpressionNode repeatCond = this.Visit(ctx.exp()).As<ExpressionNode>();
                    var notOp = new UnaryOperatorNode(ctx.Start.Line, "not", UnaryOperations.NegatePrimitive);
                    ExpressionNode negatedCond = new UnaryExpressionNode(ctx.Start.Line, notOp, repeatCond);
                    BlockStatementNode repeatBlock = this.Visit(ctx.block().Single()).As<BlockStatementNode>();
                    var loop = new WhileStatementNode(ctx.Start.Line, negatedCond, repeatBlock);
                    var block = new BlockStatementNode(ctx.Start.Line, repeatBlock, loop);
                    return new WhileStatementNode(ctx.Start.Line, repeatCond, repeatBlock);
                case "increment":
                    return new IncrementExpressionNode(ctx.Start.Line, this.Visit(ctx.var()).As<ExpressionNode>());
                case "decrement":
                    return new DecrementExpressionNode(ctx.Start.Line, this.Visit(ctx.var()).As<ExpressionNode>());
                default:
                    throw new SyntaxException("Invalid statement");
            }
        }

        public override ASTNode VisitAssignment([NotNull] AssignmentContext ctx)
        {
            ExpressionNode left = this.Visit(ctx.var()).As<ExpressionNode>();
            ExpressionNode right = this.Visit(ctx.exp()).As<ExpressionNode>();
            var assignment = new AssignmentExpressionNode(ctx.Start.Line, left, right);
            return new ExpressionStatementNode(ctx.Start.Line, assignment);
        }
    }
}
