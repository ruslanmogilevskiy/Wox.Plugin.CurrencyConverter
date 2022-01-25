using System.Collections.Generic;

namespace Wox.Plugin.CurrencyConverter.Exchanges
{
    public interface IExchange
    {
        void Authenticate(Dictionary<string, string> authData);

        Dictionary<string, double> GetAllRates(string currency);

        double? GetPairRate(string amount, string currencyFrom, string currencyTo);

        void ShowProviderHelp();
    }
}