using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Cassowary.Tests
{
    public static class IsX
    {
        public static Constraint Approx(double expected)
        {
            const double delta = 1d/(1024d*1024d);
            var min = (1d - delta)*expected;
            var max = (1d + delta)*expected;

            return Is.InRange(min, max);
        }
    }
}