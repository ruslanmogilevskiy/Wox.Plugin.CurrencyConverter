using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Windows.Forms;
using RestEase;
using Wox.Plugin.CurrencyConverter.Exchanges.ExchangeRateApi;
using Wox.Plugin.CurrencyConverter.Helpers;
using Wox.Plugin.CurrencyConverter.Logging;

namespace Wox.Plugin.CurrencyConverter.Exchanges
{
    public class ExchangeRateApiExchange : IExchange
    {
        IExchangeRateApi _apiClient;

        IExchangeRateApi ApiClient =>
            _apiClient ?? (_apiClient = RestClient.For<IExchangeRateApi>(CurrencyConverterConstants.Exchanges.ExchangeRateApi.ApiBaseUrl));

        public void Authenticate(Dictionary<string, string> authData)
        {
            ApiClient.ApiKey = authData[CurrencyConverterConstants.ExchangeAuthFields.AuthApiKey];
        }

        public Dictionary<string, double> GetAllRates(string currency)
        {
            var apiResponse = ApiClient.GetAllRates(currency).Result;
            var responseMessage = apiResponse.ResponseMessage;

            if (!responseMessage.IsSuccessStatusCode)
            {
                LogHttpError(responseMessage, "GetAllRates", $"{currency} currency request");
                return default;
            }
            
            var response = apiResponse.GetContent();

            if (response.Result != CurrencyConverterConstants.Exchanges.ExchangeRateApi.SuccessResponseState)
            {
                Logger.Log($"[ExchangeRateApiExchange.GetAllRates] The {currency} rates request has failed with error '{response.ErrorType}'.");
            }

            return response.ConversionRates;
        }

        public double? GetPairRate(string amount, string currencyFrom, string currencyTo)
        {
            var apiResponse = ApiClient.GetPairRate(currencyFrom, currencyTo, amount).Result;
            var responseMessage = apiResponse.ResponseMessage;

            if (!responseMessage.IsSuccessStatusCode)
            {
                LogHttpError(responseMessage, "GetPairRate", $"{amount} {currencyFrom}->{currencyTo} conversion request");
                return default;
            }
            var response = apiResponse.GetContent();

            if (response.Result != CurrencyConverterConstants.Exchanges.ExchangeRateApi.SuccessResponseState)
            {
                Logger.Log($"[ExchangeRateApiExchange.GetPairRate] The {amount} {currencyFrom}->{currencyTo} exchange request has failed with error '{response.ErrorType}'.");

            }

            return apiResponse.GetContent().ConversionRate;
        }

        static void LogHttpError(HttpResponseMessage responseMessage, string methodName, string contextDescription)
        {
            Logger.Log($"[ExchangeRateApiExchange.{methodName}]: Non-successful response was received for {contextDescription}.\r\n" +
                            $"Response code: {responseMessage.StatusCode}; Response body: {responseMessage.Content.ReadAsStringAsync().Result}");
        }

        public void ShowProviderHelp()
        {
            Process.Start(CurrencyConverterConstants.Exchanges.ExchangeRateApi.HelpUrl);
        }
    }
}