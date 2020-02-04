using NUnit.Framework;
using RICC.AST.Builders;
using RICC.AST.Nodes;
using RICC.Extensions;

namespace RICC.Tests.AST.Common.Json
{
    internal abstract class JsonSerializationTestsBase<TBuilder> where TBuilder : IASTBuilder, new()
    {
        protected void AssertSerialization(string src)
        {
            ASTNode ast = new TBuilder().BuildFromSource(src);
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

