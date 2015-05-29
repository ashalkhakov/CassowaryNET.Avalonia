using System.Windows;

namespace AutoLayoutPanel
{
    public class LayoutLinearExpression : DependencyObject
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                "Source",
                typeof (UIElement),
                typeof (LayoutLinearExpression));

        public UIElement Source
        {
            get { return (UIElement) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public LayoutProperty Property { get; set; }
        public double Multiplier { get; set; }
    }
}