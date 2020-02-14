using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using RICC.Exceptions;

namespace RICC.AST.Nodes
{
    public abstract class ASTNode : IEquatable<ASTNode>
    {
        public static bool operator ==(ASTNode x, ASTNode y)
            => x.Equals(y);

        public static bool operator !=(ASTNode x, ASTNode y)
            => !(x == y);


        [JsonProperty(Order = 0)]
        public string NodeType => this.GetType().Name;

        [JsonIgnore]
        public ASTNode? Parent { get; set; }

        [JsonProperty(Order = 1)]
        public int Line { get; }

        [JsonProperty(Order = 2)]
        public IReadOnlyList<ASTNode> Children { get; }


        protected ASTNode(int line, params ASTNode[] children)
        {
            this.Children = children ?? Array.Empty<ASTNode>();
            this.Line = line;
            if (children?.Any() ?? false) {
                foreach (ASTNode child in children)
                    child.Parent = this;
            }
        }

        protected ASTNode(int line, IEnumerable<ASTNode> children)
            : this(line, children.ToArray())
        {

        }


        public virtual string GetText()
            => string.Join(' ', this.Children.Select(c => c.GetText()));

        public T As<T>() where T : ASTNode
            => this as T ?? throw new NodeMismatchException(typeof(T), this.GetType());


        public override string ToString() 
            => this.GetText();

        public override int GetHashCode()
            => this.GetText().GetHashCode();

        public override bool Equals(object? obj)
            => this.Equals(obj as ASTNode);

        public virtual bool Equals([AllowNull] ASTNode other)
        {
            if (other is null || this.GetType() != other.GetType())
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (this.Children.Count != other.Children.Count)
                return false;

            return this.Children.Zip(other.Children).All(tup => tup.First.Equals(tup.Second));
        }
    }
}
