using System;
using System.IO;
using NUnit.Framework;
using RICC.AST;
using RICC.Exceptions;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class BuildingErrorTests
    {
        [Test]
        public void SourceNotFoundTest()
        {
            Assert.Throws<FileNotFoundException>(() => ASTFactory.BuildFromFile("404.c"));
        }

        [Test]
        public void InvalidDeclarationTests()
        {
            Assert.That(() => CASTProvider.BuildFromSource("something;"), Throws.InstanceOf<NotImplementedException>());
            Assert.That(() => CASTProvider.BuildFromSource("void f { };"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("void ();"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int f(int, int x);"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int f(0 x);"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int f(3);"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int f(int[] x);"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int[] f(int x);"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int f[](int x,,);"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int f(int x, int y,);"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int f(int x, int y){}{};"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = ;;"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = .3, 2..;"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = ..3;;"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = ();"), Throws.InstanceOf<SyntaxException>());
        }

        [Test]
        public void InvalidExpressionTests()
        {
            Assert.That(() => CASTProvider.BuildFromSource("int x = 1 +;"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = 1 // 2;"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = 1++"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = 1 +* 2"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = 1 << >> 2"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = 1 || || 2"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = 1 ? 2 : 3 : 2"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("int x = f(,);"), Throws.InstanceOf<SyntaxException>());
        }

        [Test]
        public void InvalidIfStatementTests()
        {
            Assert.That(() => CASTProvider.BuildFromSource("if (x)"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("if x {} else {}"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("if (x) { } { }"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("if (x > > 1) {}"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("if (1) ;; else ;"), Throws.InstanceOf<SyntaxException>());
        }

        [Test]
        public void InvalidForStatementTests()
        {
            Assert.That(() => CASTProvider.BuildFromSource("for (int x, int y; x < y; x++ y++) {}"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("for (int x, y; x < y; ; x++ y++) {}"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("for (;;;;){}"), Throws.InstanceOf<SyntaxException>());
            Assert.That(() => CASTProvider.BuildFromSource("for (int i = 0; i < n; i++,) {}"), Throws.InstanceOf<SyntaxException>());
        }
    }
}
