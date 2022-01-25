namespace Wox.Plugin.CurrencyConverter.PluginSettings
{
    /// <summary>
    /// Holds used settings for the CurrencyConverter.
    /// </summary>
    public class CurrencyConverterSettings
    {
        /// <summary>Exchange API key to access the exchange API.</summary>
        /// <remarks>You need to register on a particular exchange, get your personal API key and past it into the CurrencyConverter plugin settings.
        /// </remarks>
        public string ApiKey { get; set; }

        /// <summary>Comma-separated list of currencies to use if the Wox's entered command doesn't contain any target currency.</summary>
        /// <remarks>If no target currencies are specified in the Wox's command and this setting isn't filled then the Currency Converter will use USD
        /// and EUR currencies (<see cref="CurrencyConverterConstants.DefaultTargetCurrencies"/>) as a fallback list.</remarks>
        public string FavoriteCurrencies { get; set; }
    }
}