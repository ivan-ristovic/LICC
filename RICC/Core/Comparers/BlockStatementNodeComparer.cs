using System;
using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes;
using RICC.AST.Visitors;
using RICC.Core.Common;
using RICC.Core.Comparers.Common;
using RICC.Exceptions;
using Serilog;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace RICC.Core.Comparers
{
    internal sealed class BlockStatementNodeComparer : ASTNodeComparerBase<BlockStatementNode>
    {
        private readonly Dictionary<string, DeclaredSymbol> srcSymbols = new Dictionary<string, DeclaredSymbol>();
        private readonly Dictionary<string, DeclaredSymbol> dstSymbols = new Dictionary<string, DeclaredSymbol>();
        private Dictionary<string, DeclaredSymbol> localSrcSymbols = new Dictionary<string, DeclaredSymbol>();
        private Dictionary<string, DeclaredSymbol> localDstSymbols = new Dictionary<string, DeclaredSymbol>();


        public BlockStatementNodeComparer()
        {

        }

        public BlockStatementNodeComparer(Dictionary<string, DeclaredSymbol> srcSymbols, Dictionary<string, DeclaredSymbol> dstSymbols)
        {
            this.srcSymbols = srcSymbols;
            this.dstSymbols = dstSymbols;
        }


        public override MatchIssues Compare(BlockStatementNode n1, BlockStatementNode n2)
        {
            this.localSrcSymbols = this.GetDeclaredSymbols(n1, src: true);
            this.localDstSymbols = this.GetDeclaredSymbols(n2, src: false);
            this.CompareSymbols(this.localSrcSymbols, this.localDstSymbols);

            this.PerformStatements(n1, src: true);
            this.PerformStatements(n2, src: false);
            this.CompareSymbolValues(n2.Line);

            return this.Issues;
        }


        private Dictionary<string, DeclaredSymbol> GetDeclaredSymbols(BlockStatementNode node, bool src)
        {
            var symbols = new Dictionary<string, DeclaredSymbol>();

            foreach (DeclarationStatementNode declStat in node.ChildrenOfType<DeclarationStatementNode>()) {
                foreach (DeclaratorNode decl in declStat.DeclaratorList.Declarations) {
                    var symbol = DeclaredSymbol.From(declStat.Specifiers, decl);
                    if (symbols.TryGetValue(decl.Identifier, out DeclaredSymbol? conf) || this.TryFindSymbol(decl.Identifier, src, out conf)) {
                        if (symbol is DeclaredFunctionSymbol overload && conf is DeclaredFunctionSymbol df) {
                            if (!df.AddOverload(overload.FunctionDeclarators.Single()))
                                throw new SemanticErrorException($"Multiple overloads with same parameters found for function: {df.Identifier}", decl.Line);
                        } else {
                            throw new SemanticErrorException($"Same identifier found in multiple declarations: {decl.Identifier}", decl.Line);
                        }
                    }
                    symbols.Add(decl.Identifier, symbol);
                }
            }

            return symbols;
        }

        private bool TryFindSymbol(string key, bool src, out DeclaredSymbol? symbol)
        {
            return src
                ? this.localSrcSymbols.TryGetValue(key, out symbol) || this.srcSymbols.TryGetValue(key, out symbol)
                : this.localDstSymbols.TryGetValue(key, out symbol) || this.dstSymbols.TryGetValue(key, out symbol);
        }

        private void PerformStatements(BlockStatementNode n1, bool src)
        {
            foreach (StatementNode statement in n1.ChildrenOfType<StatementNode>()) {
                switch (statement) {
                    case ExpressionStatementNode exprStat:
                        if (exprStat.Expression is ExpressionListNode expList) {
                            foreach (ExpressionNode expr in expList.Expressions)
                                PerformAssignment(expr);
                        } else {
                            PerformAssignment(exprStat.Expression);
                        }
                        break;
                    case BlockStatementNode block:
                        // TODO recursive call, pass current symbols -- NEED TO REMOVE SYMBOLS AFTER CURRENT BLOCK!
                        // update current issues with returned issues
                        break;
                }
            }


            void PerformAssignment(ExpressionNode? expr)
            {
                if (expr is null)
                    return;
                if (IsLvalueAssignment(expr, out ExpressionNode? lvalue, out ExpressionNode? rvalue)) {
                    if (lvalue is IdentifierNode var && rvalue is { }) {
                        if (!this.TryFindSymbol(var.Identifier, src, out DeclaredSymbol? declSymbol))
                            throw new SemanticErrorException($"{var.Identifier} symbol is not declared.");
                        if (!(declSymbol is DeclaredVariableSymbol varSymbol))
                            throw new SemanticErrorException($"Cannot assign to symbol {var.Identifier}.");

                        Expr rvalueExpr = new SymbolicExpressionBuilder(rvalue).Parse();
                        if (varSymbol.SymbolicInitializer is { })
                            rvalueExpr = rvalueExpr.Substitute(varSymbol.Identifier, varSymbol.SymbolicInitializer);
                        varSymbol.SymbolicInitializer = rvalueExpr;
                        varSymbol.Initializer = rvalue.Substitute(var, varSymbol.Initializer).As<ExpressionNode>();
                    } else if (lvalue is ArrayAccessExpressionNode arr) {
                        // TODO
                        throw new NotImplementedException("Array assignment handling.");
                    }
                }
            }

            static bool IsLvalueAssignment(ExpressionNode expr, out ExpressionNode? lvalue, out ExpressionNode? rvalue)
            {
                lvalue = rvalue = null;

                if (expr is AssignmentExpressionNode ass) {
                    ass = ass.SimplifyComplexAssignment();
                    rvalue = ass.RightOperand;
                    if (ass.LeftOperand is IdentifierNode var) {
                        lvalue = var;
                        return true;
                    }
                    if (ass.LeftOperand is ArrayAccessExpressionNode arr) {
                        lvalue = arr;
                        return true;
                    }
                }

                return false;
            }
        }

        private void CompareSymbolValues(int blockEndLine)
        {
            foreach ((string identifier, DeclaredSymbol srcSymbol) in this.localSrcSymbols)
                CompareSymbolWithMatchingDstSymbol(identifier, srcSymbol);
            foreach ((string identifier, DeclaredSymbol srcSymbol) in this.srcSymbols)
                CompareSymbolWithMatchingDstSymbol(identifier, srcSymbol);


            void CompareSymbolWithMatchingDstSymbol(string identifier, DeclaredSymbol srcSymbol)
            {
                if (!this.TryFindSymbol(identifier, false, out DeclaredSymbol? dstSymbol) || dstSymbol is null)
                    return;
                string srcValue = GetInitSymbolValue(srcSymbol, src: true).ToString();
                string dstValue = GetInitSymbolValue(dstSymbol, src: false).ToString();
                if (!Equals(srcValue, dstValue))
                    this.Issues.AddError(new BlockEndValueMismatchError(identifier, blockEndLine, srcValue, dstValue));
            }

            Expr GetInitSymbolValue(DeclaredSymbol symbol, bool src)
            {
                Dictionary<string, Expr> symbolExprs = ExtractSymbolExprs(src);
                switch (symbol) {
                    case DeclaredVariableSymbol var:
                        if (var.SymbolicInitializer is { })
                            return ExpressionEvaluator.TryEvaluate(var.SymbolicInitializer, symbolExprs);
                        if (var.Initializer is { })
                            return ExpressionEvaluator.TryEvaluate(var.Initializer, symbolExprs);
                        else
                            return Expr.Undefined;
                    case DeclaredArraySymbol arr:
                        // TODO
                        break;
                    case DeclaredFunctionSymbol f:
                        // TODO
                        break;
                }

                // TODO remove
                return Expr.Undefined;
            }

            Dictionary<string, Expr> ExtractSymbolExprs(bool src)
            {
                var exprs = new Dictionary<string, Expr>();
                foreach ((string identifier, DeclaredSymbol symbol) in src ? this.localSrcSymbols : this.localDstSymbols)
                    ExtractExprsFromSymbol(identifier, symbol);
                foreach ((string identifier, DeclaredSymbol symbol) in src ? this.srcSymbols : this.dstSymbols)
                    ExtractExprsFromSymbol(identifier, symbol);
                return exprs;


                void ExtractExprsFromSymbol(string identifier, DeclaredSymbol symbol)
                {
                    switch (symbol) {
                        case DeclaredVariableSymbol var:
                            if (var.FirstSymbolicInitializer is { })
                                exprs.Add(identifier, var.FirstSymbolicInitializer);
                            else if (var.FirstInitializer is { })
                                exprs.Add(identifier, new SymbolicExpressionBuilder(var.FirstInitializer).Parse());
                            else
                                exprs.Add(identifier, Expr.Undefined);
                            break;
                        case DeclaredArraySymbol arr:
                            if (arr.SymbolicInitializers is { }) {
                                for (int i = 0; i < arr.SymbolicInitializers.Count; i++)
                                    exprs.Add($"{identifier}[{i}]", arr.SymbolicInitializers[i] ?? Expr.Undefined);
                            } else if (arr.Initializer is { }) {
                                for (int i = 0; i < arr.Initializer.Count; i++)
                                    exprs.Add($"{identifier}[{i}]", new SymbolicExpressionBuilder(arr.Initializer[i]).Parse());
                            } else {
                                exprs.Add(identifier, Expr.Undefined);
                            }
                            break;
                    }
                }
            }
        }
    }
}
