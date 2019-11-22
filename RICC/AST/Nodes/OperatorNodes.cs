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
        public UnaryOperatorNode(int line, string sign, ASTNode? parent = null)
            : base(line, sign, parent)
        {

        }
    }

    public sealed class ArithmeticOperatorNode : BinaryOperatorNode
    {
        public ArithmeticOperatorNode(int line, string sign, ASTNode? parent = null)
            : base(line, sign, parent)
        {

        }
    }
    
    public sealed class LogicOperatorNode : BinaryOperatorNode
    {
        public LogicOperatorNode(int line, string sign, ASTNode? parent = null)
            : base(line, sign, parent)
        {

        }
    }
}
