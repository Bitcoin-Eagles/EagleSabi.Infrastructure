namespace EagleSabi.Common.Records.EventSourcing;

public record AggregateUndeliveredEvents(string AggregateType, string AggregateId, IReadOnlyList<WrappedEvent> WrappedEvents)
        : AggregateKey(AggregateType, AggregateId);