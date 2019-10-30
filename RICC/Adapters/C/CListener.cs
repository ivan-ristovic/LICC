using System;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using RICC.Context;
using Serilog;

namespace RICC.Adapters.C
{
    public sealed class CListener : ParserListener, ICListener
    {
        public override Parser CreateParser(string path)
        {
            string code = File.ReadAllText(path);
            var parser = new CParser(new CommonTokenStream(new CLexer(CharStreams.fromstring(code))));
            parser.BuildParseTree = true;
            return parser;
        }

        public override void ListenParse(Parser parser)
        {
            if (parser is CParser cparser)
                ParseTreeWalker.Default.Walk(this, cparser.translationUnit());
            else
                Log.Fatal("Parser type mismatch. Expected CParser, got {ParserType}", parser.GetType());
        }


        public void EnterTranslationUnit([NotNull] CParser.TranslationUnitContext ctx)
        {
            Log.Debug("Entered translation unit: {Context}", ctx);
            this.OnEnterTranslationUnit(new EnterTranslationUnitEventArgs());
        }

        public void ExitTranslationUnit([NotNull] CParser.TranslationUnitContext ctx)
        {
            Log.Debug("Exited translation unit: {Context}", ctx);
            this.OnLeaveTranslationUnit(new LeaveTranslationUnitEventArgs());
        }

        public void EnterPrimaryExpression([NotNull] CParser.PrimaryExpressionContext context) { }
        public void ExitPrimaryExpression([NotNull] CParser.PrimaryExpressionContext context) {}
        public void EnterGenericSelection([NotNull] CParser.GenericSelectionContext context) {}
        public void ExitGenericSelection([NotNull] CParser.GenericSelectionContext context) {}
        public void EnterGenericAssocList([NotNull] CParser.GenericAssocListContext context) {}
        public void ExitGenericAssocList([NotNull] CParser.GenericAssocListContext context) {}
        public void EnterGenericAssociation([NotNull] CParser.GenericAssociationContext context) {}
        public void ExitGenericAssociation([NotNull] CParser.GenericAssociationContext context) {}
        public void EnterPostfixExpression([NotNull] CParser.PostfixExpressionContext context) {}
        public void ExitPostfixExpression([NotNull] CParser.PostfixExpressionContext context) {}
        public void EnterArgumentExpressionList([NotNull] CParser.ArgumentExpressionListContext context) {}
        public void ExitArgumentExpressionList([NotNull] CParser.ArgumentExpressionListContext context) {}
        public void EnterUnaryExpression([NotNull] CParser.UnaryExpressionContext context) {}
        public void ExitUnaryExpression([NotNull] CParser.UnaryExpressionContext context) {}
        public void EnterUnaryOperator([NotNull] CParser.UnaryOperatorContext context) {}
        public void ExitUnaryOperator([NotNull] CParser.UnaryOperatorContext context) {}
        public void EnterCastExpression([NotNull] CParser.CastExpressionContext context) {}
        public void ExitCastExpression([NotNull] CParser.CastExpressionContext context) {}
        public void EnterMultiplicativeExpression([NotNull] CParser.MultiplicativeExpressionContext context) {}
        public void ExitMultiplicativeExpression([NotNull] CParser.MultiplicativeExpressionContext context) {}
        public void EnterAdditiveExpression([NotNull] CParser.AdditiveExpressionContext context) {}
        public void ExitAdditiveExpression([NotNull] CParser.AdditiveExpressionContext context) {}
        public void EnterShiftExpression([NotNull] CParser.ShiftExpressionContext context) {}
        public void ExitShiftExpression([NotNull] CParser.ShiftExpressionContext context) {}
        public void EnterRelationalExpression([NotNull] CParser.RelationalExpressionContext context) {}
        public void ExitRelationalExpression([NotNull] CParser.RelationalExpressionContext context) {}
        public void EnterEqualityExpression([NotNull] CParser.EqualityExpressionContext context) {}
        public void ExitEqualityExpression([NotNull] CParser.EqualityExpressionContext context) {}
        public void EnterAndExpression([NotNull] CParser.AndExpressionContext context) {}
        public void ExitAndExpression([NotNull] CParser.AndExpressionContext context) {}
        public void EnterExclusiveOrExpression([NotNull] CParser.ExclusiveOrExpressionContext context) {}
        public void ExitExclusiveOrExpression([NotNull] CParser.ExclusiveOrExpressionContext context) {}
        public void EnterInclusiveOrExpression([NotNull] CParser.InclusiveOrExpressionContext context) {}
        public void ExitInclusiveOrExpression([NotNull] CParser.InclusiveOrExpressionContext context) {}
        public void EnterLogicalAndExpression([NotNull] CParser.LogicalAndExpressionContext context) {}
        public void ExitLogicalAndExpression([NotNull] CParser.LogicalAndExpressionContext context) {}
        public void EnterLogicalOrExpression([NotNull] CParser.LogicalOrExpressionContext context) {}
        public void ExitLogicalOrExpression([NotNull] CParser.LogicalOrExpressionContext context) {}
        public void EnterConditionalExpression([NotNull] CParser.ConditionalExpressionContext context) {}
        public void ExitConditionalExpression([NotNull] CParser.ConditionalExpressionContext context) {}
        public void EnterAssignmentExpression([NotNull] CParser.AssignmentExpressionContext context) {}
        public void ExitAssignmentExpression([NotNull] CParser.AssignmentExpressionContext context) {}
        public void EnterAssignmentOperator([NotNull] CParser.AssignmentOperatorContext context) {}
        public void ExitAssignmentOperator([NotNull] CParser.AssignmentOperatorContext context) {}
        public void EnterExpression([NotNull] CParser.ExpressionContext context) {}
        public void ExitExpression([NotNull] CParser.ExpressionContext context) {}
        public void EnterConstantExpression([NotNull] CParser.ConstantExpressionContext context) {}
        public void ExitConstantExpression([NotNull] CParser.ConstantExpressionContext context) {}
        public void EnterDeclaration([NotNull] CParser.DeclarationContext context) {}
        public void ExitDeclaration([NotNull] CParser.DeclarationContext context) {}
        public void EnterDeclarationSpecifiers([NotNull] CParser.DeclarationSpecifiersContext context) {}
        public void ExitDeclarationSpecifiers([NotNull] CParser.DeclarationSpecifiersContext context) {}
        public void EnterDeclarationSpecifiers2([NotNull] CParser.DeclarationSpecifiers2Context context) {}
        public void ExitDeclarationSpecifiers2([NotNull] CParser.DeclarationSpecifiers2Context context) {}
        public void EnterDeclarationSpecifier([NotNull] CParser.DeclarationSpecifierContext context) {}
        public void ExitDeclarationSpecifier([NotNull] CParser.DeclarationSpecifierContext context) {}
        public void EnterInitDeclaratorList([NotNull] CParser.InitDeclaratorListContext context) {}
        public void ExitInitDeclaratorList([NotNull] CParser.InitDeclaratorListContext context) {}
        public void EnterInitDeclarator([NotNull] CParser.InitDeclaratorContext context) {}
        public void ExitInitDeclarator([NotNull] CParser.InitDeclaratorContext context) {}
        public void EnterStorageClassSpecifier([NotNull] CParser.StorageClassSpecifierContext context) {}
        public void ExitStorageClassSpecifier([NotNull] CParser.StorageClassSpecifierContext context) {}
        public void EnterTypeSpecifier([NotNull] CParser.TypeSpecifierContext context) {}
        public void ExitTypeSpecifier([NotNull] CParser.TypeSpecifierContext context) {}
        public void EnterStructOrUnionSpecifier([NotNull] CParser.StructOrUnionSpecifierContext context) {}
        public void ExitStructOrUnionSpecifier([NotNull] CParser.StructOrUnionSpecifierContext context) {}
        public void EnterStructOrUnion([NotNull] CParser.StructOrUnionContext context) {}
        public void ExitStructOrUnion([NotNull] CParser.StructOrUnionContext context) {}
        public void EnterStructDeclarationList([NotNull] CParser.StructDeclarationListContext context) {}
        public void ExitStructDeclarationList([NotNull] CParser.StructDeclarationListContext context) {}
        public void EnterStructDeclaration([NotNull] CParser.StructDeclarationContext context) {}
        public void ExitStructDeclaration([NotNull] CParser.StructDeclarationContext context) {}
        public void EnterSpecifierQualifierList([NotNull] CParser.SpecifierQualifierListContext context) {}
        public void ExitSpecifierQualifierList([NotNull] CParser.SpecifierQualifierListContext context) {}
        public void EnterStructDeclaratorList([NotNull] CParser.StructDeclaratorListContext context) {}
        public void ExitStructDeclaratorList([NotNull] CParser.StructDeclaratorListContext context) {}
        public void EnterStructDeclarator([NotNull] CParser.StructDeclaratorContext context) {}
        public void ExitStructDeclarator([NotNull] CParser.StructDeclaratorContext context) {}
        public void EnterEnumSpecifier([NotNull] CParser.EnumSpecifierContext context) {}
        public void ExitEnumSpecifier([NotNull] CParser.EnumSpecifierContext context) {}
        public void EnterEnumeratorList([NotNull] CParser.EnumeratorListContext context) {}
        public void ExitEnumeratorList([NotNull] CParser.EnumeratorListContext context) {}
        public void EnterEnumerator([NotNull] CParser.EnumeratorContext context) {}
        public void ExitEnumerator([NotNull] CParser.EnumeratorContext context) {}
        public void EnterEnumerationConstant([NotNull] CParser.EnumerationConstantContext context) {}
        public void ExitEnumerationConstant([NotNull] CParser.EnumerationConstantContext context) {}
        public void EnterAtomicTypeSpecifier([NotNull] CParser.AtomicTypeSpecifierContext context) {}
        public void ExitAtomicTypeSpecifier([NotNull] CParser.AtomicTypeSpecifierContext context) {}
        public void EnterTypeQualifier([NotNull] CParser.TypeQualifierContext context) {}
        public void ExitTypeQualifier([NotNull] CParser.TypeQualifierContext context) {}
        public void EnterFunctionSpecifier([NotNull] CParser.FunctionSpecifierContext context) {}
        public void ExitFunctionSpecifier([NotNull] CParser.FunctionSpecifierContext context) {}
        public void EnterAlignmentSpecifier([NotNull] CParser.AlignmentSpecifierContext context) {}
        public void ExitAlignmentSpecifier([NotNull] CParser.AlignmentSpecifierContext context) {}
        public void EnterDeclarator([NotNull] CParser.DeclaratorContext context) {}
        public void ExitDeclarator([NotNull] CParser.DeclaratorContext context) {}
        public void EnterDirectDeclarator([NotNull] CParser.DirectDeclaratorContext context) {}
        public void ExitDirectDeclarator([NotNull] CParser.DirectDeclaratorContext context) {}
        public void EnterGccDeclaratorExtension([NotNull] CParser.GccDeclaratorExtensionContext context) {}
        public void ExitGccDeclaratorExtension([NotNull] CParser.GccDeclaratorExtensionContext context) {}
        public void EnterGccAttributeSpecifier([NotNull] CParser.GccAttributeSpecifierContext context) {}
        public void ExitGccAttributeSpecifier([NotNull] CParser.GccAttributeSpecifierContext context) {}
        public void EnterGccAttributeList([NotNull] CParser.GccAttributeListContext context) {}
        public void ExitGccAttributeList([NotNull] CParser.GccAttributeListContext context) {}
        public void EnterGccAttribute([NotNull] CParser.GccAttributeContext context) {}
        public void ExitGccAttribute([NotNull] CParser.GccAttributeContext context) {}
        public void EnterNestedParenthesesBlock([NotNull] CParser.NestedParenthesesBlockContext context) {}
        public void ExitNestedParenthesesBlock([NotNull] CParser.NestedParenthesesBlockContext context) {}
        public void EnterPointer([NotNull] CParser.PointerContext context) {}
        public void ExitPointer([NotNull] CParser.PointerContext context) {}
        public void EnterTypeQualifierList([NotNull] CParser.TypeQualifierListContext context) {}
        public void ExitTypeQualifierList([NotNull] CParser.TypeQualifierListContext context) {}
        public void EnterParameterTypeList([NotNull] CParser.ParameterTypeListContext context) {}
        public void ExitParameterTypeList([NotNull] CParser.ParameterTypeListContext context) {}
        public void EnterParameterList([NotNull] CParser.ParameterListContext context) {}
        public void ExitParameterList([NotNull] CParser.ParameterListContext context) {}
        public void EnterParameterDeclaration([NotNull] CParser.ParameterDeclarationContext context) {}
        public void ExitParameterDeclaration([NotNull] CParser.ParameterDeclarationContext context) {}
        public void EnterIdentifierList([NotNull] CParser.IdentifierListContext context) {}
        public void ExitIdentifierList([NotNull] CParser.IdentifierListContext context) {}
        public void EnterTypeName([NotNull] CParser.TypeNameContext context) {}
        public void ExitTypeName([NotNull] CParser.TypeNameContext context) {}
        public void EnterAbstractDeclarator([NotNull] CParser.AbstractDeclaratorContext context) {}
        public void ExitAbstractDeclarator([NotNull] CParser.AbstractDeclaratorContext context) {}
        public void EnterDirectAbstractDeclarator([NotNull] CParser.DirectAbstractDeclaratorContext context) {}
        public void ExitDirectAbstractDeclarator([NotNull] CParser.DirectAbstractDeclaratorContext context) {}
        public void EnterTypedefName([NotNull] CParser.TypedefNameContext context) {}
        public void ExitTypedefName([NotNull] CParser.TypedefNameContext context) {}
        public void EnterInitializer([NotNull] CParser.InitializerContext context) {}
        public void ExitInitializer([NotNull] CParser.InitializerContext context) {}
        public void EnterInitializerList([NotNull] CParser.InitializerListContext context) {}
        public void ExitInitializerList([NotNull] CParser.InitializerListContext context) {}
        public void EnterDesignation([NotNull] CParser.DesignationContext context) {}
        public void ExitDesignation([NotNull] CParser.DesignationContext context) {}
        public void EnterDesignatorList([NotNull] CParser.DesignatorListContext context) {}
        public void ExitDesignatorList([NotNull] CParser.DesignatorListContext context) {}
        public void EnterDesignator([NotNull] CParser.DesignatorContext context) {}
        public void ExitDesignator([NotNull] CParser.DesignatorContext context) {}
        public void EnterStaticAssertDeclaration([NotNull] CParser.StaticAssertDeclarationContext context) {}
        public void ExitStaticAssertDeclaration([NotNull] CParser.StaticAssertDeclarationContext context) {}
        public void EnterStatement([NotNull] CParser.StatementContext context) {}
        public void ExitStatement([NotNull] CParser.StatementContext context) {}
        public void EnterLabeledStatement([NotNull] CParser.LabeledStatementContext context) {}
        public void ExitLabeledStatement([NotNull] CParser.LabeledStatementContext context) {}
        public void EnterCompoundStatement([NotNull] CParser.CompoundStatementContext context) {}
        public void ExitCompoundStatement([NotNull] CParser.CompoundStatementContext context) {}
        public void EnterBlockItemList([NotNull] CParser.BlockItemListContext context) {}
        public void ExitBlockItemList([NotNull] CParser.BlockItemListContext context) {}
        public void EnterBlockItem([NotNull] CParser.BlockItemContext context) {}
        public void ExitBlockItem([NotNull] CParser.BlockItemContext context) {}
        public void EnterExpressionStatement([NotNull] CParser.ExpressionStatementContext context) {}
        public void ExitExpressionStatement([NotNull] CParser.ExpressionStatementContext context) {}
        public void EnterSelectionStatement([NotNull] CParser.SelectionStatementContext context) {}
        public void ExitSelectionStatement([NotNull] CParser.SelectionStatementContext context) {}
        public void EnterIterationStatement([NotNull] CParser.IterationStatementContext context) {}
        public void ExitIterationStatement([NotNull] CParser.IterationStatementContext context) {}
        public void EnterForCondition([NotNull] CParser.ForConditionContext context) {}
        public void ExitForCondition([NotNull] CParser.ForConditionContext context) {}
        public void EnterForDeclaration([NotNull] CParser.ForDeclarationContext context) {}
        public void ExitForDeclaration([NotNull] CParser.ForDeclarationContext context) {}
        public void EnterForExpression([NotNull] CParser.ForExpressionContext context) {}
        public void ExitForExpression([NotNull] CParser.ForExpressionContext context) {}
        public void EnterJumpStatement([NotNull] CParser.JumpStatementContext context) {}
        public void ExitJumpStatement([NotNull] CParser.JumpStatementContext context) {}
        public void EnterCompilationUnit([NotNull] CParser.CompilationUnitContext context) {}
        public void ExitCompilationUnit([NotNull] CParser.CompilationUnitContext context) {}
        public void EnterExternalDeclaration([NotNull] CParser.ExternalDeclarationContext context) {}
        public void ExitExternalDeclaration([NotNull] CParser.ExternalDeclarationContext context) {}
        public void EnterFunctionDefinition([NotNull] CParser.FunctionDefinitionContext context) {}
        public void ExitFunctionDefinition([NotNull] CParser.FunctionDefinitionContext context) {}
        public void EnterDeclarationList([NotNull] CParser.DeclarationListContext context) {}
        public void ExitDeclarationList([NotNull] CParser.DeclarationListContext context) {}
        public void VisitTerminal(ITerminalNode node) {}
        public void VisitErrorNode(IErrorNode node) {}
        public void EnterEveryRule(ParserRuleContext ctx) {}
        public void ExitEveryRule(ParserRuleContext ctx) {}
    }
}
