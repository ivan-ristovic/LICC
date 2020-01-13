using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
    public static class ASTNodeFactory
    {
        public static ExpressionNode CreateLiteralNode(int line, string value, ASTNode? parent = null)
        {
            // TODO order
            if (int.TryParse(value, out int res_int))
                return new LiteralNode(line, res_int, parent);
            else if (bool.TryParse(value, out bool res_bool))
                return new LiteralNode(line, res_bool, parent);
            else if (byte.TryParse(value, out byte res_byte))
                return new LiteralNode(line, res_byte, parent);
            else if (char.TryParse(value, out char res_char))
                return new LiteralNode(line, res_char, parent);
            else if (short.TryParse(value, out short res_short))
                return new LiteralNode(line, res_short, parent);
            else if (ushort.TryParse(value, out ushort res_ushort))
                return new LiteralNode(line, res_ushort, parent);
            else if (uint.TryParse(value, out uint res_uint))
                return new LiteralNode(line, res_uint, parent);
            else if (long.TryParse(value, out long res_long))
                return new LiteralNode(line, res_long, parent);
            else if (ulong.TryParse(value, out ulong res_ulong))
                return new LiteralNode(line, res_ulong, parent);
            else if (double.TryParse(value, out double res_double))
                return new LiteralNode(line, res_double, parent);
            else if (float.TryParse(value, out float res_float))
                return new LiteralNode(line, res_float, parent);
            else if (decimal.TryParse(value, out decimal res_decimal))
                return new LiteralNode(line, res_decimal, parent);
            else
                return new LiteralNode(line, value, parent);
        }
    }


    public abstract class ExpressionNode : ASTNode
    {
        protected ExpressionNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        protected ExpressionNode(int line, ASTNode? parent = null)
            : base(line, parent)
        {

        }
    }

    public sealed class ExpressionListNode : ExpressionNode
    {
        public IReadOnlyList<ExpressionNode> Expressions => this.Children.Cast<ExpressionNode>().ToList().AsReadOnly();
     
        
        public ExpressionListNode(int line, params ExpressionNode[] expressions) : base(line, expressions)
        {

        }

        public ExpressionListNode(int line, IEnumerable<ExpressionNode> expressions, ASTNode? parent = null) : base(line, expressions, parent)
        {

        }
    }

    public class UnaryExpressionNode : ExpressionNode
    {
        public UnaryOperatorNode Operator => this.Children[0].As<UnaryOperatorNode>();
        public ExpressionNode Operand => this.Children[1].As<ExpressionNode>();


        public UnaryExpressionNode(int line, UnaryOperatorNode @operator, ExpressionNode operand, ASTNode? parent = null)
            : base(line, new ASTNode[] { @operator, operand }, parent)
        {

        }
    }

    public abstract class BinaryExpressionNode : ExpressionNode
    {
        public BinaryOperatorNode Operator => this.Children[1].As<BinaryOperatorNode>();
        public ExpressionNode LeftOperand => this.Children[0].As<ExpressionNode>();
        public ExpressionNode RightOperand => this.Children[2].As<ExpressionNode>();


        protected BinaryExpressionNode(int line, ExpressionNode left, BinaryOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, new ASTNode[] { left, @operator, right }, parent)
        {

        }
    }

    public sealed class ArithmeticExpressionNode : BinaryExpressionNode
    {
        public ArithmeticExpressionNode(int line, ExpressionNode left, ArithmeticOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, left, @operator, right, parent)
        {

        }
    }

    public sealed class LogicExpressionNode : BinaryExpressionNode
    {
        public LogicExpressionNode(int line, ExpressionNode left, LogicOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, left, @operator, right, parent)
        {

        }
    }

    public sealed class RelationalExpressionNode : BinaryExpressionNode
    {
        public RelationalExpressionNode(int line, ExpressionNode left, RelationalOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, left, @operator, right, parent)
        {

        }
    }

    public sealed class IdentifierNode : ExpressionNode
    {
        public string Identifier { get; }


        public IdentifierNode(int line, string identifier, ASTNode? parent = null)
            : base(line, parent)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier name must be set.");
            this.Identifier = identifier;
        }
    }

    public sealed class AssignmentExpressionNode : ExpressionNode
    {
        public AssignmentOperatorNode Operator => this.Children[1].As<AssignmentOperatorNode>();
        public ExpressionNode LeftValue => this.Children[0].As<ExpressionNode>();
        public ExpressionNode RightValue => this.Children[2].As<ExpressionNode>();


        public AssignmentExpressionNode(int line, ExpressionNode left, AssignmentOperatorNode @operator, ExpressionNode right, ASTNode? parent = null)
            : base(line, new ASTNode[] { left, @operator, right }, parent)
        {

        }
    }

    public sealed class FunctionCallExpressionNode : ExpressionNode
    {
        public string Identifier => this.Children[0].As<IdentifierNode>().Identifier;
        public ExpressionListNode? Arguments => this.Children.ElementAtOrDefault(1)?.As<ExpressionListNode>();


        public FunctionCallExpressionNode(int line, IdentifierNode identifier, ASTNode? parent = null)
            : base(line, new[] { identifier }, parent)
        {

        }

        public FunctionCallExpressionNode(int line, IdentifierNode identifier, ExpressionListNode parameters, ASTNode? parent = null)
            : base(line, new ExpressionNode[] { identifier, parameters }, parent )
        {

        }
    }

    public sealed class LiteralNode : ExpressionNode
    {
        public object Value { get; }
        public TypeCode TypeCode { get; }


        public LiteralNode(int line, object value, ASTNode? parent = null)
            : base(line, parent)
        {
            this.Value = value;
            this.TypeCode = Type.GetTypeCode(value.GetType());
        }
    }
}
