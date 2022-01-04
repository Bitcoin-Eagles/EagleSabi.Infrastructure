namespace EagleSabi.Infrastructure.Common.Records.EventSourcing;

public record AggregateKey(string AggregateType, string AggregateId);