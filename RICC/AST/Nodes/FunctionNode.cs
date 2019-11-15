﻿using RICC.AST.Nodes.Common;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
    public sealed class FunctionDefinitionNode : ASTNode
    {
        public bool IsStatic { get; }
        public Access Access { get; }

        public DeclarationSpecifiersNode DeclarationSpecifiers => (DeclarationSpecifiersNode)this.Children[0];
        public string Identifier => ((IdentifierNode)this.Children[1]).Identifier;
        public FunctionParametersNode? Parameters => this.Children[2] as FunctionParametersNode ?? null;
        public BlockStatementNode Definition => (BlockStatementNode)this.Children[this.Children.Count - 1];


        public FunctionDefinitionNode(int line, ASTNode declSpecs, ASTNode identifier, ASTNode? @params, ASTNode body)
            : base(line, @params is null ? new[] { declSpecs, identifier, body } : new ASTNode[] { declSpecs, identifier, @params, body })
        {
            if (!(declSpecs is DeclarationSpecifiersNode))
                throw new NodeMismatchException("DeclarationSpecifiersNode expected.", nameof(declSpecs));
            if (!(identifier is IdentifierNode))
                throw new NodeMismatchException("IdentifierNode expected.", nameof(identifier));
            if (!(body is BlockStatementNode))
                throw new NodeMismatchException("BlockStatementNode expected.", nameof(body));
            if (@params is { } && !(@params is FunctionParametersNode))
                throw new NodeMismatchException("FunctionParametersNode expected.", nameof(@params));
        }
    }
}
