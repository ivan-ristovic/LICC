using System.Collections.Generic;
using System.Linq;
using LICC.AST.Nodes;
using LICC.Core.Common;
using LICC.Core.Comparers.Common;
using Serilog;

namespace LICC.Core.Comparers
{
    internal abstract class ASTNodeComparerBase<T> : IASTNodeComparer
        where T : ASTNode
    {
        public MatchIssues Issues { get; } = new MatchIssues();

        
        public abstract MatchIssues Compare(T n1, T n2);
        
        public virtual MatchIssues Compare(ASTNode n1, ASTNode n2) => this.Compare(n1.As<T>(), n2.As<T>());


        protected void CompareSymbols(Dictionary<string, DeclaredSymbol> srcSymbols, Dictionary<string, DeclaredSymbol> dstSymbols)
        {
            Log.Debug("Testing declarations...");

            foreach (DeclaredSymbol srcVar in srcSymbols.Select(kvp => kvp.Value)) {
                if (!dstSymbols.ContainsKey(srcVar.Identifier)) {
                    this.Issues.AddWarning(new MissingDeclarationWarning(srcVar.Specifiers, srcVar.Declarator));
                    continue;
                }
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
