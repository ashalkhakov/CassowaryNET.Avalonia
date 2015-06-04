using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CassowaryNET.Tests
{
    public static class ExpressionEx
    {
        public static void AssertEqual(
            LinearExpression actual,
            LinearExpression expected)
        {
            Assert.That(actual.Terms, Is.EquivalentTo(expected.Terms));
            Assert.That(actual.Constant, Is.EqualTo(expected.Constant));
        }
    }
}
