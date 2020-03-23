namespace RICC.Core.Comparers
{
    public interface IASTNodeComparer<T>
    {
        MatchIssues Issues { get; }

        MatchIssues Compare(T n1, T n2);
    }
}
