using System;
using System.Globalization;

namespace Ploeh.AutoFixture.Idioms
{
    public class EmptyStringInvalidValue : IInvalidValue
    {
        #region Implementation of IInvalidValue<T>

        public void Assert(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(string.Empty);
        }

        public bool IsSatisfiedBy(Type exceptionType)
        {
            if (exceptionType == null)
            {
                throw new ArgumentNullException("exceptionType");
            }

            return exceptionType == typeof(ArgumentException);
        }

        public string Description
        {
            get { return "empty string"; }
        }

        #endregion
    }
}