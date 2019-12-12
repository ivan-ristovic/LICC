using System;
using System.Collections.Generic;
using System.Text;

namespace RICC.AST.Nodes.Common
{
    public static class BinaryOperations
    {
        public static object AddPrimitive(object x, object y)
        {
            if (!IsPrimitiveType(x.GetType()) || !IsPrimitiveType(y.GetType()))
                throw new Exception(); // TODO

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
            } else if (x is sbyte || y is sbyte) {
                return (sbyte)x + (sbyte)y;
            } else if (x is byte || y is byte) {
                return (byte)x + (byte)y;
            } else
                throw new Exception(); // TODO


            static bool IsPrimitiveType(Type tx) 
                => tx.IsPrimitive || tx == typeof(decimal) || tx == typeof(string);
        }
    }
}
