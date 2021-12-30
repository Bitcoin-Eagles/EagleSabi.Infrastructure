﻿using EagleSabi.Common.Abstractions.Common.Modules;
using EagleSabi.Common.Abstractions.EventSourcing.Modules;
using EagleSabi.Infrastructure.Common.Modules;
using EagleSabi.Infrastructure.EventSourcing.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace EagleSabi.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddCommon()
            .AddEventSourcing();

    public static IServiceCollection AddInfrastructureInMemory(this IServiceCollection services) =>
        services
            .AddInfrastructure()
            .AddInMemoryEventRepository();

    public static IServiceCollection AddCommon(this IServiceCollection services) =>
        services
            .AddScoped<IPubSub, PubSub>()
            .AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

    public static IServiceCollection AddEventSourcing(this IServiceCollection services) =>
        services
            .AddScoped<IEventStore, EventStore>()
            .AddScoped<IEventPubSub, EventPubSub>();

    public static IServiceCollection AddInMemoryEventRepository(this IServiceCollection services) =>
        services
            .AddSingleton<IEventRepository, InMemoryEventRepository>();
}