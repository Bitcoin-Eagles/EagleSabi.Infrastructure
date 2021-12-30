namespace EagleSabi.Common.Abstractions.EventSourcing.Records;

public record AggregateKey(string AggregateType, string AggregateId);