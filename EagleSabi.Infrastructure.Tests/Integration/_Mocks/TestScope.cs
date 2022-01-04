using System;
using EagleSabi.Common.Abstractions.Common.Modules;
using EagleSabi.Common.Abstractions.EventSourcing.Modules;
using EagleSabi.Infrastructure.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EagleSabi.Infrastructure.Tests.Integration._Mocks;

public class TestScope
{
    public IServiceProvider Services { get; init; }
    public IEventRepository EventRepository => Services.GetRequiredService<IEventRepository>();
    public TestEventStore TestEventStore => Services.GetRequiredService<TestEventStore>();
    public IEventStore EventStore => Services.GetRequiredService<IEventStore>();
    public IBackgroundTaskQueue BackgroundTaskQueue => Services.GetRequiredService<IBackgroundTaskQueue>();
    public IEventPubSub EventPubSub => Services.GetRequiredService<IEventPubSub>();
    public IHostedService QueuedHostedService => Services.GetRequiredService<QueuedHostedService>();

    public TestScope(IServiceProvider services)
    {
        Services = services;
    }
}