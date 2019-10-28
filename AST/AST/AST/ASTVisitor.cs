using AST.Common;

namespace AST.AST
{
    public abstract class ASTVisitor
    {
        public abstract void Visit(PrimaryExpressionNode node);
        public abstract void Visit(Literal node);
        public abstract void Visit(DeclarationNode node);
        public abstract void Visit(FunctionDefinitionNode node);

        public void Visit(ASTNode node)
            => this.Visit((dynamic)node);

        public void Visit(DeclarationList node)
        {
            foreach (DeclarationNode declarationNode in node.Declarations)
                this.Visit(declarationNode);
        }
    }
}
