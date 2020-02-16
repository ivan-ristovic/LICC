using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core.Comparers
{
    internal sealed class TranslationUnitNodeComparer : IEqualityComparer<TranslationUnitNode>
    {
        public bool Equals([AllowNull] TranslationUnitNode x, [AllowNull] TranslationUnitNode y)
        {
            Log.Debug("Testing declaration counts...");
            IReadOnlyList<DeclarationStatementNode> srcDecls = GetDeclarations(x);
            IReadOnlyList<DeclarationStatementNode> dstDecls = GetDeclarations(y);
            IEnumerable<DeclarationStatementNode> intersection = srcDecls.Intersect(dstDecls, new DeclarationStatementNodeComparer());

            if (intersection.Count() != srcDecls.Count)
                Log.Warning("Expected {ExpectedCount} declarations, found {ActualCount}", srcDecls.Count, intersection.Count());
            Log.Debug("Declaration count matched.");


            // TODO

            return true;


            static IReadOnlyList<DeclarationStatementNode> GetDeclarations(ASTNode node)
            {
                return node.Children
                    .Where(c => c is DeclarationStatementNode)
                    .Cast<DeclarationStatementNode>()
                    .ToList()
                    .AsReadOnly()
                    ;
            }
        }

        public int GetHashCode([DisallowNull] TranslationUnitNode obj) => obj.GetHashCode();
    }
}
