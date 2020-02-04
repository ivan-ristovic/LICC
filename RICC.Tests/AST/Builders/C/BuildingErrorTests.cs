using System;
using System.IO;
using NUnit.Framework;
using RICC.AST;
using RICC.AST.Builders.C;
using RICC.Exceptions;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class BuildingErrorTests : BuildingErrorTestsBase<CASTBuilder>
    {
        [Test]
        public void SourceNotFoundTest()
        {
            Assert.Throws<FileNotFoundException>(() => ASTFactory.BuildFromFile("404.c"));
        }

        [Test]
        public void InvalidDeclarationTests()
        {
            this.AssertThrows<NotImplementedException>("something;");
            this.AssertThrows<SyntaxException>("void f { };");
            this.AssertThrows<SyntaxException>("void ();");
            this.AssertThrows<SyntaxException>("int f(int, int x);");
            this.AssertThrows<SyntaxException>("int f(0 x);");
            this.AssertThrows<SyntaxException>("int f(3);");
            this.AssertThrows<SyntaxException>("int f(int[] x);");
            this.AssertThrows<SyntaxException>("int[] f(int x);");
            this.AssertThrows<SyntaxException>("int f[](int x,,);");
            this.AssertThrows<SyntaxException>("int f(int x, int y,);");
            this.AssertThrows<SyntaxException>("int f(int x, int y){}{};");
            this.AssertThrows<SyntaxException>("int x = ;;");
            this.AssertThrows<SyntaxException>("int x = .3, 2..;");
            this.AssertThrows<SyntaxException>("int x = ..3;;");
            this.AssertThrows<SyntaxException>("int x = ();");
        }

        [Test]
        public void InvalidExpressionTests()
        {
            this.AssertThrows<SyntaxException>("int x = 1 +;");
            this.AssertThrows<SyntaxException>("int x = 1 // 2;");
            this.AssertThrows<SyntaxException>("int x = 1++");
            this.AssertThrows<SyntaxException>("int x = 1 +* 2");
            this.AssertThrows<SyntaxException>("int x = 1 << >> 2");
            this.AssertThrows<SyntaxException>("int x = 1 || || 2");
            this.AssertThrows<SyntaxException>("int x = 1 ? 2 : 3 : 2");
            this.AssertThrows<SyntaxException>("int x = f(,);");
        }

        [Test]
        public void InvalidIfStatementTests()
        {
            this.AssertThrows<SyntaxException>("if (x)");
            this.AssertThrows<SyntaxException>("if x {} else {}");
            this.AssertThrows<SyntaxException>("if (x) { } { }");
            this.AssertThrows<SyntaxException>("if (x > > 1) {}");
            this.AssertThrows<SyntaxException>("if (1) ;; else ;");
        }

        [Test]
        public void InvalidForStatementTests()
        {
            this.AssertThrows<SyntaxException>("for (int x, int y; x < y; x++ y++) {}");
            this.AssertThrows<SyntaxException>("for (int x, y; x < y; ; x++ y++) {}");
            this.AssertThrows<SyntaxException>("for (;;;;){}");
            this.AssertThrows<SyntaxException>("for (int i = 0; i < n; i++,) {}");
        }
    }
}
