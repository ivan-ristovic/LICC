using System;
using System.Linq;

namespace RICC.AST.Nodes.Common
{
    public sealed class DeclarationSpecifiers
    {
        public static DeclarationSpecifiers Parse(string specs)
        {
            AccessModifier access = AccessModifier.Unspecified;

            string[] split = specs.ToLowerInvariant()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToArray();

            if (split.Contains("private"))
                access = AccessModifier.Private;
            else if (split.Contains("protected"))
                access = AccessModifier.Protected;
            else if (split.Contains("internal"))
                access = AccessModifier.Internal;
            else if (split.Contains("public") || split.Contains("extern"))
                access = AccessModifier.Public;

            bool isStatic = split.Any(s => s.ToLower() == "static");
            return new DeclarationSpecifiers(access, isStatic);
        }


        public AccessModifier AccessModifiers { get; }
        public bool IsStatic { get; }


        private DeclarationSpecifiers(AccessModifier accessModifiers, bool isStatic)
        {
            this.AccessModifiers = accessModifiers;
            this.IsStatic = isStatic;
        }


        public override string ToString()
        {
            string modifiers = "";
            switch (this.AccessModifiers) {
                case AccessModifier.Private: modifiers = "private"; break;
                case AccessModifier.Protected: modifiers = "protected"; break;
                case AccessModifier.Internal: modifiers = "internal"; break;
                case AccessModifier.Public: modifiers = "public"; break;
            }
            return $"{modifiers}{(this.IsStatic ? " static" : "")}";
        }
    }

    public enum AccessModifier
    {
        Unspecified = 0, Private, Protected, Internal, Public
    }
}
