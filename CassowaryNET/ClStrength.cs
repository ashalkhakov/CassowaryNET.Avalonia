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
    public class ClStrength
    {
        #region Static

        private static readonly ClStrength required = new ClStrength(
            "<Required>",
            1000,
            1000,
            1000);

        private static readonly ClStrength strong = new ClStrength(
            "strong",
            1.0,
            0.0,
            0.0);

        private static readonly ClStrength medium = new ClStrength(
            "medium",
            0.0,
            1.0,
            0.0);

        private static readonly ClStrength weak = new ClStrength(
            "weak",
            0.0,
            0.0,
            1.0);

        public static ClStrength Required
        {
            get { return required; }
        }

        public static ClStrength Strong
        {
            get { return strong; }
        }

        public static ClStrength Medium
        {
            get { return medium; }
        }

        public static ClStrength Weak
        {
            get { return weak; }
        }

        #endregion

        #region Fields

        private readonly string name;
        private readonly ClSymbolicWeight symbolicWeight;

        #endregion

        #region Constructors

        private ClStrength(string name, ClSymbolicWeight symbolicWeight)
        {
            this.name = name;
            this.symbolicWeight = symbolicWeight;
        }

        private ClStrength(string name, double w1, double w2, double w3)
        {
            this.name = name;
            symbolicWeight = new ClSymbolicWeight(w1, w2, w3);
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
        }

        public bool IsRequired
        {
            get { return (this == Required); }
        }

        public ClSymbolicWeight SymbolicWeight
        {
            get { return symbolicWeight; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            if (IsRequired)
                return Name;
            return string.Format("{0}:{1}", Name, SymbolicWeight);
        }

        #endregion
    }
}