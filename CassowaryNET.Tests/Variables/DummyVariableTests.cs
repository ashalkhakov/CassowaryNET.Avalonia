﻿using System;
using System.Collections.Generic;
using System.Linq;
using CassowaryNET.Variables;
using NUnit.Framework;

namespace CassowaryNET.Tests.Variables
{
    [TestFixture]
    public class DummyVariableTests
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

            internal DummyVariable GetTarget()
            {
                var target = new DummyVariable();
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
                var target = new DummyVariable();

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
                var target = new DummyVariable("foo");

                Assert.That(target.Name, Does.StartWith("foo"));

                Assert.That(target.IsDummy, Is.True);
                Assert.That(target.IsExternal, Is.False);
                Assert.That(target.IsPivotable, Is.False);
                Assert.That(target.IsRestricted, Is.True);
            }
        }
    }
}
