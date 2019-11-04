using System.Collections.Generic;

namespace RICC.AST.Nodes
{
    public sealed class TranslationUnitNode : ASTNode
    {
        public TranslationUnitNode(IEnumerable<ASTNode> children) 
            : base(0, children)
        {

        }
    }
}
