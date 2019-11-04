using System;
using System.Collections.Generic;
using System.Linq;

namespace RICC.AST.Nodes
{
    public sealed class FunctionDefinitionNode : ASTNode
    {
        public string Name { get; }
        public IReadOnlyList<(string Identifier, Type Type)> Arguments { get; }
        public Type? ReturnType { get; }


        public FunctionDefinitionNode(int line, string name, IEnumerable<(string, Type)> args, Type? returnType, BlockStatementNode body) 
            : base(line, new[] { body })
        {
            this.Arguments = args.ToList().AsReadOnly();
            this.Name = name;
            this.ReturnType = returnType;
        }
    }
}
