using System.Linq;
using LICC.AST.Nodes;

namespace LICC.Core.Comparers
{
    internal sealed class SourceNodeComparer : ASTNodeComparerBase<SourceNode>
    {
        public override MatchIssues Compare(SourceNode n1, SourceNode n2) 
        {
            BlockStatNode b1 = FormBlock(n1);
            BlockStatNode b2 = FormBlock(n2);

            return new BlockStatNodeComparer().Compare(b1, b2);


            static BlockStatNode FormBlock(SourceNode node)
            {
                return node.Children.Count == 1 && node.Children.First() is BlockStatNode block
                    ? block
                    : new BlockStatNode(node.Line, node.Children);
            }
        }
    }
}
