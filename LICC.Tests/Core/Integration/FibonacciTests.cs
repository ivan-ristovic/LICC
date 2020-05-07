using LICC.AST.Nodes;
using LICC.Core;
using LICC.Core.Common;
using NUnit.Framework;

namespace LICC.Tests.Core.Integration
{
    internal sealed class FibonacciTests : CompleteTestsBase
    {
        [Test]
        public override void DifferenceTests()
        {
            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Fibonacci 
                    begin
                        function fib(n : integer) returning integer
                        begin
                            if n <= 1 then 
                                return 1
                            else
                                return call fib(n-1) + call fib(n-2)
                        end
                    end
                "),
                this.FromCSource(@"
                    int fib(int n) {
                        if (n <= 2)
                            return n;
                        else
                            return fib(n-1) + fib(n-2);
                    }
                ")
                // TODO
            );
        }

        [Test]
        public override void SemanticEquivallenceTests()
        {
            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Fibonacci 
                    begin
                        function fib(n : integer) returning integer
                        begin
                            if n <= 1 then 
                                return n
                            else
                                return call fib(n-1) + call fib(n-2)
                        end
                    end
                "),
                this.FromCSource(@"
                    int fib(int n) {
                        if (n <= 1)
                            return n;
                        else
                            return fib(n-1) + fib(n-2);
                    }
                ")
            );

            this.Compare(
                this.FromPseudoSource(@"
                    algorithm Fibonacci 
                    begin
                        function fib(n : integer) returning integer
                        begin
                            if n < 2 then 
                                return n
                            else
                                return call fib(n-1) + call fib(n-2)
                        end
                    end
                "),
                this.FromLuaSource(@"
                    function fib(n)
                        if n < 2 then
                            return n
                        else
                            return fib(n-1) + fib(n-2)
                        end
                    end
                "),
                new MatchIssues()
                    .AddWarning(
                        new DeclSpecsMismatchWarning(
                            new FuncDeclNode(1, 
                                new IdNode(1, "fib"), 
                                new FuncParamsNode(1, 
                                    new FuncParamNode(1, 
                                        new DeclSpecsNode(1), 
                                        new VarDeclNode(1, new IdNode(1, "n"))
                                    )
                                )
                            ),
                            new DeclSpecsNode(1, "integer"),
                            new DeclSpecsNode(1)
                        )
                    )
                    .AddWarning(
                        new ParameterMismatchWarning("fib", 1, 1,
                            new FuncParamNode(1, new DeclSpecsNode(1, "integer"), new VarDeclNode(1, new IdNode(1, "n"))),
                            new FuncParamNode(1, new DeclSpecsNode(1), new VarDeclNode(1, new IdNode(1, "n")))
                        )
                    )
            );
        }
    }
}
