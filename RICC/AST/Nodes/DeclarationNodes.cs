using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RICC.AST.Nodes.Common;
using Serilog;

namespace RICC.AST.Nodes
{
    public class DeclarationSpecifiersNode : ASTNode
    {
        public DeclarationKeywords Keywords { get; }
        public string TypeName { get; }

        [JsonIgnore]
        public Type? Type { get; }


        public DeclarationSpecifiersNode(int line, string specs, string type)
            : base(line)
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
                sb.Append(declSpecs).Append(' ');
            sb.Append(this.TypeName);
            return sb.ToString();
        }
    }

    public abstract class DeclarationNode : ASTNode
    {
        protected DeclarationNode(int line, IEnumerable<ASTNode> children)
            : base(line, children)
        {

        }

        protected DeclarationNode(int line, params ASTNode[] children)
            : base(line, children)
        {

        }
    }

    public class DeclarationStatementNode : SimpleStatementNode
    {
        public DeclarationStatementNode(int line, DeclarationSpecifiersNode declSpecs, DeclarationNode decl)
            : base(line, declSpecs, decl)
        {

        }
    }

    public sealed class DeclarationListNode : DeclarationNode
    {
        [JsonIgnore]
        public IEnumerable<DeclarationNode> Declarations => this.Children.Cast<DeclarationNode>();


        public DeclarationListNode(int line, IEnumerable<DeclarationNode> declarations)
            : base(line, declarations)
        {

        }
     
        public DeclarationListNode(int line, params DeclarationNode[] declarations)
            : base(line, declarations)
        {

        }
    }

    public sealed class VariableDeclarationNode : DeclarationNode
    {
        [JsonIgnore]
        public string Identifier => this.Children.First().As<IdentifierNode>().Identifier;

        [JsonIgnore]
        public ExpressionNode? Initializer => this.Children.ElementAtOrDefault(1)?.As<ExpressionNode>();


        public VariableDeclarationNode(int line, IdentifierNode identifier, ExpressionNode? initializer)
            : base(line, initializer is null ? new ASTNode[] { identifier } : new ASTNode[] { identifier, initializer })
        {

        }


        public override string GetText() => this.Initializer is null ? this.Identifier : $"{this.Identifier} = {this.Initializer.GetText()}";
    }

    public sealed class FunctionDeclaratorNode : DeclarationNode
    {
        [JsonIgnore]
        public string Identifier => this.Children.First().As<IdentifierNode>().Identifier;

        [JsonIgnore]
        public bool IsVariadic => this.ParametersNode?.IsVariadic ?? false;
        
        [JsonIgnore]
        public FunctionParametersNode? ParametersNode => this.Children.ElementAtOrDefault(1) as FunctionParametersNode ?? null;
        
        [JsonIgnore]
        public IEnumerable<FunctionParameterNode>? Parameters => this.ParametersNode?.Parameters;


        public FunctionDeclaratorNode(int line, IdentifierNode identifier, FunctionParametersNode? @params)
            : base(line, @params is null ? new[] { identifier } : new ASTNode[] { identifier, @params })
        {

        }


        public override string GetText()
            => $"{this.Identifier}({this.ParametersNode?.GetText() ?? ""})";
    }
}
