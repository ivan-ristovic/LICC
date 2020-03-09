using System;
using Antlr4.Runtime;
using RICC.AST.Nodes;

namespace RICC.AST.Builders
{
    public interface IASTBuilder<TParser> where TParser : Parser
    {
        TParser CreateParser(string code);
        ASTNode BuildFromSource(string code);
        ASTNode BuildFromSource(string code, Func<TParser, ParserRuleContext> entryProvider) ;
    }
}
