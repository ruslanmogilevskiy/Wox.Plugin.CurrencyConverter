using System;
using Newtonsoft.Json;

namespace Wox.Plugin.CurrencyConverter.Helpers
{
    static class SharedExtensions
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static bool EqualsInvariant(this string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}