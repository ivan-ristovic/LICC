using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace RICC.AST.Nodes.Common
{
    [DebuggerDisplay("{AccessModifiers} | {QualifierFlags}")]
    public sealed class DeclarationKeywords : IEquatable<DeclarationKeywords>
    {
        public static DeclarationKeywords Parse(string specs)
        {
            AccessModifiers access = AccessModifiers.Unspecified;

            string[] split = specs.ToLowerInvariant()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Distinct()
                .ToArray();

            if (split.Contains("private") || split.Contains("local"))
                access = AccessModifiers.Private;
            else if (split.Contains("protected"))
                access = AccessModifiers.Protected;
            else if (split.Contains("internal"))
                access = AccessModifiers.Internal;
            else if (split.Contains("public") || split.Contains("extern"))
                access = AccessModifiers.Public;

            QualifierFlags qualifiers = QualifierFlags.None;
            if (split.Contains("const"))
                qualifiers |= QualifierFlags.Const;
            if (split.Contains("static"))
                qualifiers |= QualifierFlags.Static;
            if (split.Contains("volatile"))
                qualifiers |= QualifierFlags.Volatile;

            return new DeclarationKeywords(access, qualifiers);
        }


        public AccessModifiers AccessModifiers { get; }
        public QualifierFlags QualifierFlags { get; }


        private DeclarationKeywords(AccessModifiers accessModifiers, QualifierFlags qualifiers)
        {
            this.AccessModifiers = accessModifiers;
            this.QualifierFlags = qualifiers;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            switch (this.AccessModifiers) {
                case AccessModifiers.Private: sb.Append("private "); break;
                case AccessModifiers.Protected: sb.Append("protected "); break;
                case AccessModifiers.Internal: sb.Append("internal "); break;
                case AccessModifiers.Public: sb.Append("public "); break;
            }
            if (this.QualifierFlags.HasFlag(QualifierFlags.Static))
               sb.Append("static ");
            if (this.QualifierFlags.HasFlag(QualifierFlags.Const))
               sb.Append("const ");
            if (this.QualifierFlags.HasFlag(QualifierFlags.Volatile))
               sb.Append("volatile ");
            return sb.ToString().Trim();
        }

        public override bool Equals(object? obj) 
            => this.Equals(obj as DeclarationKeywords);

        public bool Equals([AllowNull] DeclarationKeywords other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.AccessModifiers.Equals(other.AccessModifiers) && this.QualifierFlags.Equals(other.QualifierFlags);
        }
    }

    public enum AccessModifiers
    {
        Unspecified = 0, Private, Protected, Internal, Public
    }

    [Flags]
    public enum QualifierFlags
    {
        None = 0,
        Static = 1,
        Const = 2,
        Volatile = 4,
    }
}
