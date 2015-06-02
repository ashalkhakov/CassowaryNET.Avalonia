using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using Cassowary.Variables;

namespace AutoLayoutPanel
{
    public class LayoutVariableSet
    {
        #region Fields

        private readonly UIElement uiElement;
        private readonly IReadOnlyDictionary<LayoutProperty, LayoutVariable>
            variables;

        #endregion

        #region Constructors

        public LayoutVariableSet(UIElement uiElement)
        {
            this.uiElement = uiElement;
            this.variables = Enum
                .GetValues(typeof (LayoutProperty))
                .Cast<LayoutProperty>()
                .Select(lp => new LayoutVariable(uiElement, lp))
                .ToDictionary(o => o.Property)
                .AsReadOnly();
        }

        #endregion

        #region Properties

        public UIElement UiElement
        {
            get { return uiElement; }
        }

        public IEnumerable<ClVariable> AllVariables
        {
            get { return variables.Values.Select(o => o.Variable); }
        }

        public ClVariable Left
        {
            get { return variables[LayoutProperty.Left].Variable; }
        }

        public ClVariable HCenter
        {
            get { return variables[LayoutProperty.HCenter].Variable; }
        }

        public ClVariable Right
        {
            get { return variables[LayoutProperty.Right].Variable; }
        }

        public ClVariable Width
        {
            get { return variables[LayoutProperty.Width].Variable; }
        }

        public ClVariable Top
        {
            get { return variables[LayoutProperty.Top].Variable; }
        }

        public ClVariable VCenter
        {
            get { return variables[LayoutProperty.VCenter].Variable; }
        }

        public ClVariable Bottom
        {
            get { return variables[LayoutProperty.Bottom].Variable; }
        }

        public ClVariable Height
        {
            get { return variables[LayoutProperty.Height].Variable; }
        }

        #endregion

        #region Methods

        public ClVariable GetVariable(LayoutProperty property)
        {
            return variables[property].Variable;
        }

        #endregion
    }
}