using Avalonia;
using Avalonia.Metadata;
using System.Collections.Generic;

namespace LayoutNET
{
    public class LayoutConstraints : AvaloniaObject
    {
        public static readonly StyledProperty<List<LayoutConstraint>> ConstraintsProperty =
            AvaloniaProperty.Register<LayoutConstraints, List<LayoutConstraint>>(
                "Constraints");

        [Content]
        public List<LayoutConstraint> Constraints
        {
            get
            {
                var list = (List<LayoutConstraint>)GetValue(ConstraintsProperty);
                if (list == null)
                {
                    list = new List<LayoutConstraint>();
                    Constraints = list;
                }
                return list;
            }
            set { SetValue(ConstraintsProperty, value); }
        }
    }
}