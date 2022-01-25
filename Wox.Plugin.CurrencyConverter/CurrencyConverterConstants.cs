namespace Wox.Plugin.CurrencyConverter
{
    public class CurrencyConverterConstants
    {
        public const string LogFileName = "CurrencyConverter.log";
        
        public const string DefaultTargetCurrencies = "USD,EUR";
        
        public const string TargetCurrenciesDelimiter = "to";
        
        public const char TargetCurrenciesListSeparator = ',';

        public static class ExchangeAuthFields
        {
            public const string AuthApiKey = "authApiKey";
        }

        public static class Exchanges
        {
            public static class ExchangeRateApi
            {
                public const string ApiBaseUrl = "https://v6.exchangerate-api.com/v6";
                public const string HelpUrl = "https://www.exchangerate-api.com/docs/standard-requests";
                public const string SuccessResponseState = "success";
            }
        }
    }
}