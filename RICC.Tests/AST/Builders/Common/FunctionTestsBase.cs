using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.AST.Visitors;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class FunctionTestsBase : ASTBuilderTestBase
    {
        protected FunctionDefinitionNode AssertFunctionSignature(string src,
                                                                 int line,
                                                                 string fname,
                                                                 string returnType = "void",
                                                                 bool isVariadic = false,
                                                                 AccessModifiers access = AccessModifiers.Unspecified,
                                                                 QualifierFlags qualifiers = QualifierFlags.None,
                                                                 params (string Type, string Identifier)[] @params)
        {
            FunctionDefinitionNode f = this.GenerateAST(src).As<FunctionDefinitionNode>();
            this.AssertChildrenParentProperties(f);
            this.AssertChildrenParentProperties(f.Definition);
            Assert.That(f, Is.Not.Null);
            Assert.That(f.Line, Is.EqualTo(line));
            Assert.That(f.Declarator, Is.Not.Null);
            Assert.That(f.Declarator.Parent, Is.EqualTo(f));
            Assert.That(f.Keywords.AccessModifiers, Is.EqualTo(access));
            Assert.That(f.Keywords.QualifierFlags, Is.EqualTo(qualifiers));
            Assert.That(f.Identifier, Is.EqualTo(fname));
            Assert.That(f.ReturnTypeName, Is.EqualTo(returnType));
            Assert.That(f.IsVariadic, Is.EqualTo(isVariadic));
            if (@params?.Any() ?? false) {
                Assert.That(f.Parameters, Is.Not.Null);
                Assert.That(f.Parameters, Has.Exactly(@params.Length).Items);
                Assert.That(f.ParametersNode, Is.Not.Null);
                Assert.That(f.Parameters.Select(p => (p.DeclarationSpecifiers.TypeName, p.Declarator.Identifier)), Is.EqualTo(@params));
            }
            return f;
        }

        protected void AssertReturnValue(string code, object? expected)
        {
            FunctionDefinitionNode fnode = this.GenerateAST(code).As<FunctionDefinitionNode>();
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
