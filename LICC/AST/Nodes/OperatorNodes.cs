using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using LICC.AST.Nodes.Common;

namespace LICC.AST.Nodes
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

        public override bool Equals([AllowNull] ASTNode other)
            => base.Equals(other) && this.Symbol.Equals((other as OperatorNode)?.Symbol);
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
        public static UnaryOperatorNode FromSymbol(int line, string symbol)
            => new UnaryOperatorNode(line, symbol, UnaryOperations.UnaryFromSymbol(symbol));


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
        public static ArithmeticOperatorNode FromSymbol(int line, string symbol)
            => new ArithmeticOperatorNode(line, symbol, BinaryOperations.ArithmeticFromSymbol(symbol));

        public static ArithmeticOperatorNode FromBitwiseSymbol(int line, string symbol)
            => new ArithmeticOperatorNode(line, symbol, BinaryOperations.BitwiseBinaryFromSymbol(symbol));


        public ArithmeticOperatorNode(int line, string symbol, Func<object, object, object> logic)
            : base(line, symbol, logic) { }
    }

    public sealed class RelationalOperatorNode : BinaryOperatorNode
    {
        public static RelationalOperatorNode FromSymbol(int line, string symbol)
            => new RelationalOperatorNode(line, symbol, BinaryOperations.RelationalFromSymbol(symbol));


        public RelationalOperatorNode(int line, string symbol, Func<object, object, bool> logic)
            : base(line, symbol, (x, y) => logic(x, y)) { }
    }

    public sealed class BinaryLogicOperatorNode : BinaryOperatorNode
    {
        public static BinaryLogicOperatorNode FromSymbol(int line, string symbol)
            => new BinaryLogicOperatorNode(line, symbol, BinaryOperations.LogicFromSymbol(symbol));
        
        
        public BinaryLogicOperatorNode(int line, string symbol, Func<bool, bool, bool> logic)
            : base(line, symbol, (x, y) => logic(Convert.ToBoolean(x), Convert.ToBoolean(y))) { }
    }

    public class AssignmentOperatorNode : BinaryOperatorNode
    {
        public static AssignmentOperatorNode FromSymbol(int line, string symbol)
        {
            return symbol == "=" || symbol == ":="
                ? new AssignmentOperatorNode(line, symbol, (a, b) => b)
                : new ComplexAssignmentOperatorNode(line, symbol, BinaryOperations.AssignmentFromSymbol(symbol));
        }


        public AssignmentOperatorNode(int line, string symbol, Func<object, object, object> logic)
            : base(line, symbol, logic) { }
    }

    public sealed class ComplexAssignmentOperatorNode : AssignmentOperatorNode
    {
        public ComplexAssignmentOperatorNode(int line, string symbol, Func<object, object, object> logic)
            : base(line, symbol, logic) { }
    }
}
