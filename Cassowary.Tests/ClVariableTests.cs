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
    public class ClVariableTests
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

            internal ClVariable GetTarget()
            {
                var target = new ClVariable();
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
                var target = new ClVariable();

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
                var target = new ClVariable("foo");

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
                var target = new ClVariable(43.2d);

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
                var target = new ClVariable("foo", 43.2d);

                Assert.That(target.Name, Is.EqualTo("foo"));
                Assert.That(target.Value, Is.EqualTo(43.2d));

                Assert.That(target.IsDummy, Is.False);
                Assert.That(target.IsExternal, Is.True);
                Assert.That(target.IsPivotable, Is.False);
                Assert.That(target.IsRestricted, Is.False);
            }
        }

        //[TestFixture]
        //public class SetValueTests : TestBase
        //{
        //    [Test]
        //    public void changes_value()
        //    {
        //        var target = GetTarget();

        //        target.Value = 42.3d;

        //        Assert.That(target.Value, Is.EqualTo(42.3d));
        //    }
        //}
    }
}
