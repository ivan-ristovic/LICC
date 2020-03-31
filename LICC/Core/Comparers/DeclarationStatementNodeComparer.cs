using System.Collections.Generic;
using System.Linq;
using LICC.AST.Nodes;
using LICC.Core.Comparers.Common;
using LICC.Exceptions;

namespace LICC.Core.Comparers
{
    internal sealed class DeclarationStatementNodeComparer : ASTNodeComparerBase<DeclarationStatementNode>
    {
        public override MatchIssues Compare(DeclarationStatementNode n1, DeclarationStatementNode n2)
        {
            Dictionary<string, DeclaredSymbol> srcSymbols = this.GetDeclaredSymbols(n1);
            Dictionary<string, DeclaredSymbol> dstSymbols = this.GetDeclaredSymbols(n2);
            this.CompareSymbols(srcSymbols, dstSymbols);
            return this.Issues;
        }


        private Dictionary<string, DeclaredSymbol> GetDeclaredSymbols(DeclarationStatementNode node)
        {
            var symbols = new Dictionary<string, DeclaredSymbol>();

            foreach (DeclaratorNode decl in node.DeclaratorList.Declarations) {
                var symbol = DeclaredSymbol.From(node.Specifiers, decl);
                if (symbol is DeclaredFunctionSymbol df && symbols.ContainsKey(df.Identifier)) {
                    if (!df.AddOverload(df.FunctionDeclarators.Single()))
                        throw new SemanticErrorException($"Multiple overloads with same parameters found for function: {df.Identifier}", decl.Line);
                }
                symbols.Add(decl.Identifier, symbol);
            }

            return symbols;
        }
    }
}
