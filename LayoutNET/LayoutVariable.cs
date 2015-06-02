using System.Windows;
using CassowaryNET.Variables;

namespace LayoutNET
{
    public class LayoutVariable
    {
        #region Fields

        private readonly UIElement uiElement;
        private readonly LayoutProperty property;
        private readonly ClVariable variable;

        #endregion

        #region Constructors

        public LayoutVariable(UIElement uiElement, LayoutProperty property)
        {
            this.uiElement = uiElement;
            this.property = property;

            var elementName = ((FrameworkElement) uiElement).Name ?? "???";
            variable = new ClVariable(elementName + property);
        }

        #endregion

        #region Properties

        public UIElement UiElement
        {
            get { return uiElement; }
        }

        public LayoutProperty Property
        {
            get { return property; }
        }

        public ClVariable Variable
        {
            get { return variable; }
        }

        #endregion
    }
}