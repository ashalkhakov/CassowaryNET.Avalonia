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
using Variable = CassowaryNET.Variables.Variable;

namespace CassowaryNET.Exceptions
{
    public class VariableNotFoundException : CassowaryException
    {
        private readonly Variable variable;

        public VariableNotFoundException(Variable variable)
            : base(GetMessage(variable, ""))
        {
            this.variable = variable;
        }

        public VariableNotFoundException(string message, Variable variable)
            : base(GetMessage(variable, message))
        {
            this.variable = variable;
        }

        public Variable Variable
        {
            get { return variable; }
        }

        private static string GetMessage(Variable variable, string message)
        {
            return GetMessage(variable) + Environment.NewLine + message;
        }

        private static string GetMessage(Variable variable)
        {
            if (Equals(variable, null))
                return "The variable could not be found.";

            return string.Format("The variable ({0}) could not be found.", variable);
        }
    }
}