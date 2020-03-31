namespace LICC.Core.Common
{
    public abstract class BaseError : BaseIssue
    {
        public override string ToString() => $"ERR {base.ToString()}";
    }
}
