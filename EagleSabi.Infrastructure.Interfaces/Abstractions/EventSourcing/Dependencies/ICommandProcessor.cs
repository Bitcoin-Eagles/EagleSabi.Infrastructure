using EagleSabi.Common.Abstractions.EventSourcing.Models;
using EagleSabi.Common.Records.EventSourcing;

namespace EagleSabi.Common.Abstractions.EventSourcing.Dependencies;

public interface ICommandProcessor
{
    public Result Process(ICommand command, IState aggregateState);
}