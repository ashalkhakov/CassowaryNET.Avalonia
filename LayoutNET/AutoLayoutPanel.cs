using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CassowaryNET;
using CassowaryNET.Constraints;
using CassowaryNET.Variables;

namespace LayoutNET
{
    public class AutoLayoutPanel : Panel
    {
        // TODO: Add a dependency property for declaring constraints on the panel
        // e.g. Constraint="[Button1.Left] equalto [Button2.Right]"

        #region Dependency Properties

        // Short-hand constraints of any form
        //public static readonly DependencyProperty ChildConstraintsProperty =
        //    DependencyProperty.Register(
        //        "ChildConstraints",
        //        typeof(ChildConstraints),
        //        typeof(AutoLayoutPanel));

        //public static void SetChildConstraints(UIElement element, ChildConstraints value)
        //{
        //    element.SetValue(ChildConstraintsProperty, value);
        //}
        //public static ChildConstraints GetChildConstraints(UIElement element)
        //{
        //    return (ChildConstraints)element.GetValue(ChildConstraintsProperty);
        //}


        // Long-hand attached child constraints
        public static readonly DependencyProperty LayoutConstraintsProperty =
            DependencyProperty.RegisterAttached(
                "LayoutConstraints",
                typeof(LayoutConstraints),
                typeof(AutoLayoutPanel));

        public static void SetLayoutConstraints(UIElement element, LayoutConstraints value)
        {
            element.SetValue(LayoutConstraintsProperty, value);
        }
        public static LayoutConstraints GetLayoutConstraints(UIElement element)
        {
            return (LayoutConstraints)element.GetValue(LayoutConstraintsProperty);
        }

        // Short-hand attached child constraints
        public static readonly DependencyProperty ConstraintsProperty =
            DependencyProperty.RegisterAttached(
                "Constraints",
                typeof(string),
                typeof(AutoLayoutPanel));

        public static void SetConstraints(UIElement element, string value)
        {
            element.SetValue(ConstraintsProperty, value);
        }
        public static string GetConstraints(UIElement element)
        {
            return (string)element.GetValue(ConstraintsProperty);
        }

        #endregion

        #region Fields

        private readonly CassowarySolver solver;
        private readonly Dictionary<Variable, EqualityConstraint> variableConstraints;
        private readonly Dictionary<UIElement, LayoutVariableSet> elementVariables;
        private readonly List<UIElement> processedChildren;

        #endregion

        #region Constructors

        public AutoLayoutPanel()
        {
            solver = new CassowarySolver();
            solver.AutoSolve = false;
            variableConstraints = new Dictionary<Variable, EqualityConstraint>();

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

            InitialiseElementVariables(this);

            // force our (Left,Top) to be (0,0)
            var thisVariables = elementVariables[this];
            solver.AddConstraint(
                (thisVariables.Left == 0d)
                    .WithStrength(Strength.Required));
            solver.AddConstraint(
                (thisVariables.Top == 0d)
                    .WithStrength(Strength.Required));

            HandleChangedChildren();
        }

        private void InitialiseElementVariables(UIElement uiElement)
        {
            var variables = elementVariables.GetOrAdd(
                uiElement,
                k => new LayoutVariableSet(k));

            solver.AddConstraint(
                (variables.Width >= 0d)
                    .WithStrength(Strength.Required));
            solver.AddConstraint(
                (variables.Height >= 0d)
                    .WithStrength(Strength.Required));

            solver.AddConstraint(
                (variables.Top >= 0d)
                    .WithStrength(Strength.Required));
            solver.AddConstraint(
                (variables.Left >= 0d)
                    .WithStrength(Strength.Required));

            // HCenter == Left + 0.5*Width
            // Right == Left + Width
            solver.AddConstraint(
                (variables.HCenter == variables.Left + 0.5d * variables.Width)
                    .WithStrength(Strength.Required));
            solver.AddConstraint(
                (variables.Right == variables.Left + variables.Width)
                    .WithStrength(Strength.Required));

            // VCenter == Top + 0.5*Height
            // Bottom = Top + Height
            solver.AddConstraint(
                (variables.VCenter == variables.Top + 0.5d * variables.Height)
                    .WithStrength(Strength.Required));
            solver.AddConstraint(
                (variables.Bottom == variables.Top + variables.Height)
                    .WithStrength(Strength.Required));
        }

        private void InitialiseElementConstraints(UIElement uiElement)
        {
            processedChildren.Add(uiElement);

            var inlineConstraints = GetConstraints(uiElement);
            if (inlineConstraints == null)
                return;

            var constraints = ConstraintParser.Parse(inlineConstraints);

            //var elementConstraints = GetConstraints(uiElement);
            //if (elementConstraints == null)
            //    return;
            //constraints = elementConstraints.Constraints;

            foreach (var layoutConstraint in constraints)
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
                        new LinearExpression(layoutConstraint.Constant),
                        (agg, o) => agg + o.Variable*o.Multiplier
                    );

                var strength = GetStrength(layoutConstraint.Strength);
                var weight = layoutConstraint.Weight;
                var relationship = layoutConstraint.Relationship;

                var constraint = GetClLinearEquation(
                    variable,
                    expression,
                    strength,
                    weight,
                    relationship);

                solver.AddConstraint(constraint);
            }
        }

        private static LinearConstraint GetClLinearEquation(
            Variable variable,
            LinearExpression expression,
            Strength strength,
            double weight,
            LayoutRelationship relationship)
        {
            switch (relationship)
            {
                case LayoutRelationship.EqualTo:
                    return (variable == expression).WithStrength(strength).WithWeight(weight);
                case LayoutRelationship.LessThanOrEqualTo:
                    return (variable <= expression).WithStrength(strength).WithWeight(weight);
                case LayoutRelationship.GreaterThanOrEqualTo:
                    return (variable >= expression).WithStrength(strength).WithWeight(weight);
                default:
                    throw new ArgumentOutOfRangeException("relationship");
            }
        }

        private Strength GetStrength(
            LayoutConstraintStrength layoutConstraintStrength)
        {
            switch (layoutConstraintStrength)
            {
                case LayoutConstraintStrength.Weak:
                    return Strength.Weak;
                case LayoutConstraintStrength.Medium:
                    return Strength.Medium;
                case LayoutConstraintStrength.Strong:
                    return Strength.Strong;
                case LayoutConstraintStrength.Required:
                    return Strength.Required;
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

        private Variable FindClVariable(
            UIElement uiElement,
            LayoutProperty property)
        {
            return elementVariables[uiElement].GetVariable(property);
        }
        
        private void SetValue(
            Variable variable,
            double value,
            Strength strength)
        {
            // TODO: Find a better way then manually adding/removing constriants.

            // if it is already there, remove from the solver...
            EqualityConstraint constraint;
            if (variableConstraints.TryGetValue(variable, out constraint))
            {
                if (constraint != null)
                    solver.RemoveConstraint(constraint);
            }

            // ... and add a new equation
            var newConstraint = (variable == value).WithStrength(strength);

            try
            {
                solver.AddConstraint(newConstraint);
                variableConstraints[variable] = newConstraint;
            }
            catch //(Exception exception)
            {
                variableConstraints[variable] = null;
                throw;
            }
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

        private void HandleChangedChildren()
        {
            var addedChildren = ChildElements.Except(processedChildren).ToList();
            var removedChildren = processedChildren.Except(ChildElements).ToList();

            foreach (var child in addedChildren)
            {
                InitialiseElementVariables(child);
                processedChildren.Add(child);
            }

            foreach (var child in addedChildren)
            {
                InitialiseElementConstraints(child);
                processedChildren.Remove(child);
            }
        }

        private void SetPanelValues(Size finalSize)
        {
            var thisVariables = elementVariables[this];

            SetValue(
                thisVariables.Width,
                finalSize.Width,
                Strength.Required);
            SetValue(
                thisVariables.Height,
                finalSize.Height,
                Strength.Required);
        }

        private void SetChildValues()
        {
            foreach (var child in ChildElements)
            {
                var variables = elementVariables[child];

                SetValue(
                    variables.Width,
                    child.DesiredSize.Width,
                    Strength.Strong);
                SetValue(
                    variables.Height,
                    child.DesiredSize.Height,
                    Strength.Strong);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            HandleChangedChildren();

            SetPanelValues(finalSize);
            SetChildValues();

            solver.Solve();

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