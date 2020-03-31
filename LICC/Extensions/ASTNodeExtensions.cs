using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Extensions
{
    public static class ASTNodeExtensions
    {
        public static string? ToJson(this ASTNode ast, bool compact = false)
        {
            string? json = null;

            try {
                json = JsonConvert.SerializeObject(ast, compact ? Formatting.None : Formatting.Indented, new StringEnumConverter());
            } catch (JsonSerializationException e) {
                Log.Fatal(e, "Failed to generate JSON");
            }

            return json;
        }
    }
}
