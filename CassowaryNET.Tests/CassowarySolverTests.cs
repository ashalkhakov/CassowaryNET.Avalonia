using System;
using System.Configuration;
using System.Linq;
using CassowaryNET.Exceptions;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests
{
    [TestFixture]
    public class CassowarySolverTests
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

            internal CassowarySolver GetTarget()
            {
                return new CassowarySolver();
            }
        }

        #endregion

        [TestFixture]
        public class ConstructorTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new CassowarySolver();
                Assert.Pass();
            }
        }

        [TestFixture]
        public class AddStayTest : TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = GetTarget();
                target.AutoSolve = false;

                var x = new Variable("x", 10d);
                var y = new Variable("y", 20d);

                target.AddConstraint(x == y);

                target.AddStay(x, Strength.Required);

                Assert.That(
                    () => target.AddStay(y, Strength.Required),
                    Throws.InstanceOf<RequiredConstraintFailureException>());
            }
        }
    }
}