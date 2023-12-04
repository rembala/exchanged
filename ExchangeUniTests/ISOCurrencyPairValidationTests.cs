using System.Text;
using Converter;
using Xunit;

namespace ExchangeUniTests
{
    public class ISOCurrencyPairValidationTests
    {
        [Theory]
        [MemberData(nameof(GetSplittedUserArgumnetsTestCases))]
        public void ValidateIfUserInputArgumentsAreCorrect_SplittedUserArgumentsTestCases_ThrowsAnException(string[] splittedUserInput)
        {
            var errorMessage = "Please provide three arguments seperated by spaces according to the usage pattern";

            var isoCurrencyPairValidation = new ISOCurrencyPairValidation();

            var returnedException = Assert.Throws<Exception>(() => isoCurrencyPairValidation.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput));

            Assert.Equal(errorMessage, returnedException.Message);
        }

        [Fact]
        public void ValidateIfUserInputArgumentsAreCorrect_SplittedUserArgumentsAreThree_NotThrowsAnException()
        {
            var splittedUserInput = new string[] { "Exchange", "usd", "asfsa" };

            var isoCurrencyPairValidation = new ISOCurrencyPairValidation();

            isoCurrencyPairValidation.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput);
        }

        [Theory]
        [MemberData(nameof(GetIsoCurrenciesTestCases))]
        public void ValidateIfISOCurrenciesLengthAreCorrect_IsoCurrencyPairTestCases_ThrowsAnException(string[] isoCurrencypair)
        {
            var errorMessage = "Please provide valid ISO currency pair (main and money currency), e.g EUR/DKK";

            var isoCurrencyPairValidation = new ISOCurrencyPairValidation();

            var returnedException = Assert.Throws<Exception>(() => isoCurrencyPairValidation.ValidateIfISOCurrenciesLengthAreCorrect(isoCurrencypair));

            Assert.Equal(errorMessage, returnedException.Message); ;
        }

        [Fact]
        public void ValidateIfISOCurrenciesLengthAreCorrect_IsoCurrenciesPairAreTwo_NotThrowsAnException()
        {
            var splittedUserInput = new string[] { "dkk", "usd" };

            var isoCurrencyPairValidation = new ISOCurrencyPairValidation();

            isoCurrencyPairValidation.ValidateIfISOCurrenciesLengthAreCorrect(splittedUserInput);
        }

        [Fact]
        public void ValidateIfCurrenciesISONameIsCorrect_IsoCurrencyNamesAreNotCorrectWhenUserPassesOneInvalidCurrency_ThrowsAnException()
        {
            var splittedUserInput = new string[] { "eu", "dkk" };

            var builder = new StringBuilder($"Cannot convert ISO pair, because there are some invalid currencies: {Environment.NewLine}");

            builder.Append($"eu {Environment.NewLine}");

            var isoCurrencyPairValidation = new ISOCurrencyPairValidation();

            var returnedException = Assert.Throws<Exception>(() => isoCurrencyPairValidation.ValidateIfCurrenciesISONamesAreCorrect(splittedUserInput));

            Assert.Contains(builder.ToString(), returnedException.Message); ;
        }

        [Fact]
        public void ValidateIfCurrenciesISONameIsCorrect_IsoCurrencyNamesAreNotCorrectWhenUserPassesTwoInvalidCurrencies_ThrowsAnException()
        {
            var splittedUserInput = new string[] { "eu", "dkdk" };

            var builder = new StringBuilder($"Cannot convert ISO pair, because there are some invalid currencies: {Environment.NewLine}");

            builder.Append($"eu {Environment.NewLine}");
            builder.Append($"dkdk {Environment.NewLine}");

            var isoCurrencyPairValidation = new ISOCurrencyPairValidation();

            var returnedException = Assert.Throws<Exception>(() => isoCurrencyPairValidation.ValidateIfCurrenciesISONamesAreCorrect(splittedUserInput));

            Assert.Contains(builder.ToString(), returnedException.Message); ;
        }

        [Fact]
        public void ValidateIfCurrenciesISONameIsCorrect_IsoCurrencyNamesAretCorrect_NotThrowsAnException()
        {
            var splittedUserInput = new string[] { "eur", "dkk" };

            var isoCurrencyPairValidation = new ISOCurrencyPairValidation();

            isoCurrencyPairValidation.ValidateIfCurrenciesISONamesAreCorrect(splittedUserInput);
        }

        public static IEnumerable<object[]> GetSplittedUserArgumnetsTestCases()
        {
            yield return new object[] { new string[] { "Exchange", "usd", "100", "1002" } };
            yield return new object[] { new string[] { "Exchange", "usd" } };
        }

        public static IEnumerable<object[]> GetIsoCurrenciesTestCases()
        {
            yield return new object[] { new string[] { "Exchange", "usd", "asfsa" } };
            yield return new object[] { new string[] { "Exchange" } };
        }
    }
}