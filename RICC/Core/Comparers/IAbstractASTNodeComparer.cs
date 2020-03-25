using RICC.AST.Nodes;

namespace RICC.Core.Comparers
{
    public interface IAbstractASTNodeComparer
    {
        MatchIssues Issues { get; }

        MatchIssues Compare(ASTNode n1, ASTNode n2);
    }
}
