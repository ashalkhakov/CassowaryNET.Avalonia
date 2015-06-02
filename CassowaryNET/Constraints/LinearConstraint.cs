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

namespace CassowaryNET.Constraints
{
    public abstract class LinearConstraint : Constraint
    {
        #region Fields

        private readonly LinearExpression expression;

        #endregion

        #region Constructors

        protected LinearConstraint(
            LinearExpression expression,
            Strength strength,
            double weight)
            : base(strength, weight)
        {
            this.expression = expression;
        }

        #endregion

        #region Properties

        public override sealed LinearExpression Expression
        {
            get { return expression; }
        }

        #endregion

        #region Methods

        #endregion
    }
}