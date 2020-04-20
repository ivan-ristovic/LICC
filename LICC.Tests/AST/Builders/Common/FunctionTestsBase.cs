using System.Linq;
using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;
using LICC.AST.Visitors;

namespace LICC.Tests.AST.Builders.Common
{
    internal abstract class FunctionTestsBase : ASTBuilderTestBase
    {
        protected FuncDefNode AssertFunctionSignature(string src,
                                                                 int line,
                                                                 string fname,
                                                                 string returnType = "void",
                                                                 bool isVariadic = false,
                                                                 AccessModifiers access = AccessModifiers.Unspecified,
                                                                 QualifierFlags qualifiers = QualifierFlags.None,
                                                                 params (string Type, string Identifier)[] @params)
        {
            FuncDefNode f = this.GenerateAST(src).As<FuncDefNode>();
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
                Assert.That(f.Parameters.Select(p => (p.Specifiers.TypeName, p.Declarator.Identifier)), Is.EqualTo(@params));
            }
            return f;
        }

        protected void AssertReturnValue(string code, object? expected)
        {
            FuncDefNode fnode = this.GenerateAST(code).As<FuncDefNode>();
            JumpStatNode node = fnode.Definition.Children.Last().As<JumpStatNode>();

            Assert.That(node.GotoLabel, Is.Null);
            if (expected is null) {
                Assert.That(node.ReturnExpr, Is.Null);
            } else {
                Assert.That(node.ReturnExpr, Is.Not.Null);
                if (node.ReturnExpr is { })
                    Assert.That(ConstantExpressionEvaluator.Evaluate(node.ReturnExpr), Is.EqualTo(expected).Within(1e-10));
            }
        }
    }
}
