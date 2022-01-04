using System;
using EagleSabi.Common.Abstractions.EventSourcing.Modules;
using EagleSabi.Common.Exceptions;
using EagleSabi.Infrastructure.EventSourcing.Modules;
using EagleSabi.Infrastructure.Tests.Integration._Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace EagleSabi.Infrastructure.Tests.Integration._Helpers;

public static class Builder
{
    public static ServiceProvider ProviderInstance => _providerInstanceLazy.Value;
    private static readonly Lazy<ServiceProvider> _providerInstanceLazy = new(() => BuildServiceProvider());

    public static ServiceProvider BuildServiceProvider(Func<IServiceCollection, IServiceCollection>? configure = null)
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection = serviceCollection
            .AddInfrastructureInMemory()
            .AddTestDomain()
            .AddScoped<TestEventStore>()
            .AddScoped<IEventStore>(a => a.GetRequiredService<TestEventStore>())
            .AddScoped<InMemoryEventRepository>()
            .AddScoped<IEventRepository>(a => a.GetRequiredService<InMemoryEventRepository>());
        if (configure is not null)
            serviceCollection = configure.Invoke(serviceCollection)
                ?? throw new AssertionFailedException("configure.Invoke() returned null");
        return serviceCollection.BuildServiceProvider();
    }
}