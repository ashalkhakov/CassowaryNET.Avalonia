using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassowary.Variables;
using NUnit.Framework;
using Moq;

namespace Cassowary.Tests
{
    [TestFixture]
    public class ClDummyVariableTests
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

            internal ClDummyVariable GetTarget()
            {
                var target = new ClDummyVariable();
                return target;
            }
        }

        #endregion

        [TestFixture]
        public class ConstructorTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new ClDummyVariable();

                Assert.That(target.Name, Is.Not.Null);

                Assert.That(target.IsDummy, Is.True);
                Assert.That(target.IsExternal, Is.False);
                Assert.That(target.IsPivotable, Is.False);
                Assert.That(target.IsRestricted, Is.True);
            }
        }

        [TestFixture]
        public class Constructor_StringTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new ClDummyVariable("foo");

                Assert.That(target.Name, Is.StringStarting("foo"));

                Assert.That(target.IsDummy, Is.True);
                Assert.That(target.IsExternal, Is.False);
                Assert.That(target.IsPivotable, Is.False);
                Assert.That(target.IsRestricted, Is.True);
            }
        }
    }
}
