using System.Threading.Tasks;
using RestEase;

namespace Wox.Plugin.CurrencyConverter.Exchanges.ExchangeRateApi
{
    [BasePath("{ApiKey}")]
    [AllowAnyStatusCode]
    public interface IExchangeRateApi
    {
        [Path("ApiKey")]
        string ApiKey { get; set; }

        [Get("pair/{currency1}/{currency2}/{amount}")]
        Task<Response<PairRateResponse>> GetPairRate([Path] string currency1, [Path] string currency2, [Path] string amount);

        [Get("latest/{currency}")]
        Task<Response<AllRatesResponse>> GetAllRates([Path] string currency);
    }
}