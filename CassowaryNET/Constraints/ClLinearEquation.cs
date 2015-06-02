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
    public sealed class ClLinearEquation : ClLinearConstraint
    {
        #region Fields

        #endregion

        #region Constructors

        #region ctor(Expression)

        public ClLinearEquation(
            ClLinearExpression expression,
            ClStrength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClStrength strength)
            : this(expression, strength, 1d)
        {
        }

        public ClLinearEquation(ClLinearExpression expression)
            : this(expression, ClStrength.Required)
        {
        }

        #endregion

        #region ctor(Variable,Expression)

        public ClLinearEquation(
            ClAbstractVariable variable,
            ClLinearExpression expression,
            ClStrength strength,
            double weight)
            : base(expression - variable, strength, weight)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            ClLinearExpression expression,
            ClStrength strength)
            : this(variable, expression, strength, 1d)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            ClLinearExpression expression)
            : this(variable, expression, ClStrength.Required, 1d)
        {
        }

        #endregion

        #region ctor(Variable,double)

        public ClLinearEquation(
            ClAbstractVariable variable,
            double value)
            : this(variable, value, ClStrength.Required, 1d)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            double value,
            ClStrength strength)
            : this(variable, value, strength, 1d)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            double value,
            ClStrength strength,
            double weight)
            : this(
                value - variable,
                strength,
                weight)
        {
        }

        #endregion

        #region ctor(Expression,Variable)

        public ClLinearEquation(
            ClLinearExpression expression,
            ClAbstractVariable variable,
            ClStrength strength,
            double weight)
            : this(
                expression- variable,
                strength,
                weight)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClAbstractVariable variable,
            ClStrength strength)
            : this(expression, variable, strength, 1d)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClAbstractVariable variable)
            : this(expression, variable, ClStrength.Required, 1d)
        {
        }

        #endregion

        #region ctor(Expression,Expression)

        public ClLinearEquation(
            ClLinearExpression expression1,
            ClLinearExpression expression2)
            : this(expression1, expression2, ClStrength.Required, 1d)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression1,
            ClLinearExpression expression2,
            ClStrength strength)
            : this(expression1, expression2, strength, 1d)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression1,
            ClLinearExpression expression2,
            ClStrength strength,
            double weight)
            : this(expression1 - expression2, strength, weight)
        {
        }

        #endregion


        #endregion

        #region Properties

        #endregion
        
        #region Methods

        public ClLinearEquation WithStrength(ClStrength strength)
        {
            return new ClLinearEquation(Expression, strength, Weight);
        }

        public ClLinearEquation WithWeight(double weight)
        {
            return new ClLinearEquation(Expression, Strength, weight);
        }

        public override string ToString()
        {
            return base.ToString() + " = 0)";
        }

        #endregion
    }
}