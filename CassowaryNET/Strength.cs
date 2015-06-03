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

namespace CassowaryNET
{
    public class Strength
    {
        #region Static

        private static readonly Strength required =
            new Strength("required", new SymbolicWeight(double.PositiveInfinity, 0d, 0d));
        private static readonly Strength strong =
            new Strength("strong", new SymbolicWeight(1d, 0d, 0d));
        private static readonly Strength medium =
            new Strength("medium", new SymbolicWeight(0d, 1d, 0d));
        private static readonly Strength weak =
            new Strength("weak", new SymbolicWeight(0d, 0d, 1d));

        public static Strength Required
        {
            get { return required; }
        }

        public static Strength Strong
        {
            get { return strong; }
        }

        public static Strength Medium
        {
            get { return medium; }
        }

        public static Strength Weak
        {
            get { return weak; }
        }

        #endregion

        #region Fields

        private readonly string name;
        private readonly SymbolicWeight weight;

        #endregion

        #region Constructors

        private Strength(string name, SymbolicWeight weight)
        {
            this.name = name;
            this.weight = weight;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
        }
        
        internal SymbolicWeight Weight
        {
            get { return weight; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            if (this == Required)
                return string.Format("({0})", name);

            return string.Format("({0}:{1})", name, weight);
        }

        #endregion
    }
}