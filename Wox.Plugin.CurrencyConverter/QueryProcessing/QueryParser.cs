using System;
using System.Linq;
using Wox.Plugin.CurrencyConverter.Helpers;

namespace Wox.Plugin.CurrencyConverter.QueryProcessing
{
    public class QueryParser
    {
        readonly Query _query;

        public QueryParser(Query query)
        {
            _query = query ?? throw new InvalidOperationException("Query cannot be NULL");
        }

        public QueryParseResult Parse()
        {
            // NOTE: query must in the following formats:
            // * '100[.55] USD'                                 - converts the specified amount to the list of currencies from the settings
            // * '100[.55] USD to EUR[,<other currency>,..]'    - converts the specified amount to the specified currency(s)
            // * 'USD to EUR[,<other currency>,..]'             - shows conversion rate of the first currency to the others specified

            var parseResult = new QueryParseResult();
            if (!_query.Terms.Length.In(
                    // <amount> currency
                    2,
                    // baseCurrency to currency1[,currencyN]
                    3,
                    // <amount> baseCurrency to currency1[,currencyN]
                    4))
            {
                return parseResult;
            }

            var hasToTerm = false;
            var isValid = true;
            var isQueryCompleted = false;
            foreach (var term in _query.Terms)
            {
                if (term.IsNumeric())
                {
                    if (parseResult.Amount == null)
                    {
                        parseResult.Amount = term;
                        continue;
                    }

                    // second numeric is not what we expect.
                    isValid = false;
                    break;
                }

                if (term.Trim().EqualsInvariant(CurrencyConverterConstants.TargetCurrenciesDelimiter))
                {
                    hasToTerm = true;
                    isQueryCompleted = false;
                    continue;
                }

                var currencyCode = term.ToUpperInvariant();
                if (parseResult.BaseCurrencyCode == null)
                {
                    if (!currencyCode.IsCurrencyCode())
                    {
                        isValid = false;
                        break;
                    }
                    parseResult.BaseCurrencyCode = currencyCode;
                    isQueryCompleted = true;
                }
                else
                {
                    if (!hasToTerm)
                    {
                        isValid = false;
                        break;
                    }

                    currencyCode.SplitBy(CurrencyConverterConstants.TargetCurrenciesListSeparator)
                        .Where(_ => _.IsCurrencyCode())
                        .ForEach(parseResult.TargetCurrencyCodes.AddIfNew);

                    hasToTerm = false;
                    // if none target currencies were added the query is treated as invalid.
                    isQueryCompleted = parseResult.TargetCurrencyCodes.Any();
                }
            }

            if (!isValid || !isQueryCompleted || parseResult.BaseCurrencyCode==null || (parseResult.Amount == null && !parseResult.TargetCurrencyCodes.Any()) ||
                // ignore conversion to the same currency
                (parseResult.TargetCurrencyCodes.Count == 1 && parseResult.BaseCurrencyCode.EqualsInvariant(parseResult.TargetCurrencyCodes.First())))
            {
                parseResult.Clear();
            }

            return parseResult;
        }
    }
}