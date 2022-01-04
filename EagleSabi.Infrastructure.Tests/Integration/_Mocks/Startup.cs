using EagleSabi.Common.Abstractions.Common.Modules;
using EagleSabi.Common.Abstractions.EventSourcing.Dependencies;
using EagleSabi.Infrastructure.Common.Modules;
using EagleSabi.Infrastructure.Common.Services;
using EagleSabi.Infrastructure.Tests.Integration._Mocks.TestDomain;
using Microsoft.Extensions.DependencyInjection;

namespace EagleSabi.Infrastructure.Tests.Integration._Mocks;

public static class Startup
{
    public static IServiceCollection AddTestDomain(this IServiceCollection services) =>
        services
            .AddScoped<IAggregateFactory, TestDomainAggregateFactory>()
            .AddScoped<ICommandProcessorFactory, TestDomainCommandProcessorFactory>()
            .AddScoped<TestScope>()
            .AddScoped<IBackgroundTaskQueue, BackgroundTaskQueue>()
            .AddLogging()
            .AddScoped<QueuedHostedService>();
}