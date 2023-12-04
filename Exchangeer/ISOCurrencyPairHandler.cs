namespace Converter
{
    internal interface IISOCurrencyPairHandler
    {
        void HandleUserProvidedISOCurrencyPair();
    }

    internal class ISOCurrencyPairHandler : IISOCurrencyPairHandler
    {
        private readonly IISOCurrencyPairExchangeService _iSOCurrencyPairConverterService;

        public ISOCurrencyPairHandler(IISOCurrencyPairExchangeService iSOCurrencyPairConverterService)
        {
            _iSOCurrencyPairConverterService = iSOCurrencyPairConverterService;
        }

        public void HandleUserProvidedISOCurrencyPair()
        {
            Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange>, e.g Exchange EUR/DKK 100");

            var userInput = Console.ReadLine();

            var exchangedAmount = _iSOCurrencyPairConverterService.GetExchangedAmountOfIsoCurrencyPair(userInput);

            Console.WriteLine(exchangedAmount);
        }
    }
}