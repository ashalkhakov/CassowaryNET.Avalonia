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
    public class ClLinearInequality : ClLinearConstraint
    {
        #region Fields

        #endregion

        #region Constructors

        public ClLinearInequality(
            ClLinearExpression expression,
            ClStrength strength,
            double weight)
            : base(expression, strength, weight)
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            ClStrength strength)
            : base(expression, strength)
        {
        }

        public ClLinearInequality(ClLinearExpression expression)
            : base(expression)
        {
        }

        public ClLinearInequality(
            ClVariable clv1,
            InequalityType inequalityType,
            ClVariable clv2,
            ClStrength strength)
            : this(clv1, inequalityType, clv2, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClVariable clv1,
            InequalityType inequalityType,
            ClVariable clv2)
            : this(clv1, inequalityType, clv2, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        private ClLinearInequality(
            ClVariable clv1,
            InequalityType inequalityType,
            ClVariable clv2,
            ClStrength strength,
            double weight)
            : base(new ClLinearExpression(clv2), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.GEQ:
                    expression.MultiplyMe(-1d);
                    expression.AddVariable(clv1);
                    break;
                case InequalityType.LEQ:
                    expression.AddVariable(clv1, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        public ClLinearInequality(
            ClVariable clv,
            InequalityType inequalityType,
            double val,
            ClStrength strength,
            double weight)
            : base(new ClLinearExpression(val), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.GEQ:
                    expression.MultiplyMe(-1d);
                    expression.AddVariable(clv);
                    break;
                case InequalityType.LEQ:
                    expression.AddVariable(clv, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        public ClLinearInequality(
            ClVariable clv,
            InequalityType inequalityType,
            double val,
            ClStrength strength)
            : this(clv, inequalityType, val, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClVariable clv,
            InequalityType inequalityType,
            double val)
            : this(clv, inequalityType, val, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression cle1,
            InequalityType inequalityType,
            ClLinearExpression cle2,
            ClStrength strength)
            : this(cle1, inequalityType, cle2, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression cle1,
            InequalityType inequalityType,
            ClLinearExpression cle2)
            : this(cle1, inequalityType, cle2, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClAbstractVariable clv,
            InequalityType inequalityType,
            ClLinearExpression cle,
            ClStrength strength)
            : this(clv, inequalityType, cle, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClAbstractVariable clv,
            InequalityType inequalityType,
            ClLinearExpression cle)
            : this(clv, inequalityType, cle, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            InequalityType inequalityType,
            ClAbstractVariable clv,
            ClStrength strength)
            : this(expression, inequalityType, clv, strength, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            InequalityType inequalityType,
            ClAbstractVariable clv)
            : this(expression, inequalityType, clv, ClStrength.Required, 1d)
            /* throws ExClInternalError */
        {
        }

        public ClLinearInequality(
            ClLinearExpression cle1,
            InequalityType inequalityType,
            ClLinearExpression cle2,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(cle2), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.GEQ:
                    expression.MultiplyMe(-1d);
                    expression.AddExpression(cle1);
                    break;
                case InequalityType.LEQ:
                    expression.AddExpression(cle1, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        public ClLinearInequality(
            ClLinearExpression expression,
            InequalityType inequalityType,
            ClAbstractVariable clv,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(expression), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.LEQ:
                    base.expression.MultiplyMe(-1d);
                    base.expression.AddVariable(clv);
                    break;
                case InequalityType.GEQ:
                    base.expression.AddVariable(clv, -1d);
                    break;
                default:
                    // invalid operator
                    throw new CassowaryInternalException(
                        "Invalid operator in ClLinearInequality constructor");
            }
        }

        public ClLinearInequality(
            ClAbstractVariable clv,
            InequalityType inequalityType,
            ClLinearExpression cle,
            ClStrength strength,
            double weight)
            : base(Cloneable.Clone(cle), strength, weight)
            /* throws ExClInternalError */
        {
            switch (inequalityType)
            {
                case InequalityType.GEQ:
                    expression.MultiplyMe(-1d);
                    expression.AddVariable(clv);
                    break;
                case InequalityType.LEQ:
                    expression.AddVariable(clv, -1d);
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

        public override sealed string ToString()
        {
            return base.ToString() + " >= 0)";
        }

        #endregion
    }
}