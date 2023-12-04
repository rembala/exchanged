namespace Converter
{
    public interface IISOCurrencyPairExchangeService
    {
        double? GetExchangedAmountOfIsoCurrencyPair(string userInput);
    }

    public class ISOCurrencyPairExchangeService : IISOCurrencyPairExchangeService
    {
        private readonly IISOCurrencyPairValidation _iSOCurrencyPairValidation;

        public ISOCurrencyPairExchangeService(IISOCurrencyPairValidation iSOCurrencyPairValidation)
        {
            _iSOCurrencyPairValidation = iSOCurrencyPairValidation;
        }

        public double? GetExchangedAmountOfIsoCurrencyPair(string userInput)
        {
            var splittedUserInputArguments = userInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            _iSOCurrencyPairValidation.ValidateIfUserInputArgumentsAreCorrect(splittedUserInputArguments);

            var currencyPair = splittedUserInputArguments[1];

            var moneyAmount = splittedUserInputArguments[2];

            var validMoneyAmount = GetMoneyAmountIfItIsANumber(moneyAmount);

            var exchangedAmount = GetExchangedAmountIfCurrencyPairIsValid(currencyPair, validMoneyAmount);

            return exchangedAmount;
        }

        private double? GetExchangedAmountIfCurrencyPairIsValid(string currencyPair, double moneyAmount)
        {
            if (currencyPair.Contains('/'))
            {
                var isoCurrencypair = currencyPair.Split('/', StringSplitOptions.RemoveEmptyEntries);

                _iSOCurrencyPairValidation.ValidateIfISOCurrenciesLengthAreCorrect(isoCurrencypair);
                _iSOCurrencyPairValidation.ValidateIfCurrenciesISONamesAreCorrect(isoCurrencypair);

                var mainCurrency = isoCurrencypair.First();
                var moneyCurrency = isoCurrencypair.Last();

                var exchangedISOAmountIfPairsAreTheSame = GetExchangedAmountIfISOCurrenciesAreTheSame(mainCurrency, moneyCurrency, moneyAmount);

                if (exchangedISOAmountIfPairsAreTheSame.HasValue) return exchangedISOAmountIfPairsAreTheSame;

                var exchangedISOAmountIfMoneyCurrencyIsDanish = GetExchangedAmountIfMoneyCurrencyIsDanish(mainCurrency, moneyCurrency, moneyAmount);

                if (exchangedISOAmountIfMoneyCurrencyIsDanish.HasValue) return exchangedISOAmountIfMoneyCurrencyIsDanish;

                var exchangedAmountIfMoneyCurrencyIsNotDanish = GetExchangedAmountIfMoneyAmountIsNotDanish(moneyCurrency, moneyAmount);

                if (exchangedAmountIfMoneyCurrencyIsNotDanish.HasValue) return exchangedAmountIfMoneyCurrencyIsNotDanish;
            }

            throw new Exception("Please provide valid arguments according to the exchange pattern");
        }

        private double? GetExchangedAmountIfMoneyAmountIsNotDanish(string moneyCurrency, double moneyAmount)
        {
            if (!moneyCurrency.Equals(Currencies.DKK, StringComparison.OrdinalIgnoreCase))
            {
                var currencyAmount = 100;

                var finalResult = currencyAmount * moneyAmount;

                return finalResult;
            }

            return null;
        }

        private double? GetExchangedAmountIfMoneyCurrencyIsDanish(string mainCurrency, string moneyCurrency, double moneyAmount)
        {
            if (moneyCurrency.Equals(Currencies.DKK, StringComparison.OrdinalIgnoreCase))
            {
                var currencyRatesByCurrencyIso = new Dictionary<string, double>() {
                    { Currencies.USD, DanishKronerCurrencyExchangeRates.USD },
                    { Currencies.EUR, DanishKronerCurrencyExchangeRates.EUR },
                    { Currencies.GBP, DanishKronerCurrencyExchangeRates.GBP },
                    { Currencies.SEK, DanishKronerCurrencyExchangeRates.SEK },
                    { Currencies.NOK, DanishKronerCurrencyExchangeRates.NOK },
                    { Currencies.CHF, DanishKronerCurrencyExchangeRates.CHF },
                    { Currencies.JPY, DanishKronerCurrencyExchangeRates.JPY }
                };

                var danishCurrencyAmountValue = currencyRatesByCurrencyIso[mainCurrency.ToUpper()];

                var exchangedResult = danishCurrencyAmountValue * moneyAmount;

                return exchangedResult;
            }

            return null;
        }

        private double? GetExchangedAmountIfISOCurrenciesAreTheSame(string mainCurrency, string moneyCurrency, double userInputamount)
        {
            if (mainCurrency == moneyCurrency)
            {
                var amount = Convert.ToDouble(userInputamount);

                return amount;
            }

            return null;
        }

        private double GetMoneyAmountIfItIsANumber(string moneyAmount)
        {
            if (!double.TryParse(moneyAmount, out double amount))
            {
                throw new Exception($"Money amount {moneyAmount} is not a number, please provide a valid number");
            }

            return amount;
        }
    }
}