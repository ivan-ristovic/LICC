using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.Exceptions;
using RICC.Extensions;
using static RICC.AST.Builders.Pseudo.PseudoParser;

namespace RICC.AST.Builders.Pseudo
{
    public sealed partial class PseudoASTBuilder : PseudoBaseVisitor<ASTNode>, IASTBuilder<PseudoParser>
    {
        public override ASTNode VisitDeclaration([NotNull] DeclarationContext ctx)
        {
            switch (ctx.children.First().GetText()) {
                case "declare":
                    var declSpecs = new DeclarationSpecifiersNode(ctx.Start.Line, GetTypeName());
                    var name = new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
                    DeclaratorNode decl;
                    if (ctx.type().typename().children.Count > 1) {
                        switch (ctx.type().typename().children.Last().GetText()) {
                            case "array":
                            case "list":
                            case "set":
                                if (ctx.exp() is { }) {
                                    ExpressionNode init = this.Visit(ctx.exp()).As<ExpressionNode>();
                                    decl = new ArrayDeclaratorNode(ctx.Start.Line, name, init);
                                } else {
                                    decl = new ArrayDeclaratorNode(ctx.Start.Line, name);
                                }
                                break;
                            default:
                                throw new SyntaxException("Invalid complex type");
                        }
                    } else {
                        if (ctx.exp() is { }) {
                            ExpressionNode init = this.Visit(ctx.exp()).As<ExpressionNode>();
                            decl = new VariableDeclaratorNode(ctx.Start.Line, name, init);
                        } else {
                            decl = new VariableDeclaratorNode(ctx.Start.Line, name);
                        }
                    }
                    var declList = new DeclaratorListNode(ctx.Start.Line, decl);
                    return new DeclarationStatementNode(ctx.Start.Line, declSpecs, declList);
                case "procedure":
                case "function":
                    var fdeclSpecs = new DeclarationSpecifiersNode(ctx.Start.Line, GetTypeName());
                    var fname = new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
                    FunctionParametersNode fparams = this.Visit(ctx.parlist()).As<FunctionParametersNode>();
                    var fdecl = new FunctionDeclaratorNode(ctx.Start.Line, fname, fparams);
                    BlockStatementNode body = this.Visit(ctx.block()).As<BlockStatementNode>();
                    return new FunctionDefinitionNode(ctx.Start.Line, fdeclSpecs, fdecl, body);
                default:
                    throw new SyntaxException("Invalid statement");
            }


            string GetTypeName() => ctx.type()?.typename().GetText() ?? "void";
        }

        public override ASTNode VisitParlist([NotNull] ParlistContext ctx)
        {
            IEnumerable<FunctionParameterNode> @params = ctx.NAME().Zip(ctx.type(), (name, type) => {
                var declSpecs = new DeclarationSpecifiersNode(type.Start.Line, type.typename().GetText());
                var identifier = new IdentifierNode(ctx.Start.Line, name.GetText());
                DeclaratorNode decl;
                if (type.typename().children.Count > 1) {
                    switch (type.typename().children.Last().GetText()) {
                        case "array":
                        case "list":
                        case "set":
                            decl = new ArrayDeclaratorNode(ctx.Start.Line, identifier);
                            break;
                        default:
                            throw new SyntaxException("Invalid complex type");
                    }
                } else {
                    decl = new VariableDeclaratorNode(ctx.Start.Line, identifier);
                }
                return new FunctionParameterNode(type.Start.Line, declSpecs, decl);
            });
            return new FunctionParametersNode(ctx.Start.Line, @params);
        }
    }
}
