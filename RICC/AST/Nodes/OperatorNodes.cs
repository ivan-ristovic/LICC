using System;

namespace RICC.AST.Nodes
{
    public abstract class OperatorNode : ASTNode
    {
        public string Symbol { get; }


        protected OperatorNode(int line, string symbol, ASTNode? parent = null) 
            : base(line, parent)
        {
            this.Symbol = symbol;
        }
    }

    public abstract class BinaryOperatorNode : OperatorNode
    {
        protected BinaryOperatorNode(int line, string symbol, ASTNode? parent = null)
            : base(line, symbol, parent)
        {

        }
    }

    public sealed class UnaryOperatorNode : OperatorNode
    {
        public Func<object, object> ApplyTo { get; set; }


        public UnaryOperatorNode(int line, string symbol, Func<object, object> logic, ASTNode? parent = null)
            : base(line, symbol, parent)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class ArithmeticOperatorNode : BinaryOperatorNode
    {
        public Func<object, object, object> ApplyTo { get; set; }


        public ArithmeticOperatorNode(int line, string symbol, Func<object, object, object> logic, ASTNode? parent = null)
            : base(line, symbol, parent)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class RelationalOperatorNode : BinaryOperatorNode
    {
        public Func<object, object, bool> ApplyTo { get; set; }


        public RelationalOperatorNode(int line, string symbol, Func<object, object, bool> logic, ASTNode? parent = null)
            : base(line, symbol, parent)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class LogicOperatorNode : BinaryOperatorNode
    {
        public Func<bool, bool, bool> ApplyTo { get; set; }


        public LogicOperatorNode(int line, string symbol, Func<bool, bool, bool> logic, ASTNode? parent = null)
            : base(line, symbol, parent)
        {
            this.ApplyTo = logic;
        }
    }
}
