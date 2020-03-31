using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using LICC.AST.Nodes;
using LICC.AST.Nodes.Common;
using LICC.Exceptions;
using static LICC.AST.Builders.Lua.LuaParser;

namespace LICC.AST.Builders.Lua
{
    public sealed partial class LuaASTBuilder : LuaBaseVisitor<ASTNode>, IASTBuilder<LuaParser>
    {
        public override ASTNode VisitStat([NotNull] StatContext ctx)
        {
            if (ctx.varlist() is { }) {
                ExpressionListNode vars = this.Visit(ctx.varlist()).As<ExpressionListNode>();
                ExpressionListNode inits = this.Visit(ctx.explist()).As<ExpressionListNode>();
                return CreateAssignmentNode(ctx.Start.Line, vars, inits);
            }

            if (ctx.functioncall() is { })
                return this.Visit(ctx.functioncall());

            if (ctx.label() is { })
                return new LabeledStatementNode(ctx.Start.Line, ctx.label().NAME().GetText(), new EmptyStatementNode(ctx.Start.Line));

            switch (ctx.children.First().GetText()) {
                case ";":
                    return new EmptyStatementNode(ctx.Start.Line);
                case "break":
                    return new JumpStatementNode(ctx.Start.Line, JumpStatementType.Break);
                case "goto":
                    var label = new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
                    return new JumpStatementNode(ctx.Start.Line, label);
                case "do":
                    return this.Visit(ctx.block().Single());
                case "while":
                    ExpressionNode cond = this.Visit(ctx.exp().Single()).As<ExpressionNode>();
                    BlockStatementNode body = this.Visit(ctx.block().Single()).As<BlockStatementNode>();
                    return new WhileStatementNode(ctx.Start.Line, cond, body);
                case "repeat":
                    ExpressionNode until = this.Visit(ctx.exp().Single()).As<ExpressionNode>();
                    var negUntil = new UnaryExpressionNode(until.Line, UnaryOperatorNode.FromSymbol(until.Line, "not"), until);
                    BlockStatementNode repeatBody = this.Visit(ctx.block().Single()).As<BlockStatementNode>();
                    return new BlockStatementNode(ctx.Start.Line, repeatBody, new WhileStatementNode(ctx.Start.Line, negUntil, repeatBody));
                case "if":
                    ExpressionNode[] conds = ctx.exp().Select(e => this.Visit(e).As<ExpressionNode>()).ToArray();
                    BlockStatementNode[] blocks = ctx.block().Select(b => this.Visit(b).As<BlockStatementNode>()).ToArray();

                    StatementNode? @else = blocks.Length > 1 ? CreateElseIfNode(1) : null;

                    return @else is null
                        ? new IfStatementNode(ctx.Start.Line, conds.First(), blocks.First())
                        : new IfStatementNode(ctx.Start.Line, conds.First(), blocks.First(), @else);


                    StatementNode? CreateElseIfNode(int i)
                    {
                        if (i >= conds.Length)
                            return i >= blocks.Length ? null : blocks.Last();
                        StatementNode? @else = CreateElseIfNode(i + 1);
                        return @else is null
                            ? new IfStatementNode(conds[i].Line, conds[i], blocks[i])
                            : new IfStatementNode(conds[i].Line, conds[i], blocks[i], @else);
                    }
                case "for":
                    return new EmptyStatementNode(ctx.Start.Line); // TODO
                case "function":
                    IdentifierNode fname = this.Visit(ctx.funcname()).As<IdentifierNode>();
                    LambdaFunctionNode fdef = this.Visit(ctx.funcbody()).As<LambdaFunctionNode>();
                    FunctionDeclaratorNode fdecl = fdef.ParametersNode is null
                        ? new FunctionDeclaratorNode(ctx.Start.Line, fname)
                        : new FunctionDeclaratorNode(ctx.Start.Line, fname, fdef.ParametersNode);
                    var fdeclSpecs = new DeclarationSpecifiersNode(ctx.Start.Line);
                    return new FunctionDefinitionNode(ctx.Start.Line, fdeclSpecs, fdecl, fdef.Definition);
                case "local":
                    if (ctx.children[1].GetText().Equals("function", StringComparison.InvariantCultureIgnoreCase))
                        return new EmptyStatementNode(ctx.Start.Line);  // TODO
                    IdentifierListNode vars = this.Visit(ctx.namelist()).As<IdentifierListNode>();
                    if (ctx.explist() is { }) {
                        // TODO 'local' info is lost here
                        ExpressionListNode inits = this.Visit(ctx.explist()).As<ExpressionListNode>();
                        return CreateAssignmentNode(ctx.Start.Line, new ExpressionListNode(vars.Line, vars.Identifiers), inits);
                    } else {
                        IEnumerable<VariableDeclaratorNode> varDecls = vars.Identifiers
                            .Select(v => new VariableDeclaratorNode(ctx.Start.Line, v))
                            ;

                        var declSpecs = new DeclarationSpecifiersNode(ctx.Start.Line, "local", "object");
                        var decls = new DeclaratorListNode(ctx.Start.Line, varDecls);
                        return new DeclarationStatementNode(ctx.Start.Line, declSpecs, decls);
                    }
                default:
                    throw new SyntaxException("Invalid statement type.");
            }


            static ASTNode CreateAssignmentNode(int line, ExpressionListNode vars, ExpressionListNode initializers)
            {
                if (initializers.Children.Count < vars.Children.Count) {
                    int missingCount = vars.Children.Count - initializers.Children.Count;
                    IEnumerable<ExpressionNode> missing = Enumerable.Repeat<ExpressionNode>(new NullLiteralNode(line), missingCount);
                    initializers = new ExpressionListNode(line, initializers.Expressions.Concat(missing));
                }

                IEnumerable<AssignmentExpressionNode> assignments = vars.Expressions
                    .Zip(initializers.Expressions)
                    .Select(tup => new AssignmentExpressionNode(line, tup.First, tup.Second))
                    ;

                if (vars.Children.Count == 1)
                    return new ExpressionStatementNode(line, assignments.First());
                return new BlockStatementNode(line, assignments);
            }
        }

        public override ASTNode VisitRetstat([NotNull] RetstatContext ctx)
        {
            ExpressionNode? expr = null;
            if (ctx.explist() is { })
                expr = this.Visit(ctx.explist()).As<ExpressionNode>();
            return new JumpStatementNode(ctx.Start.Line, expr);
        }

        public override ASTNode VisitVarlist([NotNull] VarlistContext ctx)
            => new ExpressionListNode(ctx.Start.Line, ctx.var().Select(v => this.Visit(v).As<ExpressionNode>()));

        public override ASTNode VisitVar([NotNull] VarContext ctx)
        {
            if (ctx.NAME() is { }) {
                var id = new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
                if (ctx.varSuffix() is { } && ctx.varSuffix().Any()) {
                    // NOTE will require update once VisitVarSuffix is enhanced
                    ExpressionNode index = this.Visit(ctx.varSuffix().First()).As<ExpressionNode>();
                    return new ArrayAccessExpressionNode(ctx.Start.Line, id, index);
                }
            }
            return new IdentifierNode(ctx.Start.Line, ctx.NAME().GetText());
        }

        public override ASTNode VisitVarSuffix([NotNull] VarSuffixContext ctx)
        {
            if (ctx.nameAndArgs()?.Any() ?? false)
                throw new NotImplementedException("nameAndArgs*");
            if (ctx.NAME() is { })
                throw new NotImplementedException("Field access");
            return this.Visit(ctx.exp());
        }

        public override ASTNode VisitNamelist([NotNull] NamelistContext ctx)
            => new IdentifierListNode(ctx.Start.Line, ctx.NAME().Select(v => new IdentifierNode(ctx.Start.Line, v.GetText())));

        public override ASTNode VisitFunctioncall([NotNull] FunctioncallContext ctx)
        {
            IdentifierNode fname = this.Visit(ctx.varOrExp()).As<IdentifierNode>();
            if (ctx.nameAndArgs().Length > 1)
                throw new NotImplementedException("Multiple nameAndArgs");
            ExpressionListNode args = this.Visit(ctx.nameAndArgs().Single()).As<ExpressionListNode>();
            return new FunctionCallExpressionNode(ctx.Start.Line, fname, args);
        }

        public override ASTNode VisitFuncname([NotNull] FuncnameContext ctx)
        {
            if (ctx.NAME().Length > 1)
                throw new NotImplementedException("Function name*");
            return new IdentifierNode(ctx.Start.Line, ctx.NAME().Single().GetText());
        }
    }
}
