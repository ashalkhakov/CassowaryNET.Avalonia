namespace CassowaryNET.Constraints
{
    // really just a 'marker' class for public constraints
    public abstract class LinearConstraint : Constraint
    {
        #region Fields
        
        #endregion

        #region Constructors

        internal LinearConstraint(
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }

        internal LinearConstraint(
            LinearExpression expression,
            Strength strength)
            : base(expression, strength, 1d)
        {
        }

        internal LinearConstraint(
            LinearExpression expression)
            : base(expression, Strength.Required, 1d)
        {
        }

        #endregion

        #region Properties


        #endregion

        #region Methods

        public LinearConstraint WithStrength(Strength strength)
        {
            return WithStrengthCore(strength);
        }

        public LinearConstraint WithWeight(double weight)
        {
            return WithWeightCore(weight);
        }

        protected abstract LinearConstraint WithStrengthCore(Strength strength);
        protected abstract LinearConstraint WithWeightCore(double weight);

        #endregion
    }
}