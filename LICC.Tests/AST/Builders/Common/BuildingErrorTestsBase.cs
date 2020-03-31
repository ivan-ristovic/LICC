using System;
using NUnit.Framework;

namespace LICC.Tests.AST.Builders.Common
{
    internal abstract class BuildingErrorTestsBase : ASTBuilderTestBase
    {
        protected void AssertThrows<TException>(string src) where TException : Exception
            => Assert.That(() => this.GenerateAST(src), Throws.InstanceOf<TException>());
    }
}
