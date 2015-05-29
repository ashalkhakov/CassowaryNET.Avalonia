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
using System.Globalization;

namespace Cassowary
{
    // TODO: inline this class to System.Double?

    public class ClDouble
    {
        #region Fields

        private readonly double value;

        #endregion

        #region Constructors

        public ClDouble(double value)
        {
            this.value = value;
        }

        #endregion

        #region Properties

        public double Value
        {
            get { return value; }
        }

        public bool IsApproxZero
        {
            get { return CMath.Approx(value, 0d); }
        }

        #endregion

        #region Methods

        public override sealed string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
        
        //public static implicit operator ClDouble(double value)
        //{
        //    return new ClDouble(value);
        //}

        public static ClDouble operator +(ClDouble a, double b)
        {
            return new ClDouble(a.Value + b);
        }

        public static ClDouble operator +(double a, ClDouble b)
        {
            return new ClDouble(a + b.Value);
        }

        public static ClDouble operator +(ClDouble a, ClDouble b)
        {
            return new ClDouble(a.Value + b.Value);
        }
        public static ClDouble operator -(ClDouble a, double b)
        {
            return new ClDouble(a.Value - b);
        }

        public static ClDouble operator -(double a, ClDouble b)
        {
            return new ClDouble(a - b.Value);
        }

        public static ClDouble operator -(ClDouble a, ClDouble b)
        {
            return new ClDouble(a.Value - b.Value);
        }

        public static ClDouble operator *(ClDouble a, double b)
        {
            return new ClDouble(a.Value*b);
        }

        public static ClDouble operator *(double a, ClDouble b)
        {
            return new ClDouble(a*b.Value);
        }

        public static ClDouble operator *(ClDouble a, ClDouble b)
        {
            return new ClDouble(a.Value*b.Value);
        }

        public static ClDouble operator /(ClDouble a, double b)
        {
            return new ClDouble(a.Value/b);
        }

        public static ClDouble operator /(double a, ClDouble b)
        {
            return new ClDouble(a/b.Value);
        }

        public static ClDouble operator /(ClDouble a, ClDouble b)
        {
            return new ClDouble(a.Value/b.Value);
        }

        #endregion
    }
}