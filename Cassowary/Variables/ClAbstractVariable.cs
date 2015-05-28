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

namespace Cassowary.Variables
{
    public abstract class ClAbstractVariable
    {
        #region Fields

        private static int iVariableNumber;

        private readonly string name;

        #endregion

        #region Constructors

        protected ClAbstractVariable(string name)
        {
            this.name = name;
            iVariableNumber++;
        }

        protected ClAbstractVariable()
            : this("v" + iVariableNumber)
        {
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
        }

        public virtual bool IsDummy
        {
            get { return false; }
        }

        public abstract bool IsExternal { get; }

        public abstract bool IsPivotable { get; }

        public abstract bool IsRestricted { get; }
        
        #endregion

        #region Methods

        public abstract override string ToString();

        #endregion
    }
}