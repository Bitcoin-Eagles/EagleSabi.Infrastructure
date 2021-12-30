using EagleSabi.Common.Abstractions.EventSourcing.Models;
using EagleSabi.Common.Abstractions.EventSourcing.Records;

namespace EagleSabi.Common.Abstractions.EventSourcing.Dependencies;

public interface ICommandProcessor
{
    public Result Process(ICommand command, IState aggregateState);
}