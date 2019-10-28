using System;
using System.Collections.Generic;
using System.Linq;

namespace AST.AST
{
    public abstract class ASTNode
    {

    }

    public class DeclarationList : ASTNode
    {
        public IReadOnlyList<DeclarationNode> Declarations { get; set; }

        public DeclarationList(IEnumerable<DeclarationNode> declarations)
        {
            this.Declarations = declarations.ToList();
        }
    }

    public abstract class DeclarationNode : ASTNode
    {

    }

    public class FunctionDefinitionNode : DeclarationNode
    {

    }

    public abstract class PrimaryExpressionNode : ASTNode
    {

    }

    public abstract class Literal : PrimaryExpressionNode
    {

    }

    public class Literal<T> : Literal where T : struct
    {
        public T? Value { get; set; } = default;
    }
}
