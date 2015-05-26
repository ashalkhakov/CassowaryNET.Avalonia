using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassowary.Utils
{
    public static class Cloneable
    {
        public static T Clone<T>(T cloneable)
            where T : ICloneable
        {
            return (T) cloneable.Clone();
        }
    }
}
