using System.Collections.Generic;
using System.Linq;
using System.Text;
using RICC.AST.Nodes.Common;

namespace RICC.AST.Nodes
{
    public abstract class StatementNode : ASTNode
    {
        protected StatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null) 
            : base(line, children, parent)
        {

        }

        protected StatementNode(int line, ASTNode? parent = null, params ASTNode[] children)
            : base(line, parent, children)
        {

        }


        public override string GetText() => $"{base.GetText()};";
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

        protected SimpleStatementNode(int line, ASTNode? parent = null, params ASTNode[] children)
            : base(line, parent, children)
        {

        }
    }

    public abstract class CompoundStatementNode : StatementNode
    {
        protected CompoundStatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        protected CompoundStatementNode(int line, ASTNode? parent = null, params ASTNode[] children)
            : base(line, parent, children)
        {

        }
    }

    public sealed class BlockStatementNode : CompoundStatementNode
    {
        public BlockStatementNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        public BlockStatementNode(int line, ASTNode? parent = null, params ASTNode[] children)
            : base(line, parent, children)
        {

        }


        public override string GetText() => $"{{ {string.Join(" ", this.Children.Select(c => c.GetText()))} }}";
    }

    public sealed class ExpressionStatementNode : SimpleStatementNode
    {
        public ExpressionNode Expression => this.Children.First().As<ExpressionNode>();


        public ExpressionStatementNode(int line, ExpressionNode expr, ASTNode? parent = null)
            : base(line, parent, expr)
        {

        }
    }

    public sealed class IfStatementNode : CompoundStatementNode
    {
        public ExpressionNode Condition => this.Children[0].As<ExpressionNode>();
        public StatementNode ThenStatement => this.Children[1].As<StatementNode>();
        public StatementNode? ElseStatement => this.Children.Count > 2 ? this.Children[2].As<StatementNode>() : null;


        public IfStatementNode(int line, LogicExpressionNode condition, StatementNode thenBlock, StatementNode? elseBlock = null, ASTNode? parent = null)
            : base(line, elseBlock is null ? new ASTNode[] { condition, thenBlock } : new ASTNode[] { condition, thenBlock, elseBlock }, parent)
        {

        }

        public IfStatementNode(int line, RelationalExpressionNode condition, StatementNode thenBlock, StatementNode? elseBlock = null, ASTNode? parent = null)
            : base(line, elseBlock is null ? new ASTNode[] { condition, thenBlock } : new ASTNode[] { condition, thenBlock, elseBlock }, parent)
        {

        }


        public override string GetText() 
            => $"if ({this.Condition.GetText()}) {this.ThenStatement.GetText()} {(this.ElseStatement is null ? "" : $"else {this.ElseStatement.GetText()}")}";
    }

    public sealed class JumpStatementNode : SimpleStatementNode
    {
        public JumpStatementType Type { get; set; }
        public ExpressionNode? ReturnExpression { get; set; }
        public IdentifierNode? GotoLabel{ get; set; }


        public JumpStatementNode(int line, JumpStatementType type, ExpressionNode? @return = null, IdentifierNode? @goto = null, ASTNode? parent = null) 
            : base(line, parent)
        {
            this.Type = type;
            this.ReturnExpression = @return;
            this.GotoLabel = @goto;
        }


        public override string GetText()
        {
            var sb = new StringBuilder(this.Type.ToStringToken());
            if (this.Type == JumpStatementType.Return && this.ReturnExpression is { })
                sb.Append(' ').Append(this.ReturnExpression.GetText());
            else if(this.Type == JumpStatementType.Goto && this.GotoLabel is { })
                sb.Append(' ').Append(this.GotoLabel.GetText());
            sb.Append(';');
            return sb.ToString();
        }
    }

    public sealed class LabeledStatementNode : SimpleStatementNode
    {
        public string Label { get; }
        public StatementNode Statement => this.Children.First().As<StatementNode>();


        public LabeledStatementNode(int line, string label, StatementNode statement, ASTNode? parent = null)
            : base(line, parent, statement)
        {
            this.Label = label;
        }


        public override string GetText() => $"{this.Label}: {this.Statement.GetText()}";
    }
}
