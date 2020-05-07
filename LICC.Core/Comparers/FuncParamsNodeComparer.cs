using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LICC.AST.Nodes;
using LICC.Core.Issues;

namespace LICC.Core.Comparers
{
    internal sealed class FuncParamsNodeComparer : ASTNodeComparerBase<FuncParamsNode>
    {
        public string FunctionName { get; set; }
        public int Line { get; set; }


        public FuncParamsNodeComparer()
        {
            this.FunctionName = "<unknown_function>";
        }

        public FuncParamsNodeComparer(string fname, int line)
        {
            this.FunctionName = fname;
            this.Line = line;
        }


        public override MatchIssues Compare(FuncParamsNode n1, FuncParamsNode n2)
        {
            var n1Params = n1.Parameters.ToList();
            var n2Params = n2.Parameters.ToList();
            if (n1Params.Count != n2Params.Count)
                this.Issues.AddWarning(new ParameterMismatchWarning(this.FunctionName, this.Line));

            foreach ((FuncParamNode n1param, FuncParamNode n2param) in n1Params.Zip(n2Params))
                this.Issues.Add(new FuncParamNodeComparer(this.FunctionName, this.Line).Compare(n1param, n2param));

            return this.Issues;
        }
    }
}
