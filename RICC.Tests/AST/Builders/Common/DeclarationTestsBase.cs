using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.AST.Visitors;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class DeclarationTestsBase<TBuilder> where TBuilder : IASTBuilder, new()
    {
        protected DeclarationStatementNode AssertDeclarationNode(string src,
                                                                 string type,
                                                                 AccessModifiers access = AccessModifiers.Unspecified,
                                                                 QualifierFlags qualifiers = QualifierFlags.None)
        {
            ASTNode ast = new TBuilder().BuildFromSource(src);
            DeclarationStatementNode decl = ast.Children.First().As<DeclarationStatementNode>();
            Assert.That(decl.Children, Has.Exactly(2).Items);

            DeclarationSpecifiersNode declSpecsNode = decl.Children.ElementAt(0).As<DeclarationSpecifiersNode>();
            Assert.That(declSpecsNode.Parent, Is.EqualTo(decl));
            Assert.That(declSpecsNode.Keywords.AccessModifiers, Is.EqualTo(access));
            Assert.That(declSpecsNode.Keywords.QualifierFlags, Is.EqualTo(qualifiers));
            Assert.That(declSpecsNode.TypeName, Is.EqualTo(type));
            Assert.That(declSpecsNode.Children, Is.Empty);

            return decl;
        }

        protected void AssertVariableDeclaration(string src,
                                                 string identifier,
                                                 string type,
                                                 AccessModifiers access = AccessModifiers.Unspecified,
                                                 QualifierFlags qualifiers = QualifierFlags.None,
                                                 object? value = null)
        {
            DeclarationStatementNode decl = this.AssertDeclarationNode(src, type, access, qualifiers);

            DeclaratorListNode declList = decl.Children.ElementAt(1).As<DeclaratorListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            VariableDeclaratorNode var = declList.Declarations.First().As<VariableDeclaratorNode>();
            Assert.That(var.Parent, Is.EqualTo(declList));
            Assert.That(var.Identifier, Is.EqualTo(identifier));
            Assert.That(var.Children.First().As<IdentifierNode>().Identifier, Is.EqualTo(identifier));
            if (value is { }) {
                Assert.That(var.Initializer, Is.Not.Null);
                Assert.That(var.Initializer!.Parent, Is.EqualTo(var));
                Assert.That(ExpressionEvaluator.Evaluate(var.Initializer), Is.EqualTo(value).Within(1e-10));
            } else {
                Assert.That(var.Initializer, Is.Null);
            }
        }

        protected void AssertVariableDeclarationList(string src,
                                                     string type,
                                                     AccessModifiers access = AccessModifiers.Unspecified,
                                                     QualifierFlags qualifiers = QualifierFlags.None,
                                                     params (string Identifier, object? Value)[] vars)
        {
            DeclarationStatementNode decl = this.AssertDeclarationNode(src, type, access, qualifiers);

            DeclaratorListNode declList = decl.Children.ElementAt(1).As<DeclaratorListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            Assert.That(declList.Declarations.Select(var => ExtractIdentifierAndValue(var)), Is.EqualTo(vars).Within(1e-6));


            static (string, object?) ExtractIdentifierAndValue(DeclarationNode declNode)
            {
                VariableDeclaratorNode var = declNode.As<VariableDeclaratorNode>();
                return var.Initializer is null ? (var.Identifier, (object?)null)
                                               : (var.Identifier, ExpressionEvaluator.Evaluate(var.Initializer));
            }
        }

        protected void AssertFunctionDeclaration(string src,
                                                 string fname,
                                                 string returnType,
                                                 AccessModifiers access = AccessModifiers.Unspecified,
                                                 QualifierFlags qualifiers = QualifierFlags.None,
                                                 bool isVariadic = false,
                                                 params (QualifierFlags Qualifiers, string Type, string Identifier)[] @params)
        {
            DeclarationStatementNode decl = this.AssertDeclarationNode(src, returnType, access, qualifiers);

            FunctionDeclaratorNode fdecl = decl.Children.Last().Children.First().As<FunctionDeclaratorNode>();
            Assert.That(fdecl.Identifier, Is.EqualTo(fname));
            Assert.That(fdecl.IsVariadic, Is.EqualTo(isVariadic));
            if (@params.Any()) {
                Assert.That(fdecl.Parameters, Is.Not.Null);
                Assert.That(fdecl.Parameters.Select(p => ExtractParamInfo(p)), Is.EqualTo(@params));
            } else {
                Assert.That(fdecl.Parameters, Is.Null);
            }


            static (QualifierFlags, string, string) ExtractParamInfo(FunctionParameterNode param)
            {
                Assert.That(param.DeclarationSpecifiers.Keywords.AccessModifiers, Is.EqualTo(AccessModifiers.Unspecified));
                QualifierFlags qf = param.DeclarationSpecifiers.Keywords.QualifierFlags;
                string type = param.DeclarationSpecifiers.TypeName;
                return (qf, type, param.Declarator.Identifier);
            }
        }

        protected void AssertArrayDeclaration(string src,
                                              string type,
                                              string arrName,
                                              int? size = null,
                                              AccessModifiers access = AccessModifiers.Unspecified,
                                              QualifierFlags qualifiers = QualifierFlags.None,
                                              params object[]? init)
        {
            DeclarationStatementNode decl = this.AssertDeclarationNode(src, type, access, qualifiers);

            DeclaratorListNode declList = decl.Children.ElementAt(1).As<DeclaratorListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            ArrayDeclaratorNode arr = declList.Declarations.First().As<ArrayDeclaratorNode>();
            Assert.That(arr.Parent, Is.EqualTo(declList));
            Assert.That(arr.Identifier, Is.EqualTo(arrName));
            Assert.That(arr.Children.First().As<IdentifierNode>().Identifier, Is.EqualTo(arrName));
            if (size is { }) {
                Assert.That(arr.SizeExpression, Is.Not.Null);
                Assert.That(arr.SizeExpression!.Parent, Is.EqualTo(arr));
                Assert.That(ExpressionEvaluator.Evaluate(arr.SizeExpression!), Is.EqualTo(size));
            } else {
                Assert.That(arr.SizeExpression, Is.Null);
            }
            if (init is { } && init.Any()) {
                Assert.That(arr.Initializer, Is.Not.Null);
                Assert.That(arr.Initializer!.Parent, Is.EqualTo(arr));
                Assert.That(arr.Initializer.Initializers.Select(e => ExpressionEvaluator.Evaluate(e)), Is.EqualTo(init).Within(1e-10));
            } else {
                Assert.That(arr.Initializer, Is.Null);
            }
        }
    }
}
