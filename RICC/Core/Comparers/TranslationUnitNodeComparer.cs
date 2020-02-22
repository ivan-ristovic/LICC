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
            if (x is null || y is null) {
                Log.Fatal("Null tree given to comparer - this should not happen");
                return false;
            }

            if (!this.TryMatchDeclarations(x, y)) {
                Log.Information("Failed to match found declarations to all expected declarations.");
                return false;
            }
            Log.Information("Matched all expected top-level declarations.");

            // TODO
            return true;
        }

        public int GetHashCode([DisallowNull] TranslationUnitNode obj) => obj.GetHashCode();


        private bool TryMatchDeclarations(TranslationUnitNode x, TranslationUnitNode y)
        {
            bool equal = true;

            Log.Debug("Testing declarations...");

            Dictionary<string, (DeclarationSpecifiersNode Specs, DeclaratorNode Declarator)> srcDecls = GetDeclarations(x);
            Dictionary<string, (DeclarationSpecifiersNode Specs, DeclaratorNode Declarator)> dstDecls = GetDeclarations(y);

            var declComparer = new DeclaratorNodeComparer();
            foreach ((string identifier, (DeclarationSpecifiersNode expectedSpecs, DeclaratorNode expectedDecl)) in srcDecls) {
                if (dstDecls.ContainsKey(identifier)) {
                    (DeclarationSpecifiersNode actualSpecs, DeclaratorNode actualDecl) = dstDecls[identifier];
                    if (expectedSpecs != actualSpecs)
                        CoreLog.DeclarationSpecifiersMismatch(expectedDecl, expectedSpecs, actualSpecs);
                    if (!declComparer.Equals(expectedDecl, actualDecl))
                        equal = false;
                } else {
                    CoreLog.DeclarationMissing(expectedSpecs, expectedDecl);
                    equal = false;
                }
            }

            foreach (string identifier in dstDecls.Keys.Except(srcDecls.Keys)) {
                (DeclarationSpecifiersNode specs, DeclaratorNode declarator) = dstDecls[identifier];
                CoreLog.ExtraDeclarationFound(specs, declarator);
            }

            return equal;


            static Dictionary<string, (DeclarationSpecifiersNode Specs, DeclaratorNode Declarator)> GetDeclarations(ASTNode node)
            {
                return node.Children
                    .Where(c => c is DeclarationStatementNode)
                    .Cast<DeclarationStatementNode>()
                    .SelectMany(decl => decl.DeclaratorList.Declarations, (decl, declarator) => (decl, declarator))
                    .ToDictionary(tup => tup.declarator.Identifier, tup => (tup.decl.Specifiers, tup.declarator))
                    ;
            }
        }
    }
}
