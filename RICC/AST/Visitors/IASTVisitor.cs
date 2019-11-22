using RICC.AST.Nodes;

namespace RICC.AST.Visitors
{
    public interface IASTVisitor<TResult>
    {
        TResult Visit(BlockStatementNode node);
        TResult Visit(DeclarationSpecifiersNode node);
        TResult Visit(FunctionDefinitionNode node);
        TResult Visit(FunctionParameterNode node);
        TResult Visit(FunctionParametersNode node);
        TResult Visit(IdentifierNode node);
        TResult Visit(TranslationUnitNode node);

        // TODO

        TResult Visit(ASTNode node)
            => this.Visit((dynamic)node);
    }
}
