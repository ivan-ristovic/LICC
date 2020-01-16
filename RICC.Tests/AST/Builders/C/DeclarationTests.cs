using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.AST.Visitors;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class DeclarationTests
    {
        [Test]
        public void SimpleDeclarationTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("int x; float y; Point z;time_t y_y;");
            this.AssertVariableDeclaration(ast.Children.ElementAt(0), "x", "int");
            this.AssertVariableDeclaration(ast.Children.ElementAt(1), "y", "float");
            this.AssertVariableDeclaration(ast.Children.ElementAt(2), "z", "Point");
            this.AssertVariableDeclaration(ast.Children.ElementAt(3), "y_y", "time_t");
        }

        [Test]
        public void DeclarationSpecifierTest()
        {
            ASTNode ast1 = CASTProvider.BuildFromSource("static volatile time_t x;");
            this.AssertVariableDeclaration(ast1.Children.First(), "x", "time_t", AccessModifiers.Unspecified, QualifierFlags.Static | QualifierFlags.Volatile);
            ASTNode ast2 = CASTProvider.BuildFromSource("static extern const unsigned int x;");
            this.AssertVariableDeclaration(ast2.Children.First(), "x", "unsigned int", AccessModifiers.Public, QualifierFlags.Static | QualifierFlags.Const);
        }

        [Test]
        public void InitializerDeclarationTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("static signed int x = 5;");
            this.AssertVariableDeclaration(ast.Children.First(), "x", "int", AccessModifiers.Unspecified, QualifierFlags.Static, 5);
        }

        [Test]
        public void InitializerExpressionDeclarationTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("unsigned short x = 1 << 2 * 4;");
            this.AssertVariableDeclaration(ast.Children.First(), "x", "unsigned short", AccessModifiers.Unspecified, value: 1 << 8);
        }

        [Test]
        public void FunctionDeclarationNoParamsTest()
        {
            ASTNode ast1 = CASTProvider.BuildFromSource("void f();");
            this.AssertFunctionDeclaration(ast1, "f", "void");

            ASTNode ast2 = CASTProvider.BuildFromSource("extern static void f();");
            this.AssertFunctionDeclaration(ast2, "f", "void", AccessModifiers.Public, QualifierFlags.Static);
        }

        [Test]
        public void FunctionDeclarationVariadicTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("void f(const int x, ...);");
            this.AssertFunctionDeclaration(ast, "f", "void", isVariadic: true, @params: (QualifierFlags.Const, "int", "x"));
        }

        [Test]
        public void ComplexFunctionDeclarationTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("extern static time_t f(int x, const volatile unsigned long long y, ...);");
            this.AssertFunctionDeclaration(ast, "f", "time_t", AccessModifiers.Public, QualifierFlags.Static, true, 
                (QualifierFlags.None, "int", "x"), (QualifierFlags.Const | QualifierFlags.Volatile, "unsigned long long", "y")
            );
        }

        [Test]
        public void SimpleDeclarationListTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("static unsigned int x, y, z;");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "unsigned int",
                AccessModifiers.Unspecified, QualifierFlags.Static,
                ("x", null), ("y", null), ("z", null)
            );
        }

        [Test]
        public void IntDeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("extern static int x, y = 7 + (4 - 3), z = 3, w = 3*4 + 7*5, t = 2 >> (3 << 4);");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "int",
                AccessModifiers.Public, QualifierFlags.Static,
                ("x", null), ("y", 7 + (4 - 3)), ("z", 3), ("w", 47), ("t", 2 >> (3 << 4))
            );
        }

        [Test]
        public void FloatDeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("float x, y = 7.1 + 4.2, z = 3.0, w = 3.2*4.45 + 7.2*5.11 - (5.0/2.5);");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "float",
                AccessModifiers.Unspecified, QualifierFlags.None,
                ("x", null), ("y", 11.3), ("z", 3.0), ("w", 49.032)
            );
        }

        [Test]
        public void BoolDeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("bool x, y = 1 == 1, z = 3 <= 4, w = 4 != (3 + 1);");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "bool",
                AccessModifiers.Unspecified, QualifierFlags.None,
                ("x", null), ("y", true), ("z", true), ("w", false)
            );
        }

        [Test]
        public void StringDeclarationListWithInitializersTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@"char* w1, w2 = ""abc"", w3 = ""aa"" + ""bb"";");
            this.AssertVariableDeclarationList(
                ast.Children.First(),
                "char*",
                AccessModifiers.Unspecified, QualifierFlags.None,
                ("w1", null), ("w2", "abc"), ("w3", "aabb")
            );
        }


        private void AssertVariableDeclaration(ASTNode node,
                                               string identifier,
                                               string type,
                                               AccessModifiers access = AccessModifiers.Unspecified,
                                               QualifierFlags qualifiers = QualifierFlags.None,
                                               object? value = null)
        {
            DeclarationStatementNode decl = node.As<DeclarationStatementNode>();
            Assert.That(decl.Children, Has.Exactly(2).Items);

            DeclarationSpecifiersNode declSpecsNode = decl.Children.ElementAt(0).As<DeclarationSpecifiersNode>();
            Assert.That(declSpecsNode.Parent, Is.EqualTo(decl));
            Assert.That(declSpecsNode.Keywords.AccessModifiers, Is.EqualTo(access));
            Assert.That(declSpecsNode.Keywords.QualifierFlags, Is.EqualTo(qualifiers));
            Assert.That(declSpecsNode.TypeName, Is.EqualTo(type));
            Assert.That(declSpecsNode.Children, Is.Empty);

            DeclaratorListNode declList = decl.Children.ElementAt(1).As<DeclaratorListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            VariableDeclaratorNode var = declList.Declarations.First().As<VariableDeclaratorNode>();
            Assert.That(var.Parent, Is.EqualTo(declList));
            Assert.That(var.Identifier, Is.EqualTo(identifier));
            Assert.That(var.Children.First().As<IdentifierNode>().Identifier, Is.EqualTo(identifier));
            if (value is { }) {
                Assert.That(var.Initializer, Is.Not.Null);
                Assert.That(var.Initializer!.Parent, Is.EqualTo(var));
                Assert.That(ExpressionEvaluator.TryEvaluateAs(var.Initializer, out object? result));
                Assert.That(result, Is.Not.Null);
                Assert.That(result, Is.EqualTo(value).Within(1e-10));
            } else {
                Assert.That(var.Initializer, Is.Null);
            }
        }

        private void AssertVariableDeclarationList(ASTNode node,
                                                   string type,
                                                   AccessModifiers access = AccessModifiers.Unspecified,
                                                   QualifierFlags qualifiers = QualifierFlags.None,
                                                   params (string Identifier, object? Value)[] vars)
        {
            DeclarationStatementNode decl = node.As<DeclarationStatementNode>();
            Assert.That(decl.Children, Has.Exactly(2).Items);

            DeclarationSpecifiersNode declSpecsNode = decl.Children.ElementAt(0).As<DeclarationSpecifiersNode>();
            Assert.That(declSpecsNode.Parent, Is.EqualTo(decl));
            Assert.That(declSpecsNode.Keywords.AccessModifiers, Is.EqualTo(access));
            Assert.That(declSpecsNode.Keywords.QualifierFlags, Is.EqualTo(qualifiers));
            Assert.That(declSpecsNode.TypeName, Is.EqualTo(type));
            Assert.That(declSpecsNode.Children, Is.Empty);

            DeclaratorListNode declList = decl.Children.ElementAt(1).As<DeclaratorListNode>();
            Assert.That(declList.Parent, Is.EqualTo(decl));
            Assert.That(declList.Declarations.Select(var => ExtractIdentifierAndValue(var)), Is.EqualTo(vars).Within(1e-10));


            static (string, object?) ExtractIdentifierAndValue(DeclarationNode declNode)
            {
                VariableDeclaratorNode var = declNode.As<VariableDeclaratorNode>();
                return var.Initializer is null ? (var.Identifier, (object?)null)
                                               : (var.Identifier, ExpressionEvaluator.Evaluate(var.Initializer));
            }
        }

        private void AssertFunctionDeclaration(ASTNode node,
                                               string fname,
                                               string returnType,
                                               AccessModifiers access = AccessModifiers.Unspecified,
                                               QualifierFlags qualifiers = QualifierFlags.None,
                                               bool isVariadic = false,
                                               params (QualifierFlags Qualifiers, string Type, string Identifier)[] @params)
        {
            DeclarationStatementNode decl = node.Children.First().As<DeclarationStatementNode>();
            Assert.That(decl.Children, Has.Exactly(2).Items);

            DeclarationSpecifiersNode declSpecsNode = decl.Children.ElementAt(0).As<DeclarationSpecifiersNode>();
            Assert.That(declSpecsNode.Parent, Is.EqualTo(decl));
            Assert.That(declSpecsNode.Keywords.AccessModifiers, Is.EqualTo(access));
            Assert.That(declSpecsNode.Keywords.QualifierFlags, Is.EqualTo(qualifiers));
            Assert.That(declSpecsNode.TypeName, Is.EqualTo(returnType));
            Assert.That(declSpecsNode.Children, Is.Empty);

            FunctionDeclaratorNode fdecl = node.Children.First().Children.Last().Children.First().As<FunctionDeclaratorNode>();
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
                return (qf, type, param.Identifier);
            }
        }
    }
}
