using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RICC.AST.Nodes.Common;

namespace RICC.AST.Nodes
{
    public abstract class StatementNode : ASTNode
    {
        protected StatementNode(int line, IEnumerable<ASTNode> children) 
            : base(line, children)
        {

        }

        protected StatementNode(int line, params ASTNode[] children)
            : base(line, children)
        {

        }


        public override string GetText() => $"{base.GetText()};";
    }

    public sealed class EmptyStatementNode : StatementNode
    {
        public EmptyStatementNode(int line)
            : base(line)
        {

        }
    }

    public abstract class SimpleStatementNode : StatementNode
    {
        protected SimpleStatementNode(int line, IEnumerable<ASTNode> children)
            : base(line, children)
        {

        }

        protected SimpleStatementNode(int line, params ASTNode[] children)
            : base(line, children)
        {

        }
    }

    public abstract class CompoundStatementNode : StatementNode
    {
        protected CompoundStatementNode(int line, IEnumerable<ASTNode> children)
            : base(line, children)
        {

        }

        protected CompoundStatementNode(int line, params ASTNode[] children)
            : base(line, children)
        {

        }
    }

    public sealed class BlockStatementNode : CompoundStatementNode
    {
        public BlockStatementNode(int line, IEnumerable<ASTNode> children)
            : base(line, children)
        {

        }

        public BlockStatementNode(int line, params ASTNode[] children)
            : base(line, children)
        {

        }


        public override string GetText() => $"{{ {string.Join(" ", this.Children.Select(c => c.GetText()))} }}";
    }

    public sealed class ExpressionStatementNode : SimpleStatementNode
    {
        [JsonIgnore]
        public ExpressionNode Expression => this.Children.First().As<ExpressionNode>();


        public ExpressionStatementNode(int line, ExpressionNode expr)
            : base(line, expr)
        {

        }
    }

    public sealed class IfStatementNode : CompoundStatementNode
    {
        [JsonIgnore]
        public ExpressionNode Condition => this.Children[0].As<ExpressionNode>();

        [JsonIgnore]
        public StatementNode ThenStatement => this.Children[1].As<StatementNode>();
        
        [JsonIgnore]
        public StatementNode? ElseStatement => this.Children.Count > 2 ? this.Children[2].As<StatementNode>() : null;


        public IfStatementNode(int line, LogicExpressionNode condition, StatementNode thenBlock, StatementNode? elseBlock = null)
            : base(line, elseBlock is null ? new ASTNode[] { condition, thenBlock } : new ASTNode[] { condition, thenBlock, elseBlock })
        {

        }

        public IfStatementNode(int line, RelationalExpressionNode condition, StatementNode thenBlock, StatementNode? elseBlock = null)
            : base(line, elseBlock is null ? new ASTNode[] { condition, thenBlock } : new ASTNode[] { condition, thenBlock, elseBlock })
        {

        }


        public override string GetText() 
            => $"if ({this.Condition.GetText()}) {this.ThenStatement.GetText()} {(this.ElseStatement is null ? "" : $"else {this.ElseStatement.GetText()}")}";
    }

    public sealed class JumpStatementNode : SimpleStatementNode
    {
        public JumpStatementType Type { get; set; }
        
        [JsonIgnore]
        public ExpressionNode? ReturnExpression => this.Children.FirstOrDefault() as ExpressionNode ?? null;
        
        [JsonIgnore]
        public IdentifierNode? GotoLabel => this.Children.First() as IdentifierNode ?? null;


        public JumpStatementNode(int line, JumpStatementType type)
            : base(line)
        {
            this.Type = type;
        }

        public JumpStatementNode(int line, ExpressionNode? returnExpr)
            : base(line, returnExpr is null ? Enumerable.Empty<ASTNode>() : new[] { returnExpr })
        {
            this.Type = JumpStatementType.Return;
        }
        
        public JumpStatementNode(int line, IdentifierNode label)
            : base(line, label)
        {
            this.Type = JumpStatementType.Goto;
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

        [JsonIgnore]
        public StatementNode Statement => this.Children.First().As<StatementNode>();


        public LabeledStatementNode(int line, string label, StatementNode statement)
            : base(line, statement)
        {
            this.Label = label;
        }


        public override string GetText() => $"{this.Label}: {this.Statement.GetText()}";
    }
}
