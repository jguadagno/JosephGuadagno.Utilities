namespace JosephGuadagno.Utilities.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToStringValue(this decimal? value)
        {
            return value.HasValue ? value.ToString() : null;
        }

        public static string ToDbString(this decimal? value)
        {
            return value == null ? "NULL" : value.Value.ToString();
        }
    }
}