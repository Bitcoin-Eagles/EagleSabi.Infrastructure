namespace EagleSabi.Common.Abstractions.EventSourcing.Dependencies;

public interface ICommandProcessorFactory
{
    ICommandProcessor Create(string aggregateType);

    bool TryCreate(string aggregateType, out ICommandProcessor commandProcessor);
}