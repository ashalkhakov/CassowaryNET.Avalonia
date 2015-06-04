namespace CassowaryNET.Constraints
{
    // really just a 'marker' class for public constraints
    public abstract class LinearConstraint : Constraint
    {
        internal LinearConstraint(
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }
    }
}