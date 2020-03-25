using System;
using System.Linq;
using System.Reflection;
using RICC.AST.Nodes;
using RICC.Core.Comparers;
using Serilog;

namespace RICC.Core
{
    public sealed class ASTNodeComparer
    {
        private readonly ASTNode srcTree;
        private readonly ASTNode dstTree;
        private readonly Type nodeType;


        public ASTNodeComparer(ASTNode srcTree, ASTNode dstTree)
        {
            this.srcTree = srcTree;
            this.dstTree = dstTree;
            if (srcTree.GetType() != dstTree.GetType())
                throw new ArgumentException("Cannot compare instances of different ASTNode type");
            this.nodeType = srcTree.GetType();
        }


        public MatchIssues AttemptMatch()
        {
            Log.Debug("Comparing {SourceTree} with {DestinationTree}", this.srcTree, this.dstTree);

            if (this.srcTree == this.dstTree) {
                Log.Information("AST trees for given code snippets are completely equal. No further analysis nececary.");
                return new MatchIssues();
            }

            IAbstractASTNodeComparer comparer = this.DeduceComparer();
            Log.Debug("Deduced comparer of type {ComparerTypeName}", comparer.GetType().Name);

            MatchIssues issues = comparer.Compare(this.srcTree, this.dstTree);
            issues.LogIssues();
            Log.Information("EQUALITY TEST RESULT: {EqualityResult}", issues.NoSeriousIssues);
            return issues;
        }


        private IAbstractASTNodeComparer DeduceComparer()
        {
            string @namespace = $"{this.GetType().Namespace}.Comparers";
            string comparerType = $"{this.nodeType.Name}Comparer";

            Type? comparer = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == @namespace)
                .SingleOrDefault(t => t.Name == comparerType)
                ;
            if (comparer is null)
                throw new Exception("Failed to find comparer for given ASTNode type.");

            object? instance = Activator.CreateInstance(comparer);
            if (instance is null)
                throw new Exception("Failed to find comparer for given ASTNode type.");
            return (IAbstractASTNodeComparer)instance;
        }
    }
}
