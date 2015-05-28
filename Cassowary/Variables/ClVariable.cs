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

namespace Cassowary.Variables
{
    public sealed class ClVariable : ClAbstractVariable
    {
        #region Fields

        private double value;

        #endregion

        #region Constructors

        public ClVariable()
            : this(0d)
        {
        }

        public ClVariable(double value)
            : base()
        {
            this.value = value;
        }

        public ClVariable(string name)
            : this(name, 0d)
        {
        }

        public ClVariable(string name, double value)
            : base(name)
        {
            this.value = value;
        }

        #endregion

        #region Properties

        /// <remarks>
        /// Change the value held -- should *not* use this if the variable is 
        /// in a solver -- instead use AddEditVar() and SuggestValue() interface
        /// </remarks>
        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public override bool IsDummy
        {
            get { return false; }
        }

        public override bool IsExternal
        {
            get { return true; }
        }

        public override bool IsPivotable
        {
            get { return false; }
        }

        public override bool IsRestricted
        {
            get { return false; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("[{0}:{1}]", Name, value);
        }

        #endregion
    }
}