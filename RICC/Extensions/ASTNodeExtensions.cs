using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RICC.AST.Nodes;

namespace RICC.Extensions
{
    public static class ASTNodeExtensions
    {
        public static string ToJson(this ASTNode ast, bool compact = false) 
            => JsonConvert.SerializeObject(ast, compact ? Formatting.None : Formatting.Indented, new StringEnumConverter());
    }
}
