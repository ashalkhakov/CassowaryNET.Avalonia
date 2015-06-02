using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CassowaryNET.Utils;
using CassowaryNET.Variables;
using Moq;
using NUnit.Framework;

namespace CassowaryNET.Tests
{
    [TestFixture]
    public class ClLinearExpressionTests
    {
        #region Helpers

        public abstract class TestBase
        {
            [SetUp]
            public virtual void SetUp()
            {
            }

            [TearDown]
            public virtual void TearDown()
            {
            }
        }

        #endregion

        [TestFixture]
        public class ConstructorTests_double //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new ClLinearExpression(42.3d);
                Assert.Pass();
            }
        }

        [TestFixture]
        public class ConstructorTests_ClAbstractVariable_double_double //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new ClLinearExpression(new ClVariable(), 3d, 5d);
                Assert.Pass();
            }
        }

        [TestFixture]
        public class SubstituteOut_WithVariableSubstitutedBy_Regression : TestBase
        {
            [Test]
            public void regression()
            {
                var a = new ClVariable("a");
                var b_ToSub = new ClVariable("b");
                var c = new ClVariable("c");

                var x = new ClVariable("x");
                var y = new ClVariable("y");

                var mockNoteVariableChangesOld = new Mock<INoteVariableChanges>(
                    MockBehavior.Strict);
                var mockNoteVariableChangesNew = new Mock<INoteVariableChanges>(
                    MockBehavior.Strict);
                
                var subject = new ClVariable("subject");

                var additionsOld = new List<ClAbstractVariable>();
                var removalsOld = new List<ClAbstractVariable>();
                var additionsNew = new List<ClAbstractVariable>();
                var removalsNew = new List<ClAbstractVariable>();

                Func
                    <List<ClAbstractVariable>,
                        Action<ClAbstractVariable, ClAbstractVariable>>
                    getAddToList = list => (v, subj) => list.Add(v);

                Expression<Action<INoteVariableChanges>> noteAdded =
                    o => o.NoteAddedVariable(It.IsAny<ClAbstractVariable>(), subject);
                Expression<Action<INoteVariableChanges>> noteRemoved =
                    o => o.NoteRemovedVariable(It.IsAny<ClAbstractVariable>(), subject);

                mockNoteVariableChangesOld
                    .Setup(noteAdded)
                    .Callback(getAddToList(additionsOld));
                mockNoteVariableChangesOld
                    .Setup(noteRemoved)
                    .Callback(getAddToList(removalsOld));

                mockNoteVariableChangesNew
                    .Setup(noteAdded)
                    .Callback(getAddToList(additionsNew));
                mockNoteVariableChangesNew
                    .Setup(noteRemoved)
                    .Callback(getAddToList(removalsNew));

                var subtituteForB = 3.2d*x - 7d*y + 13d + 6d*c;

                var target = 0.3d*a + 2d*b_ToSub - 12d*c - 17.4d;
                
                // OLD WAY
                var substitutedOld = Cloneable.Clone(target);
                substitutedOld.SubstituteOut(
                    b_ToSub,
                    subtituteForB,
                    subject,
                    mockNoteVariableChangesOld.Object);

                // NEW WAY
                var substitutedNew = Cloneable.Clone(target)
                    .WithVariableSubstitutedBy(b_ToSub, subtituteForB);
                var addedVariables = substitutedNew.Terms.Keys
                    .Except(target.Terms.Keys)
                    .ToList();
                var removedVariables = target.Terms.Keys
                    .Except(substitutedNew.Terms.Keys)
                    .Where(v => !Equals(v, b_ToSub))
                    .ToList();
                foreach (var addedVariable in addedVariables)
                {
                    mockNoteVariableChangesNew.Object.NoteAddedVariable(
                        addedVariable,
                        subject);
                }
                foreach (var removedVariable in removedVariables)
                {
                    mockNoteVariableChangesNew.Object.NoteRemovedVariable(
                        removedVariable,
                        subject);
                }

                Assert.That(additionsNew, Is.EqualTo(additionsOld));
                Assert.That(removalsNew, Is.EqualTo(removalsOld));

                Assert.That(substitutedNew.Terms.Keys, Is.EqualTo(substitutedOld.Terms.Keys));

                foreach (var term in substitutedOld.Terms.Keys)
                {
                    var termCoefficientOld = substitutedOld.Terms[term];
                    var termCoefficientNew = substitutedNew.Terms[term];

                    Assert.That(
                        termCoefficientNew.Value,
                        IsX.Approx(termCoefficientOld.Value));
                }

                Assert.That(substitutedNew.Constant, Is.EqualTo(substitutedOld.Constant));
            }
        }
    }
}