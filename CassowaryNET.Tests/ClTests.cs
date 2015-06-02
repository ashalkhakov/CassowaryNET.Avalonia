using System;
using NUnit.Framework;

namespace CassowaryNET.Tests
{
    [TestFixture]
    public class ClTests
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
        public class Method1Tests : TestBase
        {
            [Test]
            public void when_foo_then_bar()
            {
            }
        }
    }
}