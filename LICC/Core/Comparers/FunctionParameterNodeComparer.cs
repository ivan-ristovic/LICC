using LICC.AST.Nodes;
using LICC.Core.Common;

namespace LICC.Core.Comparers
{
    internal sealed class FunctionParameterNodeComparer : ASTNodeComparerBase<FunctionParameterNode>
    {
        public string FunctionName { get; set; }
        public int Line { get; set; }


        public FunctionParameterNodeComparer()
        {
            this.FunctionName = "<unknown_function>";
        }

        public FunctionParameterNodeComparer(string functionName, int line)
        {
            this.FunctionName = functionName;
            this.Line = line;
        }


        public override MatchIssues Compare(FunctionParameterNode n1param, FunctionParameterNode n2param)
        {
            if (!n1param.Equals(n2param))
                this.Issues.AddWarning(new DeclSpecsMismatchWarning(n1param.Declarator, n1param.DeclarationSpecifiers, n2param.DeclarationSpecifiers));

            this.Issues.Add(new DeclaratorNodeComparer().Compare(n1param.Declarator, n2param.Declarator));
            return this.Issues;
        }
    }
}