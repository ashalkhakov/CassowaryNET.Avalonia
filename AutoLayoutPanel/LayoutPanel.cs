using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Documents;
using System.Windows.Markup;
using Cassowary;
using Cassowary.Constraints;
using Cassowary.Variables;

namespace AutoLayoutPanel
{
    //UIElement controlFirst,
    //LayoutProperty propertyFirst,
    //string relatedBy,
    //UIElement controlSecond,
    //LayoutProperty propertySecond,
    //double multiplier,
    //double constant

    // am i going to have to rewrite a lot of the ClLinearEquation/Inequality
    // ClVariable stuffs? I think I am...!

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

    public class LayoutPanel : Panel
    {
        #region Dependency Properties

        public static readonly DependencyProperty ConstraintsProperty =
            DependencyProperty.RegisterAttached(
                "ConstraintsInternal",
                typeof(LayoutConstraints),
                typeof(LayoutPanel));

        public static void SetConstraints(UIElement element, LayoutConstraints value)
        {
            element.SetValue(ConstraintsProperty, value);
        }
        public static LayoutConstraints GetConstraints(UIElement element)
        {
            var list = (LayoutConstraints)element.GetValue(ConstraintsProperty);
            //if (list == null)
            //{
            //    list = new LayoutConstraints();
            //    element.SetValue(ConstraintsProperty, list);
            //}
            return list;
        }

        #endregion

        #region Fields

        private readonly ClSimplexSolver solver;
        private readonly Dictionary<UIElement, string> controlIds;
        private readonly Dictionary<string, ClVariable> controlVariables;
        private readonly Dictionary<string, ClLinearEquation> variableConstraints;

        #endregion

        #region Constructors

        public LayoutPanel()
        {
            solver = new ClSimplexSolver();
            controlIds = new Dictionary<UIElement, string>();
            controlVariables = new Dictionary<string, ClVariable>();
            variableConstraints = new Dictionary<string, ClLinearEquation>();

            // Register ourselves.
            FindClControlByUIElement(this);

            // force our (X,Y) to be (0,0)
            var thisX = FindClVariable(this, LayoutProperty.Left);
            var thisY = FindClVariable(this, LayoutProperty.Top);

            solver.AddConstraint((thisX == 0d).WithStrength(ClStrength.Required));
            solver.AddConstraint((thisY == 0d).WithStrength(ClStrength.Required));
        }

        #endregion

        #region Properties

        private IEnumerable<UIElement> ChildElements
        {
            get { return InternalChildren.Cast<UIElement>(); }
        }

        #endregion

        #region Methods

        private string GetId(UIElement uiElement)
        {
            return controlIds[uiElement];
        }

        private UIElement FindClControlByUIElement(UIElement uiElement)
        {
            if (!controlIds.ContainsKey(uiElement))
            {
                controlIds.Add(uiElement, Guid.NewGuid().ToString());
                AddNewControl(uiElement);
            }
            return uiElement;
        }

        private void AddNewControl(UIElement uiElement)
        {
            var clWidth = FindClVariable(uiElement, LayoutProperty.Width);
            var clHeight = FindClVariable(uiElement, LayoutProperty.Height);
            var clLeft = FindClVariable(uiElement, LayoutProperty.Left);
            var clRight = FindClVariable(uiElement, LayoutProperty.Right);
            var clHCenter = FindClVariable(uiElement, LayoutProperty.HCenter);
            var clVCenter = FindClVariable(uiElement, LayoutProperty.VCenter);
            var clTop = FindClVariable(uiElement, LayoutProperty.Top);
            var clBottom = FindClVariable(uiElement, LayoutProperty.Bottom);

            // X = Center - (Width/2)
            // X = Right - Width
            solver.AddConstraint(
                (clHCenter == clLeft + 0.5d*clWidth).WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (clLeft == clRight - clWidth).WithStrength(ClStrength.Required));

            // Y = Middle - (Height/2)
            // Y = Bottom - Height
            solver.AddConstraint(
                (clVCenter == clTop + 0.5d * clHeight).WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (clTop == clBottom - clHeight).WithStrength(ClStrength.Required));
        }

        private ClVariable FindClVariable(
            UIElement uiElement,
            LayoutProperty property)
        {
            string key = GetId(uiElement) + "_" + property;
            if (!controlVariables.ContainsKey(key))
                controlVariables.Add(key, new ClVariable(key));
            return controlVariables[key];
        }

        public void AddLayoutConstraint(
            UIElement controlFirst,
            LayoutProperty propertyFirst,
            string relatedBy,
            UIElement controlSecond,
            LayoutProperty propertySecond,
            double multiplier,
            double constant)
        {
            controlFirst = FindClControlByUIElement(controlFirst);
            var propertyFirstVariable = FindClVariable(
                controlFirst,
                propertyFirst);

            var equality = relatedBy.Equals("<")
                ? InequalityType.LessThanOrEqual
                : relatedBy.Equals(">")
                    ? InequalityType.GreaterThanOrEqual
                    : 0;

            ClLinearConstraint constraint;
            if (controlSecond == null)
            {
                if (equality == 0)
                    constraint = new ClLinearEquation(
                        propertyFirstVariable,
                        constant,
                        ClStrength.Required);
                else
                    constraint =
                        new ClLinearInequality(
                            propertyFirstVariable,
                            equality,
                            constant,
                            ClStrength.Required);
            }
            else
            {
                controlSecond = FindClControlByUIElement(controlSecond);
                var propertySecondVariable =
                    FindClVariable(controlSecond, propertySecond);

                if (equality == 0)
                {
                    // y = m*x + c
                    constraint = new ClLinearEquation(
                        propertyFirstVariable,
                        new ClLinearExpression(propertySecondVariable)*multiplier +
                        new ClLinearExpression(constant),
                        ClStrength.Required);
                }
                else
                {
                    // y < m*x + c ||  y > m*x + c
                    constraint = new ClLinearInequality(
                        propertyFirstVariable,
                        equality,
                        new ClLinearExpression(propertySecondVariable)*multiplier +
                        new ClLinearExpression(constant),
                        ClStrength.Required);
                }
            }

            solver.AddConstraint(constraint);
        }
        
        private void SetValue(
            ClVariable variable,
            double value,
            ClStrength strength)
        {
            // TODO: Find a better way then manually adding/removing constriants.
            if (variableConstraints.ContainsKey(variable.Name))
            {
                var eq = variableConstraints[variable.Name];
                solver.RemoveConstraint(eq);
                variableConstraints.Remove(variable.Name);
            }

            var eq2 = new ClLinearEquation(
                variable,
                new ClLinearExpression(value),
                strength);
            solver.AddConstraint(eq2);
            variableConstraints.Add(variable.Name, eq2);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var child in ChildElements)
            {
                if (!child.IsMeasureValid)
                    child.Measure(availableSize);
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // TODO: hack to get it working for now...
            foreach (var uiElement in ChildElements)
            {
                FindClControlByUIElement(uiElement);
            }


            //foreach (var uiElement in ChildElements)
            //{
            //    var elementConstraints = GetConstraints(uiElement);

            //}

            SetValue(
                FindClVariable(this, LayoutProperty.Width),
                finalSize.Width,
                ClStrength.Required);
            SetValue(
                FindClVariable(this, LayoutProperty.Height),
                finalSize.Height,
                ClStrength.Required);

            foreach (var child in ChildElements)
            {
                SetValue(
                    FindClVariable(child, LayoutProperty.Width),
                    child.DesiredSize.Width,
                    ClStrength.Strong);
                SetValue(
                    FindClVariable(child, LayoutProperty.Height),
                    child.DesiredSize.Height,
                    ClStrength.Strong);
            }

            solver.Resolve();

            foreach (var child in ChildElements)
            {
                string childId = GetId(child);
                child.Arrange(
                    new Rect(
                        new Point(
                            controlVariables[childId + "_" + LayoutProperty.Left].Value,
                            controlVariables[childId + "_" + LayoutProperty.Top].Value),
                        new Size(
                            controlVariables[childId + "_" + LayoutProperty.Width].Value,
                            controlVariables[childId + "_" + LayoutProperty.Height].Value)));
            }
            return finalSize;
        }

        #endregion

    }
}