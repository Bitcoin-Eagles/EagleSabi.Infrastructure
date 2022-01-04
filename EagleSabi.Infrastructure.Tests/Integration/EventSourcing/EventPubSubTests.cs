using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EagleSabi.Common.Helpers;
using EagleSabi.Common.Records.Common;
using EagleSabi.Common.Records.EventSourcing;
using EagleSabi.Infrastructure.Tests._Common.Exceptions;
using EagleSabi.Infrastructure.Tests.Integration._Helpers;
using EagleSabi.Infrastructure.Tests.Integration._Mocks;
using EagleSabi.Infrastructure.Tests.Integration._Mocks.TestDomain;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace EagleSabi.Infrastructure.Tests.Integration.EventSourcing;

public class EventPubSubTests : IAsyncLifetime
{
    protected CancellationToken TimeoutCancellation => new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token;
    protected TestScope Scope { get; init; }

    protected readonly IServiceScope _serviceScope;

    public EventPubSubTests()
    {
        _serviceScope = Builder.ProviderInstance.CreateScope();
        Scope = _serviceScope.ServiceProvider.GetRequiredService<TestScope>();
    }

    public Task InitializeAsync()
    {
        return Scope.QueuedHostedService.StartAsync(default);
    }

    [Fact]
    public async Task Receive_Async()
    {
        // Arrange
        var command = new StartRound(1000, Guid.NewGuid());
        var receivedEvents = new List<WrappedEvent>();
        await Scope.EventPubSub.SubscribeAsync(new Subscriber<WrappedEvent<RoundStarted>>(a => receivedEvents.Add(a)));

        // Act
        var result = await Scope.EventStore.ProcessCommandAsync(command, nameof(TestRoundAggregate), "1");
        await Scope.BackgroundTaskQueue.WaitAsync(TimeoutCancellation);

        // Assert
        receivedEvents.ShouldBe(result.NewEvents);
    }

    [Fact]
    public async Task Receive_TwoSubscribers_Exceptions_Async()
    {
        // Arrange
        var command = new StartRound(1000, Guid.NewGuid());
        var receivedEvents1 = new List<WrappedEvent>();
        var receivedEvents2 = new List<WrappedEvent>();
        await Scope.EventPubSub.SubscribeAsync(new Subscriber<WrappedEvent<RoundStarted>>(a =>
        {
            receivedEvents1.Add(a);
            throw new TestException("1");
        }));
        await Scope.EventPubSub.SubscribeAsync(new Subscriber<WrappedEvent<RoundStarted>>(a =>
        {
            receivedEvents2.Add(a);
            throw new TestException("2");
        }));

        // Act
        await Scope.EventStore.ProcessCommandAsync(command!, nameof(TestRoundAggregate), "1");
        await Scope.BackgroundTaskQueue.WaitAsync(TimeoutCancellation);

        // Assert
        var appendedEvents = await Scope.EventRepository.ListEventsAsync(nameof(TestRoundAggregate), "1");
        receivedEvents1.ShouldBe(appendedEvents);
        receivedEvents2.ShouldBe(appendedEvents);
    }

    public Task DisposeAsync()
    {
        return Scope.QueuedHostedService.StopAsync(default);
    }
}