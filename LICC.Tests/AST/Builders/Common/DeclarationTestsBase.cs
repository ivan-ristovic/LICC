using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;
using LICC.AST.Visitors;

namespace LICC.Tests.AST.Builders.Common
{
    internal abstract class DeclarationTestsBase : ASTBuilderTestBase
    {
        protected DeclStatNode AssertDeclarationNode(string src,
                                                                 string type,
                                                                 AccessModifiers access = AccessModifiers.Unspecified,
                                                                 QualifierFlags qualifiers = QualifierFlags.None)
        {
            ASTNode ast = this.GenerateAST(src);
            DeclStatNode decl = ast is SourceNode block
                ? this.ExtractDeclarationFromBlock(block)
                : ast.As<DeclStatNode>();
            Assert.That(decl.Children, Has.Exactly(2).Items);
            Assert.That(decl.Specifiers.Parent, Is.EqualTo(decl));
            Assert.That(decl.Specifiers.Keywords.AccessModifiers, Is.EqualTo(access));
            Assert.That(decl.Specifiers.Keywords.QualifierFlags, Is.EqualTo(qualifiers));
            Assert.That(decl.Specifiers.TypeName, Is.EqualTo(type));
            Assert.That(decl.Specifiers.Children, Is.Empty);
            return decl;
        }

        protected void AssertVariableDeclaration(string src,
                                                 string identifier,
                                                 string type,
                                                 AccessModifiers access = AccessModifiers.Unspecified,
                                                 QualifierFlags qualifiers = QualifierFlags.None,
                                                 object? value = null)
        {
            DeclStatNode decl = this.AssertDeclarationNode(src, type, access, qualifiers);

            Assert.That(decl.DeclaratorList.Parent, Is.EqualTo(decl));
            VarDeclNode var = decl.DeclaratorList.Declarations.Single().As<VarDeclNode>();
            Assert.That(var.Parent, Is.EqualTo(decl.DeclaratorList));
            Assert.That(var.Identifier, Is.EqualTo(identifier));
            Assert.That(var.Children.First().As<IdNode>().Identifier, Is.EqualTo(identifier));
            if (value is { }) {
                Assert.That(var.Initializer, Is.Not.Null);
                Assert.That(var.Initializer!.Parent, Is.EqualTo(var));
                Assert.That(ConstantExpressionEvaluator.Evaluate(var.Initializer), Is.EqualTo(value).Within(1e-10));
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
            DeclStatNode decl = this.AssertDeclarationNode(src, type, access, qualifiers);
            Assert.That(decl.DeclaratorList.Parent, Is.EqualTo(decl));
            Assert.That(decl.DeclaratorList.Declarations.Select(var => ExtractIdentifierAndValue(var)), Is.EqualTo(vars).Within(1e-6));


            static (string, object?) ExtractIdentifierAndValue(DeclarationNode declNode)
            {
                VarDeclNode var = declNode.As<VarDeclNode>();
                return var.Initializer is null ? (var.Identifier, (object?)null)
                                               : (var.Identifier, ConstantExpressionEvaluator.Evaluate(var.Initializer));
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
            DeclStatNode decl = this.AssertDeclarationNode(src, returnType, access, qualifiers);

            FuncDeclNode fdecl = decl.DeclaratorList.Declarations.Single().As<FuncDeclNode>();
            Assert.That(fdecl.Identifier, Is.EqualTo(fname));
            Assert.That(fdecl.IsVariadic, Is.EqualTo(isVariadic));
            if (@params.Any()) {
                Assert.That(fdecl.Parameters, Is.Not.Null);
                Assert.That(fdecl.Parameters.Select(p => ExtractParamInfo(p)), Is.EqualTo(@params));
            } else {
                Assert.That(fdecl.Parameters, Is.Null);
            }


            static (QualifierFlags, string, string) ExtractParamInfo(FuncParamNode param)
            {
                Assert.That(param.Specifiers.Keywords.AccessModifiers, Is.EqualTo(AccessModifiers.Unspecified));
                QualifierFlags qf = param.Specifiers.Keywords.QualifierFlags;
                string type = param.Specifiers.TypeName;
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
            DeclStatNode decl = this.AssertDeclarationNode(src, type, access, qualifiers);

            Assert.That(decl.DeclaratorList.Parent, Is.EqualTo(decl));
            ArrDeclNode arr = decl.DeclaratorList.Declarations.First().As<ArrDeclNode>();
            Assert.That(arr.Parent, Is.EqualTo(decl.DeclaratorList));
            Assert.That(arr.Identifier, Is.EqualTo(arrName));
            Assert.That(arr.Children.First().As<IdNode>().Identifier, Is.EqualTo(arrName));
            if (size is { }) {
                Assert.That(arr.SizeExpression, Is.Not.Null);
                Assert.That(arr.SizeExpression!.Parent, Is.EqualTo(arr));
                Assert.That(ConstantExpressionEvaluator.Evaluate(arr.SizeExpression!), Is.EqualTo(size));
            } else {
                Assert.That(arr.SizeExpression, Is.Null);
            }
            if (init is { } && init.Any()) {
                Assert.That(arr.Initializer, Is.Not.Null);
                Assert.That(arr.Initializer!.Parent, Is.EqualTo(arr));
                Assert.That(arr.Initializer.Initializers.Select(e => ConstantExpressionEvaluator.Evaluate(e)), Is.EqualTo(init).Within(1e-10));
            } else {
                Assert.That(arr.Initializer, Is.Null);
            }
        }


        private DeclStatNode ExtractDeclarationFromBlock(SourceNode src)
        {
            if (src.Children.Count == 1)
                return src.Children.Single().As<DeclStatNode>();

            var decls = src.Children
                .Take(2)
                .Cast<DeclStatNode>()
                .ToList()
                ;
            IEnumerable<DeclNode> declarators = decls[0].DeclaratorList.Declarations
                .Zip(decls[1].DeclaratorList.Declarations)
                .Select(FormDeclarator)
                ;
            var declList = new DeclListNode(decls[0].DeclaratorList.Line, declarators);
            return new DeclStatNode(decls[0].Line, decls[0].Specifiers, declList);


            static DeclNode FormDeclarator((DeclNode, DeclNode) decl)
            {
                switch (decl.Item1) {
                    case VarDeclNode v1:
                        VarDeclNode v2 = decl.Item2.As<VarDeclNode>();
                        return v2.Initializer is null
                            ? new VarDeclNode(v1.Line, v1.IdentifierNode)
                            : new VarDeclNode(v1.Line, v1.IdentifierNode, v2.Initializer);
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
