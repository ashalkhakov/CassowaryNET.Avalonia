using System;
using CassowaryNET.Constraints;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests.Constraints
{
    [TestFixture]
    public class EqualityConstraintTests
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
                var target = new EqualityConstraint(expression);
                ExpressionEx.AssertEqual(target.Expression, expression);
            }
        }
        
        [TestFixture]
        public class Constructor_VariableExpressionTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var variable = new Variable("x");
                var expression = new Variable("test") + 1d;
                var target = new EqualityConstraint(variable, expression);
                ExpressionEx.AssertEqual(
                    target.Expression,
                    expression - variable);
            }
        }
        
        [TestFixture]
        public class Constructor_VariableVariableTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var variable1 = new Variable("x");
                var variable2 = new Variable("y");
                var target = new EqualityConstraint(variable1, variable2);
                ExpressionEx.AssertEqual(
                    target.Expression,
                    variable2 - variable1);
            }
        }

        
        [TestFixture]
        public class Constructor_VariableDoubleTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var variable = new Variable("x");
                var value = 42.3d;
                var target = new EqualityConstraint(variable, value);
                ExpressionEx.AssertEqual(
                    target.Expression,
                    value - variable);
            }
        }

        [TestFixture]
        public class Constructor_ExpressionVariableTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var expression = new Variable("test") + 1d;
                var variable = new Variable("x");
                var target = new EqualityConstraint(expression, variable);
                ExpressionEx.AssertEqual(
                    target.Expression,
                    expression - variable);
            }
        }

        [TestFixture]
        public class Constructor_ExpressionExpressionTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var expression1 = new Variable("test") + 1d;
                var expression2 = new Variable("x") + 3d;
                var target = new EqualityConstraint(expression1, expression2);
                ExpressionEx.AssertEqual(
                    target.Expression,
                    expression1 - expression2);
            }
        }

        [TestFixture]
        public class WithStrength //: TestBase
        {
            [Test]
            public void returns_new_constraint_with_only_strength_changed()
            {
                var expression = new Variable("test") + 1d;
                var target = new EqualityConstraint(expression, Strength.Weak);

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
                var target = new EqualityConstraint(expression, Strength.Weak, 2d);

                var modified = target.WithWeight(7d);

                Assert.That(modified.Strength, Is.EqualTo(target.Strength));
                ExpressionEx.AssertEqual(modified.Expression, target.Expression);

                Assert.That(target.Weight, Is.EqualTo(2d));
                Assert.That(modified.Weight, Is.EqualTo(7d));
            }
        }
    }
}