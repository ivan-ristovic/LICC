using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders
{
    internal abstract class ASTBuilderTestBase
    {
        protected abstract ASTNode GenerateAST(string src);
    }
}
