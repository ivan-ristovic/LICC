﻿using System;
using RICC.Exceptions;

namespace RICC.AST.Nodes.Common
{
    public static class BinaryOperations
    {
        public static Func<object, object, object> ArithmeticFromSymbol(string symbol)
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

        public static Func<object, object, bool> RelationalFromSymbol(string symbol)
        {
            return symbol switch
            {
                ">" => GreaterThanPrimitive,
                "<" => LessThanPrimitive,
                ">=" => GreaterThanOrEqualPrimitive,
                "<=" => LessThanOrEqualPrimitive,
                "==" => EqualsPrimitive,
                "!=" => NotEqualsPrimitive,
                _ => throw new UnknownOperatorException(symbol)
            };
        }

        public static object AddPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is string || y is string)
                return (string)x + (string)y;
            else if (x is decimal || y is decimal)
                return Convert.ToDecimal(x) + Convert.ToDecimal(y);
            else if (x is double || y is double)
                return Convert.ToDouble(x) + Convert.ToDouble(y);
            else if (x is float || y is float)
                return Convert.ToSingle(x) + Convert.ToSingle(y);
            else if (x is ulong || y is ulong)
                return Convert.ToUInt64(x) + Convert.ToUInt64(y);
            else if (x is long || y is long)
                return Convert.ToInt64(x) + Convert.ToInt64(y);
            else if (x is uint || y is uint)
                return Convert.ToUInt32(x) + Convert.ToUInt32(y);
            else if (x is int || y is int)
                return Convert.ToInt32(x) + Convert.ToInt32(y);
            else if (x is ushort || y is ushort)
                return Convert.ToUInt16(x) + Convert.ToUInt16(y);
            else if (x is short || y is short)
                return Convert.ToInt16(x) + Convert.ToInt16(y);
            else if (x is char || y is char)
                return Convert.ToChar(x) + Convert.ToChar(y);
            else if (x is byte || y is byte)
                return Convert.ToByte(x) + Convert.ToByte(y);
            else if (x is sbyte || y is sbyte)
                return Convert.ToSByte(x) + Convert.ToSByte(y);
            else
                throw new EvaluationException("Cannot add non-primitive types");
        }

        public static object SubtractPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal)
                return Convert.ToDecimal(x) - Convert.ToDecimal(y);
            else if (x is double || y is double)
                return Convert.ToDouble(x) - Convert.ToDouble(y);
            else if (x is float || y is float)
                return Convert.ToSingle(x) - Convert.ToSingle(y);
            else if (x is ulong || y is ulong)
                return Convert.ToUInt64(x) - Convert.ToUInt64(y);
            else if (x is long || y is long)
                return Convert.ToInt64(x) - Convert.ToInt64(y);
            else if (x is uint || y is uint)
                return Convert.ToUInt32(x) - Convert.ToUInt32(y);
            else if (x is int || y is int)
                return Convert.ToInt32(x) - Convert.ToInt32(y);
            else if (x is ushort || y is ushort)
                return Convert.ToUInt16(x) - Convert.ToUInt16(y);
            else if (x is short || y is short)
                return Convert.ToInt16(x) - Convert.ToInt16(y);
            else if (x is char || y is char)
                return Convert.ToChar(x) - Convert.ToChar(y);
            else if (x is byte || y is byte)
                return Convert.ToByte(x) - Convert.ToByte(y);
            else if (x is sbyte || y is sbyte)
                return Convert.ToSByte(x) - Convert.ToSByte(y);
            else
                throw new EvaluationException("Cannot subtract non-primitive types");
        }

        public static object MultiplyPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal)
                return Convert.ToDecimal(x) * Convert.ToDecimal(y);
            else if (x is double || y is double)
                return Convert.ToDouble(x) * Convert.ToDouble(y);
            else if (x is float || y is float)
                return Convert.ToSingle(x) * Convert.ToSingle(y);
            else if (x is ulong || y is ulong)
                return Convert.ToUInt64(x) * Convert.ToUInt64(y);
            else if (x is long || y is long)
                return Convert.ToInt64(x) * Convert.ToInt64(y);
            else if (x is uint || y is uint)
                return Convert.ToUInt32(x) * Convert.ToUInt32(y);
            else if (x is int || y is int)
                return Convert.ToInt32(x) * Convert.ToInt32(y);
            else if (x is ushort || y is ushort)
                return Convert.ToUInt16(x) * Convert.ToUInt16(y);
            else if (x is short || y is short)
                return Convert.ToInt16(x) * Convert.ToInt16(y);
            else if (x is char || y is char)
                return Convert.ToChar(x) * Convert.ToChar(y);
            else if (x is byte || y is byte)
                return Convert.ToByte(x) * Convert.ToByte(y);
            else if (x is sbyte || y is sbyte)
                return Convert.ToSByte(x) * Convert.ToSByte(y);
            else
                throw new EvaluationException("Cannot multiply non-primitive types");
        }

        public static object DividePrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal)
                return Convert.ToDecimal(x) / Convert.ToDecimal(y);
            else if (x is double || y is double)
                return Convert.ToDouble(x) / Convert.ToDouble(y);
            else if (x is float || y is float)
                return Convert.ToSingle(x) / Convert.ToSingle(y);
            else if (x is ulong || y is ulong)
                return Convert.ToUInt64(x) / Convert.ToUInt64(y);
            else if (x is long || y is long)
                return Convert.ToInt64(x) / Convert.ToInt64(y);
            else if (x is uint || y is uint)
                return Convert.ToUInt32(x) / Convert.ToUInt32(y);
            else if (x is int || y is int)
                return Convert.ToInt32(x) / Convert.ToInt32(y);
            else if (x is ushort || y is ushort)
                return Convert.ToUInt16(x) / Convert.ToUInt16(y);
            else if (x is short || y is short)
                return Convert.ToInt16(x) / Convert.ToInt16(y);
            else if (x is char || y is char)
                return Convert.ToChar(x) / Convert.ToChar(y);
            else if (x is byte || y is byte)
                return Convert.ToByte(x) / Convert.ToByte(y);
            else if (x is sbyte || y is sbyte)
                return Convert.ToSByte(x) / Convert.ToSByte(y);
            else
                throw new EvaluationException("Cannot divide non-primitive types");
        }

        public static object ShiftLeftPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (y is int || y is char || y is sbyte || y is byte)
                return x switch
                {
                    ulong _ => Convert.ToUInt64(x) << Convert.ToInt32(y),
                    long _ => Convert.ToInt64(x) << Convert.ToInt32(y),
                    uint _ => Convert.ToUInt32(x) << Convert.ToInt32(y),
                    int _ => Convert.ToInt32(x) << Convert.ToInt32(y),
                    ushort _ => Convert.ToUInt16(x) << Convert.ToInt16(y),
                    short _ => Convert.ToInt16(x) << Convert.ToInt16(y),
                    char _ => Convert.ToChar(x) << Convert.ToInt32(y),
                    sbyte _ => Convert.ToSByte(x) << Convert.ToInt32(y),
                    byte _ => Convert.ToByte(x) << Convert.ToInt32(y),
                    _ => throw new EvaluationException("Cannot shift non-integer types"),
                };
            else {
                throw new EvaluationException("Cannot shift by non-integer amount");
            }
        }

        public static object ShiftRightPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (y is int || y is char || y is sbyte || y is byte)
                return x switch
                {
                    ulong _ => Convert.ToUInt64(x) >> Convert.ToInt32(y),
                    long _ => Convert.ToInt64(x) >> Convert.ToInt32(y),
                    uint _ => Convert.ToUInt32(x) >> Convert.ToInt32(y),
                    int _ => Convert.ToInt32(x) >> Convert.ToInt32(y),
                    ushort _ => Convert.ToUInt16(x) >> Convert.ToInt16(y),
                    short _ => Convert.ToInt16(x) >> Convert.ToInt16(y),
                    char _ => Convert.ToChar(x) >> Convert.ToInt32(y),
                    sbyte _ => Convert.ToSByte(x) >> Convert.ToInt32(y),
                    byte _ => Convert.ToByte(x) >> Convert.ToInt32(y),
                    _ => throw new EvaluationException("Cannot shift non-integer types"),
                };
            else {
                throw new EvaluationException("Cannot shift by non-integer amount");
            }
        }

        public static bool LessThanPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal)
                return Convert.ToDecimal(x) < Convert.ToDecimal(y);
            else if (x is double || y is double)
                return Convert.ToDouble(x) < Convert.ToDouble(y);
            else if (x is float || y is float)
                return Convert.ToSingle(x) < Convert.ToSingle(y);
            else if (x is ulong || y is ulong)
                return Convert.ToUInt64(x) < Convert.ToUInt64(y);
            else if (x is long || y is long)
                return Convert.ToInt64(x) < Convert.ToInt64(y);
            else if (x is uint || y is uint)
                return Convert.ToUInt32(x) < Convert.ToUInt32(y);
            else if (x is int || y is int)
                return Convert.ToInt32(x) < Convert.ToInt32(y);
            else if (x is ushort || y is ushort)
                return Convert.ToUInt16(x) < Convert.ToUInt16(y);
            else if (x is short || y is short)
                return Convert.ToInt16(x) < Convert.ToInt16(y);
            else if (x is char || y is char)
                return Convert.ToChar(x) < Convert.ToChar(y);
            else if (x is byte || y is byte)
                return Convert.ToByte(x) < Convert.ToByte(y);
            else if (x is sbyte || y is sbyte)
                return Convert.ToSByte(x) < Convert.ToSByte(y);
            else
                throw new EvaluationException("Cannot compare non-primitive types");
        }

        public static bool LessThanOrEqualPrimitive(object x, object y)

        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal)
                return Convert.ToDecimal(x) <= Convert.ToDecimal(y);
            else if (x is double || y is double)
                return Convert.ToDouble(x) <= Convert.ToDouble(y);
            else if (x is float || y is float)
                return Convert.ToSingle(x) <= Convert.ToSingle(y);
            else if (x is ulong || y is ulong)
                return Convert.ToUInt64(x) <= Convert.ToUInt64(y);
            else if (x is long || y is long)
                return Convert.ToInt64(x) <= Convert.ToInt64(y);
            else if (x is uint || y is uint)
                return Convert.ToUInt32(x) <= Convert.ToUInt32(y);
            else if (x is int || y is int)
                return Convert.ToInt32(x) <= Convert.ToInt32(y);
            else if (x is ushort || y is ushort)
                return Convert.ToUInt16(x) <= Convert.ToUInt16(y);
            else if (x is short || y is short)
                return Convert.ToInt16(x) <= Convert.ToInt16(y);
            else if (x is char || y is char)
                return Convert.ToChar(x) <= Convert.ToChar(y);
            else if (x is byte || y is byte)
                return Convert.ToByte(x) <= Convert.ToByte(y);
            else if (x is sbyte || y is sbyte)
                return Convert.ToSByte(x) <= Convert.ToSByte(y);
            else
                throw new EvaluationException("Cannot compare non-primitive types");
        }
        
        public static bool GreaterThanPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal)
                return Convert.ToDecimal(x) > Convert.ToDecimal(y);
            else if (x is double || y is double)
                return Convert.ToDouble(x) > Convert.ToDouble(y);
            else if (x is float || y is float)
                return Convert.ToSingle(x) > Convert.ToSingle(y);
            else if (x is ulong || y is ulong)
                return Convert.ToUInt64(x) > Convert.ToUInt64(y);
            else if (x is long || y is long)
                return Convert.ToInt64(x) > Convert.ToInt64(y);
            else if (x is uint || y is uint)
                return Convert.ToUInt32(x) > Convert.ToUInt32(y);
            else if (x is int || y is int)
                return Convert.ToInt32(x) > Convert.ToInt32(y);
            else if (x is ushort || y is ushort)
                return Convert.ToUInt16(x) > Convert.ToUInt16(y);
            else if (x is short || y is short)
                return Convert.ToInt16(x) > Convert.ToInt16(y);
            else if (x is char || y is char)
                return Convert.ToChar(x) > Convert.ToChar(y);
            else if (x is byte || y is byte)
                return Convert.ToByte(x) > Convert.ToByte(y);
            else if (x is sbyte || y is sbyte)
                return Convert.ToSByte(x) > Convert.ToSByte(y);
            else
                throw new EvaluationException("Cannot compare non-primitive types");
        }

        public static bool GreaterThanOrEqualPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            if (x is decimal || y is decimal)
                return Convert.ToDecimal(x) >= Convert.ToDecimal(y);
            else if (x is double || y is double)
                return Convert.ToDouble(x) >= Convert.ToDouble(y);
            else if (x is float || y is float)
                return Convert.ToSingle(x) >= Convert.ToSingle(y);
            else if (x is ulong || y is ulong)
                return Convert.ToUInt64(x) >= Convert.ToUInt64(y);
            else if (x is long || y is long)
                return Convert.ToInt64(x) >= Convert.ToInt64(y);
            else if (x is uint || y is uint)
                return Convert.ToUInt32(x) >= Convert.ToUInt32(y);
            else if (x is int || y is int)
                return Convert.ToInt32(x) >= Convert.ToInt32(y);
            else if (x is ushort || y is ushort)
                return Convert.ToUInt16(x) >= Convert.ToUInt16(y);
            else if (x is short || y is short)
                return Convert.ToInt16(x) >= Convert.ToInt16(y);
            else if (x is char || y is char)
                return Convert.ToChar(x) >= Convert.ToChar(y);
            else if (x is byte || y is byte)
                return Convert.ToByte(x) >= Convert.ToByte(y);
            else if (x is sbyte || y is sbyte)
                return Convert.ToSByte(x) >= Convert.ToSByte(y);
            else
                throw new EvaluationException("Cannot compare non-primitive types");
        }

        public static bool EqualsPrimitive(object x, object y)
        {
            ThrowIfNotPrimitiveTypes(x, y);

            return x.Equals(y);
        }

        public static bool NotEqualsPrimitive(object x, object y)
            => !EqualsPrimitive(x, y);


        private static bool BooleanOperatorPrimitive(object x, object y, Func<bool, bool, bool> op)
        {
            return op(ConvertToBool(x), ConvertToBool(y));
            

            static bool ConvertToBool(object v)
            {
                return v switch 
                { 
                    bool v_bool => v_bool,
                    int v_int => v_int != 0,
                    sbyte v_sbyte => v_sbyte != 0,
                    byte v_byte => v_byte != 0,
                    char v_char => v_char != 0,
                    short v_short => v_short != 0,
                    ushort v_ushort => v_ushort != 0,
                    long v_long => v_long != 0,
                    ulong v_ulong => v_ulong != 0,
                    float v_float => v_float != 0,
                    double v_double => v_double != 0,
                    decimal v_decimal => v_decimal != 0,
                    _ => throw new EvaluationException("Some operands cannot be converted to boolean value.")
                };
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
