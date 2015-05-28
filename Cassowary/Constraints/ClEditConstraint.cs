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
using Cassowary.Variables;

namespace Cassowary.Constraints
{
    public class ClEditConstraint : ClEditOrStayConstraint
    {
        #region Fields
        
        #endregion

        #region Constructors

        public ClEditConstraint(ClVariable variable)
            : this(variable, ClStrength.Required)
        {
        }

        public ClEditConstraint(
            ClVariable variable,
            ClStrength strength)
            : this(variable, strength, 1d)
        {
        }

        public ClEditConstraint(
            ClVariable variable,
            ClStrength strength,
            double weight)
            : base(variable, strength, weight)
        {
        }

        #endregion

        #region Properties

        public override bool IsEditConstraint
        {
            get { return true; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "edit" + base.ToString();
        }

        #endregion
    }
}