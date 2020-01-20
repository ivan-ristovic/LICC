using RICC.AST.Nodes;

namespace RICC.AST.Visitors
{
    public abstract class BaseASTVisitor<TResult>
    {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
        public virtual TResult Visit(BlockStatementNode node) => default;
        public virtual TResult Visit(DeclarationSpecifiersNode node) => default;
        public virtual TResult Visit(FunctionDefinitionNode node) => default;
        public virtual TResult Visit(FunctionParameterNode node) => default;
        public virtual TResult Visit(FunctionParametersNode node) => default;
        public virtual TResult Visit(IdentifierNode node) => default;
        public virtual TResult Visit(TranslationUnitNode node) => default;
        public virtual TResult Visit(ArithmeticExpressionNode node) => default;
        public virtual TResult Visit(ArithmeticOperatorNode node) => default;
        public virtual TResult Visit(RelationalExpressionNode node) => default;
        public virtual TResult Visit(RelationalOperatorNode node) => default;
        public virtual TResult Visit(LogicExpressionNode node) => default;
        public virtual TResult Visit(BinaryLogicOperatorNode node) => default;
        public virtual TResult Visit(LiteralNode node) => default;

        // TODO
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.

        public TResult Visit(ASTNode node)
            => this.Visit((dynamic)node);
    }
}
