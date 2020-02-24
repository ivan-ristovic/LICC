using System;

namespace RICC.AST.Nodes.Common
{
    public enum JumpStatementType
    {
        Return,
        Continue,
        Break,
        Goto
    }

    public static class JumpStatementTypeConverter
    {
        public static JumpStatementType FromString(string str)
        {
            return str switch
            {
                "return" => JumpStatementType.Return,
                "continue" => JumpStatementType.Continue,
                "break" => JumpStatementType.Break,
                "goto" => JumpStatementType.Goto,
                _ => throw new ArgumentException("Invalid jump statement token"),
            };
        }

        public static string ToStringToken(this JumpStatementType type)
        {
            return type switch
            {
                JumpStatementType.Break => "break",
                JumpStatementType.Continue => "continue",
                JumpStatementType.Goto => "goto",
                JumpStatementType.Return => "return",
                _ => throw new ArgumentException("Invalid jump statement value"),
            };
        }
    }
}
