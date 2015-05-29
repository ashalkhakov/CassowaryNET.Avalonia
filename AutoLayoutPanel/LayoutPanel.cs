using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Documents;
using Cassowary;
using Cassowary.Constraints;
using Cassowary.Variables;

namespace AutoLayoutPanel
{
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
            return (LayoutConstraints)element.GetValue(ConstraintsProperty);
        }

        #endregion

        #region Fields

        private readonly ClSimplexSolver solver;
        private readonly Dictionary<ClVariable, ClLinearEquation> variableConstraints;
        private readonly Dictionary<UIElement, LayoutVariableSet> elementVariables;

        #endregion

        #region Constructors

        public LayoutPanel()
        {
            solver = new ClSimplexSolver();
            variableConstraints = new Dictionary<ClVariable, ClLinearEquation>();

            elementVariables = new Dictionary<UIElement, LayoutVariableSet>();

            // Register ourselves.
            //FindClControlByUIElement(this);
        }

        #endregion

        #region Properties

        private IEnumerable<UIElement> ChildElements
        {
            get { return InternalChildren.Cast<UIElement>(); }
        }

        #endregion

        #region Methods

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // only children added in xaml are present...
            // two passes to make sure all elements are present before adding constraints.

            InitialiseElementVariables(this);
            foreach (var uiElement in ChildElements)
            {
                InitialiseElementVariables(uiElement);
            }

            // force our (X,Y) to be (0,0)
            var thisX = FindClVariable(this, LayoutProperty.Left);
            var thisY = FindClVariable(this, LayoutProperty.Top);

            solver.AddConstraint((thisX == 0d).WithStrength(ClStrength.Required));
            solver.AddConstraint((thisY == 0d).WithStrength(ClStrength.Required));

            // find xaml constraints
            foreach (var uiElement in ChildElements)
            {
                var elementConstraints = GetConstraints(uiElement);
                if (elementConstraints == null)
                    continue;

                foreach (var layoutConstraint in elementConstraints.Constraints)
                {
                    var variable = elementVariables[uiElement]
                        .GetVariable(layoutConstraint.Property);

                    var expression = layoutConstraint.Expressions
                        .Select(
                            lle => new
                            {
                                //Variable = elementVariables[lle.Source]
                                //    .GetVariable(lle.Property),
                                Variable = elementVariables[
                                    (lle.ElementName == null)
                                        ? this
                                        : ChildElements.Single(
                                            ce =>
                                                ((FrameworkElement) ce).Name ==
                                                lle.ElementName)]
                                    .GetVariable(lle.Property),
                                Multiplier = lle.Multiplier,
                            })
                        .Aggregate(
                            new ClLinearExpression(layoutConstraint.Constant),
                            (agg, o) => agg + o.Variable*o.Multiplier
                        );

                    var constraint = variable == expression;
                    solver.AddConstraint(constraint);
                }
            }
        }

        private void InitialiseElementVariables(UIElement uiElement)
        {
            var variables = elementVariables.GetOrAdd(
                uiElement,
                k => new LayoutVariableSet(k));

            solver.AddConstraint(
                (variables.Width >= 0d).WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (variables.Height >= 0d).WithStrength(ClStrength.Required));

            // X = Center - (Width/2)
            // X = Right - Width
            solver.AddConstraint(
                (variables.HCenter == variables.Left + 0.5d*variables.Width)
                    .WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (variables.Left == variables.Right - variables.Width).WithStrength(
                    ClStrength.Required));

            // Y = Middle - (Height/2)
            // Y = Bottom - Height
            solver.AddConstraint(
                (variables.VCenter == variables.Top + 0.5d*variables.Height)
                    .WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (variables.Top == variables.Bottom - variables.Height).WithStrength(
                    ClStrength.Required));
        }
        
        private ClVariable FindClVariable(
            UIElement uiElement,
            LayoutProperty property)
        {
            return elementVariables[uiElement].GetVariable(property);
        }

        //public void AddLayoutConstraint(
        //    UIElement controlFirst,
        //    LayoutProperty propertyFirst,
        //    string relatedBy,
        //    UIElement controlSecond,
        //    LayoutProperty propertySecond,
        //    double multiplier,
        //    double constant)
        //{
        //    controlFirst = FindClControlByUIElement(controlFirst);
        //    var propertyFirstVariable = FindClVariable(
        //        controlFirst,
        //        propertyFirst);

        //    var equality = relatedBy.Equals("<")
        //        ? InequalityType.LessThanOrEqual
        //        : relatedBy.Equals(">")
        //            ? InequalityType.GreaterThanOrEqual
        //            : 0;

        //    ClLinearConstraint constraint;
        //    if (controlSecond == null)
        //    {
        //        if (equality == 0)
        //            constraint = new ClLinearEquation(
        //                propertyFirstVariable,
        //                constant,
        //                ClStrength.Required);
        //        else
        //            constraint =
        //                new ClLinearInequality(
        //                    propertyFirstVariable,
        //                    equality,
        //                    constant,
        //                    ClStrength.Required);
        //    }
        //    else
        //    {
        //        controlSecond = FindClControlByUIElement(controlSecond);
        //        var propertySecondVariable =
        //            FindClVariable(controlSecond, propertySecond);

        //        if (equality == 0)
        //        {
        //            // y = m*x + c
        //            constraint = new ClLinearEquation(
        //                propertyFirstVariable,
        //                new ClLinearExpression(propertySecondVariable)*multiplier +
        //                new ClLinearExpression(constant),
        //                ClStrength.Required);
        //        }
        //        else
        //        {
        //            // y < m*x + c ||  y > m*x + c
        //            constraint = new ClLinearInequality(
        //                propertyFirstVariable,
        //                equality,
        //                new ClLinearExpression(propertySecondVariable)*multiplier +
        //                new ClLinearExpression(constant),
        //                ClStrength.Required);
        //        }
        //    }

        //    solver.AddConstraint(constraint);
        //}

        private void SetValue(
            ClVariable variable,
            double value,
            ClStrength strength)
        {
            // TODO: Find a better way then manually adding/removing constriants.

            // if it is already there, remove from the solver...
            ClLinearEquation equation;
            if (variableConstraints.TryGetValue(variable, out equation))
            {
                solver.RemoveConstraint(equation);
            }

            // ... and add a new equation
            var newEquation = (variable == value).WithStrength(strength);
            solver.AddConstraint(newEquation);
            variableConstraints[variable] = newEquation;
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
                var variables = elementVariables[child];
                child.Arrange(
                    new Rect(
                        new Point(variables.Left.Value, variables.Top.Value),
                        new Size(variables.Width.Value, variables.Height.Value)));
            }

            return finalSize;
        }

        #endregion

    }
}