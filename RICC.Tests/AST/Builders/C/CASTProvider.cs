using RICC.AST.Builders.C;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders.C
{
    internal static class CASTProvider
    {
        public static ASTNode BuildFromSource(string code)
            => new CASTBuilder().BuildFromSource(code);
    }
}
