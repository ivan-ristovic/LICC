using System;
using System.Collections.Generic;
using System.Linq;
using LICC.AST.Nodes;
using LICC.Core.Common;
using LICC.Core.Comparers.Common;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LICC.Core.Comparers
{
    internal sealed class FuncDefNodeComparer : ASTNodeComparerBase<FuncDefNode>
    {
        private readonly Dictionary<string, DeclaredSymbol> srcGlobals = new Dictionary<string, DeclaredSymbol>();
        private readonly Dictionary<string, DeclaredSymbol> dstGlobals = new Dictionary<string, DeclaredSymbol>();


        public FuncDefNodeComparer()
        {

        }

        public FuncDefNodeComparer(Dictionary<string, DeclaredSymbol> srcSymbols, Dictionary<string, DeclaredSymbol> dstSymbols)
        {
            this.srcGlobals = srcSymbols;
            this.dstGlobals = dstSymbols;
        }


        public override MatchIssues Compare(FuncDefNode n1, FuncDefNode n2) 
        {
            this.Issues.Add(new DeclNodeComparer().Compare(n1.Declarator, n2.Declarator));
            if (n1.ParametersNode is { } && n2.ParametersNode is { })
                this.Issues.Add(new FuncParamsNodeComparer().Compare(n1.ParametersNode, n2.ParametersNode));
            else if (n1.ParametersNode is { } || n2.ParametersNode is { })
                this.Issues.AddWarning(new ParameterMismatchWarning(n1.Identifier, n2.Line, n1.IsVariadic != n2.IsVariadic));

            Dictionary<string, DeclaredSymbol> srcOuterVars = this.AddParameterSymbols(this.srcGlobals, n1);
            Dictionary<string, DeclaredSymbol> dstOuterVars = this.AddParameterSymbols(this.dstGlobals, n2);

            this.Issues.Add(new BlockStatNodeComparer(srcOuterVars, dstOuterVars).Compare(n1.Definition, n2.Definition));
            return this.Issues;
        }


        private Dictionary<string, DeclaredSymbol> AddParameterSymbols(Dictionary<string, DeclaredSymbol> srcGlobals, FuncDefNode n1)
        {
            if (!n1.Parameters?.Any() ?? true)
                return srcGlobals;
                
            var allSymbols = new Dictionary<string, DeclaredSymbol>(srcGlobals);

            IEnumerable<DeclaredSymbol> symbols = n1.Parameters.Select(p => DeclaredSymbol.From(p.Specifiers, p.Declarator));
            foreach (DeclaredSymbol symbol in symbols) {
                string init = $"param_{symbol.Identifier}";
                if (symbol is DeclaredVariableSymbol varSymbol) {
                    varSymbol.SymbolicInitializer = Expr.Variable(init);
                    allSymbols.Add(varSymbol.Identifier, varSymbol); // TODO same as global?
                } else {
                    allSymbols.Add(symbol.Identifier, symbol); // TODO same as global?
                }
                // TODO array
            }

            return allSymbols;
        }
    }
}
