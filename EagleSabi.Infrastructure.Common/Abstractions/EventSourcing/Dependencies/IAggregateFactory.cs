using EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Dependencies;

public interface IAggregateFactory
{
    IAggregate Create(string aggregateType);

    bool TryCreate(string aggregateType, out IAggregate aggregate);
}