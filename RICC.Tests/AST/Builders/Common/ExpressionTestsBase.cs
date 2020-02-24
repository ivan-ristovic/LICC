using System;
using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders;
using RICC.AST.Nodes;
using RICC.AST.Visitors;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class ExpressionTestsBase<TBuilder> where TBuilder : IASTBuilder, new()
    {
        protected void AssertInitializerValue<T>(string decl, T expected)
        {
            ExpressionNode init = this.AssertInitializer(decl);
            Assert.That(ExpressionEvaluator.TryEvaluateAs(init, out T result));
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }

        protected ExpressionNode AssertInitializer(string code)
        {
            ASTNode ast = new TBuilder().BuildFromSource(code);
            ExpressionNode? init = ast.Children
                .First().As<DeclarationStatementNode>()
                .Children.ElementAt(1).As<DeclaratorListNode>()
                .Declarations
                .First().As<VariableDeclaratorNode>()
                .Initializer;
            Assert.That(init, Is.Not.Null);
            return init!;
        }

        protected void AssertLiteralSuffix(string code, string suffix, object value, Type type)
        {
            ASTNode ast = new TBuilder().BuildFromSource(code);
            DeclarationStatementNode decl = ast.Children.First().As<DeclarationStatementNode>();
            DeclaratorListNode declList = decl.Children.ElementAt(1).As<DeclaratorListNode>();
            VariableDeclaratorNode var = declList.Declarations.First().As<VariableDeclaratorNode>();
            Assert.That(var.Initializer, Is.Not.Null);
            Assert.That(var.Initializer, Is.InstanceOf<LiteralNode>());
            Assert.That(var.Initializer!.As<LiteralNode>().Value.GetType(), Is.EqualTo(type));
            Assert.That(var.Initializer!.As<LiteralNode>().Suffix, Is.EqualTo(suffix));
            Assert.That(ExpressionEvaluator.Evaluate(var.Initializer!), Is.EqualTo(value).Within(1e-10));
        }

        protected void AssertParameterValues(string f, params object[] argValues)
        {
            FunctionDefinitionNode fnode = new TBuilder().BuildFromSource(f).Children.First().As<FunctionDefinitionNode>();
            FunctionCallExpressionNode fcall = fnode.Definition.Children.First().Children.First().As<FunctionCallExpressionNode>();
            Assert.That(fcall.Identifier, Is.EqualTo(fnode.Identifier));
            Assert.That(fcall.Parent, Is.EqualTo(fnode.Definition.Children.First()));

            if (argValues is null || !argValues.Any()) {
                Assert.That(fcall.Arguments, Is.Null);
            } else {
                Assert.That(fcall.Arguments, Is.Not.Null);
                Assert.That(fcall.Arguments!.Expressions.Count, Is.EqualTo(argValues.Length));
                foreach ((ExpressionNode arg, object? expected) in fcall.Arguments!.Expressions.Zip(argValues))
                    Assert.That(ExpressionEvaluator.Evaluate(arg), Is.EqualTo(expected).Within(1e-10));
            }
        }

        protected void AssertReturnValue(string f, object? expected)
        {
            FunctionDefinitionNode fnode = new TBuilder().BuildFromSource(f).Children.First().As<FunctionDefinitionNode>();
            JumpStatementNode node = fnode.Definition.Children.Last().As<JumpStatementNode>();

            Assert.That(node.GotoLabel, Is.Null);
            if (expected is null) {
                Assert.That(node.ReturnExpression, Is.Null);
            } else {
                Assert.That(node.ReturnExpression, Is.Not.Null);
                if (node.ReturnExpression is { })
                    Assert.That(ExpressionEvaluator.Evaluate(node.ReturnExpression), Is.EqualTo(expected).Within(1e-10));
            }
        }
    }
}
