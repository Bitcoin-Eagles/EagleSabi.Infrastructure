using EagleSabi.Common.Abstractions.EventSourcing.Modules;
using EagleSabi.Infrastructure.EventSourcing.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace EagleSabi.Infrastructure.EventSourcing;

public static class EventSourcingStartup
{
    public static IServiceCollection AddEventSourcing(this IServiceCollection services) =>
        services
            .AddScoped<IEventStore, EventStore>();

    public static IServiceCollection AddInMemoryEventStore(this IServiceCollection services) =>
        services
            .AddSingleton<IEventRepository, InMemoryEventRepository>();
}