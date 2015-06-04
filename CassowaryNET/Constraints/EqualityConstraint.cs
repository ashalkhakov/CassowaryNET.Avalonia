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
using CassowaryNET.Variables;

namespace CassowaryNET.Constraints
{
    public sealed class EqualityConstraint : LinearConstraint
    {
        #region Fields

        #endregion

        #region Constructors

        #region ctor(Expression)

        public EqualityConstraint(
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }

        public EqualityConstraint(
            LinearExpression expression,
            Strength strength)
            : base(expression, strength)
        {
        }

        public EqualityConstraint(LinearExpression expression)
            : base(expression)
        {
        }

        #endregion

        #region ctor(Variable,Expression)

        public EqualityConstraint(
            AbstractVariable variable,
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(GetExpression(variable, expression), strength, weight)
        {
        }

        public EqualityConstraint(
            AbstractVariable variable,
            LinearExpression expression,
            Strength strength)
            : base(GetExpression(variable, expression), strength)
        {
        }

        public EqualityConstraint(
            AbstractVariable variable,
            LinearExpression expression)
            : base(GetExpression(variable, expression))
        {
        }

        #endregion

        #region ctor(Variable,Variable)

        public EqualityConstraint(
            AbstractVariable variable1,
            AbstractVariable variable2,
            Strength strength,
            double weight)
            : base(GetExpression(variable1, variable2), strength, weight)
        {
        }

        public EqualityConstraint(
            AbstractVariable variable1,
            AbstractVariable variable2,
            Strength strength)
            : base(GetExpression(variable1, variable2), strength)
        {
        }

        public EqualityConstraint(
            AbstractVariable variable1,
            AbstractVariable variable2)
            : base(GetExpression(variable1, variable2))
        {
        }

        #endregion

        #region ctor(Variable,double)

        public EqualityConstraint(
            AbstractVariable variable,
            double value,
            Strength strength,
            double weight)
            : base(GetExpression(variable, value), strength, weight)
        {
        }

        public EqualityConstraint(
            AbstractVariable variable,
            double value,
            Strength strength)
            : base(GetExpression(variable, value), strength)
        {
        }

        public EqualityConstraint(
            AbstractVariable variable,
            double value)
            : base(GetExpression(variable, value))
        {
        }

        #endregion

        #region ctor(Expression,Variable)

        public EqualityConstraint(
            LinearExpression expression,
            AbstractVariable variable,
            Strength strength,
            double weight)
            : base(GetExpression(expression, variable), strength, weight)
        {
        }

        public EqualityConstraint(
            LinearExpression expression,
            AbstractVariable variable,
            Strength strength)
            : base(GetExpression(expression, variable), strength)
        {
        }

        public EqualityConstraint(
            LinearExpression expression,
            AbstractVariable variable)
            : base(GetExpression(expression, variable))
        {
        }

        #endregion

        #region ctor(Expression,Expression)

        public EqualityConstraint(
            LinearExpression expression1,
            LinearExpression expression2,
            Strength strength,
            double weight)
            : base(GetExpression(expression1, expression2), strength, weight)
        {
        }

        public EqualityConstraint(
            LinearExpression expression1,
            LinearExpression expression2,
            Strength strength)
            : base(GetExpression(expression1, expression2), strength)
        {
        }

        public EqualityConstraint(
            LinearExpression expression1,
            LinearExpression expression2)
            : base(GetExpression(expression1, expression2))
        {
        }

        #endregion

        #endregion

        #region Properties

        #endregion

        #region Methods

        private static LinearExpression GetExpression(
            AbstractVariable variable,
            LinearExpression expression)
        {
            return expression - variable;
        }

        private static LinearExpression GetExpression(
            LinearExpression expression,
            AbstractVariable variable)
        {
            return expression - variable;
        }

        private static LinearExpression GetExpression(
            AbstractVariable variable1,
            AbstractVariable variable2)
        {
            return variable2 - variable1;
        }

        private static LinearExpression GetExpression(
            AbstractVariable variable,
            double value)
        {
            return value - variable;
        }

        private static LinearExpression GetExpression(
            LinearExpression expression1,
            LinearExpression expression2)
        {
            return expression1 - expression2;
        }

        protected override LinearConstraint WithStrengthCore(Strength strength)
        {
            return WithStrength(strength);
        }

        protected override LinearConstraint WithWeightCore(double weight)
        {
            return WithWeight(weight);
        }

        public new EqualityConstraint WithStrength(Strength strength)
        {
            return new EqualityConstraint(Expression, strength, Weight);
        }

        public new EqualityConstraint WithWeight(double weight)
        {
            return new EqualityConstraint(Expression, Strength, weight);
        }

        public override string ToString()
        {
            return base.ToString() + " = 0)";
        }

        #endregion
    }
}