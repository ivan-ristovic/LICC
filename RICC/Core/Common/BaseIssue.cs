using System;
using System.Diagnostics.CodeAnalysis;

namespace RICC.Core.Common
{
    public abstract class BaseIssue : IEquatable<BaseIssue>
    {
        public static bool operator ==(BaseIssue x, BaseIssue y) => x.Equals(y);
        public static bool operator !=(BaseIssue x, BaseIssue y) => !(x == y);


        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => this.GetType().Name;

        public override bool Equals(object? obj)
            => this.Equals(obj as BaseIssue);


        public virtual bool Equals([AllowNull] BaseIssue other) => other is { } && this.GetType() == other.GetType();


        public abstract void LogIssue();
    }
}
