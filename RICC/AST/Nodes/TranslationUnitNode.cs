using System.Collections.Generic;

namespace RICC.AST.Nodes
{
    public sealed class TranslationUnitNode : ASTNode
    {
        public TranslationUnitNode(IEnumerable<ASTNode> children) 
            : base(1, children)
        {

        }

        public TranslationUnitNode(params ASTNode[] children)
            : base(1, children)
        {

        }
    }
}
