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
    public sealed class LinearEquality : LinearConstraint
    {
        #region Fields

        #endregion

        #region Constructors

        #region ctor(Expression)

        public LinearEquality(
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }

        public LinearEquality(
            LinearExpression expression,
            Strength strength)
            : this(expression, strength, 1d)
        {
        }

        public LinearEquality(LinearExpression expression)
            : this(expression, Strength.Required)
        {
        }

        #endregion

        #region ctor(Variable,Expression)

        public LinearEquality(
            AbstractVariable variable,
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(expression - variable, strength, weight)
        {
        }

        public LinearEquality(
            AbstractVariable variable,
            LinearExpression expression,
            Strength strength)
            : this(variable, expression, strength, 1d)
        {
        }

        public LinearEquality(
            AbstractVariable variable,
            LinearExpression expression)
            : this(variable, expression, Strength.Required, 1d)
        {
        }

        #endregion

        #region ctor(Variable,double)

        public LinearEquality(
            AbstractVariable variable,
            double value)
            : this(variable, value, Strength.Required, 1d)
        {
        }

        public LinearEquality(
            AbstractVariable variable,
            double value,
            Strength strength)
            : this(variable, value, strength, 1d)
        {
        }

        public LinearEquality(
            AbstractVariable variable,
            double value,
            Strength strength,
            double weight)
            : this(
                value - variable,
                strength,
                weight)
        {
        }

        #endregion

        #region ctor(Expression,Variable)

        public LinearEquality(
            LinearExpression expression,
            AbstractVariable variable,
            Strength strength,
            double weight)
            : this(
                expression- variable,
                strength,
                weight)
        {
        }

        public LinearEquality(
            LinearExpression expression,
            AbstractVariable variable,
            Strength strength)
            : this(expression, variable, strength, 1d)
        {
        }

        public LinearEquality(
            LinearExpression expression,
            AbstractVariable variable)
            : this(expression, variable, Strength.Required, 1d)
        {
        }

        #endregion

        #region ctor(Expression,Expression)

        public LinearEquality(
            LinearExpression expression1,
            LinearExpression expression2)
            : this(expression1, expression2, Strength.Required, 1d)
        {
        }

        public LinearEquality(
            LinearExpression expression1,
            LinearExpression expression2,
            Strength strength)
            : this(expression1, expression2, strength, 1d)
        {
        }

        public LinearEquality(
            LinearExpression expression1,
            LinearExpression expression2,
            Strength strength,
            double weight)
            : this(expression1 - expression2, strength, weight)
        {
        }

        #endregion


        #endregion

        #region Properties

        #endregion
        
        #region Methods

        public LinearEquality WithStrength(Strength strength)
        {
            return new LinearEquality(Expression, strength, Weight);
        }

        public LinearEquality WithWeight(double weight)
        {
            return new LinearEquality(Expression, Strength, weight);
        }

        public override string ToString()
        {
            return base.ToString() + " = 0)";
        }

        #endregion
    }
}