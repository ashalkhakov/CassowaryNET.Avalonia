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
using Cassowary.Exceptions;
using Cassowary.Utils;
using Cassowary.Variables;

namespace Cassowary
{
    public class ClLinearExpression : ICloneable
    {
        #region Fields

        private ClDouble constant;
        private readonly Dictionary<ClAbstractVariable, ClDouble> terms;

        #endregion

        #region Constructors

        public ClLinearExpression(
            ClAbstractVariable variable,
            double value,
            double constant)
        {
            this.constant = new ClDouble(constant);
            terms = new Dictionary<ClAbstractVariable, ClDouble>(1);

            if (variable != null)
                terms.Add(variable, new ClDouble(value));
        }

        public ClLinearExpression(double num)
            : this(null, 0d, num)
        {
        }

        public ClLinearExpression()
            : this(0d)
        {
        }

        public ClLinearExpression(ClAbstractVariable variable, double value)
            : this(variable, value, 0d)
        {
        }

        public ClLinearExpression(ClAbstractVariable variable)
            : this(variable, 1d, 0d)
        {
        }

        /// <summary>
        /// For use by the clone method.
        /// </summary>
        private ClLinearExpression(
            ClDouble constant,
            Dictionary<ClAbstractVariable, ClDouble> terms)
        {
            this.constant = constant;
            this.terms = new Dictionary<ClAbstractVariable, ClDouble>();

            // need to unalias the ClDouble-s that we clone (do a deep clone)
            foreach (var clv in terms.Keys)
            {
                var clDouble = terms[clv];
                this.terms.Add(clv, clDouble);
            }
        }

        #endregion

        #region Properties

        public double Constant
        {
            get { return constant.Value; }
            set { constant = new ClDouble(value); }
        }

        public Dictionary<ClAbstractVariable, ClDouble> Terms
        {
            get { return terms; }
        }

        public bool IsConstant
        {
            get { return terms.Count == 0; }
        }

        #endregion

        #region Methods

        object ICloneable.Clone()
        {
            return new ClLinearExpression(constant, terms);
        }

        public void MultiplyMe(double x)
        {
            constant *= x;

            var newTerms = terms
                .Select(
                    kvp => new
                    {
                        kvp.Key,
                        Value = kvp.Value*x,
                    })
                .ToList();

            terms.Clear();
            foreach (var item in newTerms)
            {
                terms.Add(item.Key, item.Value);
            }
        }

        public ClLinearExpression Times(double x)
        {
            var newConstant = constant*x;
            var newTerms = terms.Select(
                kvp => new
                {
                    Key = kvp.Key,
                    Value = kvp.Value*x,
                })
                .ToDictionary(o => o.Key, o => o.Value);

            return new ClLinearExpression(
                newConstant,
                newTerms);
        }

        public ClLinearExpression Times(ClLinearExpression expression)
            /*throws ExCLNonlinearExpression*/
        {
            if (IsConstant)
            {
                return expression.Times(constant.Value);
            }
            else if (!expression.IsConstant)
            {
                throw new CassowaryNonLinearExpressionException();
            }

            return Times(expression.constant.Value);
        }

        public ClLinearExpression Plus(ClLinearExpression expression)
        {
            var clone = Cloneable.Clone(this);
            clone.AddExpression(expression, 1.0);
            return clone;
        }

        public ClLinearExpression Plus(ClVariable variable)
            /*throws ExCLNonlinearExpression*/
        {
            var clone = Cloneable.Clone(this);
            clone.AddVariable(variable, 1.0);
            return clone;
        }

        public ClLinearExpression Minus(ClLinearExpression expression)
        {
            var clone = Cloneable.Clone(this);
            clone.AddExpression(expression, -1.0);
            return clone;
        }

        public ClLinearExpression Minus(ClVariable variable)
            /*throws ExCLNonlinearExpression*/
        {
            var clone = Cloneable.Clone(this);
            clone.AddVariable(variable, -1.0);
            return clone;
        }

        public ClLinearExpression Divide(double x)
            /*throws ExCLNonlinearExpression*/
        {
            if (CMath.Approx(x, 0.0))
            {
                throw new CassowaryNonLinearExpressionException();
            }

            return Times(1.0/x);
        }

        public ClLinearExpression Divide(ClLinearExpression expression)
            /*throws ExCLNonlinearExpression*/
        {
            if (!expression.IsConstant)
            {
                throw new CassowaryNonLinearExpressionException();
            }

            return Divide(expression.constant.Value);
        }

        public ClLinearExpression DivFrom(ClLinearExpression expression)
            /*throws ExCLNonlinearExpression*/
        {
            if (!IsConstant || CMath.Approx(constant.Value, 0.0))
            {
                throw new CassowaryNonLinearExpressionException();
            }

            return expression.Divide(constant.Value);
        }

        public ClLinearExpression SubtractFrom(ClLinearExpression expression)
        {
            return expression.Minus(this);
        }

        /// <summary>
        /// Add n*expr to this expression from another expression expr.
        /// Notify the solver if a variable is added or deleted from this
        /// expression.
        /// </summary>
        public ClLinearExpression AddExpression(
            ClLinearExpression expression,
            double n,
            ClAbstractVariable subject,
            ClTableau solver)
        {
            IncrementConstant(n*expression.Constant);

            foreach (var clv in expression.Terms.Keys)
            {
                double coeff = expression.Terms[clv].Value;
                AddVariable(clv, coeff*n, subject, solver);
            }

            return this;
        }

        /// <summary>
        /// Add n*expr to this expression from another expression expr.
        /// </summary>
        public ClLinearExpression AddExpression(
            ClLinearExpression expression,
            double n)
        {
            IncrementConstant(n*expression.Constant);

            foreach (var clv in expression.Terms.Keys)
            {
                double coeff = expression.Terms[clv].Value;
                AddVariable(clv, coeff*n);
            }

            return this;
        }

        public ClLinearExpression AddExpression(ClLinearExpression expression)
        {
            return AddExpression(expression, 1.0);
        }

        /// <summary>
        /// Add a term c*v to this expression.  If the expression already
        /// contains a term involving v, add c to the existing coefficient.
        /// If the new coefficient is approximately 0, delete v.
        /// </summary>
        public ClLinearExpression AddVariable(ClAbstractVariable variable, double c)
        {
            var coefficient = terms.GetOrDefault(variable);

            if (coefficient != null)
            {
                double newCoefficient = coefficient.Value + c;

                if (CMath.Approx(newCoefficient, 0.0))
                {
                    terms.Remove(variable);
                }
                else
                {
                    terms[variable] = new ClDouble(newCoefficient);
                }
            }
            else
            {
                if (!CMath.Approx(c, 0.0))
                {
                    terms.Add(variable, new ClDouble(c));
                }
            }

            return this;
        }

        public ClLinearExpression AddVariable(ClAbstractVariable variable)
        {
            return AddVariable(variable, 1.0);
        }


        public ClLinearExpression SetVariable(ClAbstractVariable variable, double c)
        {
            // Assert(c != 0.0);
            var coefficient = terms.GetOrDefault(variable);

            if (coefficient != null)
            {
                terms[variable] = new ClDouble(c);
            }
            else
            {
                terms.Add(variable, new ClDouble(c));
            }

            return this;
        }

        /// <summary>
        /// Add a term c*v to this expression.  If the expression already
        /// contains a term involving v, add c to the existing coefficient.
        /// If the new coefficient is approximately 0, delete v.  Notify the
        /// solver if v appears or disappears from this expression.
        /// </summary>
        public ClLinearExpression AddVariable(
            ClAbstractVariable variable,
            double c,
            ClAbstractVariable subject,
            ClTableau solver)
        {
            // body largely duplicated above

            var coeff = terms.GetOrDefault(variable);

            if (coeff != null)
            {
                var newCoefficient = coeff + c;

                if (CMath.Approx(newCoefficient.Value, 0.0))
                {
                    solver.NoteRemovedVariable(variable, subject);
                    terms.Remove(variable);
                }
                else
                {
                    terms[variable] = newCoefficient;
                }
            }
            else
            {
                if (!CMath.Approx(c, 0.0))
                {
                    terms.Add(variable, new ClDouble(c));
                    solver.NoteAddedVariable(variable, subject);
                }
            }

            return this;
        }

        /// <summary>
        /// Return a pivotable variable in this expression.  (It is an error
        /// if this expression is constant -- signal ExCLInternalError in
        /// that case).  Return null if no pivotable variables
        /// </summary>
        public ClAbstractVariable AnyPivotableVariable()
            /*throws ExCLInternalError*/
        {
            if (IsConstant)
            {
                throw new CassowaryInternalException("anyPivotableVariable called on a constant");
            }

            foreach (ClAbstractVariable clv in terms.Keys)
            {
                if (clv.IsPivotable)
                    return clv;
            }

            // No pivotable variables, so just return null, and let the caller
            // error if needed
            return null;
        }

        /// <summary>
        /// Replace var with a symbolic expression expr that is equal to it.
        /// If a variable has been added to this expression that wasn't there
        /// before, or if a variable has been dropped from this expression
        /// because it now has a coefficient of 0, inform the solver.
        /// PRECONDITIONS:
        ///   var occurs with a non-zero coefficient in this expression.
        /// </summary>
        public void SubstituteOut(
            ClAbstractVariable variable,
            ClLinearExpression expression,
            ClAbstractVariable subject,
            ClTableau solver)
        {
            double multiplier = terms[variable].Value;
            terms.Remove(variable);
            IncrementConstant(multiplier*expression.Constant);

            foreach (var clv in expression.Terms.Keys)
            {
                var coeff = expression.Terms[clv];
                var oldCoefficient = terms.GetOrDefault(clv);

                if (oldCoefficient != null)
                {
                    var newCoefficient = oldCoefficient + multiplier * coeff;

                    if (CMath.Approx(newCoefficient.Value, 0.0))
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
                    terms.Add(clv, multiplier*coeff);
                    solver.NoteAddedVariable(clv, subject);
                }
            }
        }

        /// <summary>
        /// This linear expression currently represents the equation
        /// oldSubject=self.  Destructively modify it so that it represents
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
        public void ChangeSubject(
            ClAbstractVariable oldSubject,
            ClAbstractVariable newSubject)
        {
            var cld = terms.GetOrDefault(oldSubject);

            if (cld != null)
            {
                terms[oldSubject] = new ClDouble(NewSubject(newSubject));
            }
            else
            {
                terms.Add(oldSubject, new ClDouble(NewSubject(newSubject)));
            }
        }

        /// <summary>
        /// This linear expression currently represents the equation self=0.  Destructively modify it so 
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
        public double NewSubject(ClAbstractVariable subject)
        {
            var coeff = terms[subject];
            terms.Remove(subject);

            double reciprocal = 1.0/coeff.Value;
            MultiplyMe(-reciprocal);

            return reciprocal;
        }

        /// <summary>
        /// Return the coefficient corresponding to variable var, i.e.,
        /// the 'ci' corresponding to the 'vi' that var is:
        ///      v1*c1 + v2*c2 + .. + vn*cn + c
        /// </summary>
        public double CoefficientFor(ClAbstractVariable variable)
        {
            var coeff = terms.GetOrDefault(variable);

            if (coeff != null)
                return coeff.Value;
            else
                return 0.0;
        }

        public void IncrementConstant(double c)
        {
            constant += c;
        }

        public override string ToString()
        {
            var s = "";

            var e = terms.GetEnumerator();

            if (!CMath.Approx(constant.Value, 0.0) || terms.Count == 0)
            {
                s += constant.ToString();
            }
            else
            {
                if (terms.Count == 0)
                {
                    return s;
                }
                e.MoveNext(); // go to first element
                var clv = e.Current.Key;
                var coeff = terms[clv];
                s += string.Format("{0}*{1}", coeff, clv);
            }
            while (e.MoveNext())
            {
                var clv = e.Current.Key;
                var coeff = terms[clv];
                s += string.Format(" + {0}*{1}", coeff, clv);
            }

            return s;
        }

        #endregion
    }
}