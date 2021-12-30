namespace EagleSabi.Common.Abstractions.EventSourcing.Records;

public record AggregateUndeliveredEvents(string AggregateType, string AggregateId, IReadOnlyList<WrappedEvent> WrappedEvents)
        : AggregateKey(AggregateType, AggregateId);