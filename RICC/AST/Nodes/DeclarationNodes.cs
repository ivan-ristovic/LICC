using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RICC.AST.Nodes.Common;
using Serilog;

namespace RICC.AST.Nodes
{
    public class DeclarationSpecifiersNode : ASTNode
    {
        public DeclarationKeywords Keywords { get; }
        public string TypeName { get; }
        public Type? Type { get; }


        public DeclarationSpecifiersNode(int line, string specs, string type, ASTNode? parent = null)
            : base(line, parent)
        {
            this.Keywords = DeclarationKeywords.Parse(specs);
            this.TypeName = type;
            TypeCode? typeCode = Types.TypeCodeFor(type);
            if (typeCode is null)
                Log.Warning("Unknown type: {Type}", type);
            else
                this.Type = Types.ToType(typeCode.Value);
        }


        public override string GetText()
        {
            var sb = new StringBuilder();
            string declSpecs = this.Keywords.ToString();
            if (!string.IsNullOrWhiteSpace(declSpecs))
                sb.Append(declSpecs);
            sb.Append(this.TypeName);
            return sb.ToString();
        }
    }

    public abstract class DeclarationNode : ASTNode
    {
        protected DeclarationNode(int line, IEnumerable<ASTNode> children, ASTNode? parent = null)
            : base(line, children, parent)
        {

        }

        protected DeclarationNode(int line, ASTNode? parent = null, params ASTNode[] children)
            : base(line, parent, children)
        {

        }
    }

    public class DeclarationStatementNode : SimpleStatementNode
    {
        public DeclarationStatementNode(int line, DeclarationSpecifiersNode declSpecs, DeclarationNode decl, ASTNode? parent = null)
            : base(line, parent, declSpecs, decl)
        {

        }
    }

    public sealed class DeclarationListNode : DeclarationNode
    {
        public IEnumerable<DeclarationNode> Declarations => this.Children.Cast<DeclarationNode>();


        public DeclarationListNode(int line, params DeclarationNode[] declarations)
            : base(line, declarations)
        {

        }

        public DeclarationListNode(int line, IEnumerable<DeclarationNode> declarations, ASTNode? parent = null)
            : base(line, declarations, parent)
        {

        }
    }

    public sealed class VariableDeclarationNode : DeclarationNode
    {
        public string Identifier => this.Children.First().As<IdentifierNode>().Identifier;
        public ExpressionNode? Initializer => this.Children.ElementAtOrDefault(1)?.As<ExpressionNode>();


        public VariableDeclarationNode(int line, IdentifierNode identifier, ExpressionNode? initializer, ASTNode? parent = null)
            : base(line, initializer is null ? new ASTNode[] { identifier } : new ASTNode[] { identifier, initializer }, parent)
        {

        }


        public override string GetText() => this.Initializer is null ? this.Identifier : $"{this.Identifier} = {this.Initializer.GetText()}";
    }

    public sealed class FunctionDeclarationNode : DeclarationNode
    {
        public DeclarationKeywords Keywords => this.Children[0].As<DeclarationSpecifiersNode>().Keywords;
        public string ReturnTypeName => this.Children[0].As<DeclarationSpecifiersNode>().TypeName;
        public Type? ReturnType => this.Children[0].As<DeclarationSpecifiersNode>().Type;
        public string Identifier => this.Children[1].As<IdentifierNode>().Identifier;
        public FunctionParametersNode? Parameters => this.Children[2] as FunctionParametersNode ?? null;

        public FunctionDeclarationNode(int line, DeclarationSpecifiersNode declSpecs, IdentifierNode identifier, FunctionParametersNode? @params)
            : base(line, @params is null ? new ASTNode[] { declSpecs, identifier } : new ASTNode[] { declSpecs, identifier, @params })
        {

        }


        public override string GetText()
            => $"{this.Keywords} {this.ReturnTypeName} {this.Identifier}({this.Parameters?.GetText() ?? ""})";
    }

}
