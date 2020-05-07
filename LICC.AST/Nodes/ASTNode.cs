using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LICC.AST.Exceptions;
using Newtonsoft.Json;

namespace LICC.AST.Nodes
{
    public abstract class ASTNode : IEquatable<ASTNode>
    {
        public static bool operator ==(ASTNode x, ASTNode y) => x.Equals(y);
        public static bool operator !=(ASTNode x, ASTNode y) => !(x == y);


        [JsonProperty(Order = 0)]
        public string NodeType => this.GetType().Name;

        [JsonIgnore]
        public ASTNode? Parent { get; set; }

        [JsonProperty(Order = 1)]
        public int Line { get; }

        [JsonProperty(Order = 2)]
        public IReadOnlyList<ASTNode> Children { get; private set; }


        protected ASTNode(int line, params ASTNode[] children)
        {
            this.Children = children ?? Array.Empty<ASTNode>();
            this.Line = line;
            if (children?.Any() ?? false) {
                foreach (ASTNode child in children) {
                    if (child.Line < this.Line)
                        throw new ArgumentException("Parent node has greater line number than the child.");
                    child.Parent = this;
                }
            }
        }

        protected ASTNode(int line, IEnumerable<ASTNode> children)
            : this(line, children.ToArray())
        {

        }


        public T As<T>() where T : ASTNode
            => this as T ?? throw new NodeMismatchException(typeof(T), this.GetType());

        public IEnumerable<T> ChildrenOfType<T>() => this.Children.Where(c => c is T).Cast<T>();

        public ASTNode Copy()
        {
            var copy = (ASTNode)this.MemberwiseClone();
            copy.Children = this.Children
                .Select(c => c.Copy())
                .ToList()
                .AsReadOnly()
                ;
            return copy;
        }

        public ASTNode Substitute(ASTNode? node, ASTNode? replacement)
            => node is null || replacement is null ? this.Copy() : this.Copy().SubstituteNode(node, replacement);

        public T Substitute<T>(ASTNode? node, ASTNode? replacement) where T : ASTNode 
            => this.Substitute(node as ASTNode, replacement as ASTNode).As<T>();


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

        public virtual string GetText()
            => string.Join(' ', this.Children.Select(c => c.GetText()));


        private ASTNode SubstituteNode(ASTNode node, ASTNode replacement)
        {
            if (node.Equals(this))
                return replacement;
            this.Children = this.Children
                .Select(c => c.Substitute(node, replacement))
                .ToList()
                .AsReadOnly()
                ;
            return this;
        }
    }
}
