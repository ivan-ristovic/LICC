using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders
{
    internal abstract class ASTBuilderTestBase
    {
        protected abstract ASTNode GenerateAST(string src);


        protected void AssertChildrenParentProperties(ASTNode node)
        {
            foreach (ASTNode child in node.Children)
                Assert.That(child.Parent, Is.EqualTo(node));
        }
    }
}
