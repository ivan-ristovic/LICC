using System;
using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes;
using Serilog;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace RICC.Core.Comparers.Common
{
    internal abstract class DeclaredSymbol
    {
        public static DeclaredSymbol From(DeclarationSpecifiersNode specs, DeclaratorNode decl)
        {
            return decl switch
            {
                VariableDeclaratorNode var => new DeclaredVariableSymbol(decl.Identifier, specs, var, var.Initializer),
                ArrayDeclaratorNode arr => new DeclaredArraySymbol(decl.Identifier, specs, arr, arr.SizeExpression, arr.Initializer),
                FunctionDeclaratorNode f => new DeclaredFunctionSymbol(decl.Identifier, specs, f),
                _ => throw new NotImplementedException("Declarator node type not yet implemented"),
            };
        }


        public string Identifier { get; set; }
        public DeclarationSpecifiersNode Specifiers { get; set; }
        public DeclaratorNode Declarator { get; set; }


        public DeclaredSymbol(string identifier, DeclarationSpecifiersNode specs, DeclaratorNode decl)
        {
            this.Identifier = identifier;
            this.Specifiers = specs;
            this.Declarator = decl;
        }
    }

    internal sealed class DeclaredVariableSymbol : DeclaredSymbol
    {
        public VariableDeclaratorNode VariableDeclarator { get; set; }
        public ExpressionNode? Initializer { get; set; }
        public Expr? SymbolicInitializer { get; set; }


        public DeclaredVariableSymbol(string name, DeclarationSpecifiersNode specs, VariableDeclaratorNode decl, ExpressionNode? init = null)
            : base(name, specs, decl)
        {
            this.VariableDeclarator = decl;
            this.Initializer = init;
            if (init is { }) {
                try {
                    this.SymbolicInitializer = Expr.Parse(init.GetText());
                } catch {
                    Log.Debug("Failed to create symbolic expression for: {Expression}", init.GetText());
                }
            }
        }
    }

    internal sealed class DeclaredArraySymbol : DeclaredSymbol
    {
        public ArrayDeclaratorNode ArrayDeclarator { get; set; }
        public ExpressionNode? SizeExpression { get; set; }
        public List<ExpressionNode>? Initializer { get; set; }
        public Expr? SymbolicSize { get; set; }
        public List<Expr?>? SymbolicInitializers { get; set; }


        public DeclaredArraySymbol(string name, DeclarationSpecifiersNode specs, ArrayDeclaratorNode decl, ExpressionNode? size = null, ArrayInitializerListNode? init = null)
            : base(name, specs, decl)
        {
            this.ArrayDeclarator = decl;
            this.SizeExpression = size;
            this.Initializer = init?.Initializers.ToList();
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
    }

    internal sealed class DeclaredFunctionSymbol : DeclaredSymbol
    {
        public List<FunctionDeclaratorNode> FunctionDeclarators { get; set; }


        public DeclaredFunctionSymbol(string name, DeclarationSpecifiersNode specs, FunctionDeclaratorNode decl)
            : base(name, specs, decl)
        {
            this.FunctionDeclarators = new List<FunctionDeclaratorNode>() { decl };
        }


        public bool AddOverload(FunctionDeclaratorNode decl)
        {
            if (this.FunctionDeclarators.Any(d => d.Equals(decl)))
                return false;
            this.FunctionDeclarators.Add(decl);
            return true;
        }
    }
}
