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
    public sealed class InequalityConstraint : Constraint
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
            : this(expression, strength, 1d)
        {
        }

        public InequalityConstraint(LinearExpression expression)
            : this(expression, Strength.Required)
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
            : this(Create(variable1, inequalityType, variable2), strength, weight)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable1,
            InequalityType inequalityType,
            AbstractVariable variable2,
            Strength strength)
            : this(variable1, inequalityType, variable2, strength, 1d)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable1,
            InequalityType inequalityType,
            AbstractVariable variable2)
            : this(variable1, inequalityType, variable2, Strength.Required, 1d)
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
            : this(Create(variable, inequalityType, value), strength, weight)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            double value,
            Strength strength)
            : this(variable, inequalityType, value, strength, 1d)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            double value)
            : this(variable, inequalityType, value, Strength.Required, 1d)
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
            : base(Create(expression1, inequalityType, expression2), strength, weight)
        {
        }

        public InequalityConstraint(
            LinearExpression expression1,
            InequalityType inequalityType,
            LinearExpression expression2,
            Strength strength)
            : this(expression1, inequalityType, expression2, strength, 1d)
        {
        }

        public InequalityConstraint(
            LinearExpression expression1,
            InequalityType inequalityType,
            LinearExpression expression2)
            : this(expression1, inequalityType, expression2, Strength.Required, 1d)
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
            : this(Create(variable, inequalityType, expression), strength, weight)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            LinearExpression expression,
            Strength strength)
            : this(variable, inequalityType, expression, strength, 1d)
        {
        }

        public InequalityConstraint(
            AbstractVariable variable,
            InequalityType inequalityType,
            LinearExpression expression)
            : this(variable, inequalityType, expression, Strength.Required, 1d)
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
            : this(Create(expression, inequalityType, variable), strength, weight)
        {
        }

        public InequalityConstraint(
            LinearExpression expression,
            InequalityType inequalityType,
            AbstractVariable variable,
            Strength strength)
            : this(expression, inequalityType, variable, strength, 1d)
        {
        }

        public InequalityConstraint(
            LinearExpression expression,
            InequalityType inequalityType,
            AbstractVariable variable)
            : this(expression, inequalityType, variable, Strength.Required, 1d)
        {
        }

        #endregion

        #endregion

        #region Properties
        
        #endregion

        #region Methods

        private static LinearExpression Create(
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

        private static LinearExpression Create(
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

        private static LinearExpression Create(
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

        private static LinearExpression Create(
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

        private static LinearExpression Create(
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

        public InequalityConstraint WithStrength(Strength strength)
        {
            return new InequalityConstraint(Expression, strength, Weight);
        }

        public InequalityConstraint WithWeight(double weight)
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