using System.Globalization;
using System.Linq;

namespace Wox.Plugin.CurrencyConverter.Helpers
{
    static class NumberHelper
    {
        public static double? TryParseNumeric(this string value)
        {
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out var number))
                return number;
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out number))
                return number;

            return null;
        }

        public static bool IsNumeric(this string value)
        {
            return value.TryParseNumeric() != null;
        }

        public static string ToStringInvariant(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static bool IsCurrencyCode(this string str)
        {
            return str.Length == 3;
        }

        public static bool In(this int value, params int[] values)
        {
            return values.Contains(value);
        }
    }
}