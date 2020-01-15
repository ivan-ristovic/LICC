using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
    public abstract class ASTNode
    {
        [JsonProperty(Order = 0)]
        public string NodeType => this.GetType().Name;

        [JsonIgnore]
        public ASTNode? Parent { get; set; }

        [JsonProperty(Order = 1)]
        public int Line { get; }

        [JsonProperty(Order = 2)]
        public IReadOnlyList<ASTNode> Children { get; }


        protected ASTNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : this(line, parent, children.ToArray())
        {

        }

        protected ASTNode(int line, ASTNode? parent = null, params ASTNode[] children)
        {
            this.Children = children ?? Array.Empty<ASTNode>();
            this.Line = line;
            this.Parent = parent;
        }


        public virtual string GetText() 
            => string.Join(" ", this.Children.Select(c => c.GetText()));

        public T As<T>() where T : ASTNode 
            => this as T ?? throw new NodeMismatchException($"Expected: {typeof(T)}, got: {this.GetType()}");
    }
}
