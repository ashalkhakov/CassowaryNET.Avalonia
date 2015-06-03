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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CassowaryNET
{
    internal class SymbolicWeight
    {
        #region Fields

        private readonly IReadOnlyList<double> weights;
        private readonly double value;
        
        #endregion

        #region Constructors

        public SymbolicWeight(double w1, double w2, double w3)
            : this(new[] {w1, w2, w3,})
        {
        }

        private SymbolicWeight(IEnumerable<double> weights)
            : this(weights.ToArray())
        {
        }

        private SymbolicWeight(ICollection<double> weights)
        {
            Debug.Assert(weights.Count == 3);

            this.weights = weights.ToList().AsReadOnly();
            this.value = GetValue(this.weights);
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public double Value
        {
            get { return value; }
        }

        private static double GetValue(IEnumerable<double> weights)
        {
            // e.g result = 100*weights[0] + 10*weights[1] + weights[2];

            return weights
                .Reverse()
                .Aggregate(
                    new
                    {
                        Sum = 0d,
                        Factor = 1d,
                    },
                    (acc, w) =>
                        new
                        {
                            Sum = acc.Sum + w * acc.Factor,
                            Factor = acc.Factor * 1000d,
                        },
                    acc => acc.Sum);
        }

        public override string ToString()
        {
            var weightsString = string.Join(",", weights);
            return string.Format("[{0}]", weightsString);
        }

        #endregion
    }
}