using System.Text;

namespace Converter
{
    public interface IISOCurrencyPairValidation
    {
        void ValidateIfUserInputArgumentsAreCorrect(string[] splittedUserInputArguments);
        void ValidateIfISOCurrenciesLengthAreCorrect(string[] isoCurrencypair);
        void ValidateIfCurrenciesISONamesAreCorrect(string[] isoCurrencypair);
    }

    public class ISOCurrencyPairValidation : IISOCurrencyPairValidation
    {
        public void ValidateIfUserInputArgumentsAreCorrect(string[] splittedUserInputArguments)
        {
            if (splittedUserInputArguments.Length > 3 || splittedUserInputArguments.Length < 3)
            {
                throw new Exception("Please provide three arguments seperated by spaces according to the usage pattern");
            }
        }

        public void ValidateIfISOCurrenciesLengthAreCorrect(string[] isoCurrencypair)
        {
            if (isoCurrencypair.Length < 2 || isoCurrencypair.Length > 2)
            {
                throw new Exception("Please provide valid ISO currency pair (main and money currency), e.g EUR/DKK");
            }
        }

        public void ValidateIfCurrenciesISONamesAreCorrect(string[] userProvidedIsoCurrencyPair)
        {
            var isoCurrencyNames = new string[] {
                Currencies.EUR,
                Currencies.USD,
                Currencies.GBP,
                Currencies.SEK,
                Currencies.NOK,
                Currencies.CHF,
                Currencies.JPY,
                Currencies.DKK
            };

            var invalidCurrencies =
                userProvidedIsoCurrencyPair.Where(providedUserCurrencyName => !isoCurrencyNames.Any(isoCurrencyName => isoCurrencyName.Equals(providedUserCurrencyName, StringComparison.OrdinalIgnoreCase))
            );

            AppendErrorMessagesIfInvalidISOCurrenciesExists(invalidCurrencies);
        }

        private void AppendErrorMessagesIfInvalidISOCurrenciesExists(IEnumerable<string> invalidCurrencies)
        {
            if (invalidCurrencies.Any())
            {
                var errorMessageBuilder = new StringBuilder($"Cannot convert ISO pair, because there are some invalid currencies: {Environment.NewLine}");

                foreach (var invalidCurrency in invalidCurrencies)
                {
                    errorMessageBuilder.Append($"{invalidCurrency} {Environment.NewLine}");
                }

                var errorMessage = errorMessageBuilder.ToString();

                throw new Exception(errorMessage);
            }
        }
    }
}