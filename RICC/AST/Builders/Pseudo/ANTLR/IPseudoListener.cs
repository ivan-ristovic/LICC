#nullable disable
namespace RICC.AST.Builders.Pseudo
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
	using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
	using IToken = Antlr4.Runtime.IToken;

	/// <summary>
	/// This interface defines a complete listener for a parse tree produced by
	/// <see cref="PseudoParser"/>.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.1")]
	[System.CLSCompliant(false)]
	public interface IPseudoListener : IParseTreeListener
	{
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.unit"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterUnit([NotNull] PseudoParser.UnitContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.unit"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitUnit([NotNull] PseudoParser.UnitContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.block"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterBlock([NotNull] PseudoParser.BlockContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.block"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitBlock([NotNull] PseudoParser.BlockContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.statement"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterStatement([NotNull] PseudoParser.StatementContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.statement"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitStatement([NotNull] PseudoParser.StatementContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.declaration"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterDeclaration([NotNull] PseudoParser.DeclarationContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.declaration"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitDeclaration([NotNull] PseudoParser.DeclarationContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.parlist"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterParlist([NotNull] PseudoParser.ParlistContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.parlist"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitParlist([NotNull] PseudoParser.ParlistContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.assignment"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterAssignment([NotNull] PseudoParser.AssignmentContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.assignment"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitAssignment([NotNull] PseudoParser.AssignmentContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.exp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterExp([NotNull] PseudoParser.ExpContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.exp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitExp([NotNull] PseudoParser.ExpContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.aexp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterAexp([NotNull] PseudoParser.AexpContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.aexp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitAexp([NotNull] PseudoParser.AexpContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.lexp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterLexp([NotNull] PseudoParser.LexpContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.lexp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitLexp([NotNull] PseudoParser.LexpContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.aop"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterAop([NotNull] PseudoParser.AopContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.aop"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitAop([NotNull] PseudoParser.AopContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.rop"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterRop([NotNull] PseudoParser.RopContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.rop"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitRop([NotNull] PseudoParser.RopContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.lop"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterLop([NotNull] PseudoParser.LopContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.lop"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitLop([NotNull] PseudoParser.LopContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.uop"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterUop([NotNull] PseudoParser.UopContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.uop"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitUop([NotNull] PseudoParser.UopContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.cexp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterCexp([NotNull] PseudoParser.CexpContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.cexp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitCexp([NotNull] PseudoParser.CexpContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.explist"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterExplist([NotNull] PseudoParser.ExplistContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.explist"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitExplist([NotNull] PseudoParser.ExplistContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.literal"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterLiteral([NotNull] PseudoParser.LiteralContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.literal"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitLiteral([NotNull] PseudoParser.LiteralContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.type"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterType([NotNull] PseudoParser.TypeContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.type"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitType([NotNull] PseudoParser.TypeContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.typename"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterTypename([NotNull] PseudoParser.TypenameContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.typename"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitTypename([NotNull] PseudoParser.TypenameContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.var"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterVar([NotNull] PseudoParser.VarContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.var"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitVar([NotNull] PseudoParser.VarContext context);
		/// <summary>
		/// Enter a parse tree produced by <see cref="PseudoParser.iexp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void EnterIexp([NotNull] PseudoParser.IexpContext context);
		/// <summary>
		/// Exit a parse tree produced by <see cref="PseudoParser.iexp"/>.
		/// </summary>
		/// <param name="context">The parse tree.</param>
		void ExitIexp([NotNull] PseudoParser.IexpContext context);
	}
}