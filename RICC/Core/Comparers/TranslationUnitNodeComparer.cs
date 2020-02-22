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

            Dictionary<DeclarationSpecifiersNode, List<DeclaratorNode>> srcDecls = GetDeclarations(x);
            Dictionary<DeclarationSpecifiersNode, List<DeclaratorNode>> dstDecls = GetDeclarations(y);

            foreach ((DeclarationSpecifiersNode specs, List<DeclaratorNode> expected) in srcDecls) {
                List<DeclaratorNode> actual = dstDecls.GetValueOrDefault(specs, Enumerable.Empty<DeclaratorNode>().ToList())!;
                
                IEnumerable<DeclaratorNode> missing = expected.Except(actual);
                IEnumerable<DeclaratorNode> extra = actual.Except(expected);

                Log.Debug(@"src: ""{Specs}"": {Declarators}", specs, expected);
                Log.Debug(@"dst: ""{Specs}"": {Declarators}", specs, actual);

                foreach (DeclaratorNode decl in missing) 
                    Log.Warning("Missing declaration for {Specs} {Symbol}, declared at line {Line}", specs, decl.Identifier, decl.Line);

                foreach (DeclaratorNode decl in extra) 
                    Log.Warning("No declaration found in source for {Specs} {Symbol}, declared at line {Line}", specs, decl.Identifier, decl.Line);

                if (missing.Any())
                    equal = false;
            }

            if (equal) {
                Log.Information("Found all expected declarations.");
            } else {
                Log.Information("Failed to find some of the expected declarations.");
                return false;
            }

            // TODO

            return equal;


            static Dictionary<DeclarationSpecifiersNode, List<DeclaratorNode>> GetDeclarations(ASTNode node)
            {
                return node.Children
                    .Where(c => c is DeclarationStatementNode)
                    .Cast<DeclarationStatementNode>()
                    .GroupBy(decl => decl.Specifiers, decl => decl.DeclaratorList)
                    .ToDictionary(
                        g => g.Key, 
                        g => g.SelectMany(x => x.Declarations).Distinct().ToList()
                    );
            }
        }

        public int GetHashCode([DisallowNull] TranslationUnitNode obj) => obj.GetHashCode();
    }
}
