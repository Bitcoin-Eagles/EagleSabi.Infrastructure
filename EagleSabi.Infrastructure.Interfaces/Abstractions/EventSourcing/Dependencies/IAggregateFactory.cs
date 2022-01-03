﻿using EagleSabi.Common.Abstractions.EventSourcing.Models;

namespace EagleSabi.Common.Abstractions.EventSourcing.Factories;

public interface IAggregateFactory
{
    IAggregate Create(string aggregateType);

    bool TryCreate(string aggregateType, out IAggregate aggregate);
}