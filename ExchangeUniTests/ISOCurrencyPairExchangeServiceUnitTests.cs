using Converter;
using Moq;
using Xunit;

namespace ExchangeUniTests
{
    public class ISOCurrencyPairExchangeServiceUnitTests
    {
        private Mock<IISOCurrencyPairValidation> isoCurrencyPairValidationMock = new Mock<IISOCurrencyPairValidation>(MockBehavior.Strict);

        [Fact]
        public void GetExchangedAmountOfIsoCurrencyPair_MoneyAmountIsNotValidNumber_ThrowsAnException()
        {
            var userInput = "Exchange usd/dkk 100s";

            var splittedUserInput = new string[] { "Exchange", "usd/dkk", "100s" };

            isoCurrencyPairValidationMock
                .Setup(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput));

            var isoCurrencyPairExchangeService = new ISOCurrencyPairExchangeService(isoCurrencyPairValidationMock.Object);

            var returnedException = Assert.Throws<Exception>(() => isoCurrencyPairExchangeService.GetExchangedAmountOfIsoCurrencyPair(userInput));

            var expectedErrorMessage = "Money amount 100s is not a number, please provide a valid number";

            Assert.Equal(expectedErrorMessage, returnedException.Message);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput), Times.Once);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfISOCurrenciesLengthAreCorrect(It.IsAny<string[]>()), Times.Never);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfCurrenciesISONamesAreCorrect(It.IsAny<string[]>()), Times.Never);
        }

        [Theory]
        [InlineData("100", 743.94, "eur")]
        [InlineData("1", 7.4394, "eur")]
        [InlineData("2", 14.8788, "eur")]
        [InlineData("100", 663.11, "usd")]
        [InlineData("1", 6.6311, "usd")]
        [InlineData("2", 13.2622, "usd")]
        [InlineData("100", 761, "sek")]
        [InlineData("1", 7.610, "sek")]
        [InlineData("2", 15.22, "sek")]
        [InlineData("100", 852.84999999999991, "gbp")]
        [InlineData("1", 8.5285, "gbp")]
        [InlineData("2", 17.057, "gbp")]
        [InlineData("100", 784, "nok")]
        [InlineData("1", 7.840, "nok")]
        [InlineData("2", 15.68, "nok")]
        [InlineData("100", 683.58, "chf")]
        [InlineData("1", 6.8358, "chf")]
        [InlineData("2", 13.6716, "chf")]
        [InlineData("100", 5.9740, "jpy")]
        [InlineData("1", 0.059740000000000001, "jpy")]
        [InlineData("2", 0.11948, "jpy")]
        public void GetExchangedAmountOfIsoCurrencyPair_MoneyAmountIsValidNumberAndMoneyCurrencyIsDanishWithOtherCurrencyTestCases_ReturnsExchangedAmount(string amountValue, double expectedValue, string mainCurrency)
        {
            var currencyPair = $"{mainCurrency}/dkk";

            var userInput = $"Exchange {currencyPair} {amountValue}";

            var splittedUserInput = new string[] { "Exchange", currencyPair, amountValue };

            var currencyPairs = new string[] { mainCurrency, "dkk" };

            isoCurrencyPairValidationMock
                .Setup(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput));

            isoCurrencyPairValidationMock
                .Setup(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfISOCurrenciesLengthAreCorrect(currencyPairs));

            isoCurrencyPairValidationMock
                .Setup(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfCurrenciesISONamesAreCorrect(currencyPairs));

            var isoCurrencyPairExchangeService = new ISOCurrencyPairExchangeService(isoCurrencyPairValidationMock.Object);

            var exchangedCurrencyPair = isoCurrencyPairExchangeService.GetExchangedAmountOfIsoCurrencyPair(userInput);

            Assert.Equal(expectedValue, exchangedCurrencyPair);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput), Times.Once);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfISOCurrenciesLengthAreCorrect(currencyPairs), Times.Once);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfCurrenciesISONamesAreCorrect(currencyPairs), Times.Once);
        }

        [Fact]
        public void GetExchangedAmountOfIsoCurrencyPair_MoneyAmountIsValidNumberAndMoneyCurrencyNotDanish_ReturnsExchangedAmount()
        {
            var userInput = "Exchange usd/eur 100";

            var splittedUserInput = new string[] { "Exchange", "usd/eur", "100" };

            isoCurrencyPairValidationMock
                .Setup(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput));

            var currencyPair = new string[] { "usd", "eur" };

            isoCurrencyPairValidationMock
                .Setup(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfISOCurrenciesLengthAreCorrect(currencyPair));

            isoCurrencyPairValidationMock
                .Setup(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfCurrenciesISONamesAreCorrect(currencyPair));

            var isoCurrencyPairExchangeService = new ISOCurrencyPairExchangeService(isoCurrencyPairValidationMock.Object);

            var exchangedCurrencyPair = isoCurrencyPairExchangeService.GetExchangedAmountOfIsoCurrencyPair(userInput);

            Assert.Equal(10000, exchangedCurrencyPair);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput), Times.Once);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfISOCurrenciesLengthAreCorrect(currencyPair), Times.Once);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfCurrenciesISONamesAreCorrect(currencyPair), Times.Once);
        }

        [Fact]
        public void GetExchangedAmountOfIsoCurrencyPair_CurrencyPairIsNotValid_ThrowsAnException()
        {
            var userInput = "Exchange usd 100";

            var splittedUserInput = new string[] { "Exchange", "usd", "100" };

            isoCurrencyPairValidationMock
                .Setup(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput));

            var isoCurrencyPairExchangeService = new ISOCurrencyPairExchangeService(isoCurrencyPairValidationMock.Object);

            var returnedException = Assert.Throws<Exception>(() => isoCurrencyPairExchangeService.GetExchangedAmountOfIsoCurrencyPair(userInput));

            var expectedErrorMessage = "Please provide valid arguments according to the exchange pattern";

            Assert.Equal(expectedErrorMessage, returnedException.Message);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfUserInputArgumentsAreCorrect(splittedUserInput), Times.Once);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfISOCurrenciesLengthAreCorrect(It.IsAny<string[]>()), Times.Never);

            isoCurrencyPairValidationMock
                .Verify(currencyPairValidationMethod => currencyPairValidationMethod.ValidateIfCurrenciesISONamesAreCorrect(It.IsAny<string[]>()), Times.Never);
        }
    }
}