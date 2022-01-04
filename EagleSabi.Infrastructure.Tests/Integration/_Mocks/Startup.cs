using EagleSabi.Common.Abstractions.EventSourcing.Dependencies;
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
            .AddLogging();
}