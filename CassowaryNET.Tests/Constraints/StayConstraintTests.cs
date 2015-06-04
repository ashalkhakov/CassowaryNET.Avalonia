using System;
using CassowaryNET.Constraints;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests.Constraints
{
    [TestFixture]
    public class StayConstraintTests
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
        public class Constructor_VariableStrengthTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var variable = new Variable("test", 42.3d);
                var strength = Strength.Medium;
                var target = new StayConstraint(variable, strength);

                Assert.That(target.Variable, Is.SameAs(variable));
                Assert.That(target.Strength, Is.EqualTo(strength));
                Assert.That(target.Weight, Is.EqualTo(1d));

                ExpressionEx.AssertEqual(target.Expression, 42.3d - variable);

                Assert.That(target.IsEditConstraint, Is.False);
                Assert.That(target.IsStayConstraint, Is.True);
                Assert.That(target.IsInequality, Is.False);
            }

            [Test]
            public void when_variable_null_throws()
            {
                Assert.That(
                    () => new StayConstraint(null, Strength.Medium),
                    Throws.InstanceOf<ArgumentNullException>());

            }

            [Test]
            public void when_strength_null_throws()
            {
                var variable = new Variable("test", 42.3d);
                Assert.That(
                    () => new StayConstraint(variable, null),
                    Throws.InstanceOf<ArgumentNullException>());

            }
        }

        [TestFixture]
        public class Constructor_VariableStrengthWeightTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var variable = new Variable("test", 42.3d);
                var strength = Strength.Medium;
                var weight = 2.3d;
                var target = new StayConstraint(variable, strength, weight);

                Assert.That(target.Variable, Is.SameAs(variable));
                Assert.That(target.Strength, Is.EqualTo(strength));
                Assert.That(target.Weight, Is.EqualTo(weight));

                ExpressionEx.AssertEqual(target.Expression, 42.3d - variable);

                Assert.That(target.IsEditConstraint, Is.False);
                Assert.That(target.IsStayConstraint, Is.True);
                Assert.That(target.IsInequality, Is.False);
            }


            [Test]
            public void when_variable_null_throws()
            {
                Assert.That(
                    () => new StayConstraint(null, Strength.Medium, 2.3d),
                    Throws.InstanceOf<ArgumentNullException>());

            }

            [Test]
            public void when_strength_null_throws()
            {
                var variable = new Variable("test", 42.3d);
                Assert.That(
                    () => new StayConstraint(variable, null, 2.3d),
                    Throws.InstanceOf<ArgumentNullException>());

            }
        }
    }
}