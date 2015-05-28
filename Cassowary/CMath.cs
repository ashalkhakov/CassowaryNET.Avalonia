/*
    Cassowary.net: an incremental constraint solver for .NET
    (http://lumumba.uhasselt.be/jo/projects/cassowary.net/)
    
    Copyright (C) 2005-2006	Jo Vermeulen (jo.vermeulen@uhasselt.be)
        
    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public License
    as published by the Free Software Foundation; either version 2.1
    of	the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.	See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA	 02111-1307, USA.
*/

using System;
using Cassowary.Exceptions;
using Cassowary.Variables;

namespace Cassowary
{
    public static class CMath
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static ClLinearExpression Plus(
            ClLinearExpression e1,
            ClLinearExpression e2)
        {
            return e1 + e2;
        }

        public static ClLinearExpression Plus(ClLinearExpression e1, double e2)
        {
            return e1 + e2;
        }

        
        public static ClLinearExpression Plus(ClLinearExpression e1, ClVariable e2)
        {
            return e1 + e2;
        }

        public static ClLinearExpression Plus(ClVariable e1, double e2)
        {
            return e1 + e2;
        }

        public static ClLinearExpression Plus(ClVariable e1, ClVariable e2)
        {
            return e1 + e2;
        }


        public static ClLinearExpression Minus(
            ClLinearExpression e1,
            ClLinearExpression e2)
        {
            return e1.Minus(e2);
        }

        public static ClLinearExpression Minus(double e1, ClLinearExpression e2)
        {
            return (new ClLinearExpression(e1)).Minus(e2);
        }

        public static ClLinearExpression Minus(ClLinearExpression e1, double e2)
        {
            return e1.Minus(new ClLinearExpression(e2));
        }

        public static ClLinearExpression Times(
            ClLinearExpression e1,
            ClLinearExpression e2)
            /*throws ExCLNonlinearExpression*/
        {
            return e1.Times(e2);
        }

        public static ClLinearExpression Times(ClLinearExpression e1, ClVariable e2)
            /*throws ExCLNonlinearExpression*/
        {
            return e1.Times(new ClLinearExpression(e2));
        }

        public static ClLinearExpression Times(ClVariable e1, ClLinearExpression e2)
            /*throws ExCLNonlinearExpression*/
        {
            return (new ClLinearExpression(e1)).Times(e2);
        }

        public static ClLinearExpression Times(ClLinearExpression e1, double e2)
            /*throws ExCLNonlinearExpression*/
        {
            return e1.Times(new ClLinearExpression(e2));
        }

        public static ClLinearExpression Times(double e1, ClLinearExpression e2)
            /*throws ExCLNonlinearExpression*/
        {
            return (new ClLinearExpression(e1)).Times(e2);
        }

        public static ClLinearExpression Times(double n, ClVariable clv)
            /*throws ExCLNonlinearExpression*/
        {
            return new ClLinearExpression(clv, n);
        }

        public static ClLinearExpression Times(ClVariable clv, double n)
            /*throws ExCLNonlinearExpression*/
        {
            return new ClLinearExpression(clv, n);
        }

        public static ClLinearExpression Divide(
            ClLinearExpression e1,
            ClLinearExpression e2)
            /*throws ExCLNonlinearExpression*/
        {
            return e1.Divide(e2);
        }

        public static ClLinearExpression Divide(
            ClLinearExpression e1,
            double v)
            /*throws ExCLNonlinearExpression*/
        {
            return e1.Divide(new ClLinearExpression(v));
        }

        public static bool Approx(double a, double b)
        {
            const double epsilon = 1.0e-8;

            if (a == 0.0)
            {
                return (Math.Abs(b) < epsilon);
            }
            if (b == 0.0)
            {
                return (Math.Abs(a) < epsilon);
            }
            
            return (Math.Abs(a - b) < Math.Abs(a)*epsilon);
        }

        #endregion
    }
}