namespace JosephGuadagno.Utilities.Extensions
{
    public static class BooleanExtensions
    {
        public static bool IsTrue(this bool? input)
        {
            return input ?? false;
        }

        public static bool IsFalse(this bool? input)
        {
            return input == false;
        }

        public static bool IsNull(this bool? input)
        {
            return !input.HasValue;
        }

        public static string ToStringValue(this bool? value)
        {
            return value.HasValue ? value.ToString() : null;
        }

        public static string ToDbString(this bool value)
        {
            return value ? "1" : "0";
        }

        public static string ToDbString(this bool? value)
        {
            return value == null 
                ? "NULL" 
                : value.Value 
                    ? "1" 
                    : "0";
        }
    }
}