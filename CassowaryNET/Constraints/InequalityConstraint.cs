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
using CassowaryNET.Exceptions;
using CassowaryNET.Variables;

namespace CassowaryNET.Constraints
{
    public sealed class InequalityConstraint : LinearConstraint
    {
        #region Fields

        #endregion

        #region Constructors

        #region ctor(Expression)

        public InequalityConstraint(
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }

        public InequalityConstraint(
            LinearExpression expression,
            Strength strength)
            : base(expression, strength)
        {
        }

        public InequalityConstraint(LinearExpression expression)
            : base(expression)
        {
        }

        #endregion

        #region ctor(Variable,Variable)

        private InequalityConstraint(
            AbstractVariable variable1,
            InequalityType inequalityType,
            AbstractVariable variable2,
            Strength strength,
            double weight)
            : base(GetExpression(variable1, inequalityType, variable2), strength, weight)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable1,
            InequalityType inequalityType,
            AbstractVariable variable2,
            Strength strength)
            : base(GetExpression(variable1, inequalityType, variable2), strength)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable1,
            InequalityType inequalityType,
            AbstractVariable variable2)
            : base(GetExpression(variable1, inequalityType, variable2))
        {
        }

        #endregion

        #region ctor(Variable,double)

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            double value,
            Strength strength,
            double weight)
            : base(GetExpression(variable, inequalityType, value), strength, weight)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            double value,
            Strength strength)
            : base(GetExpression(variable, inequalityType, value), strength)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            double value)
            : base(GetExpression(variable, inequalityType, value))
        {
        }

        #endregion

        #region ctor(Expression,Expression)

        public InequalityConstraint(
            LinearExpression expression1,
            InequalityType inequalityType,
            LinearExpression expression2,
            Strength strength,
            double weight)
            : base(GetExpression(expression1, inequalityType, expression2), strength, weight)
        {
        }

        public InequalityConstraint(
            LinearExpression expression1,
            InequalityType inequalityType,
            LinearExpression expression2,
            Strength strength)
            : base(GetExpression(expression1, inequalityType, expression2), strength)
        {
        }

        public InequalityConstraint(
            LinearExpression expression1,
            InequalityType inequalityType,
            LinearExpression expression2)
            : base(GetExpression(expression1, inequalityType, expression2))
        {
        }

        #endregion

        #region ctor(Variable,Expression)

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(GetExpression(variable, inequalityType, expression), strength, weight)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            LinearExpression expression,
            Strength strength)
            : base(GetExpression(variable, inequalityType, expression), strength)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            LinearExpression expression)
            : base(GetExpression(variable, inequalityType, expression))
        {
        }

        #endregion

        #region ctor(Expression,Variable)

        public InequalityConstraint(
            LinearExpression expression,
            InequalityType inequalityType,
            AbstractVariable variable,
            Strength strength,
            double weight)
            : base(GetExpression(expression, inequalityType, variable), strength, weight)
        {
        }

        public InequalityConstraint(
            LinearExpression expression,
            InequalityType inequalityType,
            AbstractVariable variable,
            Strength strength)
            : base(GetExpression(expression, inequalityType, variable), strength)
        {
        }

        public InequalityConstraint(
            LinearExpression expression,
            InequalityType inequalityType,
            AbstractVariable variable)
            : base(GetExpression(expression, inequalityType, variable))
        {
        }

        #endregion

        #endregion

        #region Properties
        
        #endregion

        #region Methods

        private static LinearExpression GetExpression(
            AbstractVariable variable1,
            InequalityType inequalityType,
            AbstractVariable variable2)
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    return variable1 - variable2;
                case InequalityType.LessThanOrEqual:
                    return variable2 - variable1;
                default:
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        private static LinearExpression GetExpression(
            AbstractVariable variable,
            InequalityType inequalityType,
            double value)
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    return variable - value;
                case InequalityType.LessThanOrEqual:
                    return value - variable;
                default:
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        private static LinearExpression GetExpression(
            LinearExpression expression1,
            InequalityType inequalityType,
            LinearExpression expression2)
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    return expression1 - expression2;
                case InequalityType.LessThanOrEqual:
                    return expression2 - expression1;
                default:
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        private static LinearExpression GetExpression(
            AbstractVariable variable,
            InequalityType inequalityType,
            LinearExpression expression)
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    return variable - expression;
                case InequalityType.LessThanOrEqual:
                    return expression - variable;
                default:
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        private static LinearExpression GetExpression(
            LinearExpression expression,
            InequalityType inequalityType,
            AbstractVariable variable)
        {
            switch (inequalityType)
            {
                case InequalityType.GreaterThanOrEqual:
                    return expression - variable;
                case InequalityType.LessThanOrEqual:
                    return variable - expression;
                default:
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        protected override LinearConstraint WithStrengthCore(Strength strength)
        {
            return WithStrength(strength);
        }

        protected override LinearConstraint WithWeightCore(double weight)
        {
            return WithWeight(weight);
        }

        public new InequalityConstraint WithStrength(Strength strength)
        {
            return new InequalityConstraint(Expression, strength, Weight);
        }

        public new InequalityConstraint WithWeight(double weight)
        {
            return new InequalityConstraint(Expression, Strength, weight);
        }

        public override string ToString()
        {
            return base.ToString() + " >= 0)";
        }

        #endregion
    }
}