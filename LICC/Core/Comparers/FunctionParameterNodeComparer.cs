using LICC.AST.Nodes;
using LICC.Core.Common;

namespace LICC.Core.Comparers
{
    internal sealed class FunctionParameterNodeComparer : ASTNodeComparerBase<FunctionParameterNode>
    {
        public string? FunctionName { get; set; }
        public int Line { get; set; }


        public FunctionParameterNodeComparer()
        {

        }

        public FunctionParameterNodeComparer(string functionName, int line)
        {
            this.FunctionName = functionName;
            this.Line = line;
        }


        public override MatchIssues Compare(FunctionParameterNode n1param, FunctionParameterNode n2param)
        {
            if (this.FunctionName is null && !n1param.Specifiers.Equals(n2param.Specifiers))
                this.Issues.AddWarning(new DeclSpecsMismatchWarning(n2param.Declarator, n1param.Specifiers, n2param.Specifiers));

            this.Issues.Add(new DeclaratorNodeComparer().Compare(n1param.Declarator, n2param.Declarator));
            return this.Issues;
        }
    }
}