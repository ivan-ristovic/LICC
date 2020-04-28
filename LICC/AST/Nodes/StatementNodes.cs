using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using LICC.AST.Nodes.Common;

namespace LICC.AST.Nodes
{
    public abstract class StatNode : ASTNode
    {
        protected StatNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected StatNode(int line, params ASTNode[] children)
            : base(line, children) { }


        public override string GetText() => $"{base.GetText()};";
    }

    public sealed class EmptyStatNode : StatNode
    {
        public EmptyStatNode(int line)
            : base(line) { }
    }

    public abstract class SimpleStatNode : StatNode
    {
        protected SimpleStatNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected SimpleStatNode(int line, params ASTNode[] children)
            : base(line, children) { }
    }

    public class DeclStatNode : SimpleStatNode
    {
        [JsonIgnore]
        public DeclSpecsNode Specifiers => this.Children.ElementAt(0).As<DeclSpecsNode>();
        [JsonIgnore]
        public DeclListNode DeclaratorList => this.Children.ElementAt(1).As<DeclListNode>();


        public DeclStatNode(int line, DeclSpecsNode declSpecs, DeclListNode declList)
            : base(line, declSpecs, declList) { }
    }

    public abstract class CompStatNode : StatNode
    {
        protected CompStatNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        protected CompStatNode(int line, params ASTNode[] children)
            : base(line, children) { }
    }

    public sealed class BlockStatNode : CompStatNode
    {
        public BlockStatNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }

        public BlockStatNode(int line, params ASTNode[] children)
            : base(line, children) { }


        public override string GetText() => $"{{ {string.Join(" ", this.Children.Select(c => c.GetText()))} }}";
    }

    public class ExprStatNode : SimpleStatNode
    {
        [JsonIgnore]
        public ExprNode Expression => this.Children.First().As<ExprNode>();


        public ExprStatNode(int line, ExprNode expr)
            : base(line, expr) { }
    }

    public sealed class IfStatNode : CompStatNode
    {
        [JsonIgnore]
        public ExprNode Condition => this.Children[0].As<ExprNode>();

        [JsonIgnore]
        public StatNode ThenStat => this.Children[1].As<StatNode>();

        [JsonIgnore]
        public StatNode? ElseStat => this.Children.ElementAtOrDefault(2)?.As<StatNode>() ?? null;


        public IfStatNode(int line, ExprNode cond, StatNode @then)
            : base(line, cond, @then) { }

        public IfStatNode(int line, ExprNode cond, StatNode @then, StatNode @else)
            : base(line, cond, @then, @else) { }


        public override string GetText()
            => $"if {this.Condition.GetText()} {this.ThenStat.GetText()} {(this.ElseStat is null ? "" : $"else {this.ElseStat.GetText()}")}";
    }

    public sealed class JumpStatNode : SimpleStatNode
    {
        public JumpStatType Type { get; set; }

        [JsonIgnore]
        public ExprNode? ReturnExpr => this.Children.FirstOrDefault() as ExprNode ?? null;

        [JsonIgnore]
        public IdNode? GotoLabel => this.Children.First() as IdNode ?? null;


        public JumpStatNode(int line, JumpStatType type)
            : base(line)
        {
            this.Type = type;
        }

        public JumpStatNode(int line, ExprNode? returnExpr)
            : base(line, returnExpr is null ? Enumerable.Empty<ASTNode>() : new[] { returnExpr })
        {
            this.Type = JumpStatType.Return;
        }

        public JumpStatNode(int line, IdNode label)
            : base(line, label)
        {
            this.Type = JumpStatType.Goto;
        }


        public override string GetText()
        {
            var sb = new StringBuilder(this.Type.ToStringToken());
            if (this.Type == JumpStatType.Return && this.ReturnExpr is { })
                sb.Append(' ').Append(this.ReturnExpr.GetText());
            else if (this.Type == JumpStatType.Goto && this.GotoLabel is { })
                sb.Append(' ').Append(this.GotoLabel.GetText());
            sb.Append(';');
            return sb.ToString();
        }
    }

    public sealed class LabeledStatNode : SimpleStatNode
    {
        public string Label { get; }

        [JsonIgnore]
        public StatNode Statement => this.Children.First().As<StatNode>();


        public LabeledStatNode(int line, string label, StatNode statement)
            : base(line, statement)
        {
            this.Label = label;
        }


        public override string GetText() => $"{this.Label}: {this.Statement.GetText()}";

        public override bool Equals([AllowNull] ASTNode other)
            => base.Equals(other) && this.Label.Equals((other as LabeledStatNode)?.Label);
    }

    public abstract class IterStatNode : CompStatNode
    {
        [JsonIgnore]
        public ExprNode Condition => this.Children[0].As<ExprNode>();

        [JsonIgnore]
        public StatNode Statement => this.Children[1].As<StatNode>();


        protected IterStatNode(int line, ExprNode cond, StatNode stat)
            : base(line, cond, stat) { }

        protected IterStatNode(int line, IEnumerable<ASTNode> children)
            : base(line, children) { }
    }

    public sealed class WhileStatNode : IterStatNode
    {
        public WhileStatNode(int line, ExprNode cond, StatNode stat)
            : base(line, cond, stat) { }


        public override string GetText() => $"while {this.Condition.GetText()} {{ {this.Statement.GetText()} }}";
    }

    public sealed class ForStatNode : IterStatNode
    {
        public DeclarationNode? ForDeclaration { get; }
        public ExprNode? InitExpr { get; }
        public ExprNode? IncrExpr { get; }


        public ForStatNode(int line, DeclarationNode decl, ExprNode? cond, ExprNode? expr, StatNode stat)
            : base(line, new ASTNode[] { cond ?? new LitExprNode(line, true), stat })
        {
            this.ForDeclaration = decl;
            this.InitExpr = null;
            this.IncrExpr = expr;
        }

        public ForStatNode(int line, ExprNode? initExpr, ExprNode? cond, ExprNode? incExpr, StatNode stat)
            : base(line, new ASTNode[] { cond ?? new LitExprNode(line, true), stat })
        {
            this.ForDeclaration = null;
            this.InitExpr = initExpr;
            this.IncrExpr = incExpr;
        }


        public override string GetText()
        {
            var sb = new StringBuilder("for (");
            if (this.ForDeclaration is { })
                sb.Append(this.ForDeclaration.GetText());
            else if (this.InitExpr is { })
                sb.Append(this.InitExpr.GetText());
            sb.Append("; ");
            sb.Append(this.Condition.GetText());
            sb.Append("; ");
            if (this.IncrExpr is { })
                sb.Append(this.IncrExpr.GetText());
            sb.Append(") { ");
            sb.Append(this.Statement.GetText());
            sb.Append(" }");
            return sb.ToString();
        }
    }

    public sealed class ThrowStatNode : ExprStatNode
    {
        public ThrowStatNode(int line, ExprNode exp)
            : base(line, exp) { }

        public override string GetText() => $"throw {this.Expression.GetText()}";
    }
}
