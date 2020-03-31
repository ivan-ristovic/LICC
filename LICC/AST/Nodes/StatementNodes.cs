using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using LICC.AST.Nodes.Common;

namespace LICC.AST.Nodes
{
    public abstract class StatementNode : ASTNode
    {
        protected StatementNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected StatementNode(int line, params ASTNode[] children)
            : base(line, children) { }


        public override string GetText() => $"{base.GetText()};";
    }

    public sealed class EmptyStatementNode : StatementNode
    {
        public EmptyStatementNode(int line)
            : base(line) { }
    }

    public abstract class SimpleStatementNode : StatementNode
    {
        protected SimpleStatementNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected SimpleStatementNode(int line, params ASTNode[] children)
            : base(line, children) { }
    }

    public class DeclarationStatementNode : SimpleStatementNode
    {
        public DeclarationSpecifiersNode Specifiers => this.Children.ElementAt(0).As<DeclarationSpecifiersNode>();
        public DeclaratorListNode DeclaratorList => this.Children.ElementAt(1).As<DeclaratorListNode>();


        public DeclarationStatementNode(int line, DeclarationSpecifiersNode declSpecs, DeclaratorListNode declList)
            : base(line, declSpecs, declList) { }
    }

    public abstract class CompoundStatementNode : StatementNode
    {
        protected CompoundStatementNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected CompoundStatementNode(int line, params ASTNode[] children)
            : base(line, children) { }
    }

    public sealed class BlockStatementNode : CompoundStatementNode
    {
        public BlockStatementNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        public BlockStatementNode(int line, params ASTNode[] children)
            : base(line, children) { }


        public override string GetText() => $"{{ {string.Join(" ", this.Children.Select(c => c.GetText()))} }}";
    }

    public sealed class ExpressionStatementNode : SimpleStatementNode
    {
        [JsonIgnore]
        public ExpressionNode Expression => this.Children.First().As<ExpressionNode>();


        public ExpressionStatementNode(int line, ExpressionNode expr)
            : base(line, expr) { }
    }

    public sealed class IfStatementNode : CompoundStatementNode
    {
        [JsonIgnore]
        public ExpressionNode Condition => this.Children[0].As<ExpressionNode>();

        [JsonIgnore]
        public StatementNode ThenStatement => this.Children[1].As<StatementNode>();

        [JsonIgnore]
        public StatementNode? ElseStatement => this.Children.ElementAtOrDefault(2)?.As<StatementNode>() ?? null;


        public IfStatementNode(int line, ExpressionNode condition, StatementNode thenBlock)
            : base(line, condition, thenBlock) { }

        public IfStatementNode(int line, ExpressionNode condition, StatementNode thenBlock, StatementNode elseBlock)
            : base(line, condition, thenBlock, elseBlock) { }


        public override string GetText()
            => $"if {this.Condition.GetText()} {this.ThenStatement.GetText()} {(this.ElseStatement is null ? "" : $"else {this.ElseStatement.GetText()}")}";
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
            else if (this.Type == JumpStatementType.Goto && this.GotoLabel is { })
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

        public override bool Equals([AllowNull] ASTNode other)
            => base.Equals(other) && this.Label.Equals((other as LabeledStatementNode)?.Label);
    }

    public abstract class IterationStatementNode : CompoundStatementNode
    {
        [JsonIgnore]
        public ExpressionNode Condition => this.Children[0].As<ExpressionNode>();

        [JsonIgnore]
        public StatementNode Statement => this.Children[1].As<StatementNode>();


        protected IterationStatementNode(int line, ExpressionNode condition, StatementNode statement)
            : base(line, condition, statement) { }

        protected IterationStatementNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }
    }

    public sealed class WhileStatementNode : IterationStatementNode
    {
        public WhileStatementNode(int line, ExpressionNode condition, StatementNode statement)
            : base(line, condition, statement) { }


        public override string GetText() => $"while {this.Condition.GetText()} {{ {this.Statement.GetText()} }}";
    }

    public sealed class ForStatementNode : IterationStatementNode
    {
        public DeclarationNode? ForDeclaration { get; }
        public ExpressionNode? InitExpression { get; }
        public ExpressionNode? IncrementExpression { get; }


        public ForStatementNode(int line, DeclarationNode decl, ExpressionNode? condition, ExpressionNode? expr, StatementNode statement)
            : base(line, new ASTNode[] { condition ?? new LiteralNode(line, true), statement })
        {
            this.ForDeclaration = decl;
            this.InitExpression = null;
            this.IncrementExpression = expr;
        }

        public ForStatementNode(int line, ExpressionNode? initExpr, ExpressionNode? condition, ExpressionNode? incExpr, StatementNode statement)
            : base(line, new ASTNode[] { condition ?? new LiteralNode(line, true), statement })
        {
            this.ForDeclaration = null;
            this.InitExpression = initExpr;
            this.IncrementExpression = incExpr;
        }


        public override string GetText()
        {
            var sb = new StringBuilder("for (");
            if (this.ForDeclaration is { })
                sb.Append(this.ForDeclaration.GetText());
            else if (this.InitExpression is { })
                sb.Append(this.InitExpression.GetText());
            sb.Append("; ");
            sb.Append(this.Condition.GetText());
            sb.Append("; ");
            if (this.IncrementExpression is { })
                sb.Append(this.IncrementExpression.GetText());
            sb.Append(") { ");
            sb.Append(this.Statement.GetText());
            sb.Append(" }");
            return sb.ToString();
        }
    }

    public sealed class ThrowStatementNode : StatementNode
    {
        [JsonIgnore]
        ExpressionNode Expression => this.Children.Single().As<ExpressionNode>();


        public ThrowStatementNode(int line, ExpressionNode exp)
            : base(line, exp) { }


        public override string GetText() => $"throw {this.Expression.GetText()}";
    }
}
