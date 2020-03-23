using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RICC.Core.Common;
using Serilog;

namespace RICC.Core
{
    public sealed class ComparerResult : IEquatable<ComparerResult>
    {
        public static bool operator ==(ComparerResult e1, ComparerResult e2) => e1.Equals(e2);
        public static bool operator !=(ComparerResult e1, ComparerResult e2) => !(e1 == e2);


        public bool Success => !this.issues.Any(i => i is BaseError);

        private readonly List<BaseIssue> issues;


        public ComparerResult()
        {
            this.issues = new List<BaseIssue>();
        }


        public ComparerResult WithWarning(BaseWarning wrn)
        {
            this.issues.Add(wrn);
            return this;
        }

        public ComparerResult WithError(BaseError err)
        {
            this.issues.Add(err);
            return this;
        }

        public ComparerResult WithResult(ComparerResult res)
        {
            this.issues.AddRange(res.issues);
            return this;
        }

        public void LogIssues()
        {
            Log.Information("--- COMPARER RESULT ---");
            foreach (BaseIssue issue in this.issues)
                issue.LogIssue();
            Log.Information("-----------------------");
        }

        public override bool Equals(object? obj) => this.Equals(obj as ComparerResult);

        public override int GetHashCode() => base.GetHashCode();

        public bool Equals([AllowNull] ComparerResult other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.issues.SequenceEqual(other.issues);
        }
    }
}
