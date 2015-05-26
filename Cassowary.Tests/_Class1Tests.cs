using System;
using NUnit.Framework;
using Moq;

namespace Cassowary.Tests
{
    public class Class1
    {
    }

    [TestFixture]
    public class Class1Tests
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

            internal Class1 GetTarget()
            {
                return new Class1();
            }
        }

        #endregion

        [TestFixture]
        public class ConstructorTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new Class1();
                Assert.Pass();
            }
        }

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