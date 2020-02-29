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
            // TODO
            return new LiteralNode(ctx.Start.Line, 1);
        }
    }
}
