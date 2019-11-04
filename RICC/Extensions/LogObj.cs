using Antlr4.Runtime;
using Serilog;
using Serilog.Events;

namespace RICC.Extensions
{
    public static class LogObj
    {
        public static void Context(ParserRuleContext ctx, LogEventLevel level = LogEventLevel.Debug)
        {
            Log.Write(
                level,
                "[{Depth}:{ContextType}] [{SourceInterval}] | children: {ChildrenCount} | {Code}", 
                ctx.Depth(),
                ctx.GetType().Name,
                ctx.SourceInterval,
                ctx.ChildCount,
                ctx.GetText()
            );
        }
    }
}
