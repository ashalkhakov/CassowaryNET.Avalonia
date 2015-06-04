using System;
using CassowaryNET.Constraints;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests.Constraints
{
    [TestFixture]
    public class InequalityConstraintTests
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
        public class Constructor_ExpressionTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var expression = new Variable("test") + 1d;
                var target = new InequalityConstraint(expression);
                ExpressionEx.AssertEqual(target.Expression, expression);
            }
        }
        
        [TestFixture]
        public class Constructor_VariableExpressionTests //: TestBase
        {
            [TestCase(InequalityType.GreaterThanOrEqual)]
            [TestCase(InequalityType.LessThanOrEqual)]
            public void can_be_created(InequalityType inequalityType)
            {
                var variable = new Variable("x");
                var expression = new Variable("test") + 1d;
                var target = new InequalityConstraint(variable, inequalityType, expression);

                if (inequalityType == InequalityType.GreaterThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        variable - expression);
                }
                else if (inequalityType == InequalityType.LessThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        expression - variable);
                }
            }
        }

        [TestFixture]
        public class Constructor_VariableVariableTests //: TestBase
        {
            [TestCase(InequalityType.GreaterThanOrEqual)]
            [TestCase(InequalityType.LessThanOrEqual)]
            public void can_be_created(InequalityType inequalityType)
            {
                var variable1 = new Variable("x");
                var variable2 = new Variable("y");
                var target = new InequalityConstraint(variable1, inequalityType, variable2);

                if (inequalityType == InequalityType.GreaterThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        variable1 - variable2);
                }
                else if (inequalityType == InequalityType.LessThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        variable2 - variable1);
                }
            }
        }

        [TestFixture]
        public class Constructor_VariableDoubleTests //: TestBase
        {
            [TestCase(InequalityType.GreaterThanOrEqual)]
            [TestCase(InequalityType.LessThanOrEqual)]
            public void can_be_created(InequalityType inequalityType)
            {
                var variable = new Variable("x");
                var value = 42.3d;
                var target = new InequalityConstraint(variable, inequalityType, value);

                if (inequalityType == InequalityType.GreaterThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        variable - value);
                }
                else if (inequalityType == InequalityType.LessThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        value - variable);
                }
            }
        }

        [TestFixture]
        public class Constructor_ExpressionVariableTests //: TestBase
        {
            [TestCase(InequalityType.GreaterThanOrEqual)]
            [TestCase(InequalityType.LessThanOrEqual)]
            public void can_be_created(InequalityType inequalityType)
            {
                var expression = new Variable("test") + 1d;
                var variable = new Variable("x");
                var target = new InequalityConstraint(expression, inequalityType, variable);

                if (inequalityType == InequalityType.GreaterThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        expression - variable);
                }
                else if (inequalityType == InequalityType.LessThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        variable - expression);
                }
            }
        }

        [TestFixture]
        public class Constructor_ExpressionExpressionTests //: TestBase
        {
            [TestCase(InequalityType.GreaterThanOrEqual)]
            [TestCase(InequalityType.LessThanOrEqual)]
            public void can_be_created(InequalityType inequalityType)
            {
                var expression1 = new Variable("test") + 1d;
                var expression2 = new Variable("x") + 3d;
                var target = new InequalityConstraint(expression1, inequalityType, expression2);

                if (inequalityType == InequalityType.GreaterThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        expression1 - expression2);
                }
                else if (inequalityType == InequalityType.LessThanOrEqual)
                {
                    ExpressionEx.AssertEqual(
                        target.Expression,
                        expression2 - expression1);
                }
            }
        }

        [TestFixture]
        public class WithStrength //: TestBase
        {
            [Test]
            public void returns_new_constraint_with_only_strength_changed()
            {
                var expression = new Variable("test") + 1d;
                var target = new InequalityConstraint(expression, Strength.Weak);

                var modified = target.WithStrength(Strength.Strong);

                Assert.That(modified.Weight, Is.EqualTo(target.Weight));
                ExpressionEx.AssertEqual(modified.Expression, target.Expression);

                Assert.That(target.Strength, Is.EqualTo(Strength.Weak));
                Assert.That(modified.Strength, Is.EqualTo(Strength.Strong));
            }
        }

        [TestFixture]
        public class WithWeight //: TestBase
        {
            [Test]
            public void returns_new_constraint_with_only_weight_changed()
            {
                var expression = new Variable("test") + 1d;
                var target = new InequalityConstraint(expression, Strength.Weak, 2d);

                var modified = target.WithWeight(7d);

                Assert.That(modified.Strength, Is.EqualTo(target.Strength));
                ExpressionEx.AssertEqual(modified.Expression, target.Expression);

                Assert.That(target.Weight, Is.EqualTo(2d));
                Assert.That(modified.Weight, Is.EqualTo(7d));
            }
        }
    }
}