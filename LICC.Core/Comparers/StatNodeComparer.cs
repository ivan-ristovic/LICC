using System;
using System.Collections.Generic;
using LICC.AST.Nodes;
using LICC.Core.Comparers.Common;
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
            } else {
                throw new NotImplementedException("Comparing of complex blocks other than if statements is not yet implemented.");
            }
            return this.Issues;
        }
    }
}