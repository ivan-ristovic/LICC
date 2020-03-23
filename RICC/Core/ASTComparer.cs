using RICC.AST.Nodes;
using RICC.Core.Comparers;
using Serilog;

namespace RICC.Core
{
    public sealed class ASTComparer
    {
        private readonly TranslationUnitNode srcTree;
        private readonly TranslationUnitNode dstTree;


        public ASTComparer(ASTNode srcTree, ASTNode dstTree)
        {
            this.srcTree = srcTree.As<TranslationUnitNode>();
            this.dstTree = dstTree.As<TranslationUnitNode>();
        }


        public MatchIssues AttemptMatch()
        {
            Log.Debug("Comparing {SourceTree} with {DestinationTree}", this.srcTree, this.dstTree);

            if (this.srcTree == this.dstTree) {
                Log.Information("AST trees for given code snippets are completely equal. No further analysis nececary.");
                return new MatchIssues();
            }

            MatchIssues issues = new TranslationUnitNodeComparer().Compare(this.srcTree, this.dstTree);
            issues.LogIssues();
            Log.Information("EQUALITY TEST RESULT: {EqualityResult}", issues.NoSeriousIssues);
            return issues;
        }
    }
}
