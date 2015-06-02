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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cassowary.Utils;

namespace Cassowary
{
    public class ClSymbolicWeight : ICloneable
    {
        #region Fields

        private readonly double[] weights;

        #endregion

        #region Constructors

        public ClSymbolicWeight(double w1, double w2, double w3)
            : this(new[] {w1, w2, w3,})
        {
        }

        private ClSymbolicWeight(double[] weights)
        {
            Debug.Assert(weights.Length == 3);

            this.weights = new double[weights.Length];
            Array.Copy(weights, this.weights, weights.Length);
        }

        private ClSymbolicWeight(IEnumerable<double> weights)
            : this(weights.ToArray())
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public double AsDouble()
        {
            const double multiplier = 1000;
            double sum = 0;
            double factor = 1;

            for (int i = weights.Length - 1; i >= 0; i--)
            {
                sum += weights[i]*factor;
                factor *= multiplier;
            }

            return sum;
        }

        object ICloneable.Clone()
        {
            return new ClSymbolicWeight(weights);
        }

        public override string ToString()
        {
            return string.Format("[{0}]", string.Join(",", weights));
        }
        
        public static ClSymbolicWeight operator +(
            ClSymbolicWeight symbolicWeightA,
            ClSymbolicWeight symbolicWeightB)
        {
            var weightsA = symbolicWeightA.weights;
            var weightsB = symbolicWeightB.weights;

            var weights = weightsA.Zip(
                weightsB,
                (wA, wB) => wA + wB);

            return new ClSymbolicWeight(weights);
        }

        public static ClSymbolicWeight operator -(
            ClSymbolicWeight symbolicWeightA,
            ClSymbolicWeight symbolicWeightB)
        {
            var weightsA = symbolicWeightA.weights;
            var weightsB = symbolicWeightB.weights;

            var weights = weightsA.Zip(
                weightsB,
                (wA, wB) => wA - wB);

            return new ClSymbolicWeight(weights);
        }

        public static ClSymbolicWeight operator *(
            double value,
            ClSymbolicWeight symbolicWeight)
        {
            var weights = symbolicWeight.weights.Select(w => w * value);
            return new ClSymbolicWeight(weights);
        }

        public static ClSymbolicWeight operator *(
            ClSymbolicWeight symbolicWeight,
            double value)
        {
            return value*symbolicWeight;
        }

        public static ClSymbolicWeight operator /(
            ClSymbolicWeight symbolicWeight,
            double value)
        {
            var weights = symbolicWeight.weights.Select(w => w / value);
            return new ClSymbolicWeight(weights);
        }

        #endregion
    }
}