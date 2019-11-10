using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.Extensions;
using static RICC.AST.Builders.C.CParser;

namespace RICC.AST.Builders.C
{
    public sealed partial class CASTBuilder : CBaseVisitor<ASTNode>, IASTBuilder
    {
        public override ASTNode VisitFunctionDefinition([NotNull] FunctionDefinitionContext ctx)
        {
            LogObj.Context(ctx);

            // visit children and get info

            string fname = ExtractFunctionName(ctx.GetChild<DeclaratorContext>(0));
            BlockItemListContext defn = ctx.GetChild<CompoundStatementContext>(0).GetChild<BlockItemListContext>(0);
            var body = new BlockStatementNode(defn.Start.Line, defn.children.Select(c => this.Visit(c)));

            return new FunctionDefinitionNode(ctx.Start.Line, fname, Enumerable.Empty<(string, Type)>(), null, body);


            IReadOnlyList<string> ExtractDeclarationSpecifiers(DeclarationSpecifiersContext specs)
            {
                // TODO
                return specs.children.Select(c => c.GetText()).ToList().AsReadOnly();
            }

            static string ExtractFunctionName(DeclaratorContext dctx)
            {
                DirectDeclaratorContext ddctx = dctx.GetChild<DirectDeclaratorContext>(0);
                return GetDeepestDirectDeclaratorContext(ddctx).GetChild<TerminalNodeImpl>(0).GetText();

                static DirectDeclaratorContext GetDeepestDirectDeclaratorContext(DirectDeclaratorContext ddctx)
                {
                    DirectDeclaratorContext? child = ddctx.GetChild<DirectDeclaratorContext>(0);
                    return child is null ? ddctx : GetDeepestDirectDeclaratorContext(child);
                }
            }

            IReadOnlyList<(string Type, string Arg)> ExtractFunctionArguments(DeclaratorContext dctx)
            {
                // TODO
                return Enumerable.Empty<(string, string)>().ToList().AsReadOnly();
            }
        }
    }
}
