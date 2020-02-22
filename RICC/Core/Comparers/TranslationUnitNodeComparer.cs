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
            bool equal = true;

            Log.Debug("Testing declaration counts...");

            Dictionary<string, DeclarationSpecifiersNode> srcDecls = GetDeclarations(x);
            Dictionary<string, DeclarationSpecifiersNode> dstDecls = GetDeclarations(y);

            foreach ((string identifier, DeclarationSpecifiersNode expectedSpecs) in srcDecls) {
                if (dstDecls.ContainsKey(identifier)) {
                    DeclarationSpecifiersNode actualSpecs = dstDecls[identifier];
                    if (expectedSpecs != actualSpecs)
                        CoreLog.DeclarationSpecifiersMismatch(identifier, expectedSpecs, actualSpecs);
                } else {
                    CoreLog.DeclarationMissing(identifier, expectedSpecs);
                    equal = false;
                }
            }

            foreach (string identifier in dstDecls.Keys.Except(srcDecls.Keys))
                CoreLog.ExtraDeclarationFound(identifier, dstDecls[identifier]);

            if (equal) {
                Log.Information("Found all expected top-level declarations.");
            } else {
                Log.Information("Failed to find some of the expected top-level declarations.");
                return false;
            }

            // TODO

            return equal;


            static Dictionary<string, DeclarationSpecifiersNode> GetDeclarations(ASTNode node)
            {
                return node.Children
                    .Where(c => c is DeclarationStatementNode)
                    .Cast<DeclarationStatementNode>()
                    .SelectMany(decl => decl.DeclaratorList.Declarations, (decl, declarator) => (decl, declarator))
                    .ToDictionary(tup => tup.declarator.Identifier, tup => tup.decl.Specifiers)
                    ;
            }
        }

        public int GetHashCode([DisallowNull] TranslationUnitNode obj) => obj.GetHashCode();
    }
}
