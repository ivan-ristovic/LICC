namespace RICC.AST.Nodes.Common
{
    public enum Access
    {
        Private,
        Protected,
        Internal,
        Public,
    }

    public static class AccessExtensions
    {
        public static Access ParseAccessModifier(this string s)
        {
            return (s.ToLowerInvariant()) switch
            {
                "public" => Access.Public,
                "protected" => Access.Protected,
                "internal" => Access.Internal,
                "private" => Access.Private,
                _ => Access.Private,
            };
        }
    }
}
