using LICC.AST.Nodes;
using LICC.Core.Common;

namespace LICC.Core.Comparers
{
    internal sealed class FuncParamNodeComparer : ASTNodeComparerBase<FuncParamNode>
    {
        public string? FunctionName { get; set; }
        public int Line { get; set; }


        public FuncParamNodeComparer()
        {

        }

        public FuncParamNodeComparer(string functionName, int line)
        {
            this.FunctionName = functionName;
            this.Line = line;
        }


        public override MatchIssues Compare(FuncParamNode n1param, FuncParamNode n2param)
        {
            if (this.FunctionName is null && !n1param.Specifiers.Equals(n2param.Specifiers))
                this.Issues.AddWarning(new DeclSpecsMismatchWarning(n2param.Declarator, n1param.Specifiers, n2param.Specifiers));

            this.Issues.Add(new DeclNodeComparer().Compare(n1param.Declarator, n2param.Declarator));
            return this.Issues;
        }
    }
}