using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using LICC.AST.Nodes;
using LICC.AST.Visitors;
using LICC.Core.Comparers.Common;
using LICC.Core.Issues;
using Serilog;

namespace LICC.Core.Comparers
{
    internal sealed class StatNodeComparer : ASTNodeComparerBase<StatNode>
    {
        private readonly Dictionary<string, DeclaredSymbol> srcSymbols = new Dictionary<string, DeclaredSymbol>();
        private readonly Dictionary<string, DeclaredSymbol> dstSymbols = new Dictionary<string, DeclaredSymbol>();


        public StatNodeComparer()
        {

        }

        public StatNodeComparer(Dictionary<string, DeclaredSymbol> srcSymbols, Dictionary<string, DeclaredSymbol> dstSymbols)
        {
            this.srcSymbols = srcSymbols;
            this.dstSymbols = dstSymbols;
        }


        public override MatchIssues Compare(StatNode s1, StatNode s2)
        {
            if (s1 is BlockStatNode b1 && s2 is BlockStatNode b2) {
                this.Issues.Add(new BlockStatNodeComparer(this.srcSymbols, this.dstSymbols).Compare(b1, b2));
            } else if (s1 is IfStatNode if1 && s2 is IfStatNode if2) {
                this.Issues.Add(new IfStatNodeComparer(this.srcSymbols, this.dstSymbols).Compare(if1, if2));
            } else if (s1 is JumpStatNode j1 && s2 is JumpStatNode j2) {
                if (j1.Type != j2.Type)
                    throw new NotImplementedException("JumpStatNode mismatch");
                if (j1.ReturnExpr is { } && j2.ReturnExpr is { })
                    this.Issues.Add(new ExprNodeComparer().Compare(j1.ReturnExpr, j2.ReturnExpr));
            } else if (s1 is CompStatNode&& s2 is CompStatNode) {
                throw new NotImplementedException("Comparing of complex blocks other than if statements is not yet implemented.");
            } else {
                foreach ((ASTNode c1, ASTNode c2) in s1.Children.Zip(s2.Children)) {
                    if (c1 is StatNode cs1 && c2 is StatNode cs2 && cs1 != cs2)
                        this.Issues.AddWarning(new StatMismatchWarning(cs2.Line, cs1, cs2));
                    else if (c1 is ExprNode ce1 && c2 is ExprNode ce2 && ce1 != ce2) {
                        UpdateSymbol(ce1, this.srcSymbols);
                        UpdateSymbol(ce2, this.dstSymbols);
                        this.Issues.AddWarning(new ExprNodeMismatchWarning(ce2.Line, ce1, ce2));

                        static void UpdateSymbol(ExprNode expr, Dictionary<string, DeclaredSymbol> symbols)
                        {
                            if (expr is AssignExprNode ae && ae.LeftOperand is IdNode id) {
                                var sym = symbols[id.Identifier] as DeclaredVariableSymbol;
                                if (sym is null)
                                    return;
                                try {
                                    sym.SymbolicInitializer = new SymbolicExpressionBuilder(ae.RightOperand).Parse();
                                } catch (Exception e) {
                                    Log.Debug(e, "Failed to parse expression: {Expr}", ae.RightOperand.GetText());
                                }
                            }
                        }
                    }
                }
            }
            return this.Issues;
        }
    }
}