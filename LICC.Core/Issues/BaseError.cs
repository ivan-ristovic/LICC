namespace LICC.Core.Issues
{
    public abstract class BaseError : BaseIssue
    {
        public override string ToString() => $"ERR {base.ToString()}";
    }
}
