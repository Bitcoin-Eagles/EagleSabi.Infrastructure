using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EagleSabi.Common.Exceptions;
using EagleSabi.Common.Records.EventSourcing;
using EagleSabi.Infrastructure.Tests.Integration._Helpers;
using EagleSabi.Infrastructure.Tests.Integration._Mocks;
using EagleSabi.Infrastructure.Tests.Integration._Mocks.TestDomain;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EagleSabi.Infrastructure.Tests.Integration.EventSourcing;

public class EventStoreTests : IDisposable
{
    private readonly TimeSpan _semaphoreWaitTimeout = TimeSpan.FromSeconds(20);

    protected TestScope Scope => _scopeLazy.Value;

    protected readonly IServiceScope _serviceScope;
    protected readonly Lazy<TestScope> _scopeLazy;

    public EventStoreTests()
    {
        _serviceScope = Builder.ProviderInstance.CreateScope();
        _scopeLazy = new(() => _serviceScope.ServiceProvider.GetRequiredService<TestScope>());
    }

    [Fact]
    public async Task StartRound_Success_Async()
    {
        // Arrange
        var command = new StartRound(1000, Guid.NewGuid());

        // Act
        var result = await Scope.EventStore.ProcessCommandAsync(command, nameof(TestRoundAggregate), "1");

        // Assert
        Assert.NotEmpty(result.NewEvents);
        Assert.True(result.LastSequenceId > 0);

        Assert.NotEmpty(await Scope.EventRepository.ListEventsAsync(nameof(TestRoundAggregate), "1"));
        Assert.True(
            (await Scope.EventRepository.ListAggregateIdsAsync(nameof(TestRoundAggregate)))
                .SequenceEqual(new[] { "1" }));
    }

    [Fact]
    public async Task StartRound_FailSecondTime_Async()
    {
        // Arrange
        var command1 = new StartRound(1000, Guid.NewGuid());
        var result1 = await Scope.EventStore.ProcessCommandAsync(command1, nameof(TestRoundAggregate), "1");
        var command2 = new StartRound(1000, Guid.NewGuid());

        // Act
        var exception = await Assert.ThrowsAsync<CommandFailedException>(() => Scope.EventStore.ProcessCommandAsync(command2, nameof(TestRoundAggregate), "1"));

        // Assert
        Assert.Equal(1, exception.LastSequenceId);
        Assert.True(exception.Errors.Count > 0);
        Assert.NotNull(exception.State);
    }

    [Fact]
    public async Task RegisterInput_FailNew_Async()
    {
        // Arrange
        var command = new RegisterInput("1", 1, Guid.NewGuid());

        // Act
        var exception = await Assert.ThrowsAsync<CommandFailedException>(() => Scope.EventStore.ProcessCommandAsync(command, nameof(TestRoundAggregate), "1"));

        // Assert
        Assert.Equal(nameof(TestRoundAggregate), exception.AggregateType);
        Assert.Equal("1", exception.AggregateId);
        Assert.Equal(0, exception.LastSequenceId);
        Assert.IsType<TestRoundState>(exception.State);
        Assert.Equal(TestRoundStatusEnum.New, ((TestRoundState)exception.State).Status);
        Assert.Equal(command, exception.Command);
        Assert.True(exception.Errors.Count > 0);
    }

    [Fact]
    public async Task RegisterInput_IdempotentRedelivery_Async()
    {
        // Arrange
        await Scope.EventStore.ProcessCommandAsync(new StartRound(1000, Guid.NewGuid()), nameof(TestRoundAggregate), "1");
        Assert.True(await Scope.TestEventStore.PreparedSemaphore.WaitAsync(0));
        Assert.True(await Scope.TestEventStore.AppendedSemaphore.WaitAsync(0));
        var command = new RegisterInput("1", 1_000_000, Guid.NewGuid());
        using var semaphore = new SemaphoreSlim(0);
        Scope.TestEventStore.PreparedCallback = async () =>
        {
            // Disable this callback for the second thread
            Scope.TestEventStore.PreparedCallback = null;

            // Allow to start the other thread
            semaphore.Release();

            // Wait until the other thread successfully appends its conflicting events
            Assert.True(await Scope.TestEventStore.AppendedSemaphore.WaitAsync(_semaphoreWaitTimeout));
        };

        // Act
        WrappedResult? result1 = null;
        var task1 = Task.Run(async () => result1 = await Scope.EventStore.ProcessCommandAsync(command, nameof(TestRoundAggregate), "1"));

        // Wait until we are in the PreparedCallback
        Assert.True(await semaphore.WaitAsync(_semaphoreWaitTimeout));

        var result2 = await Scope.EventStore.ProcessCommandAsync(command, nameof(TestRoundAggregate), "1");
        await task1;

        // Assert

        // result1 is conflicted by IdempotenceId (result2 went in)
        Assert.True(result1?.IdempotenceIdDuplicate);
        Assert.Equal(0, result1?.NewEvents.Count);
        Assert.Equal(2, result1?.LastSequenceId);
        Assert.NotNull(result1?.State);

        // result2 is appended
        Assert.False(result2?.IdempotenceIdDuplicate);
        Assert.Equal(1, result2?.NewEvents.Count);
        Assert.Equal(2, result2?.LastSequenceId);
        Assert.NotNull(result2?.State);
    }

    [Fact]
    public async Task RegisterInput_Conflict_Async()
    {
        // Arrange
        await Scope.EventStore.ProcessCommandAsync(new StartRound(1000, Guid.NewGuid()), nameof(TestRoundAggregate), "1");
        Assert.True(await Scope.TestEventStore.PreparedSemaphore.WaitAsync(0));
        Assert.True(await Scope.TestEventStore.AppendedSemaphore.WaitAsync(0));
        var command1 = new RegisterInput("1", 1_000_000, Guid.NewGuid());
        var command2 = new RegisterInput("2", 1_000_000, Guid.NewGuid());
        using var semaphore = new SemaphoreSlim(0);
        Scope.TestEventStore.PreparedCallback = async () =>
        {
            // Disable this callback for the second thread
            Scope.TestEventStore.PreparedCallback = null;

            // Allow to start the other thread
            semaphore.Release();

            // Wait until the other thread successfully appends its conflicting events
            Assert.True(await Scope.TestEventStore.AppendedSemaphore.WaitAsync(_semaphoreWaitTimeout));
        };

        // Act
        WrappedResult? result1 = null;
        var task1 = Task.Run(async () => result1 = await Scope.EventStore.ProcessCommandAsync(command1, nameof(TestRoundAggregate), "1"));

        // Wait until we are in the PreparedCallback
        Assert.True(await semaphore.WaitAsync(_semaphoreWaitTimeout));

        var result2 = await Scope.EventStore.ProcessCommandAsync(command2, nameof(TestRoundAggregate), "1");
        await task1;

        // Assert

        // Conflict has happened
        Assert.True(await Scope.TestEventStore.ConflictedSemaphore.WaitAsync(0));

        // result1 is conflicted and retried after result2
        Assert.False(result1?.IdempotenceIdDuplicate);
        Assert.Equal(1, result1?.NewEvents.Count);
        Assert.Equal(3, result1?.LastSequenceId);
        Assert.NotNull(result1?.State);
        Assert.IsType<TestRoundState>(result1?.State);
        Assert.Equal(2, (result1?.State as TestRoundState)?.Inputs.Count);

        // result2 is appended first
        Assert.False(result2?.IdempotenceIdDuplicate);
        Assert.Equal(1, result2?.NewEvents.Count);
        Assert.Equal(2, result2?.LastSequenceId);
        Assert.NotNull(result2?.State);
        Assert.IsType<TestRoundState>(result2?.State);
        Assert.Equal(1, (result2?.State as TestRoundState)?.Inputs.Count);
    }

    public void Dispose()
    {
        _serviceScope.Dispose();
    }
}