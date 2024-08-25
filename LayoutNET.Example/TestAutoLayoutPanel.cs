using Avalonia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayoutNET.Example
{
    public class TestAutoLayoutPanel : AutoLayoutPanel
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var sw = Stopwatch.StartNew();

            var size = base.ArrangeOverride(arrangeSize);

            sw.Stop();
            ArrangeMilliseconds += sw.ElapsedMilliseconds;

            return size;
        }
        
        public static long ArrangeMilliseconds = 0;
    }
}
