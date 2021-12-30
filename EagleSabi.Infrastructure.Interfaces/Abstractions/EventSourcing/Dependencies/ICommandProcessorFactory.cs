using EagleSabi.Common.Abstractions.EventSourcing.Dependencies;

namespace EagleSabi.Common.Abstractions.EventSourcing.Factories;

public interface ICommandProcessorFactory
{
    ICommandProcessor Create(string aggregateType);

    bool TryCreate(string aggregateType, out ICommandProcessor commandProcessor);
}