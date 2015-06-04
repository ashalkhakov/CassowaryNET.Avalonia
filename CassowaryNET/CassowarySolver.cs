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
using CassowaryNET.Constraints;
using CassowaryNET.Exceptions;
using CassowaryNET.Utils;
using CassowaryNET.Variables;

namespace CassowaryNET
{
    public sealed class CassowarySolver
    {
        #region Fields

        private class ErrorVariablePair
        {
            private readonly SlackVariable plus;
            private readonly SlackVariable minus;

            public ErrorVariablePair(SlackVariable plus, SlackVariable minus)
            {
                this.plus = plus;
                this.minus = minus;
            }

            public SlackVariable Plus
            {
                get { return plus; }
            }

            public SlackVariable Minus
            {
                get { return minus; }
            }
        }

        #endregion

        #region Fields

        private const double Epsilon = 1e-8;

        private readonly Tableau tableau;
        private readonly ObjectiveVariable objective;

        /// <summary>
        /// The array of error vars for the stay constraints
        /// (need both positive and negative since they have only non-negative
        /// values).
        /// </summary>
        private readonly List<ErrorVariablePair> stayErrorVariables;

        /// <summary>
        /// Give error variables for a non-required constraints,
        /// maps to ClSlackVariable-s.
        /// </summary>
        /// <remarks>
        /// Map ClConstraint to Set (of ClVariable).
        /// </remarks>
        private readonly Dictionary<Constraint, HashSet<SlackVariable>> errorVariables;

        /// <summary>
        /// Return a lookup table giving the marker variable for
        /// each constraints (used when deleting a constraint).
        /// </summary>
        /// <remarks>
        /// Map ClConstraint to ClVariable.
        /// </remarks>
        private readonly Dictionary<Constraint, AbstractVariable> markerVariables;

        /// <summary>
        /// Map edit variables to ClEditInfo-s.
        /// </summary>
        /// <remarks>
        /// ClEditInfo instances contain all the information for an
        /// edit constraints (the edit plus/minus vars, the index [for old-style
        /// resolve(ArrayList...)] interface), and the previous value.
        /// (ClEditInfo replaces the parallel vectors from the Smalltalk impl.)
        /// </remarks>
        private readonly Dictionary<Variable, EditInfo> editVariableInfo;

        private bool needsSolving = false;

        #endregion

        #region Constructors

        /// <remarks>
        /// Constructor initializes the fields, and creaties the objective row.
        /// </remarks>
        public CassowarySolver()
        {
            tableau = new Tableau();

            objective = new ObjectiveVariable("Z");
            Tableau.Rows.Add(objective, new LinearExpression(0d));

            stayErrorVariables = new List<ErrorVariablePair>();
            errorVariables = new Dictionary<Constraint, HashSet<SlackVariable>>();
            markerVariables = new Dictionary<Constraint, AbstractVariable>();
            editVariableInfo = new Dictionary<Variable, EditInfo>();

            AutoSolve = true;
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

        internal Tableau Tableau
        {
            get { return tableau; }
        }

        /// <summary>
        /// Map edit variables to ClEditInfo-s.
        /// </summary>
        /// <remarks>
        /// ClEditInfo instances contain all the information for an
        /// edit constraints (the edit plus/minus vars, the index [for old-style
        /// resolve(ArrayList...)] interface), and the previous value.
        /// (ClEditInfo replaces the parallel vectors from the Smalltalk impl.)
        /// </remarks>
        internal Dictionary<Variable, EditInfo> EditVariableInfo
        {
            get { return editVariableInfo; }
        }

        #endregion

        #region Methods

        public void AddConstraint(EqualityConstraint constraint)
        {
            AddConstraintCore(constraint);
            MaybeAutoSolve();
        }

        public void AddConstraint(InequalityConstraint constraint)
        {
            AddConstraintCore(constraint);
            MaybeAutoSolve();
        }

        public void AddConstraint(LinearConstraint constraint)
        /* throws ExClRequiredFailure, ExClInternalError */
        {
            if (constraint is EqualityConstraint)
                AddConstraint((EqualityConstraint)constraint);
            else if (constraint is InequalityConstraint)
                AddConstraint((InequalityConstraint)constraint);
            else
            {
                throw new ArgumentException(
                    "Uknown constraint type. " +
                    "Expected type of EqualityConstraint or InequalityConstraint");
            }
        }

        internal EditInfo AddConstraint(EditConstraint constraint)
        {
            SlackVariable plusError;
            SlackVariable minusError;
            AddConstraintCore(constraint, out plusError, out minusError);

            MaybeAutoSolve();

            var editInfo = new EditInfo(
                constraint,
                plusError,
                minusError,
                constraint.Expression.Constant);
            return editInfo;
        }

        internal void AddConstraint(StayConstraint constraint)
        {
            AddConstraintCore(constraint);
            MaybeAutoSolve();
        }

        private void AddConstraintCore(
            Constraint constraint)
        {
            SlackVariable plusError;
            SlackVariable minusError;
            AddConstraintCore(constraint, out plusError, out minusError);
        }

        private void AddConstraintCore(
            Constraint constraint,
            out SlackVariable plusError,
            out SlackVariable minusError)
        {
            var expression = GetExpressionForConstraint(
                constraint,
                out plusError,
                out minusError);

            var addedDirectly = TryAddingDirectly(expression);
            if (!addedDirectly)
            {
                AddWithArtificialVariable(expression);
            }

            needsSolving = true;
        }

        public EditSection CreateEditSection()
        {
            return new EditSection(this);
        }
        
        /// <summary>
        /// Add a stay of the given strength (default to ClStrength#Weak)
        /// of a variable to the tableau..
        /// <param name="v">
        /// Variable to add the stay constraint to.
        /// </param>
        /// </summary>
        public void AddStay(Variable v, Strength strength, double weight)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            var constraint = new StayConstraint(v, strength, weight);

            AddConstraint(constraint);
        }

        /// <remarks>
        /// Default to weight 1.0.
        /// </remarks>
        public void AddStay(Variable v, Strength strength)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddStay(v, strength, 1.0);
        }

        /// <remarks>
        /// Default to strength ClStrength#Weak.
        /// </remarks>
        public void AddStay(Variable v)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            AddStay(v, Strength.Weak, 1.0);
        }

        /// <summary>
        /// Remove a constraint from the tableau.
        /// Also remove any error variable associated with it.
        /// </summary>
        public void RemoveConstraint(Constraint constraint)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            var marker = markerVariables.GetOrDefault(constraint);
            if (Equals(marker, null))
                throw new CassowaryConstraintNotFoundException();
            markerVariables.Remove(constraint);

            needsSolving = true;

            ResetStayConstants();

            var slackVariables = errorVariables.GetOrDefault(constraint);
            if (slackVariables != null)
            {
                errorVariables.Remove(constraint);

                var objectiveRow = Tableau.Rows[objective];

                foreach (var variable in slackVariables)
                {
                    var coeff = -constraint.Weight*
                                constraint.Strength.Weight.Value;

                    var variableRow = Tableau.Rows.GetOrDefault(variable);

                    LinearExpression newObjectiveRow;
                    if (Equals(variableRow, null))
                    {
                        newObjectiveRow = objectiveRow + (variable*coeff);
                    }
                    else
                    {
                        // the error variable was in the basis
                        newObjectiveRow = objectiveRow + (variableRow*coeff);
                    }

                    // overwrite the objective row
                    Tableau.Rows[objective] = newObjectiveRow;

                    var addedVariables = newObjectiveRow.Variables
                        .Except(objectiveRow.Variables);
                    var removedVariables = objectiveRow.Variables
                        .Except(newObjectiveRow.Variables);

                    foreach (var addedVariable in addedVariables)
                    {
                        Tableau.NoteAddedVariable(addedVariable, objective);
                    }
                    foreach (var removedVariable in removedVariables)
                    {
                        Tableau.NoteRemovedVariable(removedVariable, objective);
                    }
                }
            }

            if (!Tableau.Rows.ContainsKey(marker))
            {
                // not in the basis, so need to do some more work
                var markerColumn = Tableau.Columns[marker];

                // must pivot...
                AbstractVariable exitVar = null;
                double minRatio = 0d;

                foreach (var variable in markerColumn)
                {
                    if (variable.IsRestricted)
                    {
                        var rowExpression = Tableau.RowExpression(variable);
                        var coefficient = rowExpression.CoefficientFor(marker);

                        if (coefficient >= 0d)
                            continue;

                        var ratio = -rowExpression.Constant/coefficient;
                        if (Equals(exitVar, null) || ratio < minRatio)
                        {
                            minRatio = ratio;
                            exitVar = variable;
                        }
                    }
                }

                if (Equals(exitVar, null))
                {
                    // no restriced variables in markerColumn 
                    // OR all coefficients >= 0
                    foreach (var variable in markerColumn)
                    {
                        if (!variable.IsRestricted)
                            continue;

                        var rowExpression = Tableau.RowExpression(variable);
                        var coefficient = rowExpression.CoefficientFor(marker);

                        var ratio = rowExpression.Constant/coefficient;
                        if (Equals(exitVar, null) || ratio < minRatio)
                        {
                            minRatio = ratio;
                            exitVar = variable;
                        }
                    }
                }

                if (Equals(exitVar, null))
                {
                    if (markerColumn.Count == 0)
                    {
                        Tableau.RemoveColumn(marker);
                    }
                    else
                    {
                        exitVar = markerColumn.FirstOrDefault();
                    }
                }

                if (!Equals(exitVar, null))
                {
                    Pivot(marker, exitVar);
                }
            }

            if (!Equals(Tableau.RowExpression(marker), null))
            {
                Tableau.RemoveRow(marker);
            }

            if (slackVariables != null)
            {
                foreach (var slackVariable in slackVariables)
                {
                    if (!Equals(slackVariable, marker))
                    {
                        Tableau.RemoveColumn(slackVariable);
                    }
                }
            }

            if (constraint.IsStayConstraint)
            {
                if (slackVariables != null)
                {
                    foreach (var pair in stayErrorVariables)
                    {
                        slackVariables.Remove(pair.Plus);
                        slackVariables.Remove(pair.Minus);
                    }
                }
            }
            else if (constraint.IsEditConstraint)
            {
                Debug.Assert(slackVariables != null, "eVars != null");
                var editConstraint = (EditConstraint) constraint;
                var variable = editConstraint.Variable;
                var editInfo = editVariableInfo[variable];
                var clvEditMinus = editInfo.MinusError;
                Tableau.RemoveColumn(clvEditMinus);
                editVariableInfo.Remove(variable);
            }

            MaybeAutoSolve();
        }

        private void MaybeAutoSolve()
        {
            if (!AutoSolve)
                return;

            Optimize(objective);
            SetExternalVariables();
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
            Tableau.InfeasibleRows.Clear();
            ResetStayConstants();
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

        public void SetEditedValue(Variable variable, double value)
            /* throws ExClInternalError */
        {
            if (!ContainsVariable(variable))
            {
                throw new InvalidOperationException();
                //variable.Value = value;
                //return;
            }

            if (MathHelper.Approx(value, variable.Value))
                return;

            using (var editSection = this.CreateEditSection())
            {
                editSection.Add(variable);
                editSection.SuggestValue(variable, value);
            }
        }

        public bool ContainsVariable(Variable v)
            /* throws ExClInternalError */
        {
            return Tableau.HasColumnKey(v) || !Equals(Tableau.RowExpression(v), null);
        }

        public void AddVar(Variable v)
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
        public string GetInternalInfo()
        {
            string result = Tableau.GetInternalInfo();

            result += "\nSolver info:\n";
            result += "Stay Error Variables: ";
            result += stayErrorVariables.Count*2;
            result += "Edit Variables: " + editVariableInfo.Count;
            result += "\n";

            return result;
        }

        public override string ToString()
        {
            string result = Tableau.ToString();

            result += "\n_stayErrorVars: ";
            result += stayErrorVariables;
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
        private void AddWithArtificialVariable(LinearExpression expression)
            /* throws ExClRequiredFailure, ExClInternalError */
        {
            var av = new SlackVariable("a");
            var az = new ObjectiveVariable("az");

            var expressionClone = Cloneable.Clone(expression);

            Tableau.AddRow(az, expressionClone);
            Tableau.AddRow(av, expression);

            Optimize(az);

            var azRowExpression = Tableau.RowExpression(az);

            if (!MathHelper.Approx(azRowExpression.Constant, 0.0))
            {
                Tableau.RemoveRow(az);
                Tableau.RemoveColumn(av);
                throw new CassowaryRequiredConstraintFailureException();
            }

            // see if av is a basic variable
            var avRowExpression = Tableau.RowExpression(av);

            if (!Equals(avRowExpression, null))
            {
                // find another variable in this row and pivot,
                // so that av becomes parametric
                if (avRowExpression.IsConstant)
                {
                    // if there isn't another variable in the row
                    // then the tableau contains the equation av=0 --
                    // just delete av's row
                    Tableau.RemoveRow(av);
                    Tableau.RemoveRow(az);
                    return;
                }

                var entryVar = avRowExpression.GetAnyPivotableVariable();
                Pivot(entryVar, av);
            }

            Debug.Assert(Equals(Tableau.RowExpression(av), null), "tableau.RowExpression(av) == null)");

            Tableau.RemoveColumn(av);
            Tableau.RemoveRow(az);
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
        private bool TryAddingDirectly(LinearExpression expression)
            /* throws ExClRequiredFailure */
        {
            var subject = ChooseSubject(expression);
            if (Equals(subject, null))
            {
                return false;
            }

            expression = expression.WithSubject(subject);
            if (Tableau.HasColumnKey(subject))
            {
                Tableau.SubstituteOut(subject, expression);
            }

            Tableau.AddRow(subject, expression);

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
        private AbstractVariable ChooseSubject(LinearExpression expression)
            /* ExClRequiredFailure */
        {
            AbstractVariable subject = null; // the current best subject, if any

            bool foundUnrestricted = false;
            bool foundNewRestricted = false;

            foreach (var v in expression.Terms.Keys)
            {
                double c = expression.Terms[v];

                if (foundUnrestricted)
                {
                    if (!v.IsRestricted)
                    {
                        if (!Tableau.HasColumnKey(v))
                        {
                            return v;
                        }
                    }
                }
                else
                {
                    // we haven't found an restricted variable yet
                    if (v.IsRestricted)
                    {
                        if (!foundNewRestricted && !v.IsDummy && c < 0.0)
                        {
                            var col = Tableau.Columns.GetOrDefault(v);

                            if (col == null ||
                                (col.Count == 1 && Tableau.HasColumnKey(objective)))
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

            if (!Equals(subject, null))
            {
                return subject;
            }

            double coeff = 0.0;

            foreach (var v in expression.Terms.Keys)
            {
                double c = expression.Terms[v];

                if (!v.IsDummy)
                {
                    return null; // nope, no luck
                }

                if (!Tableau.HasColumnKey(v))
                {
                    subject = v;
                    coeff = c;
                }
            }

            if (!MathHelper.Approx(expression.Constant, 0.0))
            {
                throw new CassowaryRequiredConstraintFailureException();
            }
            
            return subject;
        }

        private LinearExpression GetExpressionForConstraint(
            Constraint constraint,
            out SlackVariable plusError,
            out SlackVariable minusError)
        {
            plusError = null;
            minusError = null;

            var expression = new LinearExpression(constraint.Expression.Constant);

            foreach (var variable in constraint.Expression.Variables)
            {
                var coefficient = constraint.Expression.Terms[variable];
                var addition = Tableau.Rows.GetOption(variable).ValueOr(variable);
                expression += coefficient*addition;
            }

            if (constraint.IsInequality)
            {
                var slackVariable = new SlackVariable("s");
                expression -= slackVariable;
                markerVariables.Add(constraint, slackVariable);

                if (constraint.Strength != Strength.Required)
                {
                    minusError = new SlackVariable("em");
                    expression += minusError;

                    var weightCoefficient = constraint.Strength.Weight.Value*
                                            constraint.Weight;
                    Tableau.Rows[objective] += weightCoefficient*minusError;

                    Tableau.NoteAddedVariable(minusError, objective);
                    InsertErrorVariable(constraint, minusError);
                }
            }
            else
            {
                if (constraint.Strength == Strength.Required)
                {
                    var dummyVar = new DummyVariable("d");
                    expression += dummyVar;
                    markerVariables.Add(constraint, dummyVar);
                }
                else
                {
                    plusError = new SlackVariable("ep");
                    minusError = new SlackVariable("em");

                    expression -= plusError;
                    expression += minusError;
                    markerVariables.Add(constraint, plusError);

                    var weightCoefficient = constraint.Strength.Weight.Value*
                                            constraint.Weight;
                    Tableau.Rows[objective] += weightCoefficient*(plusError + minusError);

                    // minus error must be handled first (for some reason)
                    Tableau.NoteAddedVariable(minusError, objective);
                    InsertErrorVariable(constraint, minusError);

                    Tableau.NoteAddedVariable(plusError, objective);
                    InsertErrorVariable(constraint, plusError);

                    if (constraint.IsStayConstraint)
                    {
                        var pair = new ErrorVariablePair(plusError, minusError);
                        stayErrorVariables.Add(pair);
                    }
                }
            }

            return expression.Constant >= 0d
                ? expression
                : -expression;
        }

        /// <summary>
        /// Minimize the value of the objective.
        /// </summary>
        /// <remarks>
        /// The tableau should already be feasible.
        /// </remarks>
        private void Optimize(ObjectiveVariable zVariable)
            /* throws ExClInternalError */
        {
            var zRow = Tableau.RowExpression(zVariable);
            Debug.Assert(!Equals(zRow, null), "zRow != null");

            // NOTE: should this be inside the loop?
            AbstractVariable entryVariable = null;

            while (true)
            {
                var objectiveCoeff = 0d;
                foreach (var v in zRow.Terms.Keys)
                {
                    double c = zRow.Terms[v];
                    if (v.IsPivotable && c < objectiveCoeff)
                    {
                        objectiveCoeff = c;
                        entryVariable = v;
                    }
                }

                if (objectiveCoeff >= -Epsilon || Equals(entryVariable, null))
                    return;

                var exitVariable = GetExitVariable(entryVariable);

                Debug.Assert(!Equals(exitVariable, null));

                Pivot(entryVariable, exitVariable);
            }
        }

        private AbstractVariable GetExitVariable(AbstractVariable entryVariable)
        {
            AbstractVariable exitVariable = null;
            var minRatio = double.MaxValue;
            var minFound = false;

            var columnVariables = Tableau.Columns[entryVariable];
            foreach (var variable in columnVariables)
            {
                if (!variable.IsPivotable)
                    continue;

                var rowExpression = Tableau.RowExpression(variable);
                var coefficient = rowExpression.CoefficientFor(entryVariable);

                if (coefficient >= 0d)
                    continue;

                var ratio = - rowExpression.Constant/coefficient;
                if (ratio < minRatio)
                {
                    minFound = true;
                    minRatio = ratio;
                    exitVariable = variable;
                }
            }

            if (!minFound)
            {
                throw new CassowaryInternalException(
                    "Objective function is unbounded in Optimize");
            }

            return exitVariable;
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
        internal void DeltaEditConstant(
            double delta,
            AbstractVariable plusErrorVariable,
            AbstractVariable minusErrorVariable)
        {
            var plusErrorRow = Tableau.Rows.GetOrDefault(plusErrorVariable);
            if (!Equals(plusErrorRow, null))
            {
                plusErrorRow = plusErrorRow.WithConstantIncrementedBy(delta);
                Tableau.Rows[plusErrorVariable] = plusErrorRow;

                if (plusErrorRow.Constant < 0d)
                {
                    Tableau.InfeasibleRows.Add(plusErrorVariable);
                }
                return;
            }

            var minusErrorRow = Tableau.Rows.GetOrDefault(minusErrorVariable);
            if (!Equals(minusErrorRow, null))
            {
                minusErrorRow = minusErrorRow.WithConstantIncrementedBy(-delta);
                Tableau.Rows[minusErrorVariable] = minusErrorRow;

                if (minusErrorRow.Constant < 0d)
                {
                    Tableau.InfeasibleRows.Add(minusErrorVariable);
                }
                return;
            }

            var minusErrorColumn = Tableau.Columns[minusErrorVariable];
            foreach (var variable in minusErrorColumn)
            {
                var variableRow = Tableau.Rows[variable];

                var coefficient = variableRow.CoefficientFor(minusErrorVariable);
                variableRow = variableRow.WithConstantIncrementedBy(coefficient*delta);
                Tableau.Rows[variable] = variableRow;

                if (variable.IsRestricted && variableRow.Constant < 0d)
                {
                    Tableau.InfeasibleRows.Add(variable);
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
            LinearExpression zRow = Tableau.RowExpression(objective);

            while (Tableau.InfeasibleRows.Count != 0)
            {
                var enumIfRows = Tableau.InfeasibleRows.GetEnumerator();
                enumIfRows.MoveNext();
                var exitVar = enumIfRows.Current;

                Tableau.InfeasibleRows.Remove(exitVar);
                AbstractVariable entryVar = null;
                LinearExpression expr = Tableau.RowExpression(exitVar);

                if (Equals(expr, null))
                    continue;

                if (expr.Constant >= 0d)
                    continue;

                double ratio = Double.MaxValue;
                double r;

                foreach (AbstractVariable v in expr.Terms.Keys)
                {
                    double c = expr.Terms[v];
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
            AbstractVariable entryVariable,
            AbstractVariable exitVariable)
            /* throws ExClInternalError */
        {
            // the entryVar might be non-pivotable if we're doing a 
            // RemoveConstraint -- otherwise it should be a pivotable
            // variable -- enforced at call sites, hopefully

            var expression = Tableau.RemoveRow(exitVariable);

            expression = expression.WithSubjectChangedTo(exitVariable, entryVariable);

            Tableau.SubstituteOut(entryVariable, expression);
            Tableau.AddRow(entryVariable, expression);
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
        internal void ResetStayConstants()
        {
            foreach (var pair in stayErrorVariables)
            {
                ResetErrorRowConstants(pair);
            }
        }

        private void ResetErrorRowConstants(ErrorVariablePair pair)
        {
            if (ResetErrorRowConstant(pair.Plus))
                return;
            ResetErrorRowConstant(pair.Minus);
        }

        private bool ResetErrorRowConstant(SlackVariable errorVariable)
        {
            var expression = Tableau.Rows.GetOrDefault(errorVariable);
            if (Equals(expression, null)) 
                return false;

            expression = expression.WithConstantSetTo(0d);
            Tableau.Rows[errorVariable] = expression;
            return true;
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
            foreach (var variable in Tableau.ExternalParametricVars)
            {
                var rowExpression = Tableau.RowExpression(variable);

                if (!Equals(rowExpression, null))
                {
                    Debug.WriteLine(
                        "Error: variable {0}in _externalParametricVars is basic",
                        variable);
                    continue;
                }

                variable.Value = 0d;
            }

            foreach (var variable in Tableau.ExternalRows)
            {
                var rowExpression = Tableau.RowExpression(variable);
                variable.Value = rowExpression.Constant;
            }

            needsSolving = false;
        }

        /// <summary>
        /// Cconvenience function to insert an error variable
        /// into the _errorVars set, creating the mapping with Add as necessary.
        /// </summary>
        private void InsertErrorVariable(Constraint constraint, SlackVariable variable)
        {
            var constraintVariables = errorVariables.GetOrAdd(
                constraint,
                _ => new HashSet<SlackVariable>());

            constraintVariables.Add(variable);
        }

        #endregion
    }
}