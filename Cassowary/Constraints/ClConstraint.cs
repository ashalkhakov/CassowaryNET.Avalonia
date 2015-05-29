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

namespace Cassowary.Constraints
{
    // Type         => Edit  | Stay  | Equality | Inequality
    // IsEdit       => true  | false | false    | false
    // IsStay       => false | true  | false    | false
    // IsInequality => false | false | false    | true 

    // TODO: the subtyping / casting here is attrocious. Clean up needed.

    public abstract class ClConstraint
    {
        #region Fields

        private readonly ClStrength strength;
        private readonly double weight;

        #endregion

        #region Constructors
        
        protected ClConstraint(ClStrength strength, double weight)
        {
            this.strength = strength;
            this.weight = weight;
        }

        #endregion

        #region Properties

        public abstract ClLinearExpression Expression { get; }

        public ClStrength Strength
        {
            get { return strength; }
        }

        public double Weight
        {
            get { return weight; }
        }

        public virtual bool IsEditConstraint
        {
            get { return false; }
        }

        public virtual bool IsStayConstraint
        {
            get { return false; }
        }

        public virtual bool IsInequality
        {
            get { return false; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            // example output:
            // weak:[0,0,1] {1} (23 + -1*[update.height:23]
            return string.Format(
                "{0} {{{1}}} ({2}",
                Strength,
                Weight,
                Expression);
        }

        #endregion
    }
}