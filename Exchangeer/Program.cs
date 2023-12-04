using Converter;
using Microsoft.Extensions.DependencyInjection;
using System;

using ServiceProvider serviceProvider = GetServiceProvider();

var isoCurrencyPairHandler = serviceProvider.GetService<IISOCurrencyPairHandler>();

isoCurrencyPairHandler.HandleUserProvidedISOCurrencyPair();

ServiceProvider GetServiceProvider()
{
    var services = new ServiceCollection();

    services.AddSingleton<IISOCurrencyPairExchangeService, ISOCurrencyPairExchangeService>();

    services.AddSingleton<IISOCurrencyPairValidation, ISOCurrencyPairValidation>();

    services.AddSingleton<IISOCurrencyPairHandler, ISOCurrencyPairHandler>();

    ServiceProvider provider = services.BuildServiceProvider();

    return provider;
}