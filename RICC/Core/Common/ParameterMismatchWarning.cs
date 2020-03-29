using System;
using System.Diagnostics.CodeAnalysis;
using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core.Common
{
    public sealed class ParameterMismatchWarning : BaseWarning
    {
        public string FunctionName { get; set; } = "<anon>";
        public int Line { get; set; }
        public int Order { get; set; }
        public FunctionParameterNode? Expected { get; set; }
        public FunctionParameterNode? Actual { get; set; }
        public bool VariadicMismatch { get; set; }


        public ParameterMismatchWarning(string name, int line, bool variadicMismatch = true)
        {
            this.FunctionName = name;
            this.Line = line;
            this.VariadicMismatch = variadicMismatch;
        }

        public ParameterMismatchWarning(string name, int line, int order, FunctionParameterNode expected, FunctionParameterNode actual)
            : this(name, line, variadicMismatch: false)
        {
            if (Equals(expected, actual))
                throw new ArgumentException("Expected different objects");
            this.Expected = expected;
            this.Actual = actual;
            this.Order = order;
        }

        public override string ToString() => $"{base.ToString()}| {this.FunctionName}({this.Order}) | exp: {this.Expected} | got: {this.Actual}";

        public override void LogIssue()
        {
            if (this.VariadicMismatch) {
                Log.Warning("Variadic parameters mismatch for function {FunctionName}, at line {Line}",
                            this.FunctionName, this.Line);
            } else {
                Log.Warning("Parameter {Order} mismatch for function {FunctionName}, at line {Line}: expected {ExpectedParams}, got {ActualParams}",
                            this.Order, this.FunctionName, this.Line, this.Expected, this.Actual);
            }
        }

        public override bool Equals(object? obj)
            => this.Equals(obj as ParameterMismatchWarning);

        public override bool Equals([AllowNull] BaseIssue other)
        {
            if (!base.Equals(other))
                return false;

            var o = other as ParameterMismatchWarning;
            return this.FunctionName.Equals(o?.FunctionName)
                && this.VariadicMismatch.Equals(o?.VariadicMismatch)
                && Equals(this.Expected, o?.Expected) && Equals(this.Actual, o?.Actual)
                ;
        }
    }
}
