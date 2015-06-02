using System;
using System.Collections.Generic;
using System.Linq;

namespace CassowaryNET.Utils
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
