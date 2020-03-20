using System.Collections.Generic;

namespace RICC.AST.Nodes
{
    public sealed class TranslationUnitNode : ASTNode
    {
        public string? Name { get; set; }


        public TranslationUnitNode(IEnumerable<ASTNode> children)
            : base(1, children) { }

        public TranslationUnitNode(params ASTNode[] children)
            : base(1, children) { }

        public TranslationUnitNode(string name, IEnumerable<ASTNode> children)
            : base(1, children) 
        {
            this.Name = name;
        }

        public TranslationUnitNode(string name, params ASTNode[] children)
            : base(1, children)
        {
            this.Name = name;
        }
    }
}
