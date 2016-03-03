using System;

namespace JosephGuadagno.Utilities.Extensions
{
    public static class IntegerExtensions
    {
        public static string ToStringValue(this int? value)
        {
            return value.HasValue ? value.ToString() : null;
        }

		public static T ToEnum<T>(this int? value)
		{
			return (T)Enum.Parse(typeof(T), (value ?? 0).ToString());
		}
    }
}