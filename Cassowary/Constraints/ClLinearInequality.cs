/*
  Cassowary.net: an incremental constraint solver for .NET
  (http://lumumba.uhasselt.be/jo/projects/cassowary.net/)
  
  Copyright (C) 2005-2006  Jo Vermeulen (jo.vermeulen@uhasselt.be)
  
  This program is free software; you can redistribute it and/or
  modify it under the terms of the GNU Lesser General Public License
  as published by the Free Software Foundation; either version 2.1
  of  the License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Lesser General Public License for more details.

  You should have received a copy of the GNU Lesser General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

using System;
using Cassowary.Exceptions;
using Cassowary.Utils;
using Cassowary.Variables;

namespace Cassowary.Constraints
{
    public sealed class ClLinearInequality : ClLinearConstraint
    {
        #region Fields

        #endregion

        #region Constructors

        public ClLinearInequality(ClLinearExpression expression)
            : this(expression, ClStrength.Required)
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            ClStrength strength)
            : this(expression, strength, 1d)
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            ClStrength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }

        public ClLinearInequality(
            ClVariable variable1,
            InequalityType inequalityType,
            ClVariable variable2)
            : this(variable1, inequalityType, variable2, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClVariable variable1,
            InequalityType inequalityType,
            ClVariable variable2,
            ClStrength strength)
            : this(variable1, inequalityType, variable2, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        private ClLinearInequality(
            ClVariable variable1,
            InequalityType inequalityType,
            ClVariable variable2,
            ClStrength strength,
            double weight)
            : base(new ClLinearExpression(variable2), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    Expression.MultiplyMe(-1d);
                    Expression.AddVariable(variable1);
                    break;
                case InequalityType.LessThanOrEqual:
                    Expression.AddVariable(variable1, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        public ClLinearInequality(
            ClVariable variable,
            InequalityType inequalityType,
            double value,
            ClStrength strength,
            double weight)
            : base(new ClLinearExpression(value), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    Expression.MultiplyMe(-1d);
                    Expression.AddVariable(variable);
                    break;
                case InequalityType.LessThanOrEqual:
                    Expression.AddVariable(variable, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        public ClLinearInequality(
            ClVariable variable,
            InequalityType inequalityType,
            double value,
            ClStrength strength)
            : this(variable, inequalityType, value, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClVariable variable,
            InequalityType inequalityType,
            double value)
            : this(variable, inequalityType, value, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression1,
            InequalityType inequalityType,
            ClLinearExpression expression2,
            ClStrength strength)
            : this(expression1, inequalityType, expression2, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression1,
            InequalityType inequalityType,
            ClLinearExpression expression2)
            : this(expression1, inequalityType, expression2, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClAbstractVariable variable,
            InequalityType inequalityType,
            ClLinearExpression expression,
            ClStrength strength)
            : this(variable, inequalityType, expression, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClAbstractVariable variable,
            InequalityType inequalityType,
            ClLinearExpression expression)
            : this(variable, inequalityType, expression, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            InequalityType inequalityType,
            ClAbstractVariable variable,
            ClStrength strength)
            : this(expression, inequalityType, variable, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            InequalityType inequalityType,
            ClAbstractVariable variable)
            : this(expression, inequalityType, variable, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            InequalityType inequalityType,
            ClAbstractVariable variable,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(expression), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.LessThanOrEqual:
                    base.Expression.MultiplyMe(-1d);
                    base.Expression.AddVariable(variable);
                    break;
                case InequalityType.GreaterThanOrEqual:
                    base.Expression.AddVariable(variable, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        public ClLinearInequality(
            ClAbstractVariable variable,
            InequalityType inequalityType,
            ClLinearExpression expression,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(expression), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    Expression.MultiplyMe(-1d);
                    Expression.AddVariable(variable);
                    break;
                case InequalityType.LessThanOrEqual:
                    Expression.AddVariable(variable, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        public ClLinearInequality(
            ClLinearExpression expression1,
            InequalityType inequalityType,
            ClLinearExpression expression2,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(expression2), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    Expression.MultiplyMe(-1d);
                    Expression.AddExpression(expression1);
                    break;
                case InequalityType.LessThanOrEqual:
                    Expression.AddExpression(expression1, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        #endregion

        #region Properties

        public override sealed bool IsInequality
        {
            get { return true; }
        }

        #endregion

        #region Methods

        public ClLinearInequality WithStrength(ClStrength strength)
        {
            return new ClLinearInequality(Expression, strength, Weight);
        }

        public ClLinearInequality WithWeight(double weight)
        {
            return new ClLinearInequality(Expression, Strength, weight);
        }

        public override sealed string ToString()
        {
            return base.ToString() + " >= 0)";
        }

        #endregion
    }
}