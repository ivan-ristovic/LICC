using System;
using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Visitors;
using RICC.Exceptions;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class ExpressionTestsBase : ASTBuilderTestBase
    {
        protected void AssertEvaluationException(string code)
        {
            ExpressionNode expr = this.AssertExpression(code);
            Assert.That(() => ExpressionEvaluator.TryEvaluateAs(expr, out object result), Throws.InstanceOf<EvaluationException>());
        }

        protected void AssertExpressionValue<T>(string code, T expected)
        {
            ExpressionNode expr = this.AssertExpression(code);
            Assert.That(ExpressionEvaluator.TryEvaluateAs(expr, out T result));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }

        protected void AssertNullExpression(string code)
        {
            ExpressionNode expr = this.AssertExpression(code);
            if (expr is NullLiteralNode @null) {
                Assert.That(@null.Value, Is.Null);
                Assert.That(@null.TypeCode, Is.EqualTo(TypeCode.Empty));
            } else {
                Assert.Fail("Initializer is not of type NullLiteralNode");
            }
        }

        protected ExpressionNode AssertExpression(string code)
        {
            ExpressionNode expr = this.GenerateAST(code).As<ExpressionNode>();
            Assert.That(expr, Is.Not.Null);
            return expr;
        }

        protected void AssertLiteralSuffix(string code, string suffix, object value, Type type)
        {
            LiteralNode literal = this.GenerateAST(code).As<LiteralNode>();
            Assert.That(literal, Is.Not.Null);
            Assert.That(literal.Value?.GetType(), Is.EqualTo(type));
            Assert.That(literal.Suffix, Is.EqualTo(suffix));
            Assert.That(ExpressionEvaluator.Evaluate(literal), Is.EqualTo(value).Within(1e-10));
        }

        protected void AssertFunctionCallExpression(string code, string fname, params object[] args)
        {
            FunctionCallExpressionNode fcall = this.GenerateAST(code).As<FunctionCallExpressionNode>();
            this.AssertChildrenParentProperties(fcall);
            Assert.That(fcall.Identifier, Is.EqualTo(fname));

            if (args is null || !args.Any()) {
                Assert.That(fcall.Arguments, Is.Null);
            } else {
                Assert.That(fcall.Arguments, Is.Not.Null);
                Assert.That(fcall.Arguments!.Expressions.Count, Is.EqualTo(args.Length));
                foreach ((ExpressionNode arg, object? expected) in fcall.Arguments!.Expressions.Zip(args))
                    Assert.That(ExpressionEvaluator.Evaluate(arg), Is.EqualTo(expected).Within(1e-10));
            }
        }
    }
}
