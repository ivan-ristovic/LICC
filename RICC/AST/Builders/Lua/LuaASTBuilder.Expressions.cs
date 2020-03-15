using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using RICC.AST.Nodes;
using RICC.AST.Nodes.Common;
using RICC.Exceptions;
using static RICC.AST.Builders.Lua.LuaParser;

namespace RICC.AST.Builders.Lua
{
    public sealed partial class LuaASTBuilder : LuaBaseVisitor<ASTNode>, IASTBuilder<LuaParser>
    {
        public override ASTNode VisitExplist([NotNull] ExplistContext ctx) 
            => new ExpressionListNode(ctx.Start.Line, ctx.exp().Select(v => this.Visit(v).As<ExpressionNode>()));

        public override ASTNode VisitExp([NotNull] ExpContext ctx)
        {
            if (!ctx.exp()?.Any() ?? true) {
                string firstToken = ctx.children.First().GetText();
                switch (firstToken) {
                    case "nil":
                        return new NullLiteralNode(ctx.Start.Line);
                    case "true":
                    case "false":
                        return LiteralNode.FromString(ctx.Start.Line, firstToken);
                    case "...":
                        throw new NotImplementedException("...");
                }

                if (ctx.number() is { })
                    return LiteralNode.FromString(ctx.Start.Line, ctx.number().GetText());

                if (ctx.@string() is { }) {
                    string str = ctx.@string().GetText()[1..^1];
                    return new LiteralNode(ctx.Start.Line, str);
                }

                if (ctx.prefixexp() is { })
                    return this.Visit(ctx.prefixexp());

                if (ctx.functiondef() is { })
                    return this.Visit(ctx.functiondef());
            }

            if (ctx.operatorComparison() is { }) {
                (ExpressionNode left, string symbol, ExpressionNode right) = ParseBinaryExpression();
                var op = new RelationalOperatorNode(ctx.Start.Line, symbol, BinaryOperations.RelationalFromSymbol(symbol));
                return new RelationalExpressionNode(ctx.Start.Line, left, op, right);
            }

            if (IsArithmeticExpressionContext(ctx)) {
                (ExpressionNode left, string symbol, ExpressionNode right) = ParseBinaryExpression();
                ArithmeticOperatorNode op = ctx.operatorBitwise() is { }
                    ? new ArithmeticOperatorNode(ctx.Start.Line, symbol, BinaryOperations.BitwiseBinaryFromSymbol(symbol))
                    : new ArithmeticOperatorNode(ctx.Start.Line, symbol, BinaryOperations.ArithmeticFromSymbol(symbol));
                return new ArithmeticExpressionNode(ctx.Start.Line, left, op, right);
            }

            if (IsLogicExpressionContext(ctx, out string? logicOp)) {
                if (ctx.operatorUnary() is { }) {
                    ExpressionNode notOperand = this.Visit(ctx.exp().First()).As<ExpressionNode>();
                    var notOp = new UnaryOperatorNode(ctx.Start.Line, "not", UnaryOperations.UnaryFromSymbol("!"));
                    return new UnaryExpressionNode(ctx.Start.Line, notOp, notOperand);
                }

                (ExpressionNode left, string symbol, ExpressionNode right) = ParseBinaryExpression();
                Func<bool, bool, bool> logic = symbol switch
                {
                    "and" => (x, y) => x && y,
                    "or" => (x, y) => x || y,
                    _ => throw new UnknownOperatorException(symbol),
                };
                var op = new BinaryLogicOperatorNode(ctx.Start.Line, symbol, logic);
                return new LogicExpressionNode(ctx.Start.Line, left, op, right);
            }

            if (ctx.operatorUnary() is { }) {
                ExpressionNode unaryOperand = this.Visit(ctx.exp().First()).As<ExpressionNode>();
                string symbol = ctx.children[0].GetText();
                var unaryOp = new UnaryOperatorNode(ctx.Start.Line, symbol, UnaryOperations.UnaryFromSymbol(symbol));
                return new UnaryExpressionNode(ctx.Start.Line, unaryOp, unaryOperand);
            }

            // TODO
            throw new NotImplementedException("exp");


            (ExpressionNode left, string symbol, ExpressionNode right) ParseBinaryExpression()
            {
                ExpressionNode left = this.Visit(ctx.exp()[0]).As<ExpressionNode>();
                string op = ctx.children[1].GetText();
                if (op == "..")
                    op = "+";
                if (op == "~")
                    op = "^";
                ExpressionNode right = this.Visit(ctx.exp()[1]).As<ExpressionNode>();
                return (left, op, right);
            }

            static bool IsArithmeticExpressionContext(ExpContext ctx)
                => ctx.operatorAddSub() is { } || ctx.operatorMulDivMod() is { } || ctx.operatorBitwise() is { }
                || ctx.operatorStrcat() is { } || ctx.operatorPower() is { } ;

            static bool IsLogicExpressionContext(ExpContext ctx, out string? op)
            {
                op = null;

                if (ctx.operatorAnd() is { } || ctx.operatorOr() is { }) {
                    op = ctx.children[1].GetText();
                    return true;
                }

                if (ctx.operatorUnary()?.GetText() == "not") {
                    op = "not";
                    return true;
                }

                return false;
            }
        }

        public override ASTNode VisitPrefixexp([NotNull] PrefixexpContext ctx)
        {
            // TODO nameAndArgs*
            return this.Visit(ctx.varOrExp());
        }

        public override ASTNode VisitVarOrExp([NotNull] VarOrExpContext ctx)
        {
            if (ctx.exp() is { })
                return this.Visit(ctx.exp());
            return this.Visit(ctx.var());
        }

        public override ASTNode VisitNameAndArgs([NotNull] NameAndArgsContext ctx)
        {
            if (ctx.NAME() is { })
                throw new NotImplementedException("NAME");
            return this.Visit(ctx.args());
        }

        public override ASTNode VisitArgs([NotNull] ArgsContext ctx)
        {
            if (ctx.tableconstructor() is { } || ctx.@string() is { })
                throw new NotImplementedException("tableconstructor or string");
            if (ctx.explist() is { })
                return this.Visit(ctx.explist());
            return new ExpressionListNode(ctx.Start.Line, Enumerable.Empty<ExpressionNode>());
        }

        public override ASTNode VisitFunctiondef([NotNull] FunctiondefContext ctx)
            => this.Visit(ctx.funcbody());

        public override ASTNode VisitFuncbody([NotNull] FuncbodyContext ctx)
        {
            FunctionParametersNode? @params = null;
            if (ctx.parlist() is { })
                @params = this.Visit(ctx.parlist()).As<FunctionParametersNode>();
            BlockStatementNode def = this.Visit(ctx.block()).As<BlockStatementNode>();
            return @params is null 
                ? new LambdaFunctionNode(ctx.Start.Line, def) 
                : new LambdaFunctionNode(ctx.Start.Line, @params, def);
        }

        public override ASTNode VisitParlist([NotNull] ParlistContext ctx)
        {
            // TODO variadic?

            if (ctx.namelist() is null)
                return new FunctionParametersNode(ctx.Start.Line, Enumerable.Empty<FunctionParameterNode>());

            IdentifierListNode nameList = this.Visit(ctx.namelist()).As<IdentifierListNode>();
            IEnumerable<FunctionParameterNode> @params = nameList.Identifiers.Select(i => {
                var declSpecs = new DeclarationSpecifiersNode(i.Line);
                var decl = new VariableDeclaratorNode(i.Line, i);
                return new FunctionParameterNode(ctx.Start.Line, declSpecs, decl);
            });
            return new FunctionParametersNode(ctx.Start.Line, @params);
        }
    }
}
