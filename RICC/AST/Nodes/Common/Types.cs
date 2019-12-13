﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace RICC.AST.Nodes.Common
{
    public static class Types
    {
        private static ImmutableDictionary<string, TypeCode> _types = new Dictionary<string, TypeCode>() {
            { "byte" , TypeCode.Byte },
            { "bool" , TypeCode.Boolean },
            { "boolean" , TypeCode.Boolean },
            { "char" , TypeCode.Char },
            { "short" , TypeCode.Int16 },
            { "signed short" , TypeCode.Int16 },
            { "unsigned short" , TypeCode.UInt16 },
            { "int" , TypeCode.Int32 },
            { "signed int" , TypeCode.Int32 },
            { "unsigned int" , TypeCode.UInt32 },
            { "long" , TypeCode.Int64 },
            { "signed long" , TypeCode.Int64 },
            { "unsigned long" , TypeCode.UInt64 },
            { "float" , TypeCode.Single },
            { "double" , TypeCode.Double },
        }.ToImmutableDictionary();


        public static TypeCode? TypeCodeFor(string name) 
            => _types.GetValueOrDefault(name);

        public static Type? ToType(this TypeCode code)
        {
            return code switch
            {
                TypeCode.Boolean => typeof(bool),
                TypeCode.Byte => typeof(byte),
                TypeCode.Char => typeof(char),
                TypeCode.DateTime => typeof(DateTime),
                TypeCode.DBNull => typeof(DBNull),
                TypeCode.Decimal => typeof(decimal),
                TypeCode.Double => typeof(double),
                TypeCode.Empty => null,
                TypeCode.Int16 => typeof(short),
                TypeCode.Int32 => typeof(int),
                TypeCode.Int64 => typeof(long),
                TypeCode.Object => typeof(object),
                TypeCode.SByte => typeof(sbyte),
                TypeCode.Single => typeof(float),
                TypeCode.String => typeof(string),
                TypeCode.UInt16 => typeof(ushort),
                TypeCode.UInt32 => typeof(uint),
                TypeCode.UInt64 => typeof(ulong),
                _ => null,
            };
        }
    }
}
