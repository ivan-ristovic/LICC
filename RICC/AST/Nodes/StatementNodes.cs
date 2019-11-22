using System.Collections.Generic;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
    public abstract class StatementNode : ASTNode
    {
        protected StatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null) 
            : base(line, children, parent)
        {

        }

        protected StatementNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }
    }

    public sealed class EmptyStatementNode : StatementNode
    {
        public EmptyStatementNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }
    }

    public abstract class SimpleStatementNode : StatementNode
    {
        protected SimpleStatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        protected SimpleStatementNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }
    }

    public abstract class CompoundStatementNode : StatementNode
    {
        protected CompoundStatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        protected CompoundStatementNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }
    }

    public sealed class IfStatementNode : CompoundStatementNode
    {
        public LogicExpressionNode Condition => this.Children[0].As<LogicExpressionNode>();
        public BlockStatementNode ThenBlock => this.Children[1].As<BlockStatementNode>();
        public BlockStatementNode? ElseBlock => this.Children.Count > 2 ? this.Children[2].As<BlockStatementNode>() : null;


        public IfStatementNode(int line, BlockStatementNode thenBlock, BlockStatementNode? elseBlock = null, ASTNode? parent = null)
            : base(line, elseBlock is null ? new[] { thenBlock } : new[] { thenBlock, elseBlock }, parent)
        {

        }
    }
}
