using System.Linq;
using NUnit.Framework;
using RICC.AST.Builders.C;
using RICC.AST.Nodes;
using RICC.Tests.AST.Builders.Common;

namespace RICC.Tests.AST.Builders.C
{
    internal sealed class BlockTests : BlockTestsBase<CASTBuilder>
    {
        [Test]
        public void EmptyBlockTest()
        {
            this.AssertFunctionBlock("void f() { }", empty: true);
        }

        [Test]
        public void SimpleBlockTest()
        {
            BlockStatementNode block = this.AssertFunctionBlock(@" 
                void f() {  // line 2
                            // line 3
                    int x;  // line 4, block begins
                }
            ");
            Assert.That(block.Line, Is.EqualTo(4));
            Assert.That(block.Children.Single(), Is.InstanceOf<DeclarationStatementNode>());
        }

        [Test]
        public void ComplexBlockTest()
        {
            BlockStatementNode block = this.AssertFunctionBlock(@" 
                void f() {  // line 2
                    int x;  // line 3, block begins
                    if (x) {
                        int z = y + 4;
                        ;
                    } else {
                        float x, y = x + 3;
                        ;
                        ;
                    }

                    float w, h;
                }
            ");
            Assert.That(block.Line, Is.EqualTo(3));
            Assert.That(block.Children, Has.Exactly(3).Items);
            Assert.That(block.Children.ElementAt(0), Is.InstanceOf<DeclarationStatementNode>());
            Assert.That(block.Children.ElementAt(1), Is.InstanceOf<IfStatementNode>());
            Assert.That(block.Children.ElementAt(2), Is.InstanceOf<DeclarationStatementNode>());
        }
    }
}
