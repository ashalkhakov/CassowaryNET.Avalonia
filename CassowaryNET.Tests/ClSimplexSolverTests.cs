using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Moq;

namespace Cassowary.Tests
{
    [TestFixture]
    public class ClSimplexSolverTests
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
        public class ConstructorTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new ClSimplexSolver();
                Assert.Pass();
            }
        }
    }
}