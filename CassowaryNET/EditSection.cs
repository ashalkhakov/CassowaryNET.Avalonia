using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CassowaryNET.Constraints;
using CassowaryNET.Exceptions;
using CassowaryNET.Variables;

namespace CassowaryNET
{
    public class EditSection : IDisposable
    {
        #region Fields

        private readonly CassowarySolver solver;
        private readonly Dictionary<Variable, EditInfo> editVariableInfo;

        #endregion

        #region Constructors

        public EditSection(CassowarySolver solver)
        {
            this.solver = solver;
            this.editVariableInfo = new Dictionary<Variable, EditInfo>();

            // may later want to do more in here
            solver.Tableau.InfeasibleRows.Clear();
            solver.ResetStayConstants();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods
        
        public void Add(Variable variable, Strength strength)
            /* throws ExClInternalError */
        {
            // we get problems here is strength == Required
            if (strength == Strength.Required)
                throw new ArgumentException("Strength cannot be Strength.Required");

            try
            {
                var constraint = new EditConstraint(variable, strength);
                var editInfo = solver.AddConstraint(constraint);
                editVariableInfo.Add(constraint.Variable, editInfo);
                solver.EditVariableInfo.Add(constraint.Variable, editInfo);
            }
            catch (CassowaryRequiredConstraintFailureException)
            {
                // should not get this
                throw new CassowaryInternalException(
                    "Required failure when adding an edit variable");
            }
        }

        public void Add(Variable variable)
        {
            /* throws ExClInternalError */
            Add(variable, Strength.Strong);
        }

        public void SuggestValue(Variable variable, double value)
        /* throws ExClError */
        {
            var editInfo = editVariableInfo[variable];
            if (editInfo == null)
            {
                Console.Error.WriteLine(
                    "SuggestValue for variable {0}, but var is not an edit variable\n",
                    variable);

                throw new CassowaryException();
            }

            var plusError = editInfo.PlusError;
            var minusError = editInfo.MinusError;
            var delta = value - editInfo.PreviousValue;
            editInfo.PreviousValue = value;
            solver.DeltaEditConstant(delta, plusError, minusError);
        }

        public void Dispose()
        {
            solver.Resolve();

            foreach (var variable in editVariableInfo.Keys)
            {
                var editInfo = editVariableInfo[variable];
                solver.RemoveConstraint(editInfo.Constraint);
                
                // this gets called inside CassowarySolver...
                //solver.EditVariableInfo.Remove(variable);
            }
        }

        #endregion
    }
}
