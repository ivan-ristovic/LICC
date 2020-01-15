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
        protected BinaryOperatorNode(int line, string symbol)
            : base(line, symbol)
        {

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
        [JsonIgnore]
        public Func<object, object, object> ApplyTo { get; set; }


        public ArithmeticOperatorNode(int line, string symbol, Func<object, object, object> logic)
            : base(line, symbol)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class RelationalOperatorNode : BinaryOperatorNode
    {
        [JsonIgnore]
        public Func<object, object, bool> ApplyTo { get; set; }


        public RelationalOperatorNode(int line, string symbol, Func<object, object, bool> logic)
            : base(line, symbol)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class LogicOperatorNode : BinaryOperatorNode
    {
        [JsonIgnore]
        public Func<bool, bool, bool> ApplyTo { get; set; }


        public LogicOperatorNode(int line, string symbol, Func<bool, bool, bool> logic)
            : base(line, symbol)
        {
            this.ApplyTo = logic;
        }
    }

    public sealed class AssignmentOperatorNode : BinaryOperatorNode
    {
        [JsonIgnore]
        public Func<object, object, object> ApplyTo { get; set; }


        public AssignmentOperatorNode(int line, string symbol, Func<object, object, object> logic)
            : base(line, symbol)
        {
            this.ApplyTo = logic;
        }
    }
}
