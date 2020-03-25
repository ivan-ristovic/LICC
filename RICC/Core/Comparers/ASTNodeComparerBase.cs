using RICC.AST.Nodes;

namespace RICC.Core.Comparers
{
    public abstract class ASTNodeComparerBase<T> : IAbstractASTNodeComparer
        where T : ASTNode
    {
        public MatchIssues Issues { get; } = new MatchIssues();

        
        public abstract MatchIssues Compare(T n1, T n2);
        
        public virtual MatchIssues Compare(ASTNode n1, ASTNode n2) => this.Compare(n1.As<T>(), n2.As<T>());
    }
}
