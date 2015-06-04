using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CassowaryNET.Utils
{
    internal static class OptionEx
    {
        public static T ValueOr<T>(
            this Option<T> option,
            T otherwise)
        {
            return option.Invoke(
                v => v,
                () => otherwise);
        }
    }

    internal struct Option<T>
    {
        #region Fields

        private readonly bool hasValue;
        private readonly T value;

        #endregion

        #region Constructors

        //public Option()
        //{
        //    this.hasValue = false;
        //    this.value = default(T);
        //}

        public Option(T value)
        {
            this.hasValue = true;
            this.value = value;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void Invoke(
            Action<T> ifValue,
            Action ifNoValue)
        {
            if (hasValue)
                ifValue(value);
            else
                ifNoValue();
        }

        public TReturn Invoke<TReturn>(
            Func<T, TReturn> ifValue,
            Func<TReturn> ifNoValue)
        {
            if (hasValue)
                return ifValue(value);
            else
                return ifNoValue();
        }

        #endregion
    }
}
