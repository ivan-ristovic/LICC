using RICC.AST.Nodes;

namespace RICC.AST.Builders
{
    public interface IASTBuilder
    {
        ASTNode BuildFromSource(string code);
    }
}
