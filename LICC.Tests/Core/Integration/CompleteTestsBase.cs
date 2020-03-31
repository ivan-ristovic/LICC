using NUnit.Framework;
using LICC.AST.Builders.C;
using LICC.AST.Builders.Lua;
using LICC.AST.Builders.Pseudo;
using LICC.AST.Nodes;
using LICC.Core;

namespace LICC.Tests.Core.Integration
{
    internal abstract class CompleteTestsBase
    {
        [Test]
        public abstract void NoDifferenceTests();

        [Test]
        public abstract void DifferenceTests();


        public virtual ASTNode FromCSource(string src) 
            => new CASTBuilder().BuildFromSource(src);
        
        public virtual ASTNode FromLuaSource(string src) 
            => new LuaASTBuilder().BuildFromSource(src);
        
        public virtual ASTNode FromPseudoSource(string src) 
            => new PseudoASTBuilder().BuildFromSource(src);


        protected void Compare(ASTNode src, ASTNode dst, MatchIssues? expectedIssues = null)
        {
            MatchIssues issues = new ASTNodeComparer(src, dst).AttemptMatch();
            expectedIssues ??= new MatchIssues();
            Assert.That(issues, Is.EqualTo(expectedIssues));
        }
    }
}
