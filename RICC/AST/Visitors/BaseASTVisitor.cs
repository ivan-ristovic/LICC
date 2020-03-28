using RICC.AST.Nodes;

namespace RICC.AST.Visitors
{
    public abstract class BaseASTVisitor<TResult>
    {
        public virtual TResult VisitChildren(ASTNode node)
        {
            TResult result = this.DefaultResult;
            foreach (ASTNode child in node.Children) {
                if (!this.ShouldVisitNextChild(node, result))
                    break;
                TResult childResult = this.Visit(child);
                result = this.AggregateResult(result, childResult);
            }
            return result;
        }

        public virtual TResult Visit(ASTNode node) => this.Visit((dynamic)node);
        public virtual TResult Visit(ArithmeticExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(ArithmeticOperatorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(ArrayAccessExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(ArrayDeclaratorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(ArrayInitializerListNode node) => this.VisitChildren(node);
        public virtual TResult Visit(AssignmentExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(AssignmentOperatorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(BinaryLogicOperatorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(BlockStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(DeclarationSpecifiersNode node) => this.VisitChildren(node);
        public virtual TResult Visit(DeclarationStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(DeclaratorListNode node) => this.VisitChildren(node);
        public virtual TResult Visit(DecrementExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(DictionaryDeclaratorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(DictionaryEntryNode node) => this.VisitChildren(node);
        public virtual TResult Visit(DictionaryInitializerNode node) => this.VisitChildren(node);
        public virtual TResult Visit(EmptyStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(ExpressionListNode node) => this.VisitChildren(node);
        public virtual TResult Visit(ExpressionStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(ForStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(FunctionCallExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(FunctionDeclaratorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(FunctionDefinitionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(FunctionParameterNode node) => this.VisitChildren(node);
        public virtual TResult Visit(FunctionParametersNode node) => this.VisitChildren(node);
        public virtual TResult Visit(IdentifierNode node) => this.VisitChildren(node);
        public virtual TResult Visit(IdentifierListNode node) => this.VisitChildren(node);
        public virtual TResult Visit(IfStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(IncrementExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(JumpStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(LabeledStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(LambdaFunctionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(LiteralNode node) => this.VisitChildren(node);
        public virtual TResult Visit(LogicExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(NullLiteralNode node) => this.VisitChildren(node);
        public virtual TResult Visit(RelationalExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(RelationalOperatorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(ThrowStatementNode node) => this.VisitChildren(node);
        public virtual TResult Visit(SourceComponentNode node) => this.VisitChildren(node);
        public virtual TResult Visit(UnaryExpressionNode node) => this.VisitChildren(node);
        public virtual TResult Visit(UnaryOperatorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(VariableDeclaratorNode node) => this.VisitChildren(node);
        public virtual TResult Visit(WhileStatementNode node) => this.VisitChildren(node);

        protected virtual TResult DefaultResult => default!;
        protected virtual TResult AggregateResult(TResult aggregate, TResult nextResult) => nextResult;
        protected virtual bool ShouldVisitNextChild(ASTNode node, TResult currentResult) => true;
    }
}
