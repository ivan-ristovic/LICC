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
        public override ASTNode VisitExplist([NotNull] ExplistContext ctx) 
            => new ExpressionListNode(ctx.Start.Line, ctx.exp().Select(v => this.Visit(v).As<ExpressionNode>()));

        public override ASTNode VisitExp([NotNull] ExpContext ctx)
        {
            string firstToken = ctx.children.First().GetText();
            switch (firstToken) {
                case "nil": 
                    return new NullLiteralNode(ctx.Start.Line);
                case "true":
                case "false":
                    return LiteralNode.FromString(ctx.Start.Line, firstToken);
                case "...":
                    throw new NotImplementedException("...");
            }

            if (ctx.number() is { })
                return LiteralNode.FromString(ctx.Start.Line, ctx.number().GetText());

            if (ctx.@string() is { }) {
                string str = ctx.@string().GetText()[1..^1];
                return new LiteralNode(ctx.Start.Line, str);
            }

            // TODO
            if (ctx.functiondef() is { })
                return new FunctionDefinitionNode(ctx.Start.Line, null, null, null);

            // TODO
            throw new NotImplementedException("exp");
        }
    }
}
