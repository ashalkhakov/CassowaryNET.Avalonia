using System.Windows;

namespace LayoutNET
{
    public class LayoutLinearExpression : DependencyObject
    {
        //public static readonly DependencyProperty SourceProperty =
        //    DependencyProperty.Register(
        //        "Source",
        //        typeof (UIElement),
        //        typeof (LayoutLinearExpression));

        public LayoutLinearExpression()
        {
            Multiplier = 1d;
        }

        //public UIElement Source
        //{
        //    get { return (UIElement) GetValue(SourceProperty); }
        //    set { SetValue(SourceProperty, value); }
        //}
        
        //First works but is ugly; second returns null; third is crappy. :(
        
        //Source="{Binding Source={x:Reference MainPanel}}"
        //Source="{Binding ElementName=MainPanel}"
        //ElementName="MainPanel"

        public string ElementName { get; set; }
        public LayoutProperty Property { get; set; }
        public double Multiplier { get; set; }
    }
}