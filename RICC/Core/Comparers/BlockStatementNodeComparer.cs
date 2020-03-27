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
            Dictionary<string, DeclaredSymbol> srcSymbols = this.GetVars(n1);
            Dictionary<string, DeclaredSymbol> dstSymbols = this.GetVars(n2);

            this.TestDeclarations(srcSymbols, dstSymbols);

            // TODO
            return this.Issues;
        }


        private Dictionary<string, DeclaredSymbol> GetVars(BlockStatementNode node)
        {
            var vars = new Dictionary<string, DeclaredSymbol>();

            foreach (DeclarationStatementNode declStat in ASTNodeOperations.ExtractDeclarations(node)) {
                foreach (DeclaratorNode decl in declStat.DeclaratorList.Declarations) {
                    DeclaredSymbol v = decl switch
                    {
                        VariableDeclaratorNode var => new DeclaredVariable(decl.Identifier, declStat.Specifiers, var, var.Initializer),
                        ArrayDeclaratorNode arr => new DeclaredArray(decl.Identifier, declStat.Specifiers, arr, arr.SizeExpression, arr.Initializer),
                        FunctionDeclaratorNode f => new DeclaredFunction(decl.Identifier, declStat.Specifiers, f),
                        _ => throw new NotImplementedException(),
                    };
                    if (v is DeclaredFunction df && vars.ContainsKey(df.Identifier)) {
                        if (!df.AddOverload(df.FunctionDeclarators.Single()))
                            throw new CompilationException($"Multiple overloads with same parameters found for function: {df.Identifier}", decl.Line);
                    }
                    vars.Add(decl.Identifier, v);
                }
            }

            return vars;
        }

        private void TestDeclarations(Dictionary<string, DeclaredSymbol> srcSymbols, Dictionary<string, DeclaredSymbol> dstSymbols)
        {
            Log.Debug("Testing declarations...");

            var declComparer = new DeclaratorNodeComparer();
            foreach (DeclaredSymbol srcVar in srcSymbols.Select(kvp => kvp.Value)) {
                if (!dstSymbols.ContainsKey(srcVar.Identifier))
                    this.Issues.AddWarning(new MissingDeclarationWarning(srcVar.Specifiers, srcVar.Declarator));
                DeclaredSymbol dstVar = dstSymbols[srcVar.Identifier];

                if (srcVar.Specifiers != dstVar.Specifiers)
                    this.Issues.AddWarning(new DeclSpecsMismatchWarning(srcVar.Declarator, srcVar.Specifiers, dstVar.Specifiers));
                declComparer.Compare(srcVar.Declarator, dstVar.Declarator);
            }
            this.Issues.Add(declComparer.Issues);

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
