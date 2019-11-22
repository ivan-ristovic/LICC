using System;
using System.Collections.Generic;
using System.Linq;

namespace RICC.AST.Nodes.Common
{
    [Flags]
    public enum DeclarationSpecifiersFlags
    {
        Private = 0,
        Protected = 1,
        Public = 2,
        Static = 4,
    }

    public static class DeclarationSpecifiers
    {
        public static DeclarationSpecifiersFlags Parse(IEnumerable<string> specs)
        {
            DeclarationSpecifiersFlags retFlags = DeclarationSpecifiersFlags.Private;
            foreach (string s in specs)
                retFlags |= Parse(s);
            return retFlags;
        }

        public static DeclarationSpecifiersFlags Parse(string specs)
        {
            DeclarationSpecifiersFlags retval = DeclarationSpecifiersFlags.Private;

            string[] split = specs.ToLowerInvariant()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToArray();

            if (split.Contains("public"))
                retval |= DeclarationSpecifiersFlags.Public;
            if (split.Contains("protected"))
                retval |= DeclarationSpecifiersFlags.Protected;
            if (split.Contains("internal"))
                retval |= (DeclarationSpecifiersFlags.Public | DeclarationSpecifiersFlags.Protected);
            if (split.Contains("static"))
                retval |= DeclarationSpecifiersFlags.Static;

            return retval;
        }
    }
}
