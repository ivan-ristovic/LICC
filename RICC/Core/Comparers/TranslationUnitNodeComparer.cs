using System.Linq;
using RICC.AST.Nodes;

namespace RICC.Core.Comparers
{
    internal sealed class TranslationUnitNodeComparer : ASTNodeComparerBase<TranslationUnitNode>
    {
        public override MatchIssues Compare(TranslationUnitNode n1, TranslationUnitNode n2) 
        {
            BlockStatementNode b1 = FormBlock(n1);
            BlockStatementNode b2 = FormBlock(n2);

            return new BlockStatementNodeComparer().Compare(b1, b2);


            static BlockStatementNode FormBlock(TranslationUnitNode node)
            {
                return node.Children.Count == 1 && node.Children.First() is BlockStatementNode block
                    ? block
                    : new BlockStatementNode(node.Line, node.Children);
            }
        }
    }
}
