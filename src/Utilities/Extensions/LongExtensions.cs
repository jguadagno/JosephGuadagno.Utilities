using System;

namespace JosephGuadagno.Utilities.Extensions
{
	public static class LongExtensions
	{
		public static T ToEnum<T>(this long value)
		{
			return (T)Enum.Parse(typeof(T), value.ToString());
		}

		public static T ToEnum<T>(this long? value)
		{
			return (T)Enum.Parse(typeof(T), (value ?? 0).ToString());
		}

	    public static string ToDbString(this long? value)
	    {
	        return value == null ? "NULL" : value.Value.ToString();
	    }
	}
}