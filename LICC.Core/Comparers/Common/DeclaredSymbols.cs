using System;
using System.Collections.Generic;
using System.Linq;
using LICC.AST.Nodes;
using LICC.AST.Visitors;
using Serilog;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace LICC.Core.Comparers.Common
{
    internal abstract class DeclaredSymbol
    {
        public static DeclaredSymbol From(DeclSpecsNode specs, DeclNode decl)
        {
            return decl switch
            {
                VarDeclNode var => new DeclaredVariableSymbol(decl.Identifier, specs, var, var.Initializer),
                ArrDeclNode arr => new DeclaredArraySymbol(decl.Identifier, specs, arr, arr.SizeExpression, arr.Initializer),
                FuncDeclNode f => new DeclaredFunctionSymbol(decl.Identifier, specs, f),
                _ => throw new NotImplementedException("Declarator node type not yet implemented"),
            };
        }


        public string Identifier { get; set; }
        public DeclSpecsNode Specifiers { get; set; }
        public DeclNode Declarator { get; set; }


        public DeclaredSymbol(string identifier, DeclSpecsNode specs, DeclNode decl)
        {
            this.Identifier = identifier;
            this.Specifiers = specs;
            this.Declarator = decl;
        }


        public abstract Expr GetInitSymbolValue(Dictionary<string, Expr> symbolExprs);
    }

    internal sealed class DeclaredVariableSymbol : DeclaredSymbol
    {
        public VarDeclNode VariableDeclarator { get; set; }
        public Expr? SymbolicInitializer { get; set; }


        public DeclaredVariableSymbol(string name, DeclSpecsNode specs, VarDeclNode decl, ExprNode? init = null)
            : base(name, specs, decl)
        {
            this.VariableDeclarator = decl;
            if (init is { }) {
                try {
                    this.SymbolicInitializer = new SymbolicExpressionBuilder(init).Parse();
                } catch {
                    this.SymbolicInitializer = Expr.Variable($"i_{name}");
                    Log.Debug("Failed to create symbolic expression for: {Expression}", init.GetText());
                }
            }
        }


        public override Expr GetInitSymbolValue(Dictionary<string, Expr> symbolExprs) 
            => this.SymbolicInitializer is { } ? ExpressionEvaluator.TryEvaluate(this.SymbolicInitializer, symbolExprs) : Expr.Undefined;
    }

    internal sealed class DeclaredArraySymbol : DeclaredSymbol
    {
        public ArrDeclNode ArrayDeclarator { get; set; }
        public Expr? SymbolicSize { get; set; }
        public List<Expr?>? SymbolicInitializers { get; set; }


        public DeclaredArraySymbol(string name, DeclSpecsNode specs, ArrDeclNode decl, ExprNode? size = null, ArrInitExprNode? init = null)
            : base(name, specs, decl)
        {
            this.ArrayDeclarator = decl;
            if (size is { }) {
                try {
                    this.SymbolicSize = Expr.Parse(size.GetText());
                } catch {
                    Log.Debug("Failed to create symbolic expression for: {Expression}", size.GetText());
                }
            }
            if (init is { }) {
                try {
                    this.SymbolicInitializers = init.Initializers.Select(e => {
                        try {
                            return Expr.Parse(e.GetText());
                        } catch {
                            Log.Debug("Failed to create symbolic expression for: {Expression}", e.GetText());
                            return null;
                        }
                    }).ToList();
                } catch {
                    Log.Debug("Failed to create symbolic expression for: {Expression}", init.GetText());
                }
            }
        }


        // TODO
        public override Expr GetInitSymbolValue(Dictionary<string, Expr> symbolExprs)
            => throw new NotImplementedException();
    }

    internal sealed class DeclaredFunctionSymbol : DeclaredSymbol
    {
        public HashSet<FuncDeclNode> Overloads { get; set; }
        public FuncDeclNode FunctionDeclarator { get; set; }


        public DeclaredFunctionSymbol(string name, DeclSpecsNode specs, FuncDeclNode decl)
            : base(name, specs, decl)
        {
            this.Declarator = this.FunctionDeclarator = decl;
            this.Overloads = new HashSet<FuncDeclNode> { decl };
        }


        public bool AddOverload(FuncDeclNode decl)
        {
            if (this.Overloads.Contains(decl))
                return false;
            this.Overloads.Add(decl);
            return true;
        }

        public override Expr GetInitSymbolValue(Dictionary<string, Expr> symbolExprs)
            => throw new InvalidOperationException("Cannot get a value of a function symbol");
    }
}
