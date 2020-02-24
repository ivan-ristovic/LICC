using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders.Common
{
    internal abstract class TranslationUnitTestsBase<TBuilder> where TBuilder : IASTBuilder, new()
    {
        protected TranslationUnitNode AssertTranslationUnit(string src, bool empty = false)
        {
            TranslationUnitNode tu = new TBuilder().BuildFromSource(src).As<TranslationUnitNode>();
            Assert.That(tu, Is.Not.Null);
            Assert.That(tu, Is.InstanceOf<TranslationUnitNode>());
            Assert.That(tu.Line, Is.EqualTo(1));
            Assert.That(tu.Parent, Is.Null);
            Assert.That(tu.Children, empty ? Is.Empty : Is.Not.Empty);
            if (!empty)
                Assert.That(tu.Children, Is.Not.All.Null);
            return tu;
        }
    }
}
