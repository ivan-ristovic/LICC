using RICC.AST.Nodes;

namespace RICC.AST.Visitors
{
    public abstract class BaseASTVisitor<TResult>
    {
        public virtual TResult Visit(BlockStatementNode node) => default!;
        public virtual TResult Visit(DeclarationSpecifiersNode node) => default!;
        public virtual TResult Visit(FunctionDefinitionNode node) => default!;
        public virtual TResult Visit(FunctionParameterNode node) => default!;
        public virtual TResult Visit(FunctionParametersNode node) => default!;
        public virtual TResult Visit(IdentifierNode node) => default!;
        public virtual TResult Visit(TranslationUnitNode node) => default!;
        public virtual TResult Visit(ArithmeticExpressionNode node) => default!;
        public virtual TResult Visit(ArithmeticOperatorNode node) => default!;
        public virtual TResult Visit(RelationalExpressionNode node) => default!;
        public virtual TResult Visit(RelationalOperatorNode node) => default!;
        public virtual TResult Visit(LogicExpressionNode node) => default!;
        public virtual TResult Visit(UnaryExpressionNode node) => default!;
        public virtual TResult Visit(IncrementExpressionNode node) => default!;
        public virtual TResult Visit(DecrementExpressionNode node) => default!;
        public virtual TResult Visit(BinaryLogicOperatorNode node) => default!;
        public virtual TResult Visit(UnaryOperatorNode node) => default!;
        public virtual TResult Visit(LiteralNode node) => default!;
        public virtual TResult Visit(NullLiteralNode node) => default!;


        public TResult Visit(ASTNode node)
            => this.Visit((dynamic)node);
    }
}
