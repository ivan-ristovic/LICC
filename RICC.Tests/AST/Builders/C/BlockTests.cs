using System.Linq;
using NUnit.Framework;
using RICC.AST.Nodes;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class BlockTests
    {
        [Test]
        public void EmptyBlockTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource("void f() { }");
            BlockStatementNode block = ast.Children.Single().As<FunctionDefinitionNode>().Definition;
            Assert.That(block, Is.Not.Null);
            Assert.That(block.Parent?.Parent, Is.EqualTo(ast));
            Assert.That(block.Children, Is.Empty);
        }

        [Test]
        public void SimpleBlockTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@" 
                void f() {  // line 2
                            // line 3
                    int x;  // line 4, block begins
                }
            ");
            BlockStatementNode block = this.AssertFunctionBlockNotEmpty(ast);
            Assert.That(block.Line, Is.EqualTo(4));
            Assert.That(block.Children.Single(), Is.InstanceOf<DeclarationStatementNode>());
        }

        [Test]
        public void ComplexBlockTest()
        {
            ASTNode ast = CASTProvider.BuildFromSource(@" 
                void f() {  // line 2
                    int x;  // line 3, block begins
                    if (x) {
                        int z; y + 4;
                    } else {
                        float x, y;
                        x + 3;
                    }

                    float w, h;
                }
            ");
            BlockStatementNode block = this.AssertFunctionBlockNotEmpty(ast);
            Assert.That(block.Line, Is.EqualTo(3));
            Assert.That(block.Children, Has.Count.EqualTo(3));
            Assert.That(block.Children.ElementAt(0), Is.InstanceOf<DeclarationStatementNode>());
            Assert.That(block.Children.ElementAt(1), Is.InstanceOf<IfStatementNode>());
            Assert.That(block.Children.ElementAt(2), Is.InstanceOf<DeclarationStatementNode>());
        }


        private BlockStatementNode AssertFunctionBlockNotEmpty(ASTNode root)
        {
            FunctionDefinitionNode f = root.Children.Single().As<FunctionDefinitionNode>();
            BlockStatementNode block = f.As<FunctionDefinitionNode>().Definition;
            Assert.That(block, Is.Not.Null);
            Assert.That(block.Parent, Is.EqualTo(f));
            Assert.That(block.Children, Is.Not.Empty);
            return block;
        }
    }
}
