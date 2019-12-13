using System;
using RICC.Exceptions;

namespace RICC.AST.Nodes.Common
{
    public static class BinaryOperations
    {
        public static Func<object, object, object> DetermineFromSign(string symbol)
        {
            return symbol switch
            {
                "+" => AddPrimitive,
                "-" => SubtractPrimitive,
                "*" => MultiplyPrimitive,
                "/" => DividePrimitive,
                "<<" => ShiftLeftPrimitive,
                ">>" => ShiftRightPrimitive,
                _ => throw new UnknownOperatorException(symbol)
            };
        }

        public static object AddPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is string || y is string) {
                return (string)x + (string)y;
            } else if (x is decimal || y is decimal) {
                return (decimal)x + (decimal)y;
            } else if (x is double || y is double) {
                return (double)x + (double)y;
            } else if (x is ulong || y is ulong) {
                return (ulong)x + (ulong)y;
            } else if (x is long || y is long) {
                return (long)x + (long)y;
            } else if (x is uint || y is uint) {
                return (uint)x + (uint)y;
            } else if (x is int || y is int) {
                return (int)x + (int)y;
            } else if (x is char || y is char) {
                return (char)x + (char)y;
            } else if (x is byte || y is byte) {
                return (byte)x + (byte)y;
            } else if (x is sbyte || y is sbyte) {
                return (sbyte)x + (sbyte)y;
            } else
                throw new EvaluationException("Cannot add non-primitive types");
        }

        public static object SubtractPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal) {
                return (decimal)x - (decimal)y;
            } else if (x is double || y is double) {
                return (double)x - (double)y;
            } else if (x is ulong || y is ulong) {
                return (ulong)x - (ulong)y;
            } else if (x is long || y is long) {
                return (long)x - (long)y;
            } else if (x is uint || y is uint) {
                return (uint)x - (uint)y;
            } else if (x is int || y is int) {
                return (int)x - (int)y;
            } else if (x is char || y is char) {
                return (char)x - (char)y;
            } else if (x is byte || y is byte) {
                return (byte)x - (byte)y;
            } else if (x is sbyte || y is sbyte) {
                return (sbyte)x - (sbyte)y;
            } else
                throw new EvaluationException("Cannot subtract non-primitive types");
        }

        public static object MultiplyPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal) {
                return (decimal)x * (decimal)y;
            } else if (x is double || y is double) {
                return (double)x * (double)y;
            } else if (x is ulong || y is ulong) {
                return (ulong)x * (ulong)y;
            } else if (x is long || y is long) {
                return (long)x * (long)y;
            } else if (x is uint || y is uint) {
                return (uint)x * (uint)y;
            } else if (x is int || y is int) {
                return (int)x * (int)y;
            } else if (x is char || y is char) {
                return (char)x * (char)y;
            } else if (x is byte || y is byte) {
                return (byte)x * (byte)y;
            } else if (x is sbyte || y is sbyte) {
                return (sbyte)x * (sbyte)y;
            } else
                throw new EvaluationException("Cannot multiply non-primitive types");
        }

        public static object DividePrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal) {
                return (decimal)x / (decimal)y;
            } else if (x is double || y is double) {
                return (double)x / (double)y;
            } else if (x is ulong || y is ulong) {
                return (ulong)x / (ulong)y;
            } else if (x is long || y is long) {
                return (long)x / (long)y;
            } else if (x is uint || y is uint) {
                return (uint)x / (uint)y;
            } else if (x is int || y is int) {
                return (int)x / (int)y;
            } else if (x is char || y is char) {
                return (char)x / (char)y;
            } else if (x is byte || y is byte) {
                return (byte)x / (byte)y;
            } else if (x is sbyte || y is sbyte) {
                return (sbyte)x / (sbyte)y;
            } else
                throw new EvaluationException("Cannot divide non-primitive types");
        }

        public static object ShiftLeftPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (y is int || y is char || y is sbyte || y is byte) {
                return x switch
                {
                    ulong _ => (ulong)x << (int)y,
                    long _ => (long)x << (int)y,
                    uint _ => (uint)x << (int)y,
                    int _ => (int)x << (int)y,
                    char _ => (char)x << (int)y,
                    sbyte _ => (sbyte)x << (int)y,
                    byte _ => (byte)x << (int)y,
                    _ => throw new EvaluationException("Cannot shift non-integer types"),
                };
            } else {
                throw new EvaluationException("Cannot shift by non-integer amount");
            }
        }

        public static object ShiftRightPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (y is int || y is char || y is sbyte || y is byte) {
                return x switch
                {
                    ulong _ => (ulong)x >> (int)y,
                    long _ => (long)x >> (int)y,
                    uint _ => (uint)x >> (int)y,
                    int _ => (int)x >> (int)y,
                    char _ => (char)x >> (int)y,
                    sbyte _ => (sbyte)x >> (int)y,
                    byte _ => (byte)x >> (int)y,
                    _ => throw new EvaluationException("Cannot shift non-integer types"),
                };
            } else {
                throw new EvaluationException("Cannot shift by non-integer amount");
            }
        }


        private static void ThrowIfNotPrimitiveTypes(object x, object y)
        {
            if (!IsPrimitiveType(x.GetType()) || !IsPrimitiveType(y.GetType()))
                throw new EvaluationException("Non-primitive type supplied to arithmetic operation!");
        }

        private static bool IsPrimitiveType(Type tx)
            => tx.IsPrimitive || tx == typeof(decimal) || tx == typeof(string);
    }
}
