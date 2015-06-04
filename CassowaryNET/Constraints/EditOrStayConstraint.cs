/*
  Cassowary.net: an incremental constraint solver for .NET
  (http://lumumba.uhasselt.be/jo/projects/cassowary.net/)
    
  Copyright (C) 2005  Jo Vermeulen (jo.vermeulen@uhasselt.be)
    
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
using CassowaryNET.Utils;
using CassowaryNET.Variables;

namespace CassowaryNET.Constraints
{
    public abstract class EditOrStayConstraint : Constraint
    {
        #region Fields

        private readonly Variable variable;

        #endregion

        #region Constructors

        protected EditOrStayConstraint(
            Variable variable,
            Strength strength,
            double weight)
            : base(GetExpression(variable), strength, weight)
        {
            AssertThat.ArgumentNotNull(() => variable);
            AssertThat.ArgumentNotNull(() => strength);

            this.variable = variable;
        }

        #endregion

        #region Properties

        public Variable Variable
        {
            get { return variable; }
        }

        #endregion

        #region Methods

        private static LinearExpression GetExpression(Variable variable)
        {
            AssertThat.ArgumentNotNull(() => variable);

            return new LinearExpression(variable, -1d, variable.Value);
        }

        public override string ToString()
        {
            // add missing bracket -> see ClConstraint#ToString(...)
            return base.ToString() + ")";
        }

        #endregion
    }
}