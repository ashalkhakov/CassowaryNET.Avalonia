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
using Cassowary.Utils;

namespace Cassowary
{
    public class ClLinearExpression : Cl, ICloneable
    {
        #region Fields

        private readonly ClDouble constant;
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
            this.constant = Cloneable.Clone(constant);
            this.terms = new Dictionary<ClAbstractVariable, ClDouble>();

            // need to unalias the ClDouble-s that we clone (do a deep clone)
            foreach (var clv in terms.Keys)
            {
                var clDouble = terms[clv];
                var clDoubleClone = Cloneable.Clone(clDouble);
                this.terms.Add(clv, clDoubleClone);
            }
        }

        #endregion

        #region Properties

        public double Constant
        {
            get { return constant.Value; }
            set { constant.Value = value; }
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

        public ClLinearExpression MultiplyMe(double x)
        {
            constant.Value = constant.Value*x;

            foreach (ClAbstractVariable clv in terms.Keys)
            {
                var cld = terms[clv];
                cld.Value = cld.Value*x;
            }

            return this;
        }

        object ICloneable.Clone()
        {
            return new ClLinearExpression(constant, terms);
        }

        public ClLinearExpression Times(double x)
        {
            var clone = Cloneable.Clone(this);
            clone.MultiplyMe(x);
            return clone;
        }

        public ClLinearExpression Times(ClLinearExpression expr)
            /*throws ExCLNonlinearExpression*/
        {
            if (IsConstant)
            {
                return expr.Times(constant.Value);
            }
            else if (!expr.IsConstant)
            {
                throw new ExClNonlinearExpression();
            }

            return Times(expr.constant.Value);
        }

        public ClLinearExpression Plus(ClLinearExpression expr)
        {
            var clone = Cloneable.Clone(this);
            clone.AddExpression(expr, 1.0);
            return clone;
        }

        public ClLinearExpression Plus(ClVariable var)
            /*throws ExCLNonlinearExpression*/
        {
            var clone = Cloneable.Clone(this);
            clone.AddVariable(var, 1.0);
            return clone;
        }

        public ClLinearExpression Minus(ClLinearExpression expr)
        {
            var clone = Cloneable.Clone(this);
            clone.AddExpression(expr, -1.0);
            return clone;
        }

        public ClLinearExpression Minus(ClVariable var)
            /*throws ExCLNonlinearExpression*/
        {
            var clone = Cloneable.Clone(this);
            clone.AddVariable(var, -1.0);
            return clone;
        }

        public ClLinearExpression Divide(double x)
            /*throws ExCLNonlinearExpression*/
        {
            if (Approx(x, 0.0))
            {
                throw new ExClNonlinearExpression();
            }

            return Times(1.0/x);
        }

        public ClLinearExpression Divide(ClLinearExpression expr)
            /*throws ExCLNonlinearExpression*/
        {
            if (!expr.IsConstant)
            {
                throw new ExClNonlinearExpression();
            }

            return Divide(expr.constant.Value);
        }

        public ClLinearExpression DivFrom(ClLinearExpression expr)
            /*throws ExCLNonlinearExpression*/
        {
            if (!IsConstant || Approx(constant.Value, 0.0))
            {
                throw new ExClNonlinearExpression();
            }

            return expr.Divide(constant.Value);
        }

        public ClLinearExpression SubtractFrom(ClLinearExpression expr)
        {
            return expr.Minus(this);
        }

        /// <summary>
        /// Add n*expr to this expression from another expression expr.
        /// Notify the solver if a variable is added or deleted from this
        /// expression.
        /// </summary>
        public ClLinearExpression AddExpression(
            ClLinearExpression expr,
            double n,
            ClAbstractVariable subject,
            ClTableau solver)
        {
            IncrementConstant(n*expr.Constant);

            foreach (var clv in expr.Terms.Keys)
            {
                double coeff = expr.Terms[clv].Value;
                AddVariable(clv, coeff*n, subject, solver);
            }

            return this;
        }

        /// <summary>
        /// Add n*expr to this expression from another expression expr.
        /// </summary>
        public ClLinearExpression AddExpression(
            ClLinearExpression expr,
            double n)
        {
            IncrementConstant(n*expr.Constant);

            foreach (var clv in expr.Terms.Keys)
            {
                double coeff = expr.Terms[clv].Value;
                AddVariable(clv, coeff*n);
            }

            return this;
        }

        public ClLinearExpression AddExpression(ClLinearExpression expr)
        {
            return AddExpression(expr, 1.0);
        }

        /// <summary>
        /// Add a term c*v to this expression.  If the expression already
        /// contains a term involving v, add c to the existing coefficient.
        /// If the new coefficient is approximately 0, delete v.
        /// </summary>
        public ClLinearExpression AddVariable(ClAbstractVariable v, double c)
        {
            // body largely duplicated below
            if (Trace)
                FnEnterPrint(string.Format("AddVariable: {0}, {1}", v, c));

            var coeff = terms.GetOrDefault(v);

            if (coeff != null)
            {
                double new_coefficient = coeff.Value + c;

                if (Approx(new_coefficient, 0.0))
                {
                    terms.Remove(v);
                }
                else
                {
                    coeff.Value = new_coefficient;
                }
            }
            else
            {
                if (!Approx(c, 0.0))
                {
                    terms.Add(v, new ClDouble(c));
                }
            }

            return this;
        }

        public ClLinearExpression AddVariable(ClAbstractVariable v)
        {
            return AddVariable(v, 1.0);
        }


        public ClLinearExpression SetVariable(ClAbstractVariable v, double c)
        {
            // Assert(c != 0.0);
            var coeff = terms.GetOrDefault(v);

            if (coeff != null)
                coeff.Value = c;
            else
                terms.Add(v, new ClDouble(c));

            return this;
        }

        /// <summary>
        /// Add a term c*v to this expression.  If the expression already
        /// contains a term involving v, add c to the existing coefficient.
        /// If the new coefficient is approximately 0, delete v.  Notify the
        /// solver if v appears or disappears from this expression.
        /// </summary>
        public ClLinearExpression AddVariable(
            ClAbstractVariable v,
            double c,
            ClAbstractVariable subject,
            ClTableau solver)
        {
            // body largely duplicated above
            if (Trace)
                FnEnterPrint(
                    string.Format("AddVariable: {0}, {1}, {2}, ...", v, c, subject));

            var coeff = terms.GetOrDefault(v);

            if (coeff != null)
            {
                double new_coefficient = coeff.Value + c;

                if (Approx(new_coefficient, 0.0))
                {
                    solver.NoteRemovedVariable(v, subject);
                    terms.Remove(v);
                }
                else
                {
                    coeff.Value = new_coefficient;
                }
            }
            else
            {
                if (!Approx(c, 0.0))
                {
                    terms.Add(v, new ClDouble(c));
                    solver.NoteAddedVariable(v, subject);
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
                throw new ExClInternalError("anyPivotableVariable called on a constant");
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
            ClAbstractVariable var,
            ClLinearExpression expr,
            ClAbstractVariable subject,
            ClTableau solver)
        {
            if (Trace)
                FnEnterPrint(
                    string.Format(
                        "CLE:SubstituteOut: {0}, {1}, {2}, ...",
                        var,
                        expr,
                        subject));
            if (Trace)
                TracePrint("this = " + this);

            double multiplier = terms[var].Value;
            terms.Remove(var);
            IncrementConstant(multiplier*expr.Constant);

            foreach (var clv in expr.Terms.Keys)
            {
                double coeff = expr.Terms[clv].Value;
                var d_old_coeff = terms.GetOrDefault(clv);

                if (d_old_coeff != null)
                {
                    double old_coeff = d_old_coeff.Value;
                    double newCoeff = old_coeff + multiplier*coeff;

                    if (Approx(newCoeff, 0.0))
                    {
                        solver.NoteRemovedVariable(clv, subject);
                        terms.Remove(clv);
                    }
                    else
                    {
                        d_old_coeff.Value = newCoeff;
                    }
                }
                else
                {
                    // did not have that variable already
                    terms.Add(clv, new ClDouble(multiplier*coeff));
                    solver.NoteAddedVariable(clv, subject);
                }
            }

            if (Trace)
                TracePrint("Now this is " + this);
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
            ClAbstractVariable old_subject,
            ClAbstractVariable new_subject)
        {
            var cld = terms.GetOrDefault(old_subject);

            if (cld != null)
                cld.Value = NewSubject(new_subject);
            else
                terms.Add(old_subject, new ClDouble(NewSubject(new_subject)));
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
            if (Trace)
                FnEnterPrint(string.Format("newSubject: {0}", subject));

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
        public double CoefficientFor(ClAbstractVariable var)
        {
            var coeff = terms.GetOrDefault(var);

            if (coeff != null)
                return coeff.Value;
            else
                return 0.0;
        }

        public void IncrementConstant(double c)
        {
            constant.Value = constant.Value + c;
        }

        public override string ToString()
        {
            var s = "";

            var e = terms.GetEnumerator();

            if (!Approx(constant.Value, 0.0) || terms.Count == 0)
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

        public new static ClLinearExpression Plus(
            ClLinearExpression e1,
            ClLinearExpression e2)
        {
            return e1.Plus(e2);
        }

        public new static ClLinearExpression Minus(
            ClLinearExpression e1,
            ClLinearExpression e2)
        {
            return e1.Minus(e2);
        }

        public new static ClLinearExpression Times(
            ClLinearExpression e1,
            ClLinearExpression e2)
            /* throws ExCLNonlinearExpression */
        {
            return e1.Times(e2);
        }

        public new static ClLinearExpression Divide(
            ClLinearExpression e1,
            ClLinearExpression e2)
            /* throws ExCLNonlinearExpression */
        {
            return e1.Divide(e2);
        }

        public static bool FEquals(
            ClLinearExpression e1,
            ClLinearExpression e2)
        {
            return e1 == e2;
        }

        #endregion

    }
}