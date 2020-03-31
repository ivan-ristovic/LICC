﻿using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.Extensions;

namespace LICC.Tests.AST.Builders.Common
{
    internal abstract class JsonSerializationTestsBase : ASTBuilderTestBase
    {
        protected void AssertSerialization(string src)
        {
            ASTNode ast = this.GenerateAST(src);
            string? normal = null;
            string? compact = null;
            Assert.That(() => { normal = ast.ToJson(compact: false); }, Throws.Nothing);
            Assert.That(() => { compact = ast.ToJson(compact: true); }, Throws.Nothing);
            Assert.That(normal, Is.Not.Null);
            Assert.That(compact, Is.Not.Null);
            Assert.That(normal, Has.Length.GreaterThan(compact!.Length));
        }
    }
}

