using System;
using System.Collections.Generic;
using System.Linq;
using LICC.AST.Nodes;
using LICC.AST.Visitors;
using LICC.Core.Common;
using LICC.Core.Comparers.Common;
using LICC.Exceptions;
using Serilog;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LICC.Core.Comparers
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
            Log.Debug("Comparing block: `{SrcBlock}` with block: `{DstBlock}", n1, n2);

            this.localSrcSymbols = this.GetDeclaredSymbols(n1, src: true);
            this.localDstSymbols = this.GetDeclaredSymbols(n2, src: false);
            this.CompareSymbols(this.localSrcSymbols, this.localDstSymbols);

            this.PerformStatements(n1, n2);
            this.CompareSymbolValues(n2.Line);
            this.SubstituteLocalsInGlobals();

            return this.Issues;
        }


        private void SubstituteLocalsInGlobals()
        {
            Dictionary<string, Expr> srcSymbolExprs = ExtractSymbolExprs(src: true);
            Dictionary<string, Expr> dstSymbolExprs = ExtractSymbolExprs(src: false);
            foreach ((string _, DeclaredSymbol symbol) in this.localSrcSymbols)
                Substitute(symbol, srcSymbolExprs, true);
            foreach ((string _, DeclaredSymbol symbol) in this.localDstSymbols)
                Substitute(symbol, dstSymbolExprs, false);


            void Substitute(DeclaredSymbol localSymbol, Dictionary<string, Expr> symbolExprs, bool src)
            {
                if (!(localSymbol is DeclaredVariableSymbol localVarSymbol))
                    return;
                foreach ((string _, DeclaredSymbol symbol) in src ? this.srcSymbols : this.dstSymbols) {
                    Expr replacement = localSymbol.GetInitSymbolValue(symbolExprs);
                    switch (symbol) {
                        case DeclaredVariableSymbol var:
                            var.SymbolicInitializer = var.SymbolicInitializer?.Substitute(localSymbol.Identifier, replacement);
                            var.Initializer = var.Initializer?.Substitute<ExpressionNode>(new IdentifierNode(1, localVarSymbol.Identifier), localVarSymbol.Initializer);
                            break;
                        case DeclaredArraySymbol arr:
                            // TODO
                            break;
                        case DeclaredFunctionSymbol f:
                            // TODO
                            break;
                    }
                }
            }

            Dictionary<string, Expr> ExtractSymbolExprs(bool src)
            {
                var exprs = new Dictionary<string, Expr>();
                foreach ((string identifier, DeclaredSymbol symbol) in src ? this.localSrcSymbols : this.localDstSymbols)
                    ExtractExprsFromSymbol(identifier, symbol);
                return exprs;


                void ExtractExprsFromSymbol(string identifier, DeclaredSymbol symbol)
                {
                    switch (symbol) {
                        case DeclaredVariableSymbol var:
                            if (var.SymbolicInitializer is { })
                                exprs.Add(identifier, var.SymbolicInitializer);
                            else if (var.Initializer is { })
                                exprs.Add(identifier, new SymbolicExpressionBuilder(var.Initializer).Parse());
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

        private void PerformStatements(BlockStatementNode n1, BlockStatementNode n2)
        {
            var n1statements = n1.ChildrenOfType<StatementNode>().ToList();
            var n2statements = n2.ChildrenOfType<StatementNode>().ToList();
            if (!n1statements.Any() && !n2statements.Any())
                return;
            for (int i = 0, j = 0; true; i++, j++) {
                int ni = NextBlockIndex(n1statements, i);
                int nj = NextBlockIndex(n2statements, j);

                foreach (StatementNode statement in n1statements.GetRange(i, ni))
                    PerformStatement(statement, src: true);
                i += ni;

                foreach (StatementNode statement in n2statements.GetRange(j, nj))
                    PerformStatement(statement, src: false);
                j += nj;

                // TODO what if one ast has more blocks than the other?
                if (i >= n1statements.Count || j >= n2statements.Count)
                    break;

                var allSrcSymbols = new Dictionary<string, DeclaredSymbol>(this.localSrcSymbols.Concat(this.srcSymbols));
                var allDstSymbols = new Dictionary<string, DeclaredSymbol>(this.localDstSymbols.Concat(this.dstSymbols));
                // TODO it can be any compound statement, not just a block...
                var comparer = new BlockStatementNodeComparer(allSrcSymbols, allDstSymbols);
                this.Issues.Add(comparer.Compare(n1statements[i], n2statements[j]));
            }


            void PerformStatement(StatementNode statement, bool src)
            {
                switch (statement) {
                    case ExpressionStatementNode exprStat:
                        if (exprStat.Expression is ExpressionListNode expList) {
                            foreach (ExpressionNode expr in expList.Expressions)
                                PerformAssignment(expr, src);
                        } else {
                            PerformAssignment(exprStat.Expression, src);
                        }
                        break;
                        // TODO
                }
            }

            void PerformAssignment(ExpressionNode? expr, bool src)
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
                        varSymbol.Initializer = rvalue.Substitute<ExpressionNode>(var, varSymbol.Initializer);
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

            static int NextBlockIndex(IEnumerable<StatementNode> statements, int start = 0)
            {
                return statements
                    .Skip(start)
                    .TakeWhile(s => !(s is BlockStatementNode))
                    .Count()
                    ;
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
                string srcValue = GetInitSymbolValue(srcSymbol).ToString();
                string dstValue = GetInitSymbolValue(dstSymbol).ToString();
                if (!Equals(srcValue, dstValue))
                    this.Issues.AddError(new BlockEndValueMismatchError(identifier, blockEndLine, srcValue, dstValue));
            }

            static Expr GetInitSymbolValue(DeclaredSymbol symbol)
            {
                switch (symbol) {
                    case DeclaredVariableSymbol var:
                        if (var.SymbolicInitializer is { })
                            return var.SymbolicInitializer;
                        if (var.Initializer is { })
                            return new SymbolicExpressionBuilder(var.Initializer).Parse();
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
        }
    }
}
