using LICC.AST.Nodes;
using LICC.AST.Visitors;
using LICC.Core.Issues;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LICC.Core.Comparers
{
    internal sealed class ExprNodeComparer : ASTNodeComparerBase<ExprNode>
    {
        public ExprNodeComparer()
        {

        }


        public override MatchIssues Compare(ExprNode e1, ExprNode e2)
        {
            Expr? sym1 = new SymbolicExpressionBuilder(e1).Parse();
            Expr? sym2 = new SymbolicExpressionBuilder(e2).Parse();
            if (!sym1?.ToString()?.Equals(sym2?.ToString()) ?? true)
                this.Issues.AddWarning(new ExprNodeMismatchWarning(e2.Line, e1, e2));
            return this.Issues;
        }
    }
}