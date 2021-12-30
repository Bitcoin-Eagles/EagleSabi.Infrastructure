using System;
using EagleSabi.Common.Abstractions.EventSourcing.Modules;
using EagleSabi.Infrastructure.EventSourcing;
using EagleSabi.Infrastructure.EventSourcing.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace EagleSabi.Infrastructure.Tests.Integration.Helpers;

public static class Builder
{
    public static ServiceProvider ProviderInstance => _providerInstanceLazy.Value;
    private static readonly Lazy<ServiceProvider> _providerInstanceLazy = new(BuildServiceProvider);

    public static ServiceProvider BuildServiceProvider() =>
        new ServiceCollection()
            .AddEventSourcing()
            .AddInMemoryEventStore()
            .AddScoped<IEventRepository, InMemoryEventRepository>()
            .BuildServiceProvider();
}