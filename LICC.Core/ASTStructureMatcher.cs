using System;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Core
{
    public sealed class ASTStructureMatcher
    {
        private readonly ASTNode srcTree;
        private readonly ASTNode dstTree;
        private readonly Type nodeType;


        public ASTStructureMatcher(ASTNode srcTree, ASTNode dstTree)
        {
            this.srcTree = srcTree;
            this.dstTree = dstTree;
            if (srcTree.GetType() != dstTree.GetType())
                throw new ArgumentException("Cannot compare instances of different ASTNode type");
            this.nodeType = srcTree.GetType();
        }


        public bool AttemptMatch()
        {
            Log.Debug("Structure matching {SourceTree} with {DestinationTree}", this.srcTree, this.dstTree);

            if (this.srcTree == this.dstTree)
                return true;

            // TODO
            return true;
        }
    }
}
