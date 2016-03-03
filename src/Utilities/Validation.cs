using System;

namespace JosephGuadagno.Utilities
{
    /// <summary>
    ///     Provides validation functions
    /// </summary>
    public static class Validation
    {
        public static void CheckArgumentNotNull<T>(T argumentValue, string argumentName)
            where T : class
        {
            if (null == argumentValue)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}