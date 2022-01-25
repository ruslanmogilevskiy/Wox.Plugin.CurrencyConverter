using Newtonsoft.Json;

namespace Wox.Plugin.CurrencyConverter.Exchanges.ExchangeRateApi
{
    public class ExchangeResponseBase
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("error-type")]
        public string ErrorType { get; set; }
    }
}