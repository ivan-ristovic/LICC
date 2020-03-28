using NUnit.Framework;

namespace RICC.Tests.Core.Integration
{
    internal sealed class SwapTests : CompleteTestsBase
    {
        [Test]
        public override void DifferenceTests()
        {
            // TODO
            Assert.Inconclusive();
        }

        [Test]
        public override void NoDifferenceTests()
        {
            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Swap 
                    begin
                        procedure swap(x : integer, y : integer)
                        begin
                            declare integer tmp = x
                            x = y  
                            y = tmp
                        end
                    end
                "),
                this.FromCSource(@"
                    void swap(int x, int y) {
                        int tmp = x;
                        x = y;
                        y = tmp;
                    }
                ")
            );

            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Swap 
                    begin
                        procedure swap(x : integer, y : integer)
                        begin
                            declare integer tmp = x
                            x = y  
                            y = tmp
                        end
                    end
                "),
                this.FromLuaSource(@"
                    function swap(x, y)
                        x, y = y, x
                    end
                ")
            );
        }
    }
}
