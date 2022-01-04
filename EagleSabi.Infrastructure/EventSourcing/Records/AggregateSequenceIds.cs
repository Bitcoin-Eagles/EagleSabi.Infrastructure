namespace EagleSabi.Infrastructure.EventSourcing.Records;

public record AggregateSequenceIds(long DeliveredSequenceId, long TransactionFirstSequenceId, long TransactionLastSequenceId);