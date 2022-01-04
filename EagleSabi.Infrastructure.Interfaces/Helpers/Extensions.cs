using EagleSabi.Common.Abstractions.Common.Modules;

namespace EagleSabi.Common.Helpers;

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
}