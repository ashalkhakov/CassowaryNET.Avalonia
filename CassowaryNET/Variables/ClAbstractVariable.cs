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
    public abstract class ClAbstractVariable
#pragma warning restore 660,661
    {
        #region Fields

        private static int iVariableNumber;

        private readonly string name;

        #endregion

        #region Constructors

        // only internal constructors disallows external inheritors

        internal ClAbstractVariable(string name)
        {
            this.name = name;
            iVariableNumber++;
        }

        internal ClAbstractVariable()
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

        #region Operators

        #region +

        public static ClLinearExpression operator +(
            ClAbstractVariable a,
            ClAbstractVariable b)
        {
            return new ClLinearExpression(a) + new ClLinearExpression(b);
        }

        public static ClLinearExpression operator +(
            ClAbstractVariable a,
            double b)
        {
            return new ClLinearExpression(a) + new ClLinearExpression(b);
        }

        public static ClLinearExpression operator +(
            double a,
            ClAbstractVariable b)
        {
            return new ClLinearExpression(a) + new ClLinearExpression(b);
        }

        #endregion

        #region -

        public static ClLinearExpression operator -(
            ClAbstractVariable a,
            ClAbstractVariable b)
        {
            return new ClLinearExpression(a) - new ClLinearExpression(b);
        }

        public static ClLinearExpression operator -(
            ClAbstractVariable a,
            double b)
        {
            return new ClLinearExpression(a) - new ClLinearExpression(b);
        }

        public static ClLinearExpression operator -(
            double a,
            ClAbstractVariable b)
        {
            return new ClLinearExpression(a) - new ClLinearExpression(b);
        }

        #endregion

        #region *

        public static ClLinearExpression operator *(
            ClAbstractVariable a,
            double b)
        {
            return new ClLinearExpression(a, b);
        }

        public static ClLinearExpression operator *(
            double a,
            ClAbstractVariable b)
        {
            return new ClLinearExpression(b, a);
        }

        #endregion

        #region /

        public static ClLinearExpression operator /(
            ClAbstractVariable a,
            double b)
        {
            return new ClLinearExpression(a) / b;
        }

        #endregion

        #region ==

        public static ClLinearEquation operator ==(
            ClAbstractVariable a,
            ClAbstractVariable b)
        {
            var aExpression = new ClLinearExpression(a);
            var bExpression = new ClLinearExpression(b);
            return new ClLinearEquation(aExpression, bExpression);
        }

        public static ClLinearEquation operator !=(
            ClAbstractVariable a,
            ClAbstractVariable b)
        {
            throw new NotImplementedException();
        }

        public static ClLinearEquation operator ==(
            ClAbstractVariable a,
            double b)
        {
            var bExpression = new ClLinearExpression(b);
            return new ClLinearEquation(a, bExpression);
        }

        public static ClLinearEquation operator !=(
            ClAbstractVariable a,
            double b)
        {
            throw new NotImplementedException();
        }

        public static ClLinearEquation operator ==(
            double a,
            ClAbstractVariable b)
        {
            var aExpression = new ClLinearExpression(a);
            return new ClLinearEquation(aExpression, b);
        }

        public static ClLinearEquation operator !=(
            double a,
            ClAbstractVariable b)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region <= and >=

        public static ClLinearInequality operator <=(
            ClAbstractVariable a,
            ClAbstractVariable b)
        {
            return new ClLinearInequality(a, InequalityType.LessThanOrEqual, b);
        }

        public static ClLinearInequality operator >=(
            ClAbstractVariable a,
            ClAbstractVariable b)
        {
            return new ClLinearInequality(a, InequalityType.GreaterThanOrEqual, b);
        }

        public static ClLinearInequality operator <=(
            ClAbstractVariable a,
            double b)
        {
            var bExpression = new ClLinearExpression(b);
            return new ClLinearInequality(a, InequalityType.LessThanOrEqual, bExpression);
        }

        public static ClLinearInequality operator >=(
            ClAbstractVariable a,
            double b)
        {
            var bExpression = new ClLinearExpression(b);
            return new ClLinearInequality(a, InequalityType.GreaterThanOrEqual, bExpression);
        }

        public static ClLinearInequality operator <=(
            double a,
            ClAbstractVariable b)
        {
            var aExpression = new ClLinearExpression(a);
            return new ClLinearInequality(aExpression, InequalityType.LessThanOrEqual, b);
        }

        public static ClLinearInequality operator >=(
            double a,
            ClAbstractVariable b)
        {
            var aExpression = new ClLinearExpression(a);
            return new ClLinearInequality(aExpression, InequalityType.GreaterThanOrEqual, b);
        }

        #endregion

        #endregion
    }
}