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
using Cassowary.Utils;
using Cassowary.Variables;
using JetBrains.Annotations;

namespace Cassowary
{
    public class ClTableau
    {
        #region Fields

        /// <summary>
        /// _columns is a mapping from variables which occur in expressions to the
        /// set of basic variables whose expressions contain them
        /// i.e., it's a mapping from variables in expressions (a column) to the 
        /// set of rows that contain them.
        /// </summary>
        private readonly Dictionary<ClAbstractVariable, HashSet<ClAbstractVariable>> columns; 

        /// <summary>
        /// _rows maps basic variables to the expressions for that row in the tableau.
        /// </summary>
        private readonly Dictionary<ClAbstractVariable, ClLinearExpression> rows;

        /// <summary>
        /// Collection of basic variables that have infeasible rows
        /// (used when reoptimizing).
        /// </summary>
        private readonly HashSet<ClAbstractVariable> infeasibleRows;

        /// <summary>
        /// Set of rows where the basic variable is external
        /// this was added to the Java/C++/C# versions to reduce time in SetExternalVariables().
        /// </summary>
        private readonly HashSet<ClVariable> externalRows;

        /// <summary>
        /// Set of external variables which are parametric
        /// this was added to the Java/C++/C# versions to reduce time in SetExternalVariables().
        /// </summary>
        private readonly HashSet<ClVariable> externalParametricVars;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor is protected, since this only supports an ADT for
        /// the ClSimplexSolver class.
        /// </summary>
        protected ClTableau()
        {
            columns = new Dictionary<ClAbstractVariable, HashSet<ClAbstractVariable>>();
            rows = new Dictionary<ClAbstractVariable, ClLinearExpression>();
            infeasibleRows = new HashSet<ClAbstractVariable>();
            externalRows = new HashSet<ClVariable>();
            externalParametricVars = new HashSet<ClVariable>();
        }

        #endregion

        #region Properties

        protected Dictionary<ClAbstractVariable, HashSet<ClAbstractVariable>> Columns
        {
            get { return columns; }
        }

        protected Dictionary<ClAbstractVariable, ClLinearExpression> Rows
        {
            get { return rows; }
        }

        protected HashSet<ClAbstractVariable> InfeasibleRows
        {
            get { return infeasibleRows; }
        }

        protected IEnumerable<ClVariable> ExternalRows
        {
            get { return externalRows; }
        }

        protected IEnumerable<ClVariable> ExternalParametricVars
        {
            get { return externalParametricVars; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Variable v has been removed from an expression. If the
        /// expression is in a tableau the corresponding basic variable is
        /// subject (or if subject is nil then it's in the objective function).
        /// Update the column cross-indices.
        /// </summary>
        public void NoteRemovedVariable(
            ClAbstractVariable v,
            ClAbstractVariable subject)
        {
            if (subject != null)
            {
                columns[v].Remove(subject);
            }
        }

        /// <summary>
        /// v has been added to the linear expression for subject
        /// update column cross indices.
        /// </summary>
        public void NoteAddedVariable(
            ClAbstractVariable v,
            ClAbstractVariable subject)
        {
            if (subject != null)
            {
                InsertColVar(v, subject);
            }
        }

        /// <summary>
        /// Convenience function to insert a variable into
        /// the set of rows stored at _columns[param_var],
        /// creating a new set if needed. 
        /// </summary>
        private void InsertColVar(
            ClAbstractVariable param_var,
            ClAbstractVariable rowvar)
        {
            var rowset = columns.GetOrDefault(param_var);

            if (rowset == null)
            {
                rowset = new HashSet<ClAbstractVariable>();
                columns.Add(param_var, rowset);
            }

            rowset.Add(rowvar);
        }

        // Add v=expr to the tableau, update column cross indices
        // v becomes a basic variable
        // expr is now owned by ClTableau class, 
        // and ClTableau is responsible for deleting it
        // (also, expr better be allocated on the heap!).
        protected void AddRow(ClAbstractVariable variable, ClLinearExpression expression)
        {
            // for each variable in expr, add var to the set of rows which
            // have that variable in their expression
            rows.Add(variable, expression);

            // FIXME: check correctness!
            foreach (var expressionVariable in expression.Terms.Keys)
            {
                InsertColVar(expressionVariable, variable);

                if (expressionVariable.IsExternal)
                {
                    var clVariable = (ClVariable) expressionVariable;
                    externalParametricVars.Add(clVariable);
                }
            }

            if (variable.IsExternal)
            {
                var clVariable = (ClVariable)variable;
                externalRows.Add(clVariable);
            }
        }

        /// <summary>
        /// Remove v from the tableau -- remove the column cross indices for v
        /// and remove v from every expression in rows in which v occurs
        /// </summary>
        protected void RemoveColumn(ClAbstractVariable variable)
        {
            // remove the rows with the variables in varset

            var column = columns.GetOrDefault(variable);
            columns.Remove(variable);

            if (column != null)
            {
                foreach (var columnVariable in column)
                {
                    var expression = rows[columnVariable];
                    expression.Terms.Remove(variable);
                }
            }
            else
            {
                // Could not find var {0} in _columns
            }

            if (variable.IsExternal)
            {
                var clVariable = (ClVariable)variable;
                externalRows.Remove(clVariable);
                externalParametricVars.Remove(clVariable);
            }
        }

        /// <summary>
        /// Remove the basic variable v from the tableau row v=expr
        /// Then update column cross indices.
        /// </summary>
        protected ClLinearExpression RemoveRow(ClAbstractVariable variable)
            /*throws ExCLInternalError*/
        {
            var expression = rows[variable];
            Debug.Assert(expression != null);

            // For each variable in this expression, update
            // the column mapping and remove the variable from the list
            // of rows it is known to be in.
            foreach (var clv in expression.Terms.Keys)
            {
                var varset = columns[clv];

                if (varset != null)
                {
                    varset.Remove(variable);
                }
            }

            infeasibleRows.Remove(variable);

            if (variable.IsExternal)
            {
                var clVariable = (ClVariable) variable;
                externalRows.Remove(clVariable);
            }

            rows.Remove(variable);

            return expression;
        }

        /// <summary> 
        /// Replace all occurrences of oldVar with expr, and update column cross indices
        /// oldVar should now be a basic variable.
        /// </summary> 
        protected void SubstituteOut(
            ClAbstractVariable oldVariable,
            ClLinearExpression expression)
        {
            var oldVarColumnSet = columns[oldVariable];

            foreach (var variable in oldVarColumnSet)
            {
                var row = rows[variable];
                row.SubstituteOut(oldVariable, expression, variable, this);

                if (variable.IsRestricted && row.Constant < 0.0)
                {
                    infeasibleRows.Add(variable);
                }
            }

            if (oldVariable.IsExternal)
            {
                var oldClVariable = (ClVariable)oldVariable;
                externalRows.Add(oldClVariable);
                externalParametricVars.Remove(oldClVariable);
            }

            columns.Remove(oldVariable);
        }

        /// <summary>
        /// Return true if and only if the variable subject is in the columns keys 
        /// </summary>
        [Pure]
        protected bool ColumnsHasKey(ClAbstractVariable subject)
        {
            return columns.ContainsKey(subject);
        }

        [Pure]
        protected ClLinearExpression RowExpression(ClAbstractVariable v)
        {
            return rows.GetOrDefault(v);
        }

        /// <summary>
        /// Returns information about the tableau's internals.
        /// </summary>
        /// <remarks>
        /// Originally from Michael Noth <noth@cs.washington.edu>
        /// </remarks>
        /// <returns>
        /// String containing the information.
        /// </returns>
        public virtual string GetInternalInfo()
        {
            string s = "Tableau Information:\n";
            s += string.Format(
                "Rows: {0} (= {1} constraints)",
                rows.Count,
                rows.Count - 1);
            s += string.Format("\nColumns: {0}", columns.Count);
            s += string.Format("\nInfeasible Rows: {0}", infeasibleRows.Count);
            s += string.Format("\nExternal basic variables: {0}", externalRows.Count);
            s += string.Format(
                "\nExternal parametric variables: {0}",
                externalParametricVars.Count);

            return s;
        }

        public override string ToString()
        {
            string s = "Tableau:\n";

            foreach (var clv in rows.Keys)
            {
                var expr = rows[clv];
                s += string.Format("{0} <==> {1}\n", clv.ToString(), expr.ToString());
            }

            s += string.Format("\nColumns:\n{0}", columns.ToString());
            s += string.Format("\nInfeasible rows: {0}", infeasibleRows.ToString());

            s += string.Format(
                "\nExternal basic variables: {0}",
                externalRows.ToString());
            s += string.Format(
                "\nExternal parametric variables: {0}",
                externalParametricVars.ToString());

            return s;
        }

        #endregion
    }
}