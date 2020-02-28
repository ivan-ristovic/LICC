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
                return new EmptyStatementNode(ctx.Start.Line);
            } else if (ctx.functioncall() is { }) {
                return new EmptyStatementNode(ctx.Start.Line);
            } else if (ctx.label() is { }) {
                return new EmptyStatementNode(ctx.Start.Line);
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
    }
}
