using System;
using System.Collections.Generic;
using System.Text;
using LICC.AST.Nodes;

namespace LICC.AST.Builders
{
    public interface IAbstractASTBuilder
    {
        ASTNode BuildFromSource(string code);
    }
}
