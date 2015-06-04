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
        public class Constructor_ExpressionStrengthWeightTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var expression = new Variable("test", 42.3d) + 1d;
                var strength = Strength.Medium;
                var weight = 2.3d;

                var target = new EqualityConstraint(expression, strength, weight);

                ExpressionEx.AssertEqual(target.Expression, expression);
                Assert.That(target.Strength, Is.EqualTo(strength));
                Assert.That(target.Weight, Is.EqualTo(weight));

                Assert.That(target.IsEditConstraint, Is.False);
                Assert.That(target.IsStayConstraint, Is.False);
                Assert.That(target.IsInequality, Is.False);
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