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
using Cassowary.Constraints;
using Cassowary.Variables;

namespace Cassowary
{
    /// <summary>
    /// ClEditInfo is privately-used class
    /// that just wraps a constraint, its positive and negative
    /// error variables, and its prior edit constant.
    /// It is used as values in _editVarMap, and replaces
    /// the parallel vectors of error variables and previous edit
    /// constants from the Smalltalk version of the code.
    /// </summary>
    internal class ClEditInfo
    {
        #region Fields

        private readonly ClConstraint constraint;
        private readonly ClSlackVariable clvEditPlus;
        private readonly ClSlackVariable clvEditMinus;
        private double prevEditConstant;
        private readonly int index;

        #endregion

        #region Constructors

        public ClEditInfo(
            ClConstraint constraint,
            ClSlackVariable eplus,
            ClSlackVariable eminus,
            double prevEditConstant,
            int index)
        {
            this.constraint = constraint;
            this.clvEditPlus = eplus;
            this.clvEditMinus = eminus;
            this.prevEditConstant = prevEditConstant;
            this.index = index;
        }

        #endregion

        #region Properties

        public int Index
        {
            get { return index; }
        }

        public ClConstraint Constraint
        {
            get { return constraint; }
        }

        public ClSlackVariable ClvEditPlus
        {
            get { return clvEditPlus; }
        }

        public ClSlackVariable ClvEditMinus
        {
            get { return clvEditMinus; }
        }

        public double PrevEditConstant
        {
            get { return prevEditConstant; }
            set { prevEditConstant = value; }
        }

        #endregion

        #region Methods
        
        #endregion
    }
}