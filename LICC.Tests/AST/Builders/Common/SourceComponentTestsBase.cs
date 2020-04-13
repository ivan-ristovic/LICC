using NUnit.Framework;
using LICC.AST.Nodes;

namespace LICC.Tests.AST.Builders.Common
{
    internal abstract class SourceComponentTestsBase : ASTBuilderTestBase
    {
        protected SourceNode AssertTranslationUnit(string src, bool empty = false)
        {
            SourceNode sc = this.GenerateAST(src).As<SourceNode>();
            Assert.That(sc, Is.Not.Null);
            Assert.That(sc, Is.InstanceOf<SourceNode>());
            Assert.That(sc.Line, Is.EqualTo(1));
            Assert.That(sc.Parent, Is.Null);
            Assert.That(sc.Children, empty ? Is.Empty : Is.Not.Empty);
            if (!empty)
                Assert.That(sc.Children, Is.Not.All.Null);
            return sc;
        }
    }
}
