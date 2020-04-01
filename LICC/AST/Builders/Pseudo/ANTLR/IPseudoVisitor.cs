#nullable disable
namespace LICC.AST.Builders.Pseudo
{
    //------------------------------------------------------------------------------
    // <auto-generated>
    //     This code was generated by a tool.
    //     ANTLR Version: 4.7.1
    //
    //     Changes to this file may cause incorrect behavior and will be lost if
    //     the code is regenerated.
    // </auto-generated>
    //------------------------------------------------------------------------------

    // Generated from Pseudo.g4 by ANTLR 4.7.1

    // Unreachable code detected
#pragma warning disable 0162
    // The variable '...' is assigned but its value is never used
#pragma warning disable 0219
    // Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
    // Ambiguous reference in cref attribute
#pragma warning disable 419

    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using IToken = Antlr4.Runtime.IToken;

    /// <summary>
    /// This interface defines a complete generic visitor for a parse tree produced
    /// by <see cref="PseudoParser"/>.
    /// </summary>
    /// <typeparam name="Result">The return type of the visit operation.</typeparam>
    [System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.1")]
    [System.CLSCompliant(false)]
    public interface IPseudoVisitor<Result> : IParseTreeVisitor<Result>
    {
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.unit"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitUnit([NotNull] PseudoParser.UnitContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.block"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitBlock([NotNull] PseudoParser.BlockContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.statement"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitStatement([NotNull] PseudoParser.StatementContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.declaration"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitDeclaration([NotNull] PseudoParser.DeclarationContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.parlist"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitParlist([NotNull] PseudoParser.ParlistContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.assignment"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitAssignment([NotNull] PseudoParser.AssignmentContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.exp"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitExp([NotNull] PseudoParser.ExpContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.aexp"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitAexp([NotNull] PseudoParser.AexpContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.lexp"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitLexp([NotNull] PseudoParser.LexpContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.aop"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitAop([NotNull] PseudoParser.AopContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.rop"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitRop([NotNull] PseudoParser.RopContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.lop"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitLop([NotNull] PseudoParser.LopContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.uop"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitUop([NotNull] PseudoParser.UopContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.cexp"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitCexp([NotNull] PseudoParser.CexpContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.explist"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitExplist([NotNull] PseudoParser.ExplistContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.literal"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitLiteral([NotNull] PseudoParser.LiteralContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.type"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitType([NotNull] PseudoParser.TypeContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.typename"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitTypename([NotNull] PseudoParser.TypenameContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.var"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitVar([NotNull] PseudoParser.VarContext context);
        /// <summary>
        /// Visit a parse tree produced by <see cref="PseudoParser.iexp"/>.
        /// </summary>
        /// <param name="context">The parse tree.</param>
        /// <return>The visitor result.</return>
        Result VisitIexp([NotNull] PseudoParser.IexpContext context);
    }
}