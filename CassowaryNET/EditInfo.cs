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
using CassowaryNET.Constraints;
using CassowaryNET.Variables;

namespace CassowaryNET
{
    /// <summary>
    /// ClEditInfo is privately-used class
    /// that just wraps a constraint, its positive and negative
    /// error variables, and its prior edit constant.
    /// It is used as values in _editVarMap, and replaces
    /// the parallel vectors of error variables and previous edit
    /// constants from the Smalltalk version of the code.
    /// </summary>
    internal class EditInfo
    {
        #region Fields

        private readonly EditConstraint constraint;
        private readonly SlackVariable plusError;
        private readonly SlackVariable minusError;
        private double constraintExpressionConstant;
        private readonly int index;

        #endregion

        #region Constructors

        public EditInfo(
            EditConstraint constraint,
            SlackVariable plusError,
            SlackVariable minusError,
            double constraintExpressionConstant,
            int index)
        {
            this.constraint = constraint;
            this.plusError = plusError;
            this.minusError = minusError;
            this.constraintExpressionConstant = constraintExpressionConstant;
            this.index = index;
        }

        #endregion

        #region Properties

        public EditConstraint Constraint
        {
            get { return constraint; }
        }

        public SlackVariable PlusError
        {
            get { return plusError; }
        }

        public SlackVariable MinusError
        {
            get { return minusError; }
        }

        public double ConstraintExpressionConstant
        {
            get { return constraintExpressionConstant; }
            set { constraintExpressionConstant = value; }
        }

        public int Index
        {
            get { return index; }
        }

        #endregion

        #region Methods
        
        #endregion
    }
}