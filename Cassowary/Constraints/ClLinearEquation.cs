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
using Cassowary.Utils;
using Cassowary.Variables;

namespace Cassowary.Constraints
{
    public class ClLinearEquation : ClLinearConstraint
    {
        #region Fields

        #endregion

        #region Constructors

        public ClLinearEquation(ClLinearExpression expression)
            : this(expression, ClStrength.Required)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClStrength strength)
            : this(expression, strength, 1d)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClStrength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            ClLinearExpression expression,
            ClStrength strength,
            double weight)
            : base(expression, strength, weight)
        {
            base.Expression.AddVariable(variable, -1.0);
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            ClLinearExpression expression,
            ClStrength strength)
            : this(variable, expression, strength, 1.0)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            ClLinearExpression expression)
            : this(variable, expression, ClStrength.Required, 1.0)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            double value)
            : this(variable, value, ClStrength.Required, 1.0)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            double value,
            ClStrength strength)
            : this(variable, value, strength, 1.0)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable variable,
            double value,
            ClStrength strength,
            double weight)
            : base(new ClLinearExpression(value), strength, weight)
        {
            Expression.AddVariable(variable, -1.0);
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClAbstractVariable variable,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(expression), strength, weight)
        {
            base.Expression.AddVariable(variable, -1.0);
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClAbstractVariable variable,
            ClStrength strength)
            : this(expression, variable, strength, 1.0)
        {
        }

        public ClLinearEquation(ClLinearExpression expression, ClAbstractVariable variable)
            : this(expression, variable, ClStrength.Required, 1.0)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression1,
            ClLinearExpression expression2)
            : this(expression1, expression2, ClStrength.Required, 1.0)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression1,
            ClLinearExpression expression2,
            ClStrength strength)
            : this(expression1, expression2, strength, 1.0)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression1,
            ClLinearExpression expression2,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(expression1), strength, weight)
        {
            Expression.AddExpression(expression2, -1.0);
        }

        #endregion

        #region Properties

        #endregion
        
        #region Methods

        public override string ToString()
        {
            return base.ToString() + " = 0)";
        }

        #endregion
    }
}