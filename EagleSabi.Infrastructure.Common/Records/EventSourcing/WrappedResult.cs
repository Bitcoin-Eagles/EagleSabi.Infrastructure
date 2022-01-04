using EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Infrastructure.Common.Records.EventSourcing;

/// <summary>
/// Result of successfully processed and persisted command.
/// </summary>
public record WrappedResult(
    long LastSequenceId,
    IReadOnlyList<WrappedEvent> NewEvents,
    IState State,
    bool IdempotenceIdDuplicate = false);