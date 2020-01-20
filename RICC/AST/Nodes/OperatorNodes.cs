using System;
using Newtonsoft.Json;

namespace RICC.AST.Nodes
{
    public abstract class OperatorNode : ASTNode
    {
        public string Symbol { get; }


        protected OperatorNode(int line, string symbol)
            : base(line)
        {
            this.Symbol = symbol;
        }


        public override string GetText() => this.Symbol;
    }

    public abstract class BinaryOperatorNode : OperatorNode
    {
        [JsonIgnore]
        public Func<object, object, object> ApplyTo { get; set; }


        protected BinaryOperatorNode(int line, string symbol, Func<object, object, object> logic)
            : base(line, symbol)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class UnaryOperatorNode : OperatorNode
    {
        [JsonIgnore]
        public Func<object, object> ApplyTo { get; set; }


        public UnaryOperatorNode(int line, string symbol, Func<object, object> logic)
            : base(line, symbol)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class ArithmeticOperatorNode : BinaryOperatorNode
    {
        public ArithmeticOperatorNode(int line, string symbol, Func<object, object, object> logic)
            : base(line, symbol, logic) { }
    }

    public sealed class RelationalOperatorNode : BinaryOperatorNode
    {
        public RelationalOperatorNode(int line, string symbol, Func<object, object, bool> logic)
            : base(line, symbol, (x, y) => logic(x, y)) { }
    }

    public sealed class BinaryLogicOperatorNode : BinaryOperatorNode
    {
        public BinaryLogicOperatorNode(int line, string symbol, Func<bool, bool, bool> logic)
            : base(line, symbol, (x, y) => logic(Convert.ToBoolean(x), Convert.ToBoolean(y))) { }
    }

    public sealed class AssignmentOperatorNode : BinaryOperatorNode
    {
        public AssignmentOperatorNode(int line, string symbol, Func<object, object, object> logic)
            : base(line, symbol, logic) { }
    }
}
