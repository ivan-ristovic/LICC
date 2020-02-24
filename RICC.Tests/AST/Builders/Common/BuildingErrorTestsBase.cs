using System;
using NUnit.Framework;
using RICC.AST.Builders;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class BuildingErrorTestsBase<TBuilder> where TBuilder : IASTBuilder, new()
    {
        protected void AssertThrows<TException>(string src) where TException : Exception
            => Assert.That(() => new TBuilder().BuildFromSource(src), Throws.InstanceOf<TException>());
    }
}
