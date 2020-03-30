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
        private Dictionary<string, DeclaredSymbol> srcSymbols = new Dictionary<string, DeclaredSymbol>();
        private Dictionary<string, DeclaredSymbol> dstSymbols = new Dictionary<string, DeclaredSymbol>();


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
            this.srcSymbols = this.GetDeclaredSymbols(n1);
            this.dstSymbols = this.GetDeclaredSymbols(n2);
            this.CompareSymbols(this.srcSymbols, this.dstSymbols);

            this.PerformStatements(n1, this.srcSymbols);
            this.PerformStatements(n2, this.dstSymbols);
            this.CompareSymbolValues(n2.Line);
            // TODO remove locals!

            // TODO
            return this.Issues;
        }


        private Dictionary<string, DeclaredSymbol> GetDeclaredSymbols(BlockStatementNode node)
        {
            var symbols = new Dictionary<string, DeclaredSymbol>();

            foreach (DeclarationStatementNode declStat in node.ChildrenOfType<DeclarationStatementNode>()) {
                foreach (DeclaratorNode decl in declStat.DeclaratorList.Declarations) {
                    var symbol = DeclaredSymbol.From(declStat.Specifiers, decl);
                    if (symbols.ContainsKey(decl.Identifier)) {
                        if (symbol is DeclaredFunctionSymbol overload && symbols[decl.Identifier] is DeclaredFunctionSymbol df) {
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

        private void PerformStatements(BlockStatementNode n1, Dictionary<string, DeclaredSymbol> symbols)
        {
            foreach (StatementNode statement in n1.ChildrenOfType<StatementNode>()) {
                switch (statement) {
                    case ExpressionStatementNode expr:
                        if (IsLvalueAssignment(expr, out ExpressionNode? lvalue, out ExpressionNode? rvalue)) {
                            if (lvalue is IdentifierNode var) {
                                if (!symbols.TryGetValue(var.Identifier, out DeclaredSymbol? declSymbol))
                                    throw new SemanticErrorException($"{var.Identifier} symbol is not declared.");
                                if (!(declSymbol is DeclaredVariableSymbol varSymbol))
                                    throw new SemanticErrorException($"Cannot assign to symbol {declSymbol.Identifier}.");

                                // TODO check assignment operator as well!
                                Expr rvalueExpr = new SymbolicExpressionBuilder(rvalue!).Parse();
                                varSymbol.SymbolicInitializer = rvalueExpr.Substitute(varSymbol.Identifier, varSymbol.SymbolicInitializer);
                                ExpressionNode? oldExpr = varSymbol.Initializer;
                                if (oldExpr is { })
                                    varSymbol.Initializer = rvalue?.Substitute(var, oldExpr).As<ExpressionNode>();

                                // TODO
                            } else if (lvalue is ArrayAccessExpressionNode arr) {
                                // TODO
                                throw new NotImplementedException("Array assignment handling.");
                            }
                        }
                        break;
                    case BlockStatementNode block:
                        // TODO recursive call, pass current symbols -- NEED TO REMOVE SYMBOLS AFTER CURRENT BLOCK!
                        // update current issues with returned issues
                        break;
                }
            }


            static bool IsLvalueAssignment(ExpressionStatementNode stat, out ExpressionNode? lvalue, out ExpressionNode? rvalue)
            {
                lvalue = rvalue = null;

                if (stat.Expression is AssignmentExpressionNode ass) {
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
            foreach ((string identifier, DeclaredSymbol srcSymbol) in this.srcSymbols) {
                if (!this.dstSymbols.ContainsKey(identifier))
                    continue;
                DeclaredSymbol dstSymbol = this.dstSymbols[identifier];
                string srcValue = this.GetSymbolValue(srcSymbol, src: true).ToString();
                string dstValue = this.GetSymbolValue(dstSymbol, src: false).ToString();
                if (!Equals(srcValue, dstValue))
                    this.Issues.AddError(new BlockEndValueMismatchError(identifier, blockEndLine, srcValue, dstValue));
            }
        }

        private Expr GetSymbolValue(DeclaredSymbol symbol, bool src = true)
        {
            Dictionary<string, Expr> symbolExprs = this.ExtractSymbolExprs(src ? this.srcSymbols : this.dstSymbols);
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

        private Dictionary<string, Expr> ExtractSymbolExprs(Dictionary<string, DeclaredSymbol> symbols)
        {
            var exprs = new Dictionary<string, Expr>();
            foreach ((string identifier, DeclaredSymbol symbol) in symbols) {
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
            return exprs;
        }
    }
}
