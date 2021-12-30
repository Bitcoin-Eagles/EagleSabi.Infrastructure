using System;
using EagleSabi.Common.Abstractions.EventSourcing.Modules;
using EagleSabi.Infrastructure.EventSourcing.Modules;
using EagleSabi.Infrastructure.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace EagleSabi.Infrastructure.Tests.Integration.Helpers;

public static class Builder
{
    public static ServiceProvider ProviderInstance => _providerInstanceLazy.Value;
    private static readonly Lazy<ServiceProvider> _providerInstanceLazy = new(BuildServiceProvider);

    public static ServiceProvider BuildServiceProvider() =>
        new ServiceCollection()
            .AddInfrastructureInMemory()
            .AddTestDomain()
            .AddScoped<TestEventStore>()
            .AddScoped<IEventStore>(a => a.GetRequiredService<TestEventStore>())
            .AddScoped<InMemoryEventRepository>()
            .AddScoped<IEventRepository>(a => a.GetRequiredService<InMemoryEventRepository>())
            .BuildServiceProvider();
}