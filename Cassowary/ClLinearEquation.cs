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

namespace Cassowary
{
    public class ClLinearEquation : ClLinearConstraint
    {
        #region Fields

        #endregion

        #region Constructors

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
            : base(expression, strength)
        {
        }

        public ClLinearEquation(ClLinearExpression expression)
            : base(expression)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable clv,
            ClLinearExpression expression,
            ClStrength strength,
            double weight)
            : base(expression, strength, weight)
        {
            base.expression.AddVariable(clv, -1.0);
        }

        public ClLinearEquation(
            ClAbstractVariable clv,
            ClLinearExpression expression,
            ClStrength strength)
            : this(clv, expression, strength, 1.0)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable clv,
            ClLinearExpression expression)
            : this(clv, expression, ClStrength.Required, 1.0)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable clv,
            double val,
            ClStrength strength,
            double weight)
            : base(new ClLinearExpression(val), strength, weight)
        {
            expression.AddVariable(clv, -1.0);
        }

        public ClLinearEquation(
            ClAbstractVariable clv,
            double val,
            ClStrength strength)
            : this(clv, val, strength, 1.0)
        {
        }

        public ClLinearEquation(
            ClAbstractVariable clv,
            double val)
            : this(clv, val, ClStrength.Required, 1.0)
        {
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClAbstractVariable clv,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(expression), strength, weight)
        {
            base.expression.AddVariable(clv, -1.0);
        }

        public ClLinearEquation(
            ClLinearExpression expression,
            ClAbstractVariable clv,
            ClStrength strength)
            : this(expression, clv, strength, 1.0)
        {
        }

        public ClLinearEquation(ClLinearExpression expression, ClAbstractVariable clv)
            : this(expression, clv, ClStrength.Required, 1.0)
        {
        }

        public ClLinearEquation(
            ClLinearExpression cle1,
            ClLinearExpression cle2,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(cle1), strength, weight)
        {
            expression.AddExpression(cle2, -1.0);
        }

        public ClLinearEquation(
            ClLinearExpression cle1,
            ClLinearExpression cle2,
            ClStrength strength)
            : this(cle1, cle2, strength, 1.0)
        {
        }

        public ClLinearEquation(
            ClLinearExpression cle1,
            ClLinearExpression cle2)
            : this(cle1, cle2, ClStrength.Required, 1.0)
        {
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