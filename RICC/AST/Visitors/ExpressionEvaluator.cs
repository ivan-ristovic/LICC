using System.Collections.Generic;
using System.Linq;
using MathNet.Symbolics;
using RICC.AST.Nodes;
using RICC.Exceptions;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace RICC.AST.Visitors
{
    public sealed class ExpressionEvaluator
    {
        private static readonly int _threshold = 10000;


        public static Expr TryEvaluate(ExpressionNode node, Dictionary<string, Expr> symbols)
        {
            Expr expr = new SymbolicExpressionBuilder(node).Parse();

            IEnumerable<Expr> vars = expr.CollectVariables();
            bool canReduce = true;
            for (int i = 0; canReduce && vars.Any(); i++) {
                if (i > _threshold)
                    throw new EvaluationException("Infinite cycle detected.");
                canReduce = false;
                foreach (Expr v in vars) {
                    string varStr = v.VariableName;
                    if (symbols.ContainsKey(varStr)) {
                        expr = expr.Substitute(v, symbols[varStr]);
                        canReduce = true;
                    }
                }
                vars = expr.CollectVariables();
            }

            return expr;
        }
    }
}
