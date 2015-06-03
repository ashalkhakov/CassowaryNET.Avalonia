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
using System.Linq;
using System.Text;
using CassowaryNET.Constraints;
using CassowaryNET.Exceptions;
using CassowaryNET.Utils;
using CassowaryNET.Variables;

namespace CassowaryNET
{
#pragma warning disable 660,661
    // We are heavily using operator overloading here
    public class LinearExpression : ICloneable
#pragma warning restore 660,661
    {
        #region Fields

        private double constant;
        private readonly Dictionary<AbstractVariable, double> terms;

        #endregion

        #region Constructors

        public LinearExpression(double constant)
        {
            this.constant = constant;
            this.terms = new Dictionary<AbstractVariable, double>();
        }

        internal LinearExpression(
            AbstractVariable variable,
            double multiplier = 1d,
            double constant = 0d)
        {
            if (Equals(variable, null))
                throw new ArgumentNullException();

            this.constant = constant;
            this.terms = new Dictionary<AbstractVariable, double>
            {
                {variable, multiplier},
            };
        }

        /// <summary>
        /// For use by the clone method.
        /// </summary>
        private LinearExpression(
            double constant,
            IDictionary<AbstractVariable, double> terms)
        {
            this.constant = constant;
            this.terms = new Dictionary<AbstractVariable, double>(
                terms);
        }

        #endregion

        #region Properties

        internal Dictionary<AbstractVariable, double> Terms
        {
            get { return terms; }
        }

        public double Constant
        {
            get { return constant; }
        }

        internal IReadOnlyCollection<AbstractVariable> Variables
        {
            get { return terms.Keys.ToList().AsReadOnly(); }
        }

        public bool IsConstant
        {
            get { return terms.Count == 0; }
        }

        #endregion

        #region Methods

        object ICloneable.Clone()
        {
            return new LinearExpression(constant, terms);
        }

        internal LinearExpression WithVariableSetTo(
            AbstractVariable variable,
            double newCoefficient)
        {
            var coefficient = newCoefficient;

            var newConstant = constant;
            var newTerms = new Dictionary<AbstractVariable, double>(terms);

            //if (!coefficient.IsApproxZero)
                newTerms[variable] = coefficient;

            return new LinearExpression(newConstant, newTerms);
        }

        internal LinearExpression WithConstantIncrementedBy(
            double increment)
        {
            return WithConstantSetTo(constant + increment);
        }

        internal LinearExpression WithConstantSetTo(
            double newConstant)
        {
            var newTerms = new Dictionary<AbstractVariable, double>(terms);

            return new LinearExpression(newConstant, newTerms);
        }
        
        /// <summary>
        /// Return a pivotable variable in this expression.  (It is an error
        /// if this expression is constant -- signal ExCLInternalError in
        /// that case).  Return null if no pivotable variables
        /// </summary>
        internal AbstractVariable GetAnyPivotableVariable()
            /*throws ExCLInternalError*/
        {
            if (IsConstant)
            {
                throw new CassowaryInternalException(
                    "anyPivotableVariable called on a constant");
            }

            return terms.Keys.FirstOrDefault(clv => clv.IsPivotable);
        }

        /// <summary>
        /// Returns a new linear expression with the given variable substituted
        /// by the given expression (which should be because the expression and
        /// variable are equal to each other).
        /// </summary>
        internal LinearExpression WithVariableSubstitutedBy(
            AbstractVariable variable,
            LinearExpression expression)
        {
            // need variable to occur with a non-zero coefficient in this expression
            var coefficient = terms[variable];
            if (MathHelper.Approx(coefficient, 0d))
                throw new CassowaryInternalException("Coefficient was zero");

            var newConstant = constant;
            var newTerms = new Dictionary<AbstractVariable, double>(terms);

            newTerms.Remove(variable);
            
            var expressionWithoutVariable = new LinearExpression(newConstant, newTerms);
            return expressionWithoutVariable + coefficient*expression;
        }

        /// <summary>
        /// Replace var with a symbolic expression expr that is equal to it.
        /// If a variable has been added to this expression that wasn't there
        /// before, or if a variable has been dropped from this expression
        /// because it now has a coefficient of 0, inform the solver.
        /// PRECONDITIONS:
        ///   var occurs with a non-zero coefficient in this expression.
        /// </summary>
        internal void SubstituteOut(
            AbstractVariable variable,
            LinearExpression expression,
            AbstractVariable subject,
            INoteVariableChanges solver)
        {
            // NOTE: doesn't inform of removal of the substituted variable...

            double multiplier = terms[variable];
            terms.Remove(variable);
            this.constant += multiplier * expression.Constant;

            foreach (var clv in expression.Terms.Keys)
            {
                var coeff = expression.Terms[clv];
                double oldCoefficient;
                var oldCoefficientFound = terms.TryGetValue(clv, out oldCoefficient);

                if (oldCoefficientFound)
                {
                    var newCoefficient = oldCoefficient + multiplier * coeff;

                    if (MathHelper.Approx(newCoefficient, 0.0))
                    {
                        solver.NoteRemovedVariable(clv, subject);
                        terms.Remove(clv);
                    }
                    else
                    {
                        terms[clv] = newCoefficient;
                    }
                }
                else
                {
                    // did not have that variable already
                    terms.Add(clv, multiplier * coeff);
                    solver.NoteAddedVariable(clv, subject);
                }
            }
        }

        /// <summary>
        /// This linear expression currently represents the equation
        /// oldSubject=self.  NON-Destructively modify it so that it represents
        /// the equation newSubject=self.
        ///
        /// Precondition: newSubject currently has a nonzero coefficient in
        /// this expression.
        ///
        /// NOTES
        ///   Suppose this expression is c + a*newSubject + a1*v1 + ... + an*vn.
        ///
        ///   Then the current equation is 
        ///       oldSubject = c + a*newSubject + a1*v1 + ... + an*vn.
        ///   The new equation will be
        ///        newSubject = -c/a + oldSubject/a - (a1/a)*v1 - ... - (an/a)*vn.
        ///   Note that the term involving newSubject has been dropped.
        /// </summary>
        internal LinearExpression WithSubjectChangedTo(
            AbstractVariable oldSubject,
            AbstractVariable newSubject)
        {
            double reciprocal;
            var expression = this.WithSubject(newSubject, out reciprocal);

            return expression.WithVariableSetTo(oldSubject, reciprocal);
        }


        /// <summary>
        /// This linear expression currently represents the equation self=0.  
        /// NON-Destructively modify it so 
        /// that subject=self represents an equivalent equation.  
        ///
        /// Precondition: subject must be one of the variables in this expression.
        /// NOTES
        ///   Suppose this expression is
        ///     c + a*subject + a1*v1 + ... + an*vn
        ///   representing 
        ///     c + a*subject + a1*v1 + ... + an*vn = 0
        /// The modified expression will be
        ///    subject = -c/a - (a1/a)*v1 - ... - (an/a)*vn
        ///   representing
        ///    subject = -c/a - (a1/a)*v1 - ... - (an/a)*vn
        ///
        /// Note that the term involving subject has been dropped.
        /// Returns the reciprocal, so changeSubject can use it, too
        /// </summary>
        internal LinearExpression WithSubject(AbstractVariable subject)
        {
            double reciprocal;
            return WithSubject(subject, out reciprocal);
        }

        internal LinearExpression WithSubject(
            AbstractVariable subject,
            out double reciprocal)
        {
            var newConstant = constant;
            var newTerms = new Dictionary<AbstractVariable, double>(terms);

            newTerms.Remove(subject);

            var coefficient = terms[subject];
            reciprocal = 1d/coefficient;

            var expressionWithoutSubject = new LinearExpression(
                newConstant,
                newTerms);
            return expressionWithoutSubject*-reciprocal;
        }

        /// <summary>
        /// Return the coefficient corresponding to variable var, i.e.,
        /// the 'ci' corresponding to the 'vi' that var is:
        ///      v1*c1 + v2*c2 + .. + vn*cn + c
        /// </summary>
        internal double CoefficientFor(AbstractVariable variable)
        {
            double coeff;
            var coeffFound = terms.TryGetValue(variable, out coeff);

            if (coeffFound)
                return coeff;
            else
                return 0.0;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (!MathHelper.Approx(constant, 0d))
            {
                builder.Append(constant);
                builder.Append(" + ");
            }

            var termsString = string.Join(
                " + ",
                terms.Select(t => string.Format("{0}*{1}", t.Value, t.Key)));

            builder.Append(termsString);
            return builder.ToString();
        }

        #endregion

        #region Operators

        private static LinearExpression Add(
            LinearExpression a,
            LinearExpression b,
            double aMultiplier,
            double bMultiplier)
        {
            var constant = aMultiplier*a.constant + bMultiplier*b.constant;

            var terms = new Dictionary<AbstractVariable, double>();

            var variables = a.Terms.Keys.Union(b.Terms.Keys);
            foreach (var variable in variables)
            {
                var bCoefficient = b.Terms.GetOrDefault(variable, 0d);
                var aCoefficient = a.Terms.GetOrDefault(variable, 0d);

                var coefficient = aMultiplier*aCoefficient + bMultiplier*bCoefficient;
                if (!MathHelper.Approx(coefficient, 0d))
                {
                    terms.Add(variable, coefficient);
                }
            }

            return new LinearExpression(constant, terms);
        }

        #region Unary

        public static LinearExpression operator -(
            LinearExpression a)
        {
            return 0d - a;
        }

        public static LinearExpression operator +(
            LinearExpression a)
        {
            return 0d + a;
        }

        #endregion

        #region +

        public static LinearExpression operator +(
            LinearExpression a,
            LinearExpression b)
        {
            return Add(a, b, 1d, 1d);
        }

        public static LinearExpression operator +(
            LinearExpression a,
            AbstractVariable b)
        {
            return a + new LinearExpression(b);
        }

        public static LinearExpression operator +(
            AbstractVariable a,
            LinearExpression b)
        {
            return new LinearExpression(a) + b;
        }

        public static LinearExpression operator +(
            LinearExpression a,
            double b)
        {
            return a + new LinearExpression(b);
        }

        public static LinearExpression operator +(
            double a,
            LinearExpression b)
        {
            return new LinearExpression(a) + b;
        }

        #endregion

        #region -


        public static LinearExpression operator -(
            LinearExpression a,
            LinearExpression b)
        {
            return Add(a, b, 1d, -1d);
        }

        public static LinearExpression operator -(
            LinearExpression a,
            AbstractVariable b)
        {
            return a - new LinearExpression(b);
        }

        public static LinearExpression operator -(
            AbstractVariable a,
            LinearExpression b)
        {
            return new LinearExpression(a) - b;
        }

        public static LinearExpression operator -(
            LinearExpression a,
            double b)
        {
            return a - new LinearExpression(b);
        }

        public static LinearExpression operator -(
            double a,
            LinearExpression b)
        {
            return new LinearExpression(a) - b;
        }

        #endregion


        private static LinearExpression Multiply(LinearExpression a, double b)
        {
            var newConstant = a.constant*b;
            var newTerms = a.terms.Select(
                kvp => new
                {
                    Key = kvp.Key,
                    Value = kvp.Value*b,
                })
                .Where(o => !MathHelper.Approx(o.Value, 0d))
                .ToDictionary(o => o.Key, o => o.Value);

            return new LinearExpression(
                newConstant,
                newTerms);
        }

        #region *

        public static LinearExpression operator *(
            LinearExpression a,
            LinearExpression b)
        {
            if (a.IsConstant)
                return a.constant*b;

            if (b.IsConstant)
                return a*b.constant;

            throw new CassowaryNonLinearExpressionException();
        }

        public static LinearExpression operator *(
            LinearExpression a,
            AbstractVariable b)
        {
            return a * new LinearExpression(b);
        }

        public static LinearExpression operator *(
            AbstractVariable a,
            LinearExpression b)
        {
            return new LinearExpression(a) * b;
        }

        public static LinearExpression operator *(
            LinearExpression a,
            double b)
        {
            return Multiply(a, b);
        }

        public static LinearExpression operator *(
            double a,
            LinearExpression b)
        {
            return Multiply(b, a);
        }

        #endregion

        #region /

        public static LinearExpression operator /(
            LinearExpression a,
            LinearExpression b)
        {
            if (!b.IsConstant) 
                throw new CassowaryNonLinearExpressionException();

            return a/b.constant;
        }

        public static LinearExpression operator /(
            AbstractVariable a,
            LinearExpression b)
        {
            return new LinearExpression(a) / b;
        }

        public static LinearExpression operator /(
            LinearExpression a,
            double b)
        {
            // cannot divide by zero
            if (MathHelper.Approx(b, 0d))
                throw new CassowaryNonLinearExpressionException();

            return a*(1d/b);
        }

        public static LinearExpression operator /(
            double a,
            LinearExpression b)
        {
            return new LinearExpression(a)/b;
        }

        #endregion

        #region ==

        public static LinearEquality operator ==(
            LinearExpression a,
            LinearExpression b)
        {
            return new LinearEquality(a, b);
        }

        public static LinearEquality operator !=(
            LinearExpression a,
            LinearExpression b)
        {
            throw new NotImplementedException();
        }

        public static LinearEquality operator ==(
            LinearExpression a,
            AbstractVariable b)
        {
            return new LinearEquality(a, b);
        }

        public static LinearEquality operator !=(
            LinearExpression a,
            AbstractVariable b)
        {
            throw new NotImplementedException();
        }

        public static LinearEquality operator ==(
            AbstractVariable a,
            LinearExpression b)
        {
            return new LinearEquality(a, b);
        }

        public static LinearEquality operator !=(
            AbstractVariable a,
            LinearExpression b)
        {
            throw new NotImplementedException();
        }

        public static LinearEquality operator ==(
            LinearExpression a,
            double b)
        {
            var bExpression = new LinearExpression(b);
            return new LinearEquality(a, bExpression);
        }

        public static LinearEquality operator !=(
            LinearExpression a,
            double b)
        {
            throw new NotImplementedException();
        }

        public static LinearEquality operator ==(
            double a,
            LinearExpression b)
        {
            var aExpression = new LinearExpression(a);
            return new LinearEquality(aExpression, b);
        }

        public static LinearEquality operator !=(
            double a,
            LinearExpression b)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region <= and >=

        public static LinearInequality operator <=(
            LinearExpression a,
            LinearExpression b)
        {
            return new LinearInequality(a, InequalityType.LessThanOrEqual, b);
        }

        public static LinearInequality operator >=(
            LinearExpression a,
            LinearExpression b)
        {
            return new LinearInequality(a, InequalityType.GreaterThanOrEqual, b);
        }

        public static LinearInequality operator <=(
            LinearExpression a,
            AbstractVariable b)
        {
            return new LinearInequality(a, InequalityType.LessThanOrEqual, b);
        }

        public static LinearInequality operator >=(
            LinearExpression a,
            AbstractVariable b)
        {
            return new LinearInequality(a, InequalityType.GreaterThanOrEqual, b);
        }

        public static LinearInequality operator <=(
            AbstractVariable a,
            LinearExpression b)
        {
            return new LinearInequality(a, InequalityType.LessThanOrEqual, b);
        }

        public static LinearInequality operator >=(
            AbstractVariable a,
            LinearExpression b)
        {
            return new LinearInequality(a, InequalityType.GreaterThanOrEqual, b);
        }

        public static LinearInequality operator <=(
            LinearExpression a,
            double b)
        {
            var bExpression = new LinearExpression(b);
            return new LinearInequality(a, InequalityType.LessThanOrEqual, bExpression);
        }

        public static LinearInequality operator >=(
            LinearExpression a,
            double b)
        {
            var bExpression = new LinearExpression(b);
            return new LinearInequality(a, InequalityType.GreaterThanOrEqual, bExpression);
        }

        public static LinearInequality operator <=(
            double a,
            LinearExpression b)
        {
            var aExpression = new LinearExpression(a);
            return new LinearInequality(aExpression, InequalityType.LessThanOrEqual, b);
        }

        public static LinearInequality operator >=(
            double a,
            LinearExpression b)
        {
            var aExpression = new LinearExpression(a);
            return new LinearInequality(aExpression, InequalityType.GreaterThanOrEqual, b);
        }

        #endregion

        #endregion
    }
}