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
using CassowaryNET.Constraints;

namespace CassowaryNET.Variables
{
    // Type      => Dummy | External | Pivotable | Restricted
    // Variable  => false | true     | false     | false
    // Objective => false | false    | false     | false
    // Dummy     => true  | false    | false     | true 
    // Slack     => false | false    | true      | true 

    // TODO: the subtyping / casting here is attrocious. Clean up needed.

#pragma warning disable 660,661
    // We are heavily using operator overloading here
    public abstract class AbstractVariable
#pragma warning restore 660,661
    {
        #region Fields

        private static int iVariableNumber;

        private readonly string name;

        #endregion

        #region Constructors

        // only internal constructors disallows external inheritors

        internal AbstractVariable(string name)
        {
            this.name = name;
            iVariableNumber++;
        }

        internal AbstractVariable()
            : this("v" + iVariableNumber)
        {
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return name; }
        }

        internal bool IsDummy
        {
            get { return this is DummyVariable; }
        }

        internal bool IsExternal
        {
            get { return this is Variable; }
        }

        internal bool IsPivotable
        {
            get { return this is SlackVariable; }
        }

        internal bool IsRestricted
        {
            get { return this is SlackVariable || this is DummyVariable; }
        }
        
        #endregion

        #region Methods

        public abstract override string ToString();

        #endregion

        #region Operators

        #region +

        public static LinearExpression operator +(
            AbstractVariable a,
            AbstractVariable b)
        {
            return new LinearExpression(a) + new LinearExpression(b);
        }

        public static LinearExpression operator +(
            AbstractVariable a,
            double b)
        {
            return new LinearExpression(a) + new LinearExpression(b);
        }

        public static LinearExpression operator +(
            double a,
            AbstractVariable b)
        {
            return new LinearExpression(a) + new LinearExpression(b);
        }

        #endregion

        #region -

        public static LinearExpression operator -(
            AbstractVariable a,
            AbstractVariable b)
        {
            return new LinearExpression(a) - new LinearExpression(b);
        }

        public static LinearExpression operator -(
            AbstractVariable a,
            double b)
        {
            return new LinearExpression(a) - new LinearExpression(b);
        }

        public static LinearExpression operator -(
            double a,
            AbstractVariable b)
        {
            return new LinearExpression(a) - new LinearExpression(b);
        }

        #endregion

        #region *

        public static LinearExpression operator *(
            AbstractVariable a,
            double b)
        {
            return new LinearExpression(a, b);
        }

        public static LinearExpression operator *(
            double a,
            AbstractVariable b)
        {
            return new LinearExpression(b, a);
        }

        #endregion

        #region /

        public static LinearExpression operator /(
            AbstractVariable a,
            double b)
        {
            return new LinearExpression(a) / b;
        }

        #endregion

        #region ==

        public static EqualityConstraint operator ==(
            AbstractVariable a,
            AbstractVariable b)
        {
            return new EqualityConstraint(a, b);
        }

        public static EqualityConstraint operator !=(
            AbstractVariable a,
            AbstractVariable b)
        {
            throw new NotImplementedException();
        }

        public static EqualityConstraint operator ==(
            AbstractVariable a,
            double b)
        {
            return new EqualityConstraint(a, b);
        }

        public static EqualityConstraint operator !=(
            AbstractVariable a,
            double b)
        {
            throw new NotImplementedException();
        }

        public static EqualityConstraint operator ==(
            double a,
            AbstractVariable b)
        {
            return new EqualityConstraint(a, b);
        }

        public static EqualityConstraint operator !=(
            double a,
            AbstractVariable b)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region <= and >=

        public static InequalityConstraint operator <=(
            AbstractVariable a,
            AbstractVariable b)
        {
            return new InequalityConstraint(a, InequalityType.LessThanOrEqual, b);
        }

        public static InequalityConstraint operator >=(
            AbstractVariable a,
            AbstractVariable b)
        {
            return new InequalityConstraint(a, InequalityType.GreaterThanOrEqual, b);
        }

        public static InequalityConstraint operator <=(
            AbstractVariable a,
            double b)
        {
            return new InequalityConstraint(a, InequalityType.LessThanOrEqual, b);
        }

        public static InequalityConstraint operator >=(
            AbstractVariable a,
            double b)
        {
            return new InequalityConstraint(a, InequalityType.GreaterThanOrEqual, b);
        }

        public static InequalityConstraint operator <=(
            double a,
            AbstractVariable b)
        {
            return new InequalityConstraint(a, InequalityType.LessThanOrEqual, b);
        }

        public static InequalityConstraint operator >=(
            double a,
            AbstractVariable b)
        {
            return new InequalityConstraint(a, InequalityType.GreaterThanOrEqual, b);
        }

        #endregion

        #endregion
    }
}