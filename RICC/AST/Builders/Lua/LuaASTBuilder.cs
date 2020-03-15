using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.AST.Nodes;
using RICC.Exceptions;
using RICC.Extensions;
using static RICC.AST.Builders.Lua.LuaParser;

namespace RICC.AST.Builders.Lua
{
    public sealed partial class LuaASTBuilder : LuaBaseVisitor<ASTNode>, IASTBuilder<LuaParser>
    {
        public LuaParser CreateParser(string code)
        {
            ICharStream stream = CharStreams.fromstring(code);
            var lexer = new LuaLexer(stream);
            lexer.AddErrorListener(new ThrowExceptionErrorListener());
            ITokenStream tokens = new CommonTokenStream(lexer);
            var parser = new LuaParser(tokens);
            parser.BuildParseTree = true;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ThrowExceptionErrorListener());
            return parser;
        }

        public ASTNode BuildFromSource(string code)
            => this.Visit(this.CreateParser(code).chunk());

        public ASTNode BuildFromSource(string code, Func<LuaParser, ParserRuleContext> entryProvider)
            => this.Visit(entryProvider(this.CreateParser(code)));

        public override ASTNode Visit(IParseTree tree)
        {
            LogObj.Visit(tree as ParserRuleContext);
            try {
                return base.Visit(tree);
            } catch (NullReferenceException e) {
                throw new SyntaxException("Source file contained unexpected content", e);
            }
        }

        public override ASTNode VisitChunk([NotNull] ChunkContext ctx)
        {
            BlockStatementNode block = this.Visit(ctx.block()).As<BlockStatementNode>();
            return new TranslationUnitNode(block.Children);
        }

        public override ASTNode VisitBlock([NotNull] BlockContext ctx)
        {
            IEnumerable<ASTNode> statements = ctx.stat().Select(c => this.Visit(c));
            if (!statements.Any() && ctx.retstat() is null)
                throw new SyntaxException("Missing statements in block");
            if (ctx.retstat() is { })
                statements = statements.Concat(new[] { this.Visit(ctx.retstat()) });

            return new BlockStatementNode(ctx.Start.Line, this.AddDeclarations(statements));
        }


        private IReadOnlyList<ASTNode> AddDeclarations(IEnumerable<ASTNode> statements)
        {
            var nodes = new List<ASTNode>();
            foreach (ASTNode stat in statements) {
                if (stat is ExpressionStatementNode expr && expr.Expression is AssignmentExpressionNode assignmentExpr) {
                    if (assignmentExpr.LeftOperand is IdentifierNode v && !IsDeclared(v)) {
                        var declList = new DeclaratorListNode(v.Line, new VariableDeclaratorNode(v.Line, v, assignmentExpr.RightOperand));
                        var declSpecs = new DeclarationSpecifiersNode(v.Line);
                        nodes.Add(new DeclarationStatementNode(v.Line, declSpecs, declList));
                    } else if (assignmentExpr.LeftOperand is ArrayAccessExpressionNode arr) {
                        IdentifierNode arrayExpr = arr.Array as IdentifierNode ?? throw new NotSupportedException("Complex array access expressions");
                        if (!IsDeclared(arrayExpr)) {
                            var declList = new DeclaratorListNode(arr.Line, new ArrayDeclaratorNode(arr.Line, arrayExpr));
                            var declSpecs = new DeclarationSpecifiersNode(arr.Line);
                            nodes.Add(new DeclarationStatementNode(arr.Line, declSpecs, declList));
                        }
                        nodes.Add(stat);
                    }
                } else if (stat is BlockStatementNode block && block.Children.All(c => c is AssignmentExpressionNode ae &&
                                                                                       ae.LeftOperand is IdentifierNode
                )) {
                    var declSpecs = new DeclarationSpecifiersNode(block.Line);

                    IEnumerable<AssignmentExpressionNode> declList = block.Children.Cast<AssignmentExpressionNode>();
                    var notDeclared = declList
                        .Where(e => !IsDeclared(e.LeftOperand.As<IdentifierNode>()))
                        .Select(e => new VariableDeclaratorNode(block.Line, e.LeftOperand.As<IdentifierNode>(), e.RightOperand))
                        .ToList()
                        ;
                    var declared = declList
                        .Where(e => IsDeclared(e.LeftOperand.As<IdentifierNode>()))
                        .Select(e => new ExpressionStatementNode(e.Line, e))
                        .ToList()
                        ;

                    if (notDeclared.Any())
                        nodes.Add(new DeclarationStatementNode(block.Line, declSpecs, new DeclaratorListNode(block.Line, notDeclared)));
                    if (declared.Any())
                        nodes.AddRange(declared);
                } else {
                    nodes.Add(stat);
                }
            }

            return nodes.AsReadOnly();


            bool IsDeclared(IdentifierNode node)
            {
                return nodes
                    .Where(n => n is DeclarationStatementNode)
                    .Cast<DeclarationStatementNode>()
                    .Any(decl => decl.DeclaratorList.Declarations.Any(d => d.IdentifierNode.Equals(node)))
                    ;
            }
        }
    }
}
