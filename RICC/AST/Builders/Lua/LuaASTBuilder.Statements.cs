using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;
using RICC.Extensions;
using static RICC.AST.Builders.Lua.LuaParser;

namespace RICC.AST.Builders.Lua
{
    public sealed partial class LuaASTBuilder : LuaBaseVisitor<ASTNode>, IASTBuilder
    {
        public override ASTNode VisitStat([NotNull] StatContext ctx)
        {
            if (ctx.varlist() is { }) {
                IdentifierListNode vars = this.Visit(ctx.varlist()).As<IdentifierListNode>();
                ExpressionListNode explist = this.Visit(ctx.explist()).As<ExpressionListNode>();
                if (explist.Children.Count < vars.Children.Count) {
                    int missingCount = vars.Children.Count - explist.Children.Count;
                    IEnumerable<ExpressionNode> missing = Enumerable.Repeat<ExpressionNode>(new LiteralNode(ctx.Start.Line, "nil"), missingCount);
                    explist = new ExpressionListNode(ctx.Start.Line, explist.Expressions.Concat(missing));
                }
                IEnumerable<AssignmentExpressionNode> assignments = vars.Identifiers
                    .Zip(explist.Expressions)
                    .Select(tup => new AssignmentExpressionNode(ctx.Start.Line, tup.First, tup.Second));
                // TODO declare vars if not seen before
                if (vars.Children.Count == 1) 
                    return new ExpressionStatementNode(ctx.Start.Line, assignments.First());
                return new BlockStatementNode(ctx.Start.Line, assignments);
            } else if (ctx.functioncall() is { }) {
                return new EmptyStatementNode(ctx.Start.Line);  // TODO
            } else if (ctx.label() is { }) {
                return new LabeledStatementNode(ctx.Start.Line, ctx.label().NAME().GetText(), new EmptyStatementNode(ctx.Start.Line));
            } else {
                switch (ctx.children.First().GetText()) {
                    case ";": 
                        return new EmptyStatementNode(ctx.Start.Line);
                    case "break":
                        return new JumpStatementNode(ctx.Start.Line, JumpStatementType.Break);
                    case "goto":
                        var label = new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
                        return new JumpStatementNode(ctx.Start.Line, label);
                    case "do":
                        return this.Visit(ctx.block().Single());
                    case "while":
                        return new EmptyStatementNode(ctx.Start.Line); // TODO
                    case "repeat":
                        return new EmptyStatementNode(ctx.Start.Line); // TODO
                    case "if":
                        return new EmptyStatementNode(ctx.Start.Line); // TODO
                    case "for":
                        return new EmptyStatementNode(ctx.Start.Line); // TODO
                    case "function":
                        return new EmptyStatementNode(ctx.Start.Line); // TODO
                    case "local":
                        return new EmptyStatementNode(ctx.Start.Line); // TODO
                    default:
                        throw new SyntaxException("Invalid statement type.");
                }
            }
        }

        public override ASTNode VisitRetstat([NotNull] RetstatContext ctx)
        {
            // TODO
            return new JumpStatementNode(ctx.Start.Line, JumpStatementType.Return);
        }

        public override ASTNode VisitVarlist([NotNull] VarlistContext ctx) 
            => new IdentifierListNode(ctx.Start.Line, ctx.var().Select(v => this.Visit(v).As<IdentifierNode>()));

        public override ASTNode VisitVar([NotNull] VarContext ctx)
        {
            // TODO
            return new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
        }
    }
}
