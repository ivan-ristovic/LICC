using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace LICC.AST.Nodes
{
    public sealed class DictionaryDeclaratorNode : DeclaratorNode
    {
        [JsonIgnore]
        public DictionaryInitializerNode? Initializer => this.Children.ElementAtOrDefault(1)?.As<DictionaryInitializerNode>();


        public DictionaryDeclaratorNode(int line, IdentifierNode identifier)
            : base(line, identifier) { }

        public DictionaryDeclaratorNode(int line, IdentifierNode identifier, DictionaryInitializerNode initializer)
            : base(line, identifier, initializer) { }


        public override string GetText() => $"{this.Identifier} = {this.Initializer?.GetText() ?? "{}"}";
    }

    public sealed class DictionaryEntryNode : ASTNode
    {
        [JsonIgnore]
        public IdentifierNode Key => this.Children[0].As<IdentifierNode>();

        [JsonIgnore]
        public ExpressionNode Value => this.Children[1].As<ExpressionNode>();


        public DictionaryEntryNode(int line, IdentifierNode key, ExpressionNode value)
            : base(line, key, value) { }


        public override string GetText() => $"'{this.Key}' : {this.Value}";
    }

    public sealed class DictionaryInitializerNode : ExpressionNode
    {
        [JsonIgnore]
        public IEnumerable<DictionaryEntryNode> Entries => this.Children.Cast<DictionaryEntryNode>();


        public DictionaryInitializerNode(int line, IEnumerable<DictionaryEntryNode> entries)
            : base(line, entries) { }

        public DictionaryInitializerNode(int line, params DictionaryEntryNode[] entries)
            : base(line, entries) { }


        public override string GetText()
            => new StringBuilder("{ ").AppendJoin(", ", this.Entries.Select(e => e.GetText())).Append(" }").ToString();
    }
}
