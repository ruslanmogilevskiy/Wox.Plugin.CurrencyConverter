using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wox.Plugin.CurrencyConverter.Exchanges.ExchangeRateApi
{
    public class AllRatesResponse : ExchangeResponseBase
    {
        [JsonProperty("base_code")]
        public string BaseCode { get; set; }

        [JsonProperty("conversion_rates")]
        public Dictionary<string, double> ConversionRates { get; set; }
    }
}