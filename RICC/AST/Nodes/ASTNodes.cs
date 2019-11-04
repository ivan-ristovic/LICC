using System;
using System.Collections.Generic;
using System.Linq;

namespace RICC.AST.Nodes
{
    /*
    public class DeclarationListNode : ASTNode
    {
        public IReadOnlyList<DeclarationNode> Declarations { get; set; }

        public DeclarationListNode(IEnumerable<DeclarationNode> declarations)
        {
            this.Declarations = declarations.ToList();
        }
    }

    public abstract class DeclarationNode : ASTNode
    {

    }

    public sealed class FunctionDeclarationNode : DeclarationNode
    {
        public string Name { get; set; }
        public IReadOnlyList<(string Identifier, Type Type)> Arguments { get; set; }
        public Type? ReturnType { get; set; }
        public BlockStatementNode? Definition { get; set; }


        public FunctionDeclarationNode(string name, IEnumerable<(string, Type)> args, Type? returnType = null, BlockStatementNode? defn = null)
        {
            this.Name = name;
            this.Arguments = args.ToList().AsReadOnly();
            this.ReturnType = returnType;
            this.Definition = defn;
        }
    }

    public sealed class VariableDeclarationNode : DeclarationNode
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object? Value { get; set; }

        public VariableDeclarationNode(string name, Type type, object? value = null)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
    }

    public sealed class BlockStatementNode : ASTNode
    {
        public IReadOnlyList<PrimaryExpressionNode> Expressions { get; set; }

        public BlockStatementNode(IEnumerable<PrimaryExpressionNode> declarations)
        {
            this.Expressions = declarations.ToList();
        }
    }

    public abstract class PrimaryExpressionNode : ASTNode
    {

    }

    public class LiteralNode : PrimaryExpressionNode
    {

    }

    public class LiteralNode<T> : LiteralNode where T : struct
    {
        public T? Value { get; set; } = default;
    }
    */
}
