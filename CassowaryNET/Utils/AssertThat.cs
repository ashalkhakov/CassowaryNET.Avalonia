using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CassowaryNET.Utils
{
    internal static class AssertThat
    {
        public static void ArgumentNotNull<T>(Expression<Func<T>> expression)
            where T : class
        {
            //Contract.Requires(expression != null);
            if (expression == null)
                throw new ArgumentNullException("expression");

            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(
                    "Expression.Body was not a MemberExpression. " +
                    "This is likely because the expression refers to a constant " +
                    "rather than a parameter, field, property or variable. ",
                    "expression");
            }

            var parameterName = memberExpression.Member.Name;

            var parameterFunc = expression.Compile();
            //Contract.Assume(parameterFunc != null);

            var parameterValue = parameterFunc();

            if (parameterValue == null)
                throw new ArgumentNullException(parameterName);
        }
    }
}
