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
            Expressions = new List<LayoutLinearExpression>();
        }

        #endregion

        #region Properties

        public LayoutProperty Property { get; set; }
        public double Constant { get; set; }
        public List<LayoutLinearExpression> Expressions { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}