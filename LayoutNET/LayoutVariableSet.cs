using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using CassowaryNET.Variables;

namespace LayoutNET
{
    public class LayoutVariableSet
    {
        #region Fields

        private readonly Control uiElement;
        private readonly IReadOnlyDictionary<LayoutProperty, LayoutVariable>
            variables;

        #endregion

        #region Constructors

        public LayoutVariableSet(Control uiElement)
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

        public Control UiElement
        {
            get { return uiElement; }
        }

        public IEnumerable<Variable> AllVariables
        {
            get { return variables.Values.Select(o => o.Variable); }
        }

        public Variable Left
        {
            get { return variables[LayoutProperty.Left].Variable; }
        }

        public Variable HCenter
        {
            get { return variables[LayoutProperty.HCenter].Variable; }
        }

        public Variable Right
        {
            get { return variables[LayoutProperty.Right].Variable; }
        }

        public Variable Width
        {
            get { return variables[LayoutProperty.Width].Variable; }
        }

        public Variable Top
        {
            get { return variables[LayoutProperty.Top].Variable; }
        }

        public Variable VCenter
        {
            get { return variables[LayoutProperty.VCenter].Variable; }
        }

        public Variable Bottom
        {
            get { return variables[LayoutProperty.Bottom].Variable; }
        }

        public Variable Height
        {
            get { return variables[LayoutProperty.Height].Variable; }
        }

        #endregion

        #region Methods

        public Variable GetVariable(LayoutProperty property)
        {
            return variables[property].Variable;
        }

        #endregion
    }
}