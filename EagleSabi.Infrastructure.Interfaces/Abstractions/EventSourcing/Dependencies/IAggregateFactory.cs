using EagleSabi.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Common.Abstractions.EventSourcing.Dependencies;

public interface IAggregateFactory
{
    IAggregate Create(string aggregateType);

    bool TryCreate(string aggregateType, out IAggregate aggregate);
}