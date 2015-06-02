using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace AutoLayoutPanel
{
    [ContentProperty("Expressions")]
    public class LayoutConstraint
    {
        #region Fields

        #endregion

        #region Constructors

        public LayoutConstraint()
        {
            Constant = 0d;
            Relationship = LayoutRelationship.EqualTo;
            Strength = LayoutConstraintStrength.Required;
            Weight = 1d;
            Expressions = new List<LayoutLinearExpression>();
        }

        #endregion

        #region Properties

        public LayoutProperty Property { get; set; }
        public double Constant { get; set; }
        public LayoutRelationship Relationship { get; set; }
        public LayoutConstraintStrength Strength { get; set; }
        public double Weight { get; set; }
        public List<LayoutLinearExpression> Expressions { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}