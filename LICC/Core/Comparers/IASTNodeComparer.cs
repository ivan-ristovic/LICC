using LICC.AST.Nodes;

namespace LICC.Core.Comparers
{
    public interface IASTNodeComparer
    {
        MatchIssues Issues { get; }

        MatchIssues Compare(ASTNode n1, ASTNode n2);
    }
}
