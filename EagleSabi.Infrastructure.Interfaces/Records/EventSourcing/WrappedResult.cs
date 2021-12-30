using EagleSabi.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Common.Abstractions.EventSourcing.Records;

/// <summary>
/// Result of successfully processed and persisted command.
/// </summary>
public record WrappedResult(
    long LastSequenceId,
    IReadOnlyList<WrappedEvent> NewEvents,
    IState State,
    bool IdempotenceIdDuplicate = false);