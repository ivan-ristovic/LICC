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
            Dictionary<string, DeclaredSymbol> srcSymbols = this.GetSymbols(n1);
            Dictionary<string, DeclaredSymbol> dstSymbols = this.GetSymbols(n2);

            this.TestDeclarations(srcSymbols, dstSymbols);

            // TODO
            return this.Issues;
        }


        private Dictionary<string, DeclaredSymbol> GetSymbols(BlockStatementNode node)
        {
            var symbols = new Dictionary<string, DeclaredSymbol>();

            IEnumerable<DeclarationStatementNode> declStats = node.Children
                .Where(c => c is DeclarationStatementNode)
                .Cast<DeclarationStatementNode>()
                ;
            foreach (DeclarationStatementNode declStat in declStats) {
                foreach (DeclaratorNode decl in declStat.DeclaratorList.Declarations) {
                    var symbol = DeclaredSymbol.From(declStat.Specifiers, decl);
                    if (symbol is DeclaredFunction df && symbols.ContainsKey(df.Identifier)) {
                        if (!df.AddOverload(df.FunctionDeclarators.Single()))
                            throw new CompilationException($"Multiple overloads with same parameters found for function: {df.Identifier}", decl.Line);
                    }
                    symbols.Add(decl.Identifier, symbol);
                }
            }

            return symbols;
        }

        private void TestDeclarations(Dictionary<string, DeclaredSymbol> srcSymbols, Dictionary<string, DeclaredSymbol> dstSymbols)
        {
            Log.Debug("Testing declarations...");

            foreach (DeclaredSymbol srcVar in srcSymbols.Select(kvp => kvp.Value)) {
                if (!dstSymbols.ContainsKey(srcVar.Identifier))
                    this.Issues.AddWarning(new MissingDeclarationWarning(srcVar.Specifiers, srcVar.Declarator));
                DeclaredSymbol dstVar = dstSymbols[srcVar.Identifier];

                if (srcVar.Specifiers != dstVar.Specifiers)
                    this.Issues.AddWarning(new DeclSpecsMismatchWarning(srcVar.Declarator, srcVar.Specifiers, dstVar.Specifiers));

                var declComparer = new DeclaratorNodeComparer(srcVar, dstVar);
                this.Issues.Add(declComparer.Compare(srcVar.Declarator, dstVar.Declarator));
            }

            foreach (string identifier in dstSymbols.Keys.Except(srcSymbols.Keys)) {
                DeclaredSymbol extra = dstSymbols[identifier];
                this.Issues.AddWarning(new ExtraDeclarationWarning(extra.Specifiers, extra.Declarator));
            }

            if (!this.Issues.NoSeriousIssues)
                Log.Error("Failed to match found declarations to all expected declarations.");
            else
                Log.Debug("Matched all expected top-level declarations.");
        }
    }
}
