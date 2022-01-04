using EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Models;
using EagleSabi.Infrastructure.Common.Records.EventSourcing;

namespace EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Dependencies;

public interface ICommandProcessor
{
    public Result Process(ICommand command, IState aggregateState);
}