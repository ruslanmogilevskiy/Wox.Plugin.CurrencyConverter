using System.Collections.Generic;

namespace Wox.Plugin.CurrencyConverter.QueryProcessing
{
    public class QueryParseResult
    {
        public bool IsValid => BaseCurrencyCode != null;

        public string Amount { get; set; }
        public string BaseCurrencyCode { get; set; }
        public IList<string> TargetCurrencyCodes { get; set; } = new List<string>();

        public void Clear()
        {
            Amount = BaseCurrencyCode = null;
            TargetCurrencyCodes.Clear();
        }
    }
}