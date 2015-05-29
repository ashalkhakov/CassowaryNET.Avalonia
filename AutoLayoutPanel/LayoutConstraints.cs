using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace AutoLayoutPanel
{
    [ContentProperty("Constraints")]
    public class LayoutConstraints : DependencyObject
    {
        public static readonly DependencyProperty ConstraintsProperty =
            DependencyProperty.Register(
                "Constraints",
                typeof(List<LayoutConstraint>),
                typeof(LayoutConstraints),
                new PropertyMetadata(new List<LayoutConstraint>()));

        public List<LayoutConstraint> Constraints
        {
            get { return (List<LayoutConstraint>)GetValue(ConstraintsProperty); }
            set { SetValue(ConstraintsProperty, value); }
        }
    }
}