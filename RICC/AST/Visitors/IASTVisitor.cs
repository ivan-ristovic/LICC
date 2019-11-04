using RICC.AST.Nodes;

namespace RICC.AST.Visitors
{
    public interface IASTVisitor<TResult>
    {
        TResult Visit(TranslationUnitNode node);
        TResult Visit(FunctionDefinitionNode node);
        /*
        TResult Visit(BlockStatementNode node);
        TResult Visit(DeclarationListNode node);
        TResult Visit(FunctionDeclarationNode node);
        TResult Visit(LiteralNode node);
        TResult Visit(VariableDeclarationNode node);
        */

        TResult Visit(ASTNode node)
            => this.Visit((dynamic)node);

        /*
        T Visit(DeclarationList node)
        {
            foreach (DeclarationNode declarationNode in node.Declarations)
                this.Visit(declarationNode);
        }
        */
    }
}
