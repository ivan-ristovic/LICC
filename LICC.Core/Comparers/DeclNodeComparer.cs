using System.Collections.Generic;
using System.Linq;
using LICC.AST.Nodes;
using LICC.Core.Issues;
using LICC.Core.Comparers.Common;
using Serilog;

namespace LICC.Core.Comparers
{
    internal sealed class DeclNodeComparer : ASTNodeComparerBase<DeclNode>
    {
        public DeclaredSymbol? Symbol1 { get; set; }
        public DeclaredSymbol? Symbol2 { get; set; }


        public DeclNodeComparer()
        {

        }

        public DeclNodeComparer(DeclaredSymbol sym1, DeclaredSymbol sym2)
        {
            this.Symbol1 = sym1;
            this.Symbol2 = sym2;
        }


        public override MatchIssues Compare(DeclNode n1, DeclNode n2)
        {
            Log.Debug("Comparing declarators: `{SrcDecl}` with: `{DstDecl}`", n1, n2);

            if (n1.Identifier != n2.Identifier)
                this.Issues.AddWarning(new DeclaratorMismatchWarning(n1, n2));

            if (n1 is VarDeclNode vn1 && n2 is VarDeclNode vn2) {
                if (this.Symbol1 is DeclaredVariableSymbol v1 && this.Symbol2 is DeclaredVariableSymbol v2) {
                    string? v1init = v1.SymbolicInitializer?.ToString() ?? "<unknown>";
                    string? v2init = v2.SymbolicInitializer?.ToString() ?? "<unknown>";
                    if (!Equals(v1init, v2init))
                        this.Issues.AddError(new InitializerMismatchError(v1.Identifier, v2.Declarator.Line, v1init, v2init));
                } else {
                    if (!Equals(vn1.Initializer, vn2.Initializer))
                        this.Issues.AddError(new InitializerMismatchError(n1.Identifier, vn2.Line, vn1.Initializer, vn2.Initializer));
                }
            } else if (n1 is ArrDeclNode arrn1 && n2 is ArrDeclNode arrn2) {
                if (this.Symbol1 is DeclaredArraySymbol arr1 && this.Symbol2 is DeclaredArraySymbol arr2) {
                    string? arr1size = arr1.SymbolicSize?.ToString() ?? "<unknown>";
                    string? arr2size = arr2.SymbolicSize?.ToString() ?? "<unknown>";
                    if (!Equals(arr1size, arr2size))
                        this.Issues.AddWarning(new SizeMismatchWarning(arr1.Identifier, arr2.Declarator.Line, arr1size, arr2size));
                    IEnumerable<string?>? v1init = arr1.SymbolicInitializers?.Select(e => e?.ToString() ?? "null");
                    IEnumerable<string?>? v2init = arr2.SymbolicInitializers?.Select(e => e?.ToString() ?? "null");
                    if (v1init is { } && v2init is { } && v1init.Any() && v2init.Any()) {
                        int i = 0;
                        foreach ((string? i1, string? i2) in v1init.Zip(v2init)) {
                            if (!Equals(i1, i2))
                                this.Issues.AddError(new InitializerMismatchError(arr1.Identifier, arr2.Declarator.Line, i1, i2, i));
                            i++;
                        }
                    } else if (v1init is { } || v2init is { }) {
                        string? v1initStr = v1init is null ? null : (v1init.Any() ? $"[{string.Join(',', v1init)}]" : "[]");
                        string? v2initStr = v2init is null ? null : (v2init.Any() ? $"[{string.Join(',', v2init)}]" : "[]");
                        this.Issues.AddError(new InitializerMismatchError(arr1.Identifier, arr2.Declarator.Line, v1initStr, v2initStr));
                    }
                } else {
                    if (!Equals(arrn1.Initializer, arrn2.Initializer))
                        this.Issues.AddError(new InitializerMismatchError(n1.Identifier, arrn1.Line, arrn1.Initializer, arrn2.Initializer));
                }
            } else if (n1 is FuncDeclNode fn1 && n2 is FuncDeclNode fn2) {
                if (this.Symbol1 is DeclaredFunctionSymbol f1 && this.Symbol2 is DeclaredFunctionSymbol f2) {
                    if (f1.Overloads.Count != f2.Overloads.Count)
                        this.Issues.AddWarning(new ParameterMismatchWarning(fn1.Identifier, fn2.Line));
                    foreach ((FuncDeclNode fdecl1, FuncDeclNode fdecl2) in f1.Overloads.Zip(f2.Overloads)) 
                        CheckFunctionParameters(fdecl1, fdecl2);
                } else {
                    CheckFunctionParameters(fn1, fn2);
                }
            } else {
                // Avoid logging same issue twice
                if (n1.Identifier == n2.Identifier)
                    this.Issues.AddWarning(new DeclaratorMismatchWarning(n1, n2));
            }

            return this.Issues;


            void CheckFunctionParameters(FuncDeclNode fdecl1, FuncDeclNode fdecl2)
            {
                if (fdecl1.IsVariadic != fdecl2.IsVariadic || fdecl1.Parameters?.Count() != fdecl2.Parameters?.Count())
                    this.Issues.AddWarning(new ParameterMismatchWarning(fdecl1.Identifier, fdecl2.Line));
                if (fdecl1.Parameters is { } && fdecl2.Parameters is { }) {
                    int i = 1;
                    foreach ((FuncParamNode fp1, FuncParamNode fp2) in fdecl1.Parameters.Zip(fdecl2.Parameters)) {
                        if (fp1 != fp2)
                            this.Issues.AddWarning(new ParameterMismatchWarning(fdecl1.Identifier, fdecl2.Line, i, fp1, fp2));
                        i++;
                    }
                }
            }
        }
    }
}
