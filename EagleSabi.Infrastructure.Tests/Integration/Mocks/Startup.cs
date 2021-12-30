using EagleSabi.Common.Abstractions.EventSourcing.Factories;
using EagleSabi.Infrastructure.Tests.Integration.Mocks.TestDomain;
using Microsoft.Extensions.DependencyInjection;

namespace EagleSabi.Infrastructure.Tests.Integration.Mocks;

public static class Startup
{
    public static IServiceCollection AddTestDomain(this IServiceCollection services) =>
        services.AddScoped<IAggregateFactory, TestDomainAggregateFactory>()
            .AddScoped<ICommandProcessorFactory, TestDomainCommandProcessorFactory>();
}