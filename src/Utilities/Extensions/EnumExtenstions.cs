using System;
using System.ComponentModel.DataAnnotations;

namespace JosephGuadagno.Utilities.Extensions
{
	public static class EnumExtensions
	{
		public static string GetDisplayName(this Enum value)
		{
			return value.GetAttributePropertyValue<DisplayAttribute, string>("Name");
		}

		public static int? ToNullableInt (this Enum value)
		{
			return Convert.ToInt32(value) == 0 ? null : (int?)Convert.ToInt32(value);
		}

		public static long? ToNullableLong(this Enum value)
		{
			return Convert.ToInt64(value) == 0 ? null : (long?)Convert.ToInt64(value);
		}

	    public static string ToDbString(this Enum value)
	    {
	        return value.ToDbString(true);
	    }

	    public static string ToDbString(this Enum value, bool zeroAsNull)
	    {
	        var intValue = Convert.ToInt32(value);

	        return intValue == 0 && zeroAsNull
	            ? "NULL"
	            : intValue.ToString();
	    }
	}
}