using EagleSabi.Infrastructure.Common.Abstractions.Common.Dependencies;
using EagleSabi.Infrastructure.Common.Abstractions.Common.Modules;
using EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Modules;
using EagleSabi.Infrastructure.Common.Records.EventSourcing;

namespace EagleSabi.Infrastructure.Common.Helpers;

public static class Extensions
{
    /// <summary>
    /// Executes <paramref name="action"/> on each element of the <paramref name="list"/>
    /// regardless if one throws an exception. Then throws AggregateException
    /// containing all exceptions if any. Unpacks one level of AggregateException
    /// thrown from an action.
    /// </summary>
    public static void ForEachAggregatingExceptions<T>(this IEnumerable<T> list, Action<T> action, string? message = null)
    {
        Helpers.AggregateExceptions(list.Select(a => (Action)(() => action(a))), message);
    }

    /// <summary>
    /// Executes <paramref name="action"/> on each element of the <paramref name="list"/>
    /// regardless if one throws an exception. Then throws AggregateException
    /// containing all exceptions if any. Unpacks one level of AggregateException
    /// thrown from an action.
    /// </summary>
    /// <exception cref="OperationCanceledException">if <paramref name="cancellationToken"/> requested cancellation</exception>
    /// <exception cref="ObjectDisposedException">if <paramref name="cancellationToken"/> is disposed</exception>
    public static async Task ForEachAggregatingExceptionsAsync<T>(this IEnumerable<T> list, Func<T, Task> action, string? message = null, CancellationToken cancellationToken = default)
    {
        await Helpers.AggregateExceptionsAsync(
            list.Select(a => (Func<Task>)(async () => await action.Invoke(a).ConfigureAwait(false))),
            message, cancellationToken)
            .ConfigureAwait(false);
    }

    public static async Task PublishDynamicAsync(this IPubSub pubSub, dynamic message)
    {
        await pubSub.PublishAsync(message).ConfigureAwait(false);
    }

    public static async Task SubscribeAllAsync(this IEventPubSub eventPubSub, object subscriber)
    {
        Guard.NotNull(eventPubSub, nameof(eventPubSub));
        Guard.NotNull(subscriber, nameof(subscriber));
        var interfaces = subscriber.GetType().GetInterfaces();
        foreach (var @interface in interfaces)
        {
            Type topic;
            if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(ISubscriber<>)
                && (topic = @interface.GetGenericArguments()[0]).IsGenericType
                    && topic.GetGenericTypeDefinition() == typeof(WrappedEvent<>))
            {
                var domainEventType = topic.GetGenericArguments()[0];
                var subscribeTask = (Task)eventPubSub.GetType().GetMethod(nameof(IEventPubSub.SubscribeAsync))!
                    .MakeGenericMethod(domainEventType).Invoke(eventPubSub, new[] { subscriber })!;
                await subscribeTask.ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Waits until all previously enqueued tasks have been finished. Note: Queue doesn't necesarilly has to be empty afterwards.
    /// </summary>
    public static async Task WaitAsync(this IBackgroundTaskQueue queue, CancellationToken cancellationToken)
    {
        var processed = new TaskCompletionSource();
        var enqueue = queue.QueueBackgroundWorkItemAsync(_ => { processed.SetResult(); return ValueTask.CompletedTask; });
        await enqueue.AsTask().WaitAsync(cancellationToken).ConfigureAwait(false);
        await processed.Task.WaitAsync(cancellationToken).ConfigureAwait(false);
    }
}