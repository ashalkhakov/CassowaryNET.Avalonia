using System;
using System.Security;
using CassowaryNET.Constraints;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests.Constraints
{
    [TestFixture]
    public class LinearConstraintTests
    {
        #region Helpers

        private class LinearConstraintFake : LinearConstraint
        {
            public LinearConstraintFake(LinearExpression expression, Strength strength, double weight)
                : base(expression, strength, weight)
            {
            }

            public LinearConstraintFake(LinearExpression expression, Strength strength)
                : base(expression, strength)
            {
            }

            public LinearConstraintFake(LinearExpression expression)
                : base(expression)
            {
            }

            protected override LinearConstraint WithStrengthCore(Strength strength)
            {
                throw new NotImplementedException();
            }

            protected override LinearConstraint WithWeightCore(double weight)
            {
                throw new NotImplementedException();
            }
        }

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
        public class Constructor_ExpressionStrengthWeightTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var expression = new Variable("test", 42.3d) + 1d;
                var strength = Strength.Medium;
                var weight = 2.3d;

                var target = new LinearConstraintFake(expression, strength, weight);

                ExpressionEx.AssertEqual(target.Expression, expression);
                Assert.That(target.Strength, Is.EqualTo(strength));
                Assert.That(target.Weight, Is.EqualTo(weight));
            }

            [Test]
            public void when_variable_null_throws()
            {
                Assert.That(
                    () => new EditConstraint(null, Strength.Medium),
                    Throws.InstanceOf<ArgumentNullException>());
            }

            [Test]
            public void when_strength_null_throws()
            {
                var variable = new Variable("test", 42.3d);
                Assert.That(
                    () => new EditConstraint(variable, null),
                    Throws.InstanceOf<ArgumentNullException>());
            }
        }

        [TestFixture]
        public class Constructor_ExpressionStrengthTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var expression = new Variable("test", 42.3d) + 1d;
                var strength = Strength.Medium;

                var target = new LinearConstraintFake(expression, strength);

                ExpressionEx.AssertEqual(target.Expression, expression);
                Assert.That(target.Strength, Is.EqualTo(strength));
                Assert.That(target.Weight, Is.EqualTo(1d));
            }

            [Test]
            public void when_variable_null_throws()
            {
                Assert.That(
                    () => new EditConstraint(null, Strength.Medium),
                    Throws.InstanceOf<ArgumentNullException>());
            }

            [Test]
            public void when_strength_null_throws()
            {
                var variable = new Variable("test", 42.3d);
                Assert.That(
                    () => new EditConstraint(variable, null),
                    Throws.InstanceOf<ArgumentNullException>());
            }
        }

        [TestFixture]
        public class Constructor_ExpressionTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var expression = new Variable("test", 42.3d) + 1d;

                var target = new LinearConstraintFake(expression);

                ExpressionEx.AssertEqual(target.Expression, expression);
                Assert.That(target.Strength, Is.EqualTo(Strength.Required));
                Assert.That(target.Weight, Is.EqualTo(1d));
            }

            [Test]
            public void when_variable_null_throws()
            {
                Assert.That(
                    () => new EditConstraint(null, Strength.Medium),
                    Throws.InstanceOf<ArgumentNullException>());
            }

            [Test]
            public void when_strength_null_throws()
            {
                var variable = new Variable("test", 42.3d);
                Assert.That(
                    () => new EditConstraint(variable, null),
                    Throws.InstanceOf<ArgumentNullException>());
            }
        }

    }
}