using Newtonsoft.Json;

namespace Wox.Plugin.CurrencyConverter.Exchanges.ExchangeRateApi
{
    public class PairRateResponse : ExchangeResponseBase
    {
        [JsonProperty("base_code")]
        public string BaseCode { get; set; }

        [JsonProperty("target_code")]
        public string TargetCode { get; set; }

        [JsonProperty("conversion_rate")]
        public double ConversionRate { get; set; }
    }
}