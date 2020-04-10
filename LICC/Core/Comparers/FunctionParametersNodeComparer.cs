using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LICC.AST.Nodes;
using LICC.Core.Common;

namespace LICC.Core.Comparers
{
    internal sealed class FunctionParametersNodeComparer : ASTNodeComparerBase<FunctionParametersNode>
    {
        public string FunctionName { get; set; }
        public int Line { get; set; }


        public FunctionParametersNodeComparer()
        {
            this.FunctionName = "<unknown_function>";
        }

        public FunctionParametersNodeComparer(string fname, int line)
        {
            this.FunctionName = fname;
            this.Line = line;
        }


        public override MatchIssues Compare(FunctionParametersNode n1, FunctionParametersNode n2)
        {
            var n1Params = n1.Parameters.ToList();
            var n2Params = n2.Parameters.ToList();
            if (n1Params.Count != n2Params.Count)
                this.Issues.AddWarning(new ParameterMismatchWarning(this.FunctionName, this.Line));

            foreach ((FunctionParameterNode n1param, FunctionParameterNode n2param) in n1Params.Zip(n2Params))
                this.Issues.Add(new FunctionParameterNodeComparer(this.FunctionName, this.Line).Compare(n1param, n2param));

            return this.Issues;
        }
    }
}
