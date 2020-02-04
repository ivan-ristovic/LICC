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
            this.AssertVariableDeclaration("int x;", "x", "int");
            this.AssertVariableDeclaration("float y;", "y", "float");
            this.AssertVariableDeclaration("Point z;", "z", "Point");
            this.AssertVariableDeclaration("time_t y_y;", "y_y", "time_t");
        }

        [Test]
        public void DeclarationSpecifierTest()
        {
            this.AssertVariableDeclaration(
                "static volatile time_t x;",
                "x",
                "time_t",
                AccessModifiers.Unspecified,
                QualifierFlags.Static | QualifierFlags.Volatile
            );
            this.AssertVariableDeclaration(
                "static extern const unsigned int x;",
                "x",
                "unsigned int",
                AccessModifiers.Public,
                QualifierFlags.Static | QualifierFlags.Const
            );
        }

        [Test]
        public void InitializerDeclarationTest()
        {
            this.AssertVariableDeclaration("static signed int x = 5;", "x", "int", AccessModifiers.Unspecified, QualifierFlags.Static, 5);
        }

        [Test]
        public void InitializerExpressionDeclarationTest()
        {
            this.AssertVariableDeclaration("unsigned short x = 1 << 2 * 4;", "x", "unsigned short", AccessModifiers.Unspecified, value: 1 << 8);
        }

        [Test]
        public void FunctionDeclarationNoParamsTest()
        {
            this.AssertFunctionDeclaration("void f();", "f", "void");
            this.AssertFunctionDeclaration("extern static void f();", "f", "void", AccessModifiers.Public, QualifierFlags.Static);
        }

        [Test]
        public void FunctionDeclarationVariadicTest()
        {
            this.AssertFunctionDeclaration(
                "void f(const int x, ...);",
                "f",
                "void",
                isVariadic: true,
                @params: (QualifierFlags.Const, "int", "x")
            );
        }

        [Test]
        public void FunctionDeclarationQualifiersTest()
        {
            this.AssertFunctionDeclaration(
                "extern static time_t f(int x, const volatile unsigned long long y, ...);",
                "f",
                "time_t",
                AccessModifiers.Public,
                QualifierFlags.Static,
                true,
                (QualifierFlags.None, "int", "x"),
                (QualifierFlags.Const | QualifierFlags.Volatile, "unsigned long long", "y")
            );
        }

        [Test]
        public void FunctionDeclarationArrayParamsTest()
        {
            this.AssertFunctionDeclaration(
                "int f(int a[], const float b[4]);",
                "f",
                "int",
                AccessModifiers.Unspecified,
                QualifierFlags.None,
                false,
                (QualifierFlags.None, "int", "a"),
                (QualifierFlags.Const, "float", "b")
            );
        }

        [Test]
        public void SimpleDeclarationListTest()
        {
            this.AssertVariableDeclarationList(
                "static unsigned int x, y, z;",
                "unsigned int",
                AccessModifiers.Unspecified, QualifierFlags.Static,
                ("x", null), ("y", null), ("z", null)
            );
        }

        [Test]
        public void IntDeclarationListWithInitializersTest()
        {
            this.AssertVariableDeclarationList(
                "extern static int x, y = 7 + (4 - 3), z = 3, w = 3*4 + 7*5, t = 2 >> (3 << 4);",
                "int",
                AccessModifiers.Public, QualifierFlags.Static,
                ("x", null), ("y", 7 + (4 - 3)), ("z", 3), ("w", 47), ("t", 2 >> (3 << 4))
            );

            this.AssertVariableDeclarationList(
                "extern static int x = 010, y = 0xfF, z = 0xabcdef, w = 0xA + 010, t = 0x1 << 4;",
                "int",
                AccessModifiers.Public, QualifierFlags.Static,
                ("x", 8), ("y", 0xFF), ("z", 0xABCDEF), ("w", 0xA + 8), ("t", 1 << 4)
            );

            this.AssertVariableDeclarationList(
                "extern static int x = 010u, y = 0xfFl, z = 0xabcdefUL, w = 0xAL + 010l, t = 0x1uLL << 4;",
                "int",
                AccessModifiers.Public, QualifierFlags.Static,
                ("x", 8), ("y", 0xFF), ("z", 0xABCDEF), ("w", 0xA + 8), ("t", 1 << 4)
            );
        }

        [Test]
        public void FloatDeclarationListWithInitializersTest()
        {
            this.AssertVariableDeclarationList(
                "float x, y = 7.1 + 4.2, z = 3.0, w = 3.2*4.45 + 7.2*5.11 - (5.0/2.5);",
                "float",
                AccessModifiers.Unspecified, QualifierFlags.None,
                ("x", null), ("y", 11.3), ("z", 3.0), ("w", 49.032)
            );

            this.AssertVariableDeclarationList(
                "float x, y = 7.1f + 4.2, z = 3.0f, w = 3.2L*4.45f + 7.2f*5.11f - (5.0/2.5);",
                "float",
                AccessModifiers.Unspecified, QualifierFlags.None,
                ("x", null), ("y", 11.3), ("z", 3.0f), ("w", 49.032)
            );

            this.AssertVariableDeclarationList(
                "float x = -1e-10, y = 7.1f + 4.2f, z = +3e-10, w = 3.2e+0*4.45e0 + 7.2*5.11 - (5.0/2.5);",
                "float",
                AccessModifiers.Unspecified, QualifierFlags.None,
                ("x", -1e-10), ("y", 11.3f), ("z", 3e-10), ("w", 49.032)
            );
        }

        [Test]
        public void BoolDeclarationListWithInitializersTest()
        {
            this.AssertVariableDeclarationList(
                "bool x, y = 1 == 1, z = 3 <= 4, w = 4 != (3 + 1);",
                "bool",
                AccessModifiers.Unspecified, QualifierFlags.None,
                ("x", null), ("y", true), ("z", true), ("w", false)
            );
        }

        [Test]
        public void StringDeclarationListWithInitializersTest()
        {
            this.AssertVariableDeclarationList(
                @"char* w1, w2 = ""abc"", w3 = ""aa"" + ""bb"";",
                "char*",
                AccessModifiers.Unspecified, QualifierFlags.None,
                ("w1", null), ("w2", "abc"), ("w3", "aabb")
            );
        }

        [Test]
        public void ArrayDeclarationTest()
        {
            this.AssertArrayDeclaration(
                "static unsigned int x[3];",
                "unsigned int",
                "x",
                3,
                AccessModifiers.Unspecified, QualifierFlags.Static
            );
            this.AssertArrayDeclaration(
                "static unsigned int x[3 + 4 * 1 - 3];",
                "unsigned int",
                "x",
                4,
                AccessModifiers.Unspecified, QualifierFlags.Static
            );
            this.AssertArrayDeclaration(
                "const unsigned long long int x[(2 << 1) / 2];",
                "unsigned long long int",
                "x",
                2,
                AccessModifiers.Unspecified, QualifierFlags.Const
            );
        }

        [Test]
        public void ArrayDeclarationInitializerTest()
        {
            this.AssertArrayDeclaration(
                "extern int x[3] = { 3, 4, 5 };",
                "int",
                "x",
                3,
                AccessModifiers.Public, QualifierFlags.None,
                3, 4, 5
            );
            this.AssertArrayDeclaration(
                "const int x[3] = { 1 + 2, 2 << 1 };",
                "int",
                "x",
                3,
                AccessModifiers.Unspecified, QualifierFlags.Const,
                3, 4
            );
            this.AssertArrayDeclaration(
                "int x[] = { 1 + 2, 2 << 1 };",
                "int",
                "x",
                null,
                AccessModifiers.Unspecified, QualifierFlags.None,
                3, 4
            );
            this.AssertArrayDeclaration(
                "volatile int x[] = { 1 + 2, 2 << 1, 200, 0x31 };",
                "int",
                "x",
                null,
                AccessModifiers.Unspecified, QualifierFlags.Volatile,
                3, 4, 200, 0x31
            );
        }


        private DeclarationStatementNode AssertDeclarationNode(string src,
                                                               string type,
                                                               AccessModifiers access = AccessModifiers.Unspecified,
                                                               QualifierFlags qualifiers = QualifierFlags.None)
        {
            ASTNode ast = CASTProvider.BuildFromSource(src);
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

        private void AssertVariableDeclaration(string src,
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

        private void AssertVariableDeclarationList(string src,
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

        private void AssertFunctionDeclaration(string src,
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

        private void AssertArrayDeclaration(string src,
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
