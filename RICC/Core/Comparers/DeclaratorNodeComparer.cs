using System.Collections.Generic;
using System.Linq;
using RICC.AST.Nodes;
using RICC.Core.Common;
using RICC.Core.Comparers.Common;

namespace RICC.Core.Comparers
{
    internal sealed class DeclaratorNodeComparer : ASTNodeComparerBase<DeclaratorNode>
    {
        public DeclaredSymbol? Symbol1 { get; set; }
        public DeclaredSymbol? Symbol2 { get; set; }


        public DeclaratorNodeComparer()
        {

        }

        public DeclaratorNodeComparer(DeclaredSymbol sym1, DeclaredSymbol sym2)
        {
            this.Symbol1 = sym1;
            this.Symbol2 = sym2;
        }


        public override MatchIssues Compare(DeclaratorNode n1, DeclaratorNode n2)
        {
            if (n1.Identifier != n2.Identifier)
                this.Issues.AddWarning(new DeclaratorMismatchWarning(n1, n2));

            if (n1 is VariableDeclaratorNode vn1 && n2 is VariableDeclaratorNode vn2) {
                if (this.Symbol1 is DeclaredVariableSymbol v1 && this.Symbol2 is DeclaredVariableSymbol v2) {
                    string? v1init = v1.SymbolicInitializer?.ToString() ?? v1.Initializer?.GetText();
                    string? v2init = v2.SymbolicInitializer?.ToString() ?? v2.Initializer?.GetText();
                    if (!v1init?.Equals(v2init) ?? false)
                        this.Issues.AddError(new InitializerMismatchError(v1.Identifier, v2.Declarator.Line, v1init, v2init));
                } else {
                    if (!Equals(vn1.Initializer, vn2.Initializer))
                        this.Issues.AddError(new InitializerMismatchError(n1.Identifier, vn2.Line, vn1.Initializer, vn2.Initializer));
                }
            } else if (n1 is ArrayDeclaratorNode arrn1 && n2 is ArrayDeclaratorNode arrn2) {
                if (this.Symbol1 is DeclaredArraySymbol arr1 && this.Symbol2 is DeclaredArraySymbol arr2) {
                    string? arr1size = arr1.SymbolicSize?.ToString() ?? arr1.SizeExpression?.GetText();
                    string? arr2size = arr2.SymbolicSize?.ToString() ?? arr2.SizeExpression?.GetText();
                    if (!arr1size?.Equals(arr2size) ?? false)
                        this.Issues.AddWarning(new SizeMismatchWarning(arr1.Identifier, arr2.Declarator.Line, arr1size, arr2size));
                    IEnumerable<string?> v1init = arr1.SymbolicInitializers.Select(i => i?.ToString() ?? "null");
                    IEnumerable<string?> v2init = arr2.SymbolicInitializers.Select(i => i?.ToString() ?? "null");
                    int i = 0;
                    foreach ((string? i1, string? i2) in v1init.Zip(v2init)) {
                        if (!i1?.Equals(i2) ?? false)
                            this.Issues.AddError(new InitializerMismatchError(arr1.Identifier, arr2.Declarator.Line, i1, i2, i));
                        i++;
                    }
                } else {
                    if (!Equals(arrn1.Initializer, arrn2.Initializer))
                        this.Issues.AddError(new InitializerMismatchError(n1.Identifier, arrn1.Line, arrn1.Initializer, arrn2.Initializer));
                }
            } else if (n1 is FunctionDeclaratorNode fn1 && n2 is FunctionDeclaratorNode fn2) {
                if (this.Symbol1 is DeclaredFunctionSymbol f1 && this.Symbol2 is DeclaredFunctionSymbol f2) {
                    if (f1.Specifiers != f2.Specifiers)
                        this.Issues.AddWarning(new DeclSpecsMismatchWarning(f1.Declarator, f1.Specifiers, f2.Specifiers));
                    this.Symbol1 = this.Symbol2 = null;
                    foreach ((FunctionDeclaratorNode fdecl1, FunctionDeclaratorNode fdecl2) in f1.FunctionDeclarators.Zip(f2.FunctionDeclarators))
                        this.Compare(fdecl1, fdecl2);
                } else {
                    if ((fn1.IsVariadic && !fn2.IsVariadic) || (!fn1.IsVariadic && fn2.IsVariadic)) {
                        var wrn = new ParameterMismatchWarning(fn1.Identifier, fn2.Line);
                        this.Issues.AddWarning(wrn);
                    }
                    int i = 1;
                    foreach ((FunctionParameterNode fp1, FunctionParameterNode fp2) in fn1.Parameters.Zip(fn2.Parameters)) {
                        if (fp1 != fp2)
                            this.Issues.AddWarning(new ParameterMismatchWarning(fn1.Identifier, fn2.Line, i, fp1, fp2));
                        i++;
                    }
                }
            } else {
                // Avoid logging same issue twice
                if (n1.Identifier == n2.Identifier)
                    this.Issues.AddWarning(new DeclaratorMismatchWarning(n1, n2));
            }

            return this.Issues;
        }
    }
}
