using System;

namespace RICC.AST.Nodes
{
    public abstract class OperatorNode : ASTNode
    {
        public string Sign { get; }


        protected OperatorNode(int line, string sign, ASTNode? parent = null) 
            : base(line, parent)
        {
            this.Sign = sign;
        }
    }

    public abstract class BinaryOperatorNode : OperatorNode
    {
        protected BinaryOperatorNode(int line, string sign, ASTNode? parent = null)
            : base(line, sign, parent)
        {

        }
    }

    public sealed class UnaryOperatorNode : OperatorNode
    {
        public Func<object, object> ApplyTo { get; set; }


        public UnaryOperatorNode(int line, string sign, Func<object, object> logic, ASTNode? parent = null)
            : base(line, sign, parent)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class ArithmeticOperatorNode : BinaryOperatorNode
    {
        public Func<object, object, object> ApplyTo { get; set; }


        public ArithmeticOperatorNode(int line, string sign, Func<object, object, object> logic, ASTNode? parent = null)
            : base(line, sign, parent)
        {
            this.ApplyTo = logic;
        }
    }
    
    public sealed class LogicOperatorNode : BinaryOperatorNode
    {
        public Func<bool, bool, bool> ApplyTo { get; set; }


        public LogicOperatorNode(int line, string sign, Func<bool, bool, bool> logic, ASTNode? parent = null)
            : base(line, sign, parent)
        {
            this.ApplyTo = logic;
        }
    }
}
