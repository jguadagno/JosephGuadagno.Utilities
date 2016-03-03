using System;

namespace JosephGuadagno.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToFormattedStringValue(this DateTime? value, string format)
        {
            return value.HasValue ? value.Value.ToString(format) : null;
        }
    }
}