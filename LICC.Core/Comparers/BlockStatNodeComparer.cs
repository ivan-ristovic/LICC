using System;
using System.Collections.Generic;
using System.Linq;
using LICC.AST.Exceptions;
using LICC.AST.Nodes;
using LICC.AST.Visitors;
using LICC.Core.Issues;
using LICC.Core.Comparers.Common;
using Serilog;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LICC.Core.Comparers
{
    internal sealed class BlockStatNodeComparer : ASTNodeComparerBase<BlockStatNode>
    {
        private readonly Dictionary<string, DeclaredSymbol> srcSymbols = new Dictionary<string, DeclaredSymbol>();
        private readonly Dictionary<string, DeclaredSymbol> dstSymbols = new Dictionary<string, DeclaredSymbol>();
        private Dictionary<string, DeclaredSymbol> localSrcSymbols = new Dictionary<string, DeclaredSymbol>();
        private Dictionary<string, DeclaredSymbol> localDstSymbols = new Dictionary<string, DeclaredSymbol>();


        public BlockStatNodeComparer()
        {

        }

        public BlockStatNodeComparer(Dictionary<string, DeclaredSymbol> srcSymbols, Dictionary<string, DeclaredSymbol> dstSymbols)
        {
            this.srcSymbols = srcSymbols;
            this.dstSymbols = dstSymbols;
        }


        public override MatchIssues Compare(BlockStatNode n1, BlockStatNode n2)
        {
            Log.Debug("Comparing blocks: `{SrcBlock}` with: `{DstBlock}`", n1, n2);

            this.GetDeclaredSymbols(n1, src: true);
            this.GetDeclaredSymbols(n2, src: false);
            this.CompareSymbols(this.localSrcSymbols, this.localDstSymbols);

            this.PerformStatements(n1, n2);

            Dictionary<string, Expr> srcSymbolExprs = this.ExtractSymbolExprs(src: true);
            Dictionary<string, Expr> dstSymbolExprs = this.ExtractSymbolExprs(src: false);
            this.SubstituteTemporaryVariables(srcSymbolExprs, dstSymbolExprs);
            this.SubstituteMissingVariables(srcSymbolExprs);
            this.SubstituteExtraVariables(dstSymbolExprs);
            this.CompareSymbolValues(n2.Line);
            this.SubstituteLocalsInGlobals(srcSymbolExprs, dstSymbolExprs);

            return this.Issues;
        }


        private void SubstituteMissingVariables(Dictionary<string, Expr> symbolExprs)
        {
            foreach ((string identifier, DeclaredSymbol symbol) in this.localSrcSymbols) {
                if (!this.localDstSymbols.ContainsKey(identifier))
                    this.Substitute(symbol, symbolExprs, true);
            }
        }

        private void SubstituteExtraVariables(Dictionary<string, Expr> symbolExprs)
        {
            foreach ((string identifier, DeclaredSymbol symbol) in this.localDstSymbols) {
                if (!this.localSrcSymbols.ContainsKey(identifier))
                    this.Substitute(symbol, symbolExprs, false);
            }
        }

        private void SubstituteTemporaryVariables(Dictionary<string, Expr> srcSymbolExprs, Dictionary<string, Expr> dstSymbolExprs)
        {
            foreach ((string identifier, DeclaredSymbol symbol) in this.localSrcSymbols) {
                if (identifier.StartsWith("tmp__"))
                    this.Substitute(symbol, srcSymbolExprs, true);
            }
            foreach ((string identifier, DeclaredSymbol symbol) in this.localDstSymbols) {
                if (identifier.StartsWith("tmp__"))
                    this.Substitute(symbol, dstSymbolExprs, false);
            }
        }

        private void SubstituteLocalsInGlobals(Dictionary<string, Expr> srcSymbolExprs, Dictionary<string, Expr> dstSymbolExprs)
        {
            foreach ((string _, DeclaredSymbol symbol) in this.localSrcSymbols)
                this.Substitute(symbol, srcSymbolExprs, true);
            foreach ((string _, DeclaredSymbol symbol) in this.localDstSymbols)
                this.Substitute(symbol, dstSymbolExprs, false);
        }

        private void Substitute(DeclaredSymbol localSymbol, Dictionary<string, Expr> symbolExprs, bool src)
        {
            if (!(localSymbol is DeclaredVariableSymbol localVarSymbol))
                return;
            foreach ((string _, DeclaredSymbol symbol) in src ? this.srcSymbols : this.dstSymbols) {
                Expr replacement = localSymbol.GetInitSymbolValue(symbolExprs);
                switch (symbol) {
                    case DeclaredVariableSymbol var:
                        var.SymbolicInitializer = var.SymbolicInitializer?.Substitute(localSymbol.Identifier, replacement);
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

        private Dictionary<string, Expr> ExtractSymbolExprs(bool src)
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
                        if (var.SymbolicInitializer is { })
                            exprs.Add(identifier, var.SymbolicInitializer);
                        else
                            exprs.Add(identifier, Expr.Undefined);
                        break;
                    case DeclaredArraySymbol arr:
                        if (arr.SymbolicInitializers is { }) {
                            for (int i = 0; i < arr.SymbolicInitializers.Count; i++)
                                exprs.Add($"{identifier}[{i}]", arr.SymbolicInitializers[i] ?? Expr.Undefined);
                        } else {
                            exprs.Add(identifier, Expr.Undefined);
                        }
                        break;
                }
            }
        }

        private void GetDeclaredSymbols(BlockStatNode node, bool src)
        {
            Dictionary<string, DeclaredSymbol> symbols = src ? this.localSrcSymbols : this.localDstSymbols;

            foreach (DeclStatNode declStat in node.ChildrenOfType<DeclStatNode>()) {
                foreach (DeclNode decl in declStat.DeclaratorList.Declarators) {
                    var symbol = DeclaredSymbol.From(declStat.Specifiers, decl);
                    if (symbols.TryGetValue(decl.Identifier, out DeclaredSymbol? conf) || this.TryFindSymbol(decl.Identifier, src, out conf)) {
                        if (symbol is DeclaredFunctionSymbol overload && conf is DeclaredFunctionSymbol df) {
                            if (!df.AddOverload(overload.FunctionDeclarator))
                                throw new SemanticErrorException($"Multiple overloads with same parameters found for function: {df.Declarator}", decl.Line);
                        } else {
                            throw new SemanticErrorException($"Same identifier found in multiple declarations: {decl.Identifier}", decl.Line);
                        }
                    }
                    if (symbol is DeclaredVariableSymbol varSymbol && varSymbol.SymbolicInitializer is { }) {
                        Dictionary<string, Expr> exprs = this.ExtractSymbolExprs(src);
                        varSymbol.SymbolicInitializer = ExpressionEvaluator.TryEvaluate(varSymbol.SymbolicInitializer, exprs);
                        symbols.Add(decl.Identifier, varSymbol);
                    } else {
                        symbols.Add(decl.Identifier, symbol);
                    }
                }
            }
        }

        private bool TryFindSymbol(string key, bool src, out DeclaredSymbol? symbol)
        {
            return src
                ? this.localSrcSymbols.TryGetValue(key, out symbol) || this.srcSymbols.TryGetValue(key, out symbol)
                : this.localDstSymbols.TryGetValue(key, out symbol) || this.dstSymbols.TryGetValue(key, out symbol);
        }

        private void PerformStatements(BlockStatNode n1, BlockStatNode n2)
        {
            var n1statements = n1.ChildrenOfType<StatNode>().ToList();
            var n2statements = n2.ChildrenOfType<StatNode>().ToList();
            if (!n1statements.Any() && !n2statements.Any())
                return;
            for (int i = 0, j = 0; true; i++, j++) {
                int ni = NextBlockIndex(n1statements, i);
                int nj = NextBlockIndex(n2statements, j);

                foreach (StatNode statement in n1statements.GetRange(i, ni))
                    PerformStatement(statement, src: true);
                i += ni;

                foreach (StatNode statement in n2statements.GetRange(j, nj))
                    PerformStatement(statement, src: false);
                j += nj;

                // TODO what if one ast has more blocks than the other?
                if (i >= n1statements.Count || j >= n2statements.Count)
                    break;

                var allSrcSymbols = new Dictionary<string, DeclaredSymbol>(this.localSrcSymbols.Concat(this.srcSymbols));
                var allDstSymbols = new Dictionary<string, DeclaredSymbol>(this.localDstSymbols.Concat(this.dstSymbols));
                // TODO it can be any compound statement, not just a block...
                var comparer = new BlockStatNodeComparer(allSrcSymbols, allDstSymbols);
                this.Issues.Add(comparer.Compare(n1statements[i], n2statements[j]));
            }


            void PerformStatement(StatNode statement, bool src)
            {
                switch (statement) {
                    case ExprStatNode exprStat:
                        if (exprStat.Expression is ExprListNode expList) {
                            foreach (ExprNode expr in expList.Expressions)
                                PerformAssignment(expr, src);
                        } else {
                            PerformAssignment(exprStat.Expression, src);
                        }
                        break;
                    case FuncDefNode fdef:
                        if (src)
                            CompareWithMatchingFunctionDefinition(fdef);
                        break;
                }
            }

            void PerformAssignment(ExprNode? expr, bool src)
            {
                if (expr is null)
                    return;
                if (IsLvalueAssignment(expr, out ExprNode? lvalue, out ExprNode? rvalue)) {
                    if (lvalue is IdNode var && rvalue is { }) {
                        if (!this.TryFindSymbol(var.Identifier, src, out DeclaredSymbol? declSymbol))
                            throw new SemanticErrorException($"{var.Identifier} symbol is not declared.");
                        if (!(declSymbol is DeclaredVariableSymbol varSymbol))
                            throw new SemanticErrorException($"Cannot assign to symbol {var.Identifier}.");

                        Expr rvalueExpr = new SymbolicExpressionBuilder(rvalue).Parse();
                        if (varSymbol.SymbolicInitializer is { })
                            rvalueExpr = rvalueExpr.Substitute(varSymbol.Identifier, varSymbol.SymbolicInitializer);
                        Dictionary<string, Expr> exprs = this.ExtractSymbolExprs(src);
                        varSymbol.SymbolicInitializer = ExpressionEvaluator.TryEvaluate(rvalueExpr, exprs);
                    } else if (lvalue is ArrAccessExprNode arr) {
                        // TODO
                        throw new NotImplementedException("Array assignment handling.");
                    }
                }
            }

            void CompareWithMatchingFunctionDefinition(FuncDefNode srcDef)
            {
                FuncDefNode? dstDef = n2
                    .ChildrenOfType<FuncDefNode>()
                    .FirstOrDefault(MatchWithSrcDefViaParamNames)
                    ;
                if (dstDef is null) {
                    this.Issues.AddError(new MissingFunctionDefinitionError(srcDef.Specifiers, srcDef.Declarator));
                    return;
                }

                if (srcDef.Specifiers != dstDef.Specifiers)
                    this.Issues.AddWarning(new DeclSpecsMismatchWarning(dstDef.Declarator, srcDef.Specifiers, dstDef.Specifiers));
                if (srcDef.ParametersNode is { } && dstDef.ParametersNode is { })
                    this.Issues.Add(new FuncParamsNodeComparer().Compare(srcDef.ParametersNode, dstDef.ParametersNode));
                this.Issues.Add(new FuncDefNodeComparer(this.localSrcSymbols, this.localDstSymbols).Compare(srcDef, dstDef));


                bool MatchWithSrcDefViaParamNames(FuncDefNode f)
                {
                    if (f.Identifier != srcDef.Identifier)
                        return false;

                    if (srcDef.Parameters is null)
                        return f.Parameters is null;

                    if (f.Parameters is null)
                        return false;

                    if (f.Parameters.Count() != srcDef.Parameters.Count())
                        return false;

                    foreach ((FuncParamNode p1, FuncParamNode p2) in f.Parameters.Zip(srcDef.Parameters)) {
                        if (p1.Declarator.Identifier != p2.Declarator.Identifier)
                            return false;
                    }

                    return true;
                }
            }

            static bool IsLvalueAssignment(ExprNode expr, out ExprNode? lvalue, out ExprNode? rvalue)
            {
                lvalue = rvalue = null;

                if (expr is AssignExprNode ass) {
                    ass = ass.SimplifyComplexAssignment();
                    rvalue = ass.RightOperand;
                    if (ass.LeftOperand is IdNode var) {
                        lvalue = var;
                        return true;
                    }
                    if (ass.LeftOperand is ArrAccessExprNode arr) {
                        lvalue = arr;
                        return true;
                    }
                }

                return false;
            }

            static int NextBlockIndex(IEnumerable<StatNode> statements, int start = 0)
            {
                return statements
                    .Skip(start)
                    .TakeWhile(s => !(s is BlockStatNode))
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
                if (identifier.StartsWith("tmp__"))
                    return;
                if (!this.TryFindSymbol(identifier, false, out DeclaredSymbol? dstSymbol) || dstSymbol is null)
                    return;
                string srcValue = GetSymbolInitializer(srcSymbol).ToString();
                string dstValue = GetSymbolInitializer(dstSymbol).ToString();
                if (!Equals(srcValue, dstValue))
                    this.Issues.AddError(new BlockEndValueMismatchError(identifier, blockEndLine, srcValue, dstValue));
            }

            static Expr GetSymbolInitializer(DeclaredSymbol symbol)
            {
                switch (symbol) {
                    case DeclaredVariableSymbol var:
                        return var.SymbolicInitializer is { } ? var.SymbolicInitializer : Expr.Undefined;
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
