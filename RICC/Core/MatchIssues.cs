﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using RICC.Core.Common;
using Serilog;

namespace RICC.Core
{
    public sealed class MatchIssues : IEquatable<MatchIssues>
    {
        public static bool operator ==(MatchIssues e1, MatchIssues e2) => e1.Equals(e2);
        public static bool operator !=(MatchIssues e1, MatchIssues e2) => !(e1 == e2);


        public bool NoSeriousIssues => !this.issues.Any(i => i is BaseError);

        private readonly List<BaseIssue> issues;


        public MatchIssues()
        {
            this.issues = new List<BaseIssue>();
        }


        public MatchIssues AddWarning(BaseWarning wrn)
        {
            this.issues.Add(wrn);
            return this;
        }

        public MatchIssues AddError(BaseError err)
        {
            this.issues.Add(err);
            return this;
        }

        public MatchIssues Add(MatchIssues res)
        {
            this.issues.AddRange(res.issues);
            return this;
        }

        public void LogIssues()
        {
            Log.Information("--- AST MATCH ISSUES ---");
            foreach (BaseIssue issue in this.issues)
                issue.LogIssue();
            Log.Information("-----------------------");
        }

        public override bool Equals(object? obj) => this.Equals(obj as MatchIssues);

        public override int GetHashCode() => base.GetHashCode();

        public bool Equals([AllowNull] MatchIssues other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return this.issues.SequenceEqual(other.issues);
        }

        public override string ToString() => string.Join(" ;; ", this.issues);
    }
}
