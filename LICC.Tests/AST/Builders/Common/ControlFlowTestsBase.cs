﻿using NUnit.Framework;
using LICC.AST.Nodes;
using LICC.AST.Visitors;

namespace LICC.Tests.AST.Builders.Common
{
    internal abstract class ControlFlowTestsBase : ASTBuilderTestBase
    {
        protected IfStatementNode AssertIfStatement(string src, object condValue, int thenStatementCount = 1, int? elseStatementCount = null)
        {
            IfStatementNode node = this.GenerateAST(src).As<IfStatementNode>();
            Assert.That(node, Is.Not.Null);
            this.AssertChildrenParentProperties(node);
            Assert.That(ConstantExpressionEvaluator.Evaluate(node.Condition), Is.EqualTo(condValue));
            Assert.That(node.ThenStatement.Children, Has.Exactly(thenStatementCount).Items);
            if (elseStatementCount is { }) {
                Assert.That(node.ElseStatement, Is.Not.Null);
                Assert.That(node.ElseStatement!.Children, Has.Exactly(elseStatementCount.Value).Items);
            } else {
                Assert.That(node.ElseStatement, Is.Null);
            }
            return node;
        }

        protected WhileStatementNode AssertWhileStatement(string src, object condValue, int statCount = 1)
        {
            WhileStatementNode node = this.GenerateAST(src).As<WhileStatementNode>();
            Assert.That(node, Is.Not.Null);
            this.AssertChildrenParentProperties(node);
            Assert.That(ConstantExpressionEvaluator.Evaluate(node.Condition), Is.EqualTo(condValue));
            if (node.Statement is BlockStatementNode block)
                Assert.That(block.Children, Has.Exactly(statCount).Items);
            else
                Assert.That(node.Statement.Children, Has.Exactly(statCount).Items);
            return node;
        }
    }
}
