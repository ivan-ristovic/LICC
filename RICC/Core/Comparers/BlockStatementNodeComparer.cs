using System;
using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes;
using RICC.Core.Common;
using RICC.Core.Comparers.Common;
using RICC.Exceptions;
using Serilog;

namespace RICC.Core.Comparers
{
    internal sealed class BlockStatementNodeComparer : ASTNodeComparerBase<BlockStatementNode>
    {
        public override MatchIssues Compare(BlockStatementNode n1, BlockStatementNode n2)
        {
            Dictionary<string, DeclaredSymbol> srcSymbols = this.GetDeclaredSymbols(n1);
            Dictionary<string, DeclaredSymbol> dstSymbols = this.GetDeclaredSymbols(n2);

            this.CompareSymbols(srcSymbols, dstSymbols);

            // TODO
            return this.Issues;
        }


        private Dictionary<string, DeclaredSymbol> GetDeclaredSymbols(BlockStatementNode node)
        {
            var symbols = new Dictionary<string, DeclaredSymbol>();

            IEnumerable<DeclarationStatementNode> declStats = node.Children
                .Where(c => c is DeclarationStatementNode)
                .Cast<DeclarationStatementNode>()
                ;
            foreach (DeclarationStatementNode declStat in declStats) {
                foreach (DeclaratorNode decl in declStat.DeclaratorList.Declarations) {
                    var symbol = DeclaredSymbol.From(declStat.Specifiers, decl);
                    if (symbols.ContainsKey(decl.Identifier)) {
                        if (symbol is DeclaredFunctionSymbol overload && symbols[decl.Identifier] is DeclaredFunctionSymbol df) {
                            if (!df.AddOverload(overload.FunctionDeclarators.Single()))
                                throw new CompilationException($"Multiple overloads with same parameters found for function: {df.Identifier}", decl.Line);
                        } else {
                            throw new CompilationException($"Same identifier found in multiple declarations: {decl.Identifier}", decl.Line);
                        }
                    }
                    symbols.Add(decl.Identifier, symbol);
                }
            }

            return symbols;
        }
    }
}
