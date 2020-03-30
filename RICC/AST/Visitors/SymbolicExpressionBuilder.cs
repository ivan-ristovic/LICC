using System.Collections.Generic;
using System.Text.RegularExpressions;
using RICC.AST.Nodes;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace RICC.AST.Visitors
{
    public sealed class SymbolicExpressionBuilder : BaseASTVisitor<Expr>
    {
        private static readonly Regex _wildcardRegex = new Regex(@"v__\d+", RegexOptions.Compiled);
        private static int _lastUsedId = 0;
        private static readonly Dictionary<string, int> _wildcards = new Dictionary<string, int>();


        public static string WildcardReplace(string expr, string replacement = "?")
            => _wildcardRegex.Replace(expr, replacement);


        public ASTNode Node { get; set; }


        public SymbolicExpressionBuilder(ASTNode node)
        {
            this.Node = node;
        }


        public Expr Parse()
            => this.Visit(this.Node);


        public override Expr Visit(ArithmeticExpressionNode node)
            => this.EvaluateBinaryExpression(node);

        public override Expr Visit(RelationalExpressionNode node)
            => this.EvaluateBinaryExpression(node);

        public override Expr Visit(LogicExpressionNode node)
            => this.EvaluateBinaryExpression(node);

        public override Expr Visit(UnaryExpressionNode node)
            => this.EvaluateUnaryExpression(node);

        public override Expr Visit(IdentifierNode node) 
            => Expr.Variable(node.Identifier);

        public override Expr Visit(LiteralNode node)
            // TODO string literals need to be substituted as well...
            => node.Value is null ? Expr.Undefined : Expr.Parse(node.Value.ToString());

        public override Expr Visit(NullLiteralNode node)
            => Expr.Undefined;


        private Expr EvaluateBinaryExpression(BinaryExpressionNode node)
        {
            Expr left = this.Visit(node.LeftOperand);
            Expr right = this.Visit(node.RightOperand);
            return this.TryEvaluate(node, $"{left} {node.Operator.Symbol} {right}");
        }

        private Expr EvaluateUnaryExpression(UnaryExpressionNode node)
        {
            Expr operand = this.Visit(node.Operand);
            return this.TryEvaluate(node, $"{node.Operator.Symbol}({operand})");
        }

        private Expr TryEvaluate(ExpressionNode e, string exprStr)
        {
            Expr expr;
            object? constantValue = null;
            try {
                constantValue = ConstantExpressionEvaluator.Evaluate(e);
            } catch {
                
            }

            if (constantValue is { }) {
                try {
                    expr = Expr.Parse(constantValue.ToString());
                    return expr;
                } catch {

                }
            }

            try {
                expr = Expr.Parse(exprStr);
            } catch {
                expr = this.GetWildcard(exprStr);
            }
            return expr;
        }

        private Expr GetWildcard(string exprStr)
        {
            if (_wildcards.TryGetValue(exprStr, out int id))
                return Expr.Variable($"v__{id}");
            _wildcards.Add(exprStr, _lastUsedId);
            return Expr.Variable($"v__{_lastUsedId++}");
        }
    }
}
