﻿using RICC.AST.Nodes;
using RICC.Core.Comparers;
using Serilog;

namespace RICC.Core
{
    public sealed class ComparerAlgorithm
    {
        private readonly TranslationUnitNode srcTree;
        private readonly TranslationUnitNode dstTree;


        public ComparerAlgorithm(ASTNode srcTree, ASTNode dstTree)
        {
            this.srcTree = srcTree.As<TranslationUnitNode>();
            this.dstTree = dstTree.As<TranslationUnitNode>();
        }


        public void Execute()
        {
            Log.Debug("Comparing {SourceTree} with {DestinationTree}", this.srcTree, this.dstTree);

            if (this.srcTree == this.dstTree) {
                Log.Information("AST trees for given code snippets are completely equal. No further analysis nececary.");
                return;
            }

            ComparerResult result = new TranslationUnitNodeComparer().Compare(this.srcTree, this.dstTree);
            result.LogIssues();
            Log.Information("EQUALITY TEST RESULT: {EqualityResult}", result.Success);
        }
    }
}
