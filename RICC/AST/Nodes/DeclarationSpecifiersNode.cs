using System;
using System.Collections.Generic;
using RICC.AST.Nodes.Common;
using Serilog;

namespace RICC.AST.Nodes
{
    public class DeclarationSpecifiersNode : ASTNode
    {
        public DeclarationSpecifiersFlags Specifiers { get; }
        public string TypeName { get; }
        public Type? Type { get; }


        public DeclarationSpecifiersNode(int line, string type, IEnumerable<string> specs, ASTNode? parent = null) 
            : base(line, parent)
        {
            this.Specifiers = DeclarationSpecifiers.Parse(specs);
            this.TypeName = type;
            TypeCode? typeCode = Types.TypeCodeFor(type);
            if (typeCode is null)
                Log.Warning("Unknown type: {Type}", type);
            else
                this.Type = Types.ToType(typeCode.Value);
        }
    }
}
