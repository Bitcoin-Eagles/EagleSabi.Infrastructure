using System;
using EagleSabi.Common.Abstractions.EventSourcing.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace EagleSabi.Infrastructure.Tests.Integration._Mocks;

public class TestScope
{
    public IServiceProvider Services { get; init; }
    public IEventRepository EventRepository => Services.GetRequiredService<IEventRepository>();
    public TestEventStore TestEventStore => Services.GetRequiredService<TestEventStore>();
    public IEventStore EventStore => Services.GetRequiredService<IEventStore>();

    public TestScope(IServiceProvider services)
    {
        Services = services;
    }
}