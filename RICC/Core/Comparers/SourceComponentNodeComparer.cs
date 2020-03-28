using System.Linq;
using RICC.AST.Nodes;

namespace RICC.Core.Comparers
{
    internal sealed class SourceComponentNodeComparer : ASTNodeComparerBase<SourceComponentNode>
    {
        public override MatchIssues Compare(SourceComponentNode n1, SourceComponentNode n2) 
        {
            BlockStatementNode b1 = FormBlock(n1);
            BlockStatementNode b2 = FormBlock(n2);

            return new BlockStatementNodeComparer().Compare(b1, b2);


            static BlockStatementNode FormBlock(SourceComponentNode node)
            {
                return node.Children.Count == 1 && node.Children.First() is BlockStatementNode block
                    ? block
                    : new BlockStatementNode(node.Line, node.Children);
            }
        }
    }
}
