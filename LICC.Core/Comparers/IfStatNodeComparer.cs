using System.Collections.Generic;
using LICC.AST.Nodes;
using LICC.Core.Comparers.Common;
using Serilog;

namespace LICC.Core.Comparers
{
    internal sealed class IfStatNodeComparer : ASTNodeComparerBase<IfStatNode>
    {
        private readonly Dictionary<string, DeclaredSymbol> srcSymbols = new Dictionary<string, DeclaredSymbol>();
        private readonly Dictionary<string, DeclaredSymbol> dstSymbols = new Dictionary<string, DeclaredSymbol>();


        public IfStatNodeComparer()
        {

        }

        public IfStatNodeComparer(Dictionary<string, DeclaredSymbol> srcSymbols, Dictionary<string, DeclaredSymbol> dstSymbols)
        {
            this.srcSymbols = srcSymbols;
            this.dstSymbols = dstSymbols;
        }


        public override MatchIssues Compare(IfStatNode if1, IfStatNode if2)
        {
            this.Issues.Add(new ExprNodeComparer().Compare(if1.Condition, if2.Condition));
            this.Issues.Add(new StatNodeComparer(this.srcSymbols, this.dstSymbols).Compare(if1.ThenStat, if2.ThenStat));
            if (if1.ElseStat is { } && if2.ElseStat is { })
                this.Issues.Add(new StatNodeComparer(this.srcSymbols, this.dstSymbols).Compare(if1.ElseStat, if2.ElseStat));
            else
                Log.Warning("Potential structure mismatch detected. If statemetnts do not both have else branches.");
            return this.Issues;
        }
    }
}