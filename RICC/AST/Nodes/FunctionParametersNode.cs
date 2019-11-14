using System.Collections.Generic;

namespace RICC.AST.Nodes
{
    public sealed class FunctionParametersNode : ASTNode
    {
        public FunctionParametersNode(int line, IEnumerable<ASTNode> @params, ASTNode? parent = null) 
            : base(line, @params, parent)
        {

        }
    }
}
