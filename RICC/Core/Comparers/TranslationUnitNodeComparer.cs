using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RICC.AST.Nodes;
using RICC.Core.Common;
using Serilog;

namespace RICC.Core.Comparers
{
    internal sealed class TranslationUnitNodeComparer : IASTNodeComparer<TranslationUnitNode>
    {
        public ComparerResult Result { get; } = new ComparerResult();


        public ComparerResult Compare(TranslationUnitNode n1, TranslationUnitNode n2) 
        {
            this.TryMatchDeclarations(n1, n2);
            if (!this.Result.Success) {
                Log.Information("Failed to match found declarations to all expected declarations.");
                return this.Result;
            }
            Log.Information("Matched all expected top-level declarations.");

            // TODO
            return this.Result;
        }


        private void TryMatchDeclarations(TranslationUnitNode n1, TranslationUnitNode n2)
        {
            Log.Debug("Testing declarations...");

            Dictionary<string, (DeclarationSpecifiersNode Specs, DeclaratorNode Declarator)> srcDecls = GetDeclarations(n1);
            Dictionary<string, (DeclarationSpecifiersNode Specs, DeclaratorNode Declarator)> dstDecls = GetDeclarations(n2);

            var declComparer = new DeclaratorNodeComparer();
            foreach ((string identifier, (DeclarationSpecifiersNode expectedSpecs, DeclaratorNode expectedDecl)) in srcDecls) {
                if (dstDecls.ContainsKey(identifier)) {
                    (DeclarationSpecifiersNode actualSpecs, DeclaratorNode actualDecl) = dstDecls[identifier];
                    if (expectedSpecs != actualSpecs)
                        this.Result.WithWarning(new DeclSpecsMismatchWarning(expectedDecl, expectedSpecs, actualSpecs));
                    declComparer.Compare(expectedDecl, actualDecl);
                } else {
                    this.Result.WithWarning(new MissingDeclarationWarning(expectedSpecs, expectedDecl));
                }
            }
            this.Result.WithResult(declComparer.Result);

            foreach (string identifier in dstDecls.Keys.Except(srcDecls.Keys)) {
                (DeclarationSpecifiersNode specs, DeclaratorNode decl) = dstDecls[identifier];
                this.Result.WithWarning(new ExtraDeclarationWarning(specs, decl));
            }


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
