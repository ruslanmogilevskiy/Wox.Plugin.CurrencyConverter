using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;
using Wox.Infrastructure.Storage;
using Wox.Plugin.CurrencyConverter.Exchanges;
using Wox.Plugin.CurrencyConverter.Helpers;
using Wox.Plugin.CurrencyConverter.Logging;
using Wox.Plugin.CurrencyConverter.PluginSettings;
using Wox.Plugin.CurrencyConverter.QueryProcessing;

namespace Wox.Plugin.CurrencyConverter
{
    public class Plugin : IPlugin, ISettingProvider
    {
        readonly CurrencyConverterSettings _customSettings;
        static IExchange Exchange { get; } = new ExchangeRateApiExchange();

        public Plugin()
        {
        }

        // NOTE: for testing purposes only.
        internal Plugin(CurrencyConverterSettings customSettings)
        {
            _customSettings = customSettings;
        }

        public void Init(PluginInitContext context)
        {
        }

        public System.Windows.Controls.Control CreateSettingPanel()
        {
            return new SettingsControl();
        }

        CurrencyConverterSettings GetSettings() => _customSettings ?? new PluginJsonStorage<CurrencyConverterSettings>().Load();

        public List<Result> Query(Query query)
        {
            try
            {
                return ProcessQuery(query);
            }
            catch (Exception exc)
            {
                Logger.Log($"[Plugin.Query] an error while doing the conversion: {exc.Message}\r\n" + $"Stack trace: {exc.StackTrace}");

                return new List<Result>
                {
                    CreateResult("CurrencyConverter: an error while doing the conversion", exc.Message)
                };
            }
        }

        List<Result> ProcessQuery(Query query)
        {
            var queryParseResult = new QueryParser(query).Parse();
            if (!queryParseResult.IsValid)
                return null;

            var settings = GetSettings();
            if (string.IsNullOrEmpty(settings.ApiKey))
            {
                return new List<Result> { CreateResult("Currency Converter: apiKey isn't specified in the plugin settings") };
            }

            Exchange.Authenticate(new Dictionary<string, string> { { CurrencyConverterConstants.ExchangeAuthFields.AuthApiKey, settings.ApiKey } });

            var targetCurrencyCodes = new List<string>(queryParseResult.TargetCurrencyCodes);
            if (!targetCurrencyCodes.Any())
            {
                var favoriteCurrencies = string.IsNullOrEmpty(settings.FavoriteCurrencies)
                    ? CurrencyConverterConstants.DefaultTargetCurrencies
                    : settings.FavoriteCurrencies;
                targetCurrencyCodes.AddRange(favoriteCurrencies.SplitBy(CurrencyConverterConstants.TargetCurrenciesListSeparator)
                    .Select(_ => _.ToUpperInvariant())
                    .Where(_ => _.IsCurrencyCode()));
            }
            // don't show the rate for the base currency.
            targetCurrencyCodes.Remove(queryParseResult.BaseCurrencyCode);

            // NOTE: for the case 'USD to EUR'
            if (queryParseResult.Amount == null)
            {
                queryParseResult.Amount = 1.ToString();
            }

            var results = Convert(queryParseResult.Amount, queryParseResult.BaseCurrencyCode, targetCurrencyCodes);
            return results;
        }

        List<Result> Convert(string amountStr, string baseCurrencyCode, IList<string> targetCurrencyCodes)
        {
            var amount = amountStr.TryParseNumeric()!.Value;
            var rates = GetExchangeRate(amount, baseCurrencyCode, targetCurrencyCodes);
            if (rates == null || !rates.Any())
            {
                return new List<Result>
                {
                    CreateResult("No exchange rates were returned for specified currencies", "Click to see the details", e =>
                    {
                        Exchange.ShowProviderHelp();
                        return true;
                    })
                };
            }

            var results = new List<Result>(targetCurrencyCodes.Count);
            foreach (var targetCurrency in targetCurrencyCodes)
            {
                if (!rates.TryGetValue(targetCurrency, out var rate))
                    continue;

                var exchangeResult = amount * rate;
                var result = CreateResult($"{amountStr} {baseCurrencyCode} = {exchangeResult} {targetCurrency}", 
                    $"1 {targetCurrency}={(1/rate):F} {baseCurrencyCode}. Press enter to copy the result",
                    e =>
                    {
                        Clipboard.SetText(exchangeResult.ToString(CultureInfo.CurrentCulture));
                        return true;
                    });
                results.Add(result);
            }

            return results;
        }

        static Dictionary<string, double> GetExchangeRate(double amount, string baseCurrencyCode, IList<string> targetCurrencyCodes)
        {
            if (targetCurrencyCodes.Count == 1)
            {
                var targetCurrency = targetCurrencyCodes.First();
                var rate = Exchange.GetPairRate(amount.ToStringInvariant(), baseCurrencyCode, targetCurrency);
                if (rate == null)
                    return null;
                return new Dictionary<string, double> { { targetCurrency, rate.Value } };
            }

            return Exchange.GetAllRates(baseCurrencyCode);
        }

        Result CreateResult(string title, string subTitle = null, Func<ActionContext, bool> action = null)
        {
            return new Result
            {
                Title = title,
                SubTitle = subTitle,
                IcoPath = "images\\exchange-white.png",
                Action = action,
                Score = int.MaxValue
            };
        }
    }
}
