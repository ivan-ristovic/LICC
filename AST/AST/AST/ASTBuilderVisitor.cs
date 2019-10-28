using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using AST.Common;
using Parsers.CPP14;

namespace AST.AST
{
    public class ASTBuilderVisitor : CPP14BaseVisitor<ASTNode>
    {
        public List<DeclarationNode> Declarations { get; set; } = new List<DeclarationNode>();


        public override ASTNode VisitDeclarationseq([NotNull] CPP14Parser.DeclarationseqContext context)
        {
            Logger.LogMany("ASTBuilder", $"Reached Declarationseq", $"Content: {context.GetText()}", string.Join(", ", context.children));
            return this.VisitChildren(context); 
        }

        public override ASTNode VisitDeclaration([NotNull] CPP14Parser.DeclarationContext context)
        {
            Logger.LogMany("ASTBuilder", $"Reached Declaration", $"Content: {context.GetText()}", string.Join(", ", context.children));
            return this.VisitChildren(context);
        }

        public override ASTNode VisitFunctiondefinition([NotNull] CPP14Parser.FunctiondefinitionContext context)
        {
            Logger.LogMany("ASTBuilder", $"Reached Functiondefinition", $"Content: {context.GetText()}", string.Join(", ", context.children));
            return new FunctionDefinitionNode();
        }

        public override ASTNode VisitTranslationunit([NotNull] CPP14Parser.TranslationunitContext context)
        {
            Logger.LogMany("ASTBuilder", $"Reached Translationunit", $"Content: {context.GetText()}", string.Join(", ", context.children));
            if (context.declarationseq() is null)
                return null;
            this.VisitDeclarationseq(context.declarationseq());
            return new DeclarationList(this.Declarations);
        }
    }
}
