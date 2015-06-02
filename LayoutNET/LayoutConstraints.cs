using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace LayoutNET
{
    [ContentProperty("Constraints")]
    public class LayoutConstraints : DependencyObject
    {
        public static readonly DependencyProperty ConstraintsProperty =
            DependencyProperty.Register(
                "Constraints",
                typeof(List<LayoutConstraint>),
                typeof(LayoutConstraints));

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