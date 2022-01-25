using System;
using System.Collections.Generic;
using System.Linq;

namespace Wox.Plugin.CurrencyConverter.Helpers
{
    static class CollectionHelper
    {
        public static void AddIfNew<T>(this IList<T> list, T value)
        {
            if (list != null && !list.Contains(value))
                list.Add(value);
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> doAction)
        {
            foreach (var item in list)
            {
                doAction(item);
            }
        }

        public static IEnumerable<string> SplitBy(this string str, char separator)
        {
            return str.Split(separator)
                .Select(_ => _.Trim())
                .Where(_ => _.Length > 0);
        }
    }
}