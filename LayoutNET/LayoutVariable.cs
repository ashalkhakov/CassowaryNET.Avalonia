using Avalonia.Controls;
using CassowaryNET.Variables;

namespace LayoutNET
{
    public class LayoutVariable
    {
        #region Fields

        private readonly Control uiElement;
        private readonly LayoutProperty property;
        private readonly Variable variable;

        #endregion

        #region Constructors

        public LayoutVariable(Control uiElement, LayoutProperty property)
        {
            this.uiElement = uiElement;
            this.property = property;

            var elementName = uiElement.Name ?? "???";
            variable = new Variable(elementName + property);
        }

        #endregion

        #region Properties

        public Control UiElement
        {
            get { return uiElement; }
        }

        public LayoutProperty Property
        {
            get { return property; }
        }

        public Variable Variable
        {
            get { return variable; }
        }

        #endregion
    }
}