using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using LICC.AST.Nodes;
using LICC.Exceptions;
using LICC.Extensions;
using Serilog;
using static LICC.AST.Builders.Lua.LuaParser;

namespace LICC.AST.Builders.Lua
{
    [ASTBuilder(".lua")]
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
            return new SourceComponentNode(this.AddDeclarations(block.Children));
        }

        public override ASTNode VisitBlock([NotNull] BlockContext ctx)
        {
            IEnumerable<ASTNode> statements = ctx.stat().Select(c => this.Visit(c));
            if (!statements.Any() && ctx.retstat() is null)
                throw new SyntaxException("Missing statements in block");
            if (ctx.retstat() is { })
                statements = statements.Concat(new[] { this.Visit(ctx.retstat()) });

            return new BlockStatementNode(ctx.Start.Line, statements);
        }


        private IReadOnlyList<ASTNode> AddDeclarations(IEnumerable<ASTNode> statements, HashSet<string>? declaredVars = null)
        {
            declaredVars ??= new HashSet<string>();

            var nodes = new List<ASTNode>();
            foreach (ASTNode stat in statements) {
                if (stat is ExpressionStatementNode expr && expr.Expression is AssignmentExpressionNode assignmentExpr) {
                    if (assignmentExpr.LeftOperand is IdentifierNode v) {
                        if (IsDeclared(v)) {
                            nodes.Add(new ExpressionStatementNode(v.Line, assignmentExpr));
                        } else {
                            var declList = new DeclaratorListNode(v.Line, CreateDeclarator(v, assignmentExpr.RightOperand));
                            var declSpecs = new DeclarationSpecifiersNode(v.Line);
                            nodes.Add(new DeclarationStatementNode(v.Line, declSpecs, declList));
                            declaredVars.Add(v.Identifier);
                        }
                    } else if (assignmentExpr.LeftOperand is ArrayAccessExpressionNode arr) {
                        IdentifierNode arrayExpr = arr.Array as IdentifierNode ?? throw new NotSupportedException("Complex array access expressions");
                        if (!IsDeclared(arrayExpr)) {
                            var declList = new DeclaratorListNode(arr.Line, new ArrayDeclaratorNode(arr.Line, arrayExpr));
                            var declSpecs = new DeclarationSpecifiersNode(arr.Line);
                            nodes.Add(new DeclarationStatementNode(arr.Line, declSpecs, declList));
                            declaredVars.Add(arrayExpr.Identifier);
                        }
                        nodes.Add(stat);
                    } else {
                        Log.Debug("Skipping statement: {Stat}", stat.GetText());
                    }
                } else if (stat is BlockStatementNode block) {
                    if (block.Children.All(c => c is AssignmentExpressionNode ae && ae.LeftOperand is IdentifierNode)) {
                        var declSpecs = new DeclarationSpecifiersNode(block.Line);
                        IEnumerable<AssignmentExpressionNode> declList = block.Children.Cast<AssignmentExpressionNode>();

                        // Declare non-declared vars
                        var notDeclared = declList
                            .Where(e => !IsDeclared(e.LeftOperand.As<IdentifierNode>()))
                            .Select(e => CreateDeclarator(e.LeftOperand.As<IdentifierNode>(), e.RightOperand, ignoreInitializer: true))
                            .ToList()
                            ;
                        if (notDeclared.Any()) {
                            nodes.Add(new DeclarationStatementNode(block.Line, declSpecs, new DeclaratorListNode(block.Line, notDeclared)));
                            foreach (string v in notDeclared.Select(d => d.Identifier))
                                declaredVars.Add(v);
                        }

                        // Declare temporary variables and initialize them
                        var tmpDeclList = new DeclaratorListNode(block.Line, declList.Select(d => CreateTmpDeclarator(d)));
                        var tmpDeclSpecs = new DeclarationSpecifiersNode(block.Line);
                        var tmpDecl = new DeclarationStatementNode(block.Line, tmpDeclSpecs, tmpDeclList);
                        nodes.Add(tmpDecl);
                        foreach (string v in tmpDeclList.Declarations.Select(d => d.Identifier))
                            declaredVars.Add(v);

                        // Add tmp assignments
                        var tmpAssignments = new ExpressionListNode(block.Line, declList.Select(decl => CreateTmpAssignment(decl)));
                        nodes.Add(new ExpressionStatementNode(block.Line, tmpAssignments));


                        static IdentifierNode CreateTmpIdentifier(IdentifierNode id)
                            => new IdentifierNode(id.Line, $"tmp__{id.Identifier}");

                        static DeclaratorNode CreateTmpDeclarator(AssignmentExpressionNode expr)
                            => CreateDeclarator(CreateTmpIdentifier(expr.LeftOperand.As<IdentifierNode>()), expr.RightOperand);

                        static ExpressionNode CreateTmpAssignment(AssignmentExpressionNode expr)
                        {
                            IdentifierNode tmpId = CreateTmpIdentifier(expr.LeftOperand.As<IdentifierNode>());
                            return new AssignmentExpressionNode(expr.Line, expr.LeftOperand, tmpId);
                        }
                    } else {
                        nodes.Add(new BlockStatementNode(block.Line, this.AddDeclarations(block.Children, declaredVars)));
                    }
                } else if (stat is FunctionDefinitionNode fdef) {
                    var @params = new List<string>();
                    if (fdef.Parameters is { })
                        @params.AddRange(fdef.Parameters.Select(p => p.Declarator.Identifier));
                    @params = @params.Except(declaredVars).ToList();

                    foreach (string p in @params)
                        declaredVars.Add(p);
                    var alteredDefinition = new BlockStatementNode(fdef.Definition.Line, this.AddDeclarations(fdef.Definition.Children, declaredVars));
                    nodes.Add(new FunctionDefinitionNode(fdef.Line, fdef.Specifiers, fdef.Declarator, alteredDefinition));
                    foreach (string p in @params)
                        declaredVars.Remove(p);
                } else if (stat is IfStatementNode @if) {
                    IfStatementNode alteredIf;
                    var alteredThen = new BlockStatementNode(@if.ThenStatement.Line, this.AddDeclarations(@if.ThenStatement.Children, declaredVars));
                    if (@if.ElseStatement is { }) {
                        var alteredElse = new BlockStatementNode(@if.ElseStatement.Line, this.AddDeclarations(@if.ElseStatement.Children, declaredVars));
                        alteredIf = new IfStatementNode(@if.Line, @if.Condition, alteredThen, alteredElse);
                    } else {
                        alteredIf = new IfStatementNode(@if.Line, @if.Condition, alteredThen);
                    }
                    nodes.Add(alteredIf);
                } else {
                    nodes.Add(stat);
                }
            }

            return nodes.AsReadOnly();


            bool IsDeclared(IdentifierNode node)
                => declaredVars.Contains(node.Identifier);
                //=> nodes.Any(n => n is DeclarationStatementNode decl && decl.DeclaratorList.Declarations.Any(d => d.IdentifierNode.Equals(node)));

            static DeclaratorNode CreateDeclarator(IdentifierNode identifier, ExpressionNode initializer, bool ignoreInitializer = false)
            {
                if (ignoreInitializer) {
                    return initializer switch
                    {
                        ExpressionListNode expList => new ArrayDeclaratorNode(
                            identifier.Line,
                            identifier,
                            LiteralNode.FromString(expList.Line, expList.Expressions.Count().ToString())
                        ),
                        DictionaryInitializerNode dict => new DictionaryDeclaratorNode(identifier.Line, identifier),
                        ExpressionNode varInit => new VariableDeclaratorNode(identifier.Line, identifier),
                        _ => throw new SyntaxException("Unexpected variable initializer"),
                    };
                } else {
                    return initializer switch
                    {
                        ExpressionListNode expList => new ArrayDeclaratorNode(
                            identifier.Line,
                            identifier,
                            new ArrayInitializerListNode(expList.Line, expList.Expressions)
                        ),
                        DictionaryInitializerNode dict => new DictionaryDeclaratorNode(identifier.Line, identifier, dict),
                        ExpressionNode varInit => new VariableDeclaratorNode(identifier.Line, identifier, varInit),
                        _ => throw new SyntaxException("Unexpected variable initializer"),
                    };
                }
            }
        }
    }
}
