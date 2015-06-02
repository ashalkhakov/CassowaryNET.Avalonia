using System;
using System.Collections.Generic;
using System.Linq;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests.Variables
{
    [TestFixture]
    public class VariableTests
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

            internal Variable GetTarget()
            {
                var target = new Variable();
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
                var target = new Variable();

                Assert.That(target.Name, Is.Not.Null);
                Assert.That(target.Value, Is.EqualTo(0d));

                Assert.That(target.IsDummy, Is.False);
                Assert.That(target.IsExternal, Is.True);
                Assert.That(target.IsPivotable, Is.False);
                Assert.That(target.IsRestricted, Is.False);
            }
        }

        [TestFixture]
        public class Constructor_StringTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new Variable("foo");

                Assert.That(target.Name, Is.EqualTo("foo"));
                Assert.That(target.Value, Is.EqualTo(0d));

                Assert.That(target.IsDummy, Is.False);
                Assert.That(target.IsExternal, Is.True);
                Assert.That(target.IsPivotable, Is.False);
                Assert.That(target.IsRestricted, Is.False);
            }
        }

        [TestFixture]
        public class Constructor_DoubleTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new Variable(43.2d);

                Assert.That(target.Name, Is.Not.Null);
                Assert.That(target.Value, Is.EqualTo(43.2d));

                Assert.That(target.IsDummy, Is.False);
                Assert.That(target.IsExternal, Is.True);
                Assert.That(target.IsPivotable, Is.False);
                Assert.That(target.IsRestricted, Is.False);
            }
        }

        [TestFixture]
        public class Constructor_StringDoubleTests //: TestBase
        {
            [Test]
            public void can_be_created()
            {
                var target = new Variable("foo", 43.2d);

                Assert.That(target.Name, Is.EqualTo("foo"));
                Assert.That(target.Value, Is.EqualTo(43.2d));

                Assert.That(target.IsDummy, Is.False);
                Assert.That(target.IsExternal, Is.True);
                Assert.That(target.IsPivotable, Is.False);
                Assert.That(target.IsRestricted, Is.False);
            }
        }

        [TestFixture]
        public class SetValueTests : TestBase
        {
            [Test]
            public void changes_value()
            {
                var target = GetTarget();

                target.Value = 42.3d;

                Assert.That(target.Value, Is.EqualTo(42.3d));
            }
        }
    }
}
