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
using Cassowary.Constraints;
using Cassowary.Exceptions;
using Cassowary.Utils;
using Cassowary.Variables;

namespace Cassowary
{
    public sealed class ClSimplexSolver : ClTableau
    {
        #region Fields

        private const double Epsilon = 1e-8;

        /// <summary>
        /// The array of negative error vars for the stay constraints
        /// (need both positive and negative since they have only non-negative
        /// values).
        /// </summary>
        private readonly List<ClSlackVariable> stayMinusErrorVars;

        /// <summary>
        /// The array of positive error vars for the stay constraints
        /// (need both positive and negative since they have only non-negative
        /// values).
        /// </summary>
        private readonly List<ClSlackVariable> stayPlusErrorVars;

        /// <summary>
        /// Give error variables for a non-required constraints,
        /// maps to ClSlackVariable-s.
        /// </summary>
        /// <remarks>
        /// Map ClConstraint to Set (of ClVariable).
        /// </remarks>
        private readonly Dictionary<ClConstraint, HashSet<ClSlackVariable>> errorVariables;

        /// <summary>
        /// Return a lookup table giving the marker variable for
        /// each constraints (used when deleting a constraint).
        /// </summary>
        /// <remarks>
        /// Map ClConstraint to ClVariable.
        /// </remarks>
        private readonly Dictionary<ClConstraint, ClAbstractVariable> markerVariables;

        /// <summary>
        /// Map edit variables to ClEditInfo-s.
        /// </summary>
        /// <remarks>
        /// ClEditInfo instances contain all the information for an
        /// edit constraints (the edit plus/minus vars, the index [for old-style
        /// resolve(ArrayList...)] interface), and the previous value.
        /// (ClEditInfo replaces the parallel vectors from the Smalltalk impl.)
        /// </remarks>
        private readonly Dictionary<ClVariable, ClEditInfo> editVarMap;

        private readonly ClObjectiveVariable objective;
        private readonly Stack<int> stkCedcns;

        private bool needsSolving = false;

        #endregion

        #region Constructors

        /// <remarks>
        /// Constructor initializes the fields, and creaties the objective row.
        /// </remarks>
        public ClSimplexSolver()
        {
            AutoSolve = true;

            stayMinusErrorVars = new List<ClSlackVariable>();
            stayPlusErrorVars = new List<ClSlackVariable>();
            errorVariables = new Dictionary<ClConstraint, HashSet<ClSlackVariable>>();
            markerVariables = new Dictionary<ClConstraint, ClAbstractVariable>();

            objective = new ClObjectiveVariable("Z");

            editVarMap = new Dictionary<ClVariable, ClEditInfo>();

            var expression = new ClLinearExpression();
            Rows.Add(objective, expression);
            stkCedcns = new Stack<int>();
            stkCedcns.Push(0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Controls wether optimization and setting of external variables is done
        /// automatically or not.
        /// </summary>
        /// <remarks>
        /// By default it is done automatically and <see cref="Solve"/> never needs
        /// to be explicitly called by client code. If <see cref="AutoSolve"/> is
        /// put to false, then <see cref="Solve"/> needs to be invoked explicitly
        /// before using variables' values. 
        /// (Turning off <see cref="AutoSolve"/> while addings lots and lots
        /// of constraints [ala the AddDel test in ClTests] saved about 20 % in
        /// runtime, from 60sec to 54sec for 900 constraints, with 126 failed adds).
        /// </remarks>
        public bool AutoSolve { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Convenience function for creating a linear inequality constraint.
        /// </summary>
        public void AddLowerBound(ClAbstractVariable variable, double lower)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            var constraint = new ClLinearInequality(
                variable,
                InequalityType.GreaterThanOrEqual,
                new ClLinearExpression(lower));
            AddConstraint(constraint);
        }

        /// <summary>
        /// Convenience function for creating a linear inequality constraint.
        /// </summary>
        public void AddUpperBound(ClAbstractVariable variable, double upper)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            var constraint = new ClLinearInequality(
                variable,
                InequalityType.LessThanOrEqual,
                new ClLinearExpression(upper));
            AddConstraint(constraint);
        }

        /// <summary>
        /// Convenience function for creating a pair of linear inequality constraints.
        /// </summary>
        public void AddBounds(ClAbstractVariable variable, double lower, double upper)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddLowerBound(variable, lower);
            AddUpperBound(variable, upper);
        }

        /// <summary>
        /// Add a constraint to the solver.
        /// <param name="constraint">
        /// The constraint to be added.
        /// </param>
        /// </summary>
        public void AddConstraint(ClConstraint constraint)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            ClSlackVariable eplus;
            ClSlackVariable eminus;
            ClDouble prevEConstant;
            var expr = NewExpression(
                constraint,
                out eplus,
                out eminus,
                out prevEConstant);

            var cAddedOkDirectly = TryAddingDirectly(expr);
            if (!cAddedOkDirectly)
            {
                // could not add directly
                AddWithArtificialVariable(expr);
            }

            needsSolving = true;

            if (constraint.IsEditConstraint)
            {
                int i = editVarMap.Count;
                var cnEdit = (ClEditConstraint)constraint;

                editVarMap.Add(
                    cnEdit.Variable,
                    new ClEditInfo(
                        cnEdit,
                        eplus,
                        eminus,
                        prevEConstant.Value,
                        i));
            }

            if (AutoSolve)
            {
                Optimize(objective);
                SetExternalVariables();
            }
        }

        /// <summary>
        /// Same as AddConstraint, throws no exceptions.
        /// <returns>
        /// False if the constraint resulted in an unsolvable system, otherwise true.
        /// </returns>
        /// </summary>
        public bool AddConstraintNoException(ClConstraint cn)
        {
            try
            {
                AddConstraint(cn);
                return true;
            }
            catch (CassowaryRequiredConstraintFailureException)
            {
                return false;
            }
        }

        /// <summary>
        /// Add an edit constraint for a variable with a given strength.
        /// <param name="v">Variable to add an edit constraint to.</param>
        /// <param name="strength">Strength of the edit constraint.</param>
        /// </summary>
        public void AddEditVar(ClVariable v, ClStrength strength)
            /* throws ExClInternalError */
        {
            // we get problems here is strength == Required

            try
            {
                var cnEdit = new ClEditConstraint(v, strength);
                AddConstraint(cnEdit);
            }
            catch (CassowaryRequiredConstraintFailureException)
            {
                // should not get this
                throw new CassowaryInternalException(
                    "Required failure when adding an edit variable");
            }
        }

        /// <remarks>
        /// Add an edit constraint with strength ClStrength#Strong.
        /// </remarks>
        public void AddEditVar(ClVariable v)
        {
            /* throws ExClInternalError */
            AddEditVar(v, ClStrength.Strong);
        }

        /// <summary>
        /// Marks the start of an edit session.
        /// </summary>
        /// <remarks>
        /// BeginEdit should be called before sending Resolve()
        /// messages, after adding the appropriate edit variables.
        /// </remarks>
        public void BeginEdit()
            /* throws ExClInternalError */
        {
            // TODO: Make this return some form of IDisposable

            Debug.Assert(editVarMap.Count > 0, "_editVarMap.Count > 0");
            // may later want to do more in here
            InfeasibleRows.Clear();
            ResetStayConstants();
            stkCedcns.Push(editVarMap.Count);
        }

        /// <summary>
        /// Marks the end of an edit session.
        /// </summary>
        /// <remarks>
        /// EndEdit should be called after editing has finished for now, it
        /// just removes all edit variables.
        /// </remarks>
        public void EndEdit()
            /* throws ExClInternalError */
        {
            Debug.Assert(editVarMap.Count > 0, "_editVarMap.Count > 0");
            Resolve();

            stkCedcns.Pop();
            var n = stkCedcns.Peek();

            RemoveEditVarsTo(n);
            // may later want to do more in hore
        }

        /// <summary>
        /// Eliminates all the edit constraints that were added.
        /// </summary>
        public void RemoveAllEditVars()
            /* throws ExClInternalError */
        {
            RemoveEditVarsTo(0);
        }

        /// <summary>
        /// Remove the last added edit vars to leave only
        /// a specific number left.
        /// <param name="n">
        /// Number of edit variables to keep.
        /// </param>
        /// </summary>
        public void RemoveEditVarsTo(int n)
            /* throws ExClInternalError */
        {
            var toRemove = new List<ClVariable>();
            try
            {
                foreach (var v in editVarMap.Keys)
                {
                    var cei = editVarMap[v];
                    if (cei.Index >= n)
                    {
                        toRemove.Add(v);
                    }
                }

                foreach (var v in toRemove)
                {
                    RemoveEditVar(v);
                }

                Debug.Assert(editVarMap.Count == n, "_editVarMap.Count == n");
            }
            catch (CassowaryConstraintNotFoundException)
            {
                // should not get this
                throw new CassowaryInternalException("Constraint not found in RemoveEditVarsTo");
            }
        }

        /// <summary>
        /// Remove the edit constraint previously added.
        /// <param name="v">Variable to which the edit constraint was added before.</param>
        /// </summary>
        private void RemoveEditVar(ClVariable v)
            /* throws ExClInternalError, ExClConstraintNotFound */
        {
            var editInfo = editVarMap[v];
            var constraint = editInfo.Constraint;
            RemoveConstraint(constraint);
        }

        /// <summary>
        /// Add weak stays to the x and y parts of each point. These
        /// have increasing weights so that the solver will try to satisfy
        /// the x and y stays on the same point, rather than the x stay on
        /// one and the y stay on another.
        /// <param name="listOfPoints">
        /// List of points to add weak stay constraints for.
        /// </param>
        /// </summary>
        public void AddPointStays(IEnumerable<ClPoint> listOfPoints)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            double weight = 1.0;
            const double multiplier = 2.0;

            foreach (var p in listOfPoints)
            {
                AddPointStay(p, weight);
                weight *= multiplier;
            }
        }

        public void AddPointStay(ClVariable vx, ClVariable vy, double weight)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddStay(vx, ClStrength.Weak, weight);
            AddStay(vy, ClStrength.Weak, weight);
        }

        public void AddPointStay(
            ClVariable vx,
            ClVariable vy)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddPointStay(vx, vy, 1.0);
        }

        public void AddPointStay(ClPoint clp, double weight)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddStay(clp.X, ClStrength.Weak, weight);
            AddStay(clp.Y, ClStrength.Weak, weight);
        }

        public void AddPointStay(ClPoint clp)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddPointStay(clp, 1.0);
        }

        /// <summary>
        /// Add a stay of the given strength (default to ClStrength#Weak)
        /// of a variable to the tableau..
        /// <param name="v">
        /// Variable to add the stay constraint to.
        /// </param>
        /// </summary>
        public void AddStay(ClVariable v, ClStrength strength, double weight)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            var constraint = new ClStayConstraint(v, strength, weight);

            AddConstraint(constraint);
        }

        /// <remarks>
        /// Default to weight 1.0.
        /// </remarks>
        public void AddStay(ClVariable v, ClStrength strength)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddStay(v, strength, 1.0);
        }

        /// <remarks>
        /// Default to strength ClStrength#Weak.
        /// </remarks>
        public void AddStay(ClVariable v)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddStay(v, ClStrength.Weak, 1.0);
        }

        /// <summary>
        /// Remove a constraint from the tableau.
        /// Also remove any error variable associated with it.
        /// </summary>
        public void RemoveConstraint(ClConstraint constraint)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            needsSolving = true;

            ResetStayConstants();

            var slackVariables = errorVariables.GetOrDefault(constraint);
            if (slackVariables != null)
            {
                errorVariables.Remove(constraint);

                var objectiveRowExpression = RowExpression(objective);

                foreach (var variable in slackVariables)
                {
                    var coeff =
                        -constraint.Weight*constraint.Strength.SymbolicWeight.AsDouble();

                    var rowExpression = RowExpression(variable);
                    if (rowExpression == null)
                    {
                        objectiveRowExpression.AddVariable(
                            variable,
                            coeff,
                            objective,
                            this);
                    }
                    else // the error variable was in the basis
                    {
                        objectiveRowExpression.AddExpression(
                            rowExpression,
                            coeff,
                            objective,
                            this);
                    }
                }
            }

            var marker = markerVariables.GetOrDefault(constraint);
            if (marker == null)
                throw new CassowaryConstraintNotFoundException();

            markerVariables.Remove(constraint);
            
            if (RowExpression(marker) == null)
            {
                // not in the basis, so need to do some more work
                var markerColumn = Columns[marker];

                // must pivot...
                ClAbstractVariable exitVar = null;
                double minRatio = 0d;

                foreach (var variable in markerColumn)
                {
                    if (variable.IsRestricted)
                    {
                        var rowExpression = RowExpression(variable);
                        var coefficient = rowExpression.CoefficientFor(marker);

                        if (coefficient >= 0d) 
                            continue;

                        var ratio = -rowExpression.Constant/coefficient;
                        if (exitVar == null || ratio < minRatio)
                        {
                            minRatio = ratio;
                            exitVar = variable;
                        }
                    }
                }

                if (exitVar == null)
                {
                    // no restriced variables in markerColumn 
                    // OR all coefficients >= 0
                    foreach (var variable in markerColumn)
                    {
                        if (!variable.IsRestricted)
                            continue;

                        var rowExpression = RowExpression(variable);
                        var coefficient = rowExpression.CoefficientFor(marker);

                        var ratio = rowExpression.Constant/coefficient;
                        if (exitVar == null || ratio < minRatio)
                        {
                            minRatio = ratio;
                            exitVar = variable;
                        }
                    }
                }

                if (exitVar == null)
                {
                    if (markerColumn.Count == 0)
                    {
                        RemoveColumn(marker);
                    }
                    else
                    {
                        exitVar = markerColumn.FirstOrDefault();
                    }
                }

                if (exitVar != null)
                {
                    Pivot(marker, exitVar);
                }
            }

            if (RowExpression(marker) != null)
            {
                RemoveRow(marker);
            }

            if (slackVariables != null)
            {
                foreach (var slackVariable in slackVariables)
                {
                    // FIXME: decide wether to use equals or !=
                    if (slackVariable != marker)
                    {
                        RemoveColumn(slackVariable);
                    }
                }
            }

            if (constraint.IsStayConstraint)
            {
                if (slackVariables != null)
                {
                    for (int i = 0; i < stayPlusErrorVars.Count; i++)
                    {
                        slackVariables.Remove(stayPlusErrorVars[i]);
                        slackVariables.Remove(stayMinusErrorVars[i]);
                    }
                }
            }
            else if (constraint.IsEditConstraint)
            {
                Debug.Assert(slackVariables != null, "eVars != null");
                var editConstraint = (ClEditConstraint) constraint;
                var variable = editConstraint.Variable;
                var editInfo = editVarMap[variable];
                var clvEditMinus = editInfo.ClvEditMinus;
                RemoveColumn(clvEditMinus);
                editVarMap.Remove(variable);
            }

            if (AutoSolve)
            {
                Optimize(objective);
                SetExternalVariables();
            }
        }

        /// <summary>
        /// Re-initialize this solver from the original constraints, thus
        /// getting rid of any accumulated numerical problems 
        /// </summary>
        /// <remarks>
        /// Actually, we haven't definitely observed any such problems yet.
        /// </remarks>
        public void Reset()
            /* throws ExClInternalError */
        {
            throw new CassowaryInternalException("Reset not implemented");
        }

        /// <summary>
        /// Re-solve the current collection of constraints, given the new
        /// values for the edit variables that have already been
        /// suggested (see <see cref="SuggestValue"/> method).
        /// </summary>
        public void Resolve()
            /* throws ExClInternalError */
        {
            DualOptimize();
            SetExternalVariables();
            InfeasibleRows.Clear();
            ResetStayConstants();
        }

        /// <summary>
        /// Suggest a new value for an edit variable. 
        /// </summary>
        /// <remarks>
        /// The variable needs to be added as an edit variable and 
        /// BeginEdit() needs to be called before this is called.
        /// The tableau will not be solved completely until after Resolve()
        /// has been called.
        /// </remarks>
        public void SuggestValue(ClVariable v, double x)
            /* throws ExClError */
        {
            var cei = editVarMap[v];
            if (cei == null)
            {
                Console.Error.WriteLine(
                    "SuggestValue for variable " + v +
                    ", but var is not an edit variable\n");

                throw new CassowaryException();
            }

            ClSlackVariable clvEditPlus = cei.ClvEditPlus;
            ClSlackVariable clvEditMinus = cei.ClvEditMinus;
            double delta = x - cei.PrevEditConstant;
            cei.PrevEditConstant = x;
            DeltaEditConstant(delta, clvEditPlus, clvEditMinus);
        }

        public void Solve()
            /* throws ExClInternalError */
        {
            if (needsSolving)
            {
                Optimize(objective);
                SetExternalVariables();
            }
        }

        public void SetEditedValue(ClVariable variable, double value)
            /* throws ExClInternalError */
        {
            if (!ContainsVariable(variable))
            {
                throw new InvalidOperationException();
                //variable.Value = value;
                //return;
            }

            if (CMath.Approx(value, variable.Value)) 
                return;

            AddEditVar(variable);
            BeginEdit();
            try
            {
                SuggestValue(variable, value);
            }
            catch (CassowaryException)
            {
                // just added it above, so we shouldn't get an error
                throw new CassowaryInternalException("Error in SetEditedValue");
            }
            EndEdit();
        }

        public bool ContainsVariable(ClVariable v)
            /* throws ExClInternalError */
        {
            return ColumnsHasKey(v) || (RowExpression(v) != null);
        }

        public void AddVar(ClVariable v)
            /* throws ExClInternalError */
        {
            if (ContainsVariable(v)) 
                return;

            try
            {
                AddStay(v);
            }
            catch (CassowaryRequiredConstraintFailureException)
            {
                // cannot have a required failure, since we add w/ weak
                throw new CassowaryInternalException(
                    "Error in AddVar -- required failure is impossible");
            }
        }

        /// <summary>
        /// Returns information about the solver's internals.
        /// </summary>
        /// <remarks>
        /// Originally from Michael Noth <noth@cs.washington.edu>
        /// </remarks>
        /// <returns>
        /// String containing the information.
        /// </returns>
        public override string GetInternalInfo()
        {
            string result = base.GetInternalInfo();

            result += "\nSolver info:\n";
            result += "Stay Error Variables: ";
            result += stayPlusErrorVars.Count + stayMinusErrorVars.Count;
            result += " (" + stayPlusErrorVars.Count + " +, ";
            result += stayMinusErrorVars.Count + " -)\n";
            result += "Edit Variables: " + editVarMap.Count;
            result += "\n";

            return result;
        }

        public override string ToString()
        {
            string result = base.ToString();

            result += "\n_stayPlusErrorVars: ";
            result += stayPlusErrorVars;
            result += "\n_stayMinusErrorVars: ";
            result += stayMinusErrorVars;
            result += "\n";

            return result;
        }
        
        /// <summary>
        /// Add the constraint expr=0 to the inequality tableau using an
        /// artificial variable.
        /// </summary>
        /// <remarks>
        /// To do this, create an artificial variable av and add av=expr
        /// to the inequality tableau, then make av be 0 (raise an exception
        /// if we can't attain av=0).
        /// </remarks>
        private void AddWithArtificialVariable(ClLinearExpression expression)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            var av = new ClSlackVariable("a");
            var az = new ClObjectiveVariable("az");

            var expressionClone = Cloneable.Clone(expression);

            AddRow(az, expressionClone);
            AddRow(av, expression);

            Optimize(az);

            var azRowExpression = RowExpression(az);

            if (!CMath.Approx(azRowExpression.Constant, 0.0))
            {
                RemoveRow(az);
                RemoveColumn(av);
                throw new CassowaryRequiredConstraintFailureException();
            }

            // see if av is a basic variable
            var avRowExpression = RowExpression(av);

            if (avRowExpression != null)
            {
                // find another variable in this row and pivot,
                // so that av becomes parametric
                if (avRowExpression.IsConstant)
                {
                    // if there isn't another variable in the row
                    // then the tableau contains the equation av=0 --
                    // just delete av's row
                    RemoveRow(av);
                    RemoveRow(az);
                    return;
                }

                var entryVar = avRowExpression.GetAnyPivotableVariable();
                Pivot(entryVar, av);
            }

            Debug.Assert(RowExpression(av) == null, "RowExpression(av) == null)");

            RemoveColumn(av);
            RemoveRow(az);
        }

        /// <summary>
        /// Try to add expr directly to the tableau without creating an
        /// artificial variable.
        /// </summary>
        /// <remarks>
        /// We are trying to add the constraint expr=0 to the appropriate
        /// tableau.
        /// </remarks>
        /// <returns>
        /// True if successful and false if not.
        /// </returns>
        private bool TryAddingDirectly(ClLinearExpression expression)
            /* throws ExClRequiredFailure */
        {
            var subject = ChooseSubject(expression);
            if (subject == null)
            {
                return false;
            }

            expression.NewSubject(subject);
            if (ColumnsHasKey(subject))
            {
                SubstituteOut(subject, expression);
            }

            AddRow(subject, expression);

            return true; // succesfully added directly
        }

        /// <summary>
        /// Try to choose a subject (a variable to become basic) from
        /// among the current variables in expr.
        /// </summary>
        /// <remarks>
        /// We are trying to add the constraint expr=0 to the tableaux.
        /// If expr constains any unrestricted variables, then we must choose
        /// an unrestricted variable as the subject. Also if the subject is
        /// new to the solver, we won't have to do any substitutions, so we
        /// prefer new variables to ones that are currently noted as parametric.
        /// If expr contains only restricted variables, if there is a restricted
        /// variable with a negative coefficient that is new to the solver we can
        /// make that the subject. Otherwise we can't find a subject, so return nil.
        /// (In this last case we have to add an artificial variable and use that
        /// variable as the subject -- this is done outside this method though.)
        /// </remarks>
        private ClAbstractVariable ChooseSubject(ClLinearExpression expr)
            /* ExClRequiredFailure */
        {
            ClAbstractVariable subject = null; // the current best subject, if any

            bool foundUnrestricted = false;
            bool foundNewRestricted = false;

            var terms = expr.Terms;

            foreach (var v in terms.Keys)
            {
                double c = terms[v].Value;

                if (foundUnrestricted)
                {
                    if (!v.IsRestricted)
                    {
                        if (!ColumnsHasKey(v))
                            return v;
                    }
                }
                else
                {
                    // we haven't found an restricted variable yet
                    if (v.IsRestricted)
                    {
                        if (!foundNewRestricted && !v.IsDummy && c < 0.0)
                        {
                            var col = Columns.GetOrDefault(v);

                            if (col == null ||
                                (col.Count == 1 && ColumnsHasKey(objective)))
                            {
                                subject = v;
                                foundNewRestricted = true;
                            }
                        }
                    }
                    else
                    {
                        subject = v;
                        foundUnrestricted = true;
                    }
                }
            }

            if (subject != null)
                return subject;

            double coeff = 0.0;

            foreach (var v in terms.Keys)
            {
                double c = terms[v].Value;

                if (!v.IsDummy)
                    return null; // nope, no luck

                if (!ColumnsHasKey(v))
                {
                    subject = v;
                    coeff = c;
                }
            }

            if (!CMath.Approx(expr.Constant, 0.0))
            {
                throw new CassowaryRequiredConstraintFailureException();
            }
            if (coeff > 0.0)
            {
                expr.MultiplyMe(-1);
            }

            return subject;
        }

        private ClLinearExpression NewExpression(
            ClConstraint cn,
            out ClSlackVariable eplus_,
            out ClSlackVariable eminus_,
            out ClDouble prevEConstant)
        {
            eplus_ = null;
            eminus_ = null;
            prevEConstant = new ClDouble(0d);

            var cnExpr = cn.Expression;
            var expr = new ClLinearExpression(cnExpr.Constant);
            var cnTerms = cnExpr.Terms;

            foreach (ClAbstractVariable v in cnTerms.Keys)
            {
                double c = cnTerms[v].Value;
                ClLinearExpression e = RowExpression(v);
                if (e == null)
                    expr.AddVariable(v, c);
                else
                    expr.AddExpression(e, c);
            }

            if (cn.IsInequality)
            {
                var slackVar = new ClSlackVariable("s");
                expr.SetVariable(slackVar, -1);
                markerVariables.Add(cn, slackVar);

                if (!cn.Strength.IsRequired)
                {
                    var eminus = new ClSlackVariable("em");
                    expr.SetVariable(eminus, 1.0);
                    ClLinearExpression zRow = RowExpression(objective);
                    ClSymbolicWeight sw = cn.Strength.SymbolicWeight * cn.Weight;
                    zRow.SetVariable(eminus, sw.AsDouble());
                    InsertErrorVar(cn, eminus);
                    NoteAddedVariable(eminus, objective);
                }
            }
            else
            {
                // cn is an equality
                if (cn.Strength.IsRequired)
                {
                    var dummyVar = new ClDummyVariable("d");
                    expr.SetVariable(dummyVar, 1.0);
                    markerVariables.Add(cn, dummyVar);
                }
                else
                {
                    var eplus = new ClSlackVariable("ep");
                    var eminus = new ClSlackVariable("em");

                    expr.SetVariable(eplus, -1.0);
                    expr.SetVariable(eminus, 1.0);
                    markerVariables.Add(cn, eplus);
                    ClLinearExpression zRow = RowExpression(objective);
                    ClSymbolicWeight sw = cn.Strength.SymbolicWeight * cn.Weight;
                    double swCoeff = sw.AsDouble();
                    
                    zRow.SetVariable(eplus, swCoeff);
                    NoteAddedVariable(eplus, objective);
                    zRow.SetVariable(eminus, swCoeff);
                    NoteAddedVariable(eminus, objective);
                    InsertErrorVar(cn, eminus);
                    InsertErrorVar(cn, eplus);

                    if (cn.IsStayConstraint)
                    {
                        stayPlusErrorVars.Add(eplus);
                        stayMinusErrorVars.Add(eminus);
                    }
                    else if (cn.IsEditConstraint)
                    {
                        eplus_ = eplus;
                        eminus_ = eminus;

                        prevEConstant = new ClDouble(cnExpr.Constant);
                    }
                }
            }

            if (expr.Constant < 0)
            {
                expr = -expr;
            }

            return expr;
        }

        /// <summary>
        /// Minimize the value of the objective.
        /// </summary>
        /// <remarks>
        /// The tableau should already be feasible.
        /// </remarks>
        private void Optimize(ClObjectiveVariable zVar)
            /* throws ExClInternalError */
        {
            ClLinearExpression zRow = RowExpression(zVar);
            Debug.Assert(zRow != null, "zRow != null");

            ClAbstractVariable entryVar = null;
            ClAbstractVariable exitVar = null;

            while (true)
            {
                double objectiveCoeff = 0;
                var terms = zRow.Terms;
                foreach (ClAbstractVariable v in terms.Keys)
                {
                    double c = terms[v].Value;
                    if (v.IsPivotable && c < objectiveCoeff)
                    {
                        objectiveCoeff = c;
                        entryVar = v;
                    }
                }

                if (objectiveCoeff >= -Epsilon || entryVar == null)
                    return;

                double minRatio = Double.MaxValue;
                var columnVars = Columns[entryVar];
                double r = 0.0;

                foreach (ClAbstractVariable v in columnVars)
                {
                    if (v.IsPivotable)
                    {
                        ClLinearExpression expr = RowExpression(v);
                        double coeff = expr.CoefficientFor(entryVar);

                        if (coeff < 0.0)
                        {
                            r = - expr.Constant/coeff;
                            if (r < minRatio)
                            {
                                // New minRatio... of r
                                minRatio = r;
                                exitVar = v;
                            }
                        }
                    }
                }

                if (minRatio == double.MaxValue)
                {
                    throw new CassowaryInternalException(
                        "Objective function is unbounded in Optimize");
                }

                Pivot(entryVar, exitVar);
            }
        }

        /// <summary>
        /// Fix the constants in the equations representing the edit constraints.
        /// </summary>
        /// <remarks>
        /// Each of the non-required edits will be represented by an equation
        /// of the form:
        ///   v = c + eplus - eminus
        /// where v is the variable with the edit, c is the previous edit value,
        /// and eplus and eminus are slack variables that hold the error in 
        /// satisfying the edit constraint. We are about to change something,
        /// and we want to fix the constants in the equations representing
        /// the edit constraints. If one of eplus and eminus is basic, the other
        /// must occur only in the expression for that basic error variable. 
        /// (They can't both be basic.) Fix the constant in this expression.
        /// Otherwise they are both non-basic. Find all of the expressions
        /// in which they occur, and fix the constants in those. See the
        /// UIST paper for details.
        /// (This comment was for ResetEditConstants(), but that is now
        /// gone since it was part of the screwey vector-based interface
        /// to resolveing. --02/16/99 gjb)
        /// </remarks>
        private void DeltaEditConstant(
            double delta,
            ClAbstractVariable plusErrorVar,
            ClAbstractVariable minusErrorVar)
        {
            ClLinearExpression exprPlus = RowExpression(plusErrorVar);
            if (exprPlus != null)
            {
                exprPlus.IncrementConstant(delta);

                if (exprPlus.Constant < 0.0)
                {
                    InfeasibleRows.Add(plusErrorVar);
                }
                return;
            }

            ClLinearExpression exprMinus = RowExpression(minusErrorVar);
            if (exprMinus != null)
            {
                exprMinus.IncrementConstant(-delta);
                if (exprMinus.Constant < 0.0)
                {
                    InfeasibleRows.Add(minusErrorVar);
                }
                return;
            }

            var columnVars = Columns[minusErrorVar];

            foreach (ClAbstractVariable basicVar in columnVars)
            {
                ClLinearExpression expr = RowExpression(basicVar);
                Debug.Assert(expr != null, "expr != null");

                double c = expr.CoefficientFor(minusErrorVar);
                expr.IncrementConstant(c*delta);

                if (basicVar.IsRestricted && expr.Constant < 0.0)
                {
                    InfeasibleRows.Add(basicVar);
                }
            }
        }

        /// <summary>
        /// Re-optimize using the dual simplex algorithm.
        /// </summary>
        /// <remarks>
        /// We have set new values for the constants in the edit constraints.
        /// </remarks>
        private void DualOptimize()
            /* throws ExClInternalError */
        {
            ClLinearExpression zRow = RowExpression(objective);

            while (InfeasibleRows.Count != 0)
            {
                var enumIfRows = InfeasibleRows.GetEnumerator();
                enumIfRows.MoveNext();
                var exitVar = enumIfRows.Current;

                InfeasibleRows.Remove(exitVar);
                ClAbstractVariable entryVar = null;
                ClLinearExpression expr = RowExpression(exitVar);

                if (expr == null) 
                    continue;

                if (expr.Constant >= 0d)
                    continue;

                double ratio = Double.MaxValue;
                double r;
                var terms = expr.Terms;

                foreach (ClAbstractVariable v in terms.Keys)
                {
                    double c = terms[v].Value;
                    if (c > 0.0 && v.IsPivotable)
                    {
                        double zc = zRow.CoefficientFor(v);
                        r = zc/c; // FIXME: zc / c or zero, as ClSymbolicWeigth-s

                        if (r < ratio)
                        {
                            entryVar = v;
                            ratio = r;
                        }
                    }
                }

                if (ratio == double.MaxValue)
                {
                    throw new CassowaryInternalException(
                        "ratio == nil (Double.MaxValue) in DualOptimize");
                }

                Pivot(entryVar, exitVar);
            }
        }

        /// <summary>
        /// Do a pivot. Move entryVar into the basis and move exitVar 
        /// out of the basis.
        /// </summary>
        /// <remarks>
        /// We could for example make entryVar a basic variable and
        /// make exitVar a parametric variable.
        /// </remarks>
        private void Pivot(
            ClAbstractVariable entryVariable,
            ClAbstractVariable exitVariable)
            /* throws ExClInternalError */
        {
            // the entryVar might be non-pivotable if we're doing a 
            // RemoveConstraint -- otherwise it should be a pivotable
            // variable -- enforced at call sites, hopefully
            
            var expression = RemoveRow(exitVariable);

            expression.ChangeSubject(exitVariable, entryVariable);
            SubstituteOut(entryVariable, expression);
            AddRow(entryVariable, expression);
        }

        /// <summary>
        /// Fix the constants in the equations representing the stays.
        /// </summary>
        /// <remarks>
        /// Each of the non-required stays will be represented by an equation
        /// of the form
        ///   v = c + eplus - eminus
        /// where v is the variable with the stay, c is the previous value
        /// of v, and eplus and eminus are slack variables that hold the error
        /// in satisfying the stay constraint. We are about to change something,
        /// and we want to fix the constants in the equations representing the
        /// stays. If both eplus and eminus are nonbasic they have value 0
        /// in the current solution, meaning the previous stay was exactly
        /// satisfied. In this case nothing needs to be changed. Otherwise one
        /// of them is basic, and the other must occur only in the expression
        /// for that basic error variable. Reset the constant of this
        /// expression to 0.
        /// </remarks>
        private void ResetStayConstants()
        {
            for (int i = 0; i < stayPlusErrorVars.Count; i++)
            {
                var stayPlusErrorVar = stayPlusErrorVars[i];
                var expression = RowExpression(stayPlusErrorVar);

                if (expression == null)
                {
                    var stayMinusErrorVar = stayMinusErrorVars[i];
                    expression = RowExpression(stayMinusErrorVar);
                }

                if (expression != null)
                {
                    expression.Constant = 0d;
                }
            }
        }

        /// <summary>
        /// Set the external variables known to this solver to their appropriate values.
        /// </summary>
        /// <remarks>
        /// Set each external basic variable to its value, and set each external parametric
        /// variable to 0. (It isn't clear that we will ever have external parametric
        /// variables -- every external variable should either have a stay on it, or have an
        /// equation that defines it in terms of other external variables that do have stays.
        /// For the moment I'll put this in though.) Variables that are internal to the solver
        /// don't actually store values -- their values are just implicit in the tableau -- so
        /// we don't need to set them.
        /// </remarks>
        private void SetExternalVariables()
        {
            foreach (var variable in ExternalParametricVars)
            {
                var rowExpression = RowExpression(variable);

                if (rowExpression != null)
                {
                    Debug.WriteLine(
                        "Error: variable {0}in _externalParametricVars is basic",
                        variable);
                    continue;
                }

                variable.Value = 0d;
            }

            foreach (var variable in ExternalRows)
            {
                var rowExpression = RowExpression(variable);
                variable.Value = rowExpression.Constant;
            }

            needsSolving = false;
        }

        /// <summary>
        /// Protected convenience function to insert an error variable
        /// into the _errorVars set, creating the mapping with Add as necessary.
        /// </summary>
        private void InsertErrorVar(ClConstraint constraint, ClSlackVariable variable)
        {
            HashSet<ClSlackVariable> constraintVariables;
            if (!errorVariables.TryGetValue(constraint, out constraintVariables))
            {
                constraintVariables = new HashSet<ClSlackVariable>();
                errorVariables.Add(constraint, constraintVariables);
            }

            constraintVariables.Add(variable);
        }

        #endregion
    }
}