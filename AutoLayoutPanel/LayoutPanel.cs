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
        private readonly List<UIElement> processedChildren;

        #endregion

        #region Constructors

        public LayoutPanel()
        {
            solver = new ClSimplexSolver();
            solver.AutoSolve = false;
            variableConstraints = new Dictionary<ClVariable, ClLinearEquation>();

            elementVariables = new Dictionary<UIElement, LayoutVariableSet>();
            processedChildren = new List<UIElement>();
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

            // force our (Left,Top) to be (0,0)
            var thisLeft = FindClVariable(this, LayoutProperty.Left);
            var thisTop = FindClVariable(this, LayoutProperty.Top);
            solver.AddConstraint((thisLeft == 0d).WithStrength(ClStrength.Required));
            solver.AddConstraint((thisTop == 0d).WithStrength(ClStrength.Required));

            // find xaml constraints
            foreach (var uiElement in ChildElements)
            {
                InitialiseElementConstraints(uiElement);
            }
        }

        private void InitialiseElementConstraints(UIElement uiElement)
        {
            processedChildren.Add(uiElement);

            var elementConstraints = GetConstraints(uiElement);
            if (elementConstraints == null)
                return;

            foreach (var layoutConstraint in elementConstraints.Constraints)
            {
                var variable = elementVariables[uiElement]
                    .GetVariable(layoutConstraint.Property);

                var expression = layoutConstraint.Expressions
                    .Select(
                        lle => new
                        {
                            Variable = GetElementVariable(lle)
                                .GetVariable(lle.Property),
                            Multiplier = lle.Multiplier,
                        })
                    .Aggregate(
                        new ClLinearExpression(layoutConstraint.Constant),
                        (agg, o) => agg + o.Variable*o.Multiplier
                    );

                var strength = GetStrength(layoutConstraint.Strength);

                var constraint = GetClLinearEquation(
                    variable,
                    expression,
                    strength,
                    layoutConstraint.Relationship);

                solver.AddConstraint(constraint);
            }
        }

        private static ClLinearConstraint GetClLinearEquation(
            ClVariable variable,
            ClLinearExpression expression,
            ClStrength strength,
            LayoutRelationship relationship)
        {
            switch (relationship)
            {
                case LayoutRelationship.EqualTo:
                    return (variable == expression).WithStrength(strength);
                case LayoutRelationship.LessThanOrEqualTo:
                    return (variable <= expression).WithStrength(strength);
                case LayoutRelationship.GreaterThanOrEqualTo:
                    return (variable >= expression).WithStrength(strength);
                default:
                    throw new ArgumentOutOfRangeException("relationship");
            }
        }

        private ClStrength GetStrength(
            LayoutConstraintStrength layoutConstraintStrength)
        {
            switch (layoutConstraintStrength)
            {
                case LayoutConstraintStrength.Weak:
                    return ClStrength.Weak;
                case LayoutConstraintStrength.Medium:
                    return ClStrength.Medium;
                case LayoutConstraintStrength.Strong:
                    return ClStrength.Strong;
                case LayoutConstraintStrength.Required:
                    return ClStrength.Required;
                default:
                    throw new ArgumentOutOfRangeException("layoutConstraintStrength");
            }
        }

        private LayoutVariableSet GetElementVariable(
            LayoutLinearExpression layoutLinearExpression)
        {
            //var uiElement = layoutLinearExpression.Source;
            var uiElement = (UIElement) FindName(layoutLinearExpression.ElementName);
            return elementVariables[uiElement];
        }

        private void InitialiseElementVariables(UIElement uiElement)
        {
            var variables = elementVariables.GetOrAdd(
                uiElement,
                k => new LayoutVariableSet(k));

            solver.AddConstraint(
                (variables.Width >= 0d)
                    .WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (variables.Height >= 0d)
                    .WithStrength(ClStrength.Required));

            solver.AddConstraint(
                (variables.Top >= 0d)
                    .WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (variables.Left >= 0d)
                    .WithStrength(ClStrength.Required));

            // HCenter == Left + 0.5*Width
            // Right == Left + Width
            solver.AddConstraint(
                (variables.HCenter == variables.Left + 0.5d * variables.Width)
                    .WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (variables.Right == variables.Left + variables.Width)
                    .WithStrength(ClStrength.Required));

            // VCenter == Top + 0.5*Height
            // Bottom = Top + Height
            solver.AddConstraint(
                (variables.VCenter == variables.Top + 0.5d * variables.Height)
                    .WithStrength(ClStrength.Required));
            solver.AddConstraint(
                (variables.Bottom == variables.Top + variables.Height)
                    .WithStrength(ClStrength.Required));
        }

        private ClVariable FindClVariable(
            UIElement uiElement,
            LayoutProperty property)
        {
            if (!elementVariables.ContainsKey(uiElement))
                return new ClVariable();

            return elementVariables[uiElement].GetVariable(property);
        }
        
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
            var addedChildren = ChildElements.Except(processedChildren).ToList();
            var removedChildren = processedChildren.Except(ChildElements);

            foreach (var child in addedChildren)
            {
                InitialiseElementVariables(child);
            }
            foreach (var child in addedChildren)
            {
                InitialiseElementConstraints(child);
            }

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