using RICC.AST.Nodes;
using Serilog;

namespace RICC.Core
{
    public sealed class ComparerAlgorithm
    {
        private readonly ASTNode srcTree;
        private readonly ASTNode dstTree;


        public ComparerAlgorithm(ASTNode srcTree, ASTNode dstTree)
        {
            this.srcTree = srcTree;
            this.dstTree = dstTree;
        }


        public void Execute()
        {
            Log.Debug("Comparing {SourceTree} with {DestinationTree}", this.srcTree, this.dstTree);
            // TODO
        }
    }
}
