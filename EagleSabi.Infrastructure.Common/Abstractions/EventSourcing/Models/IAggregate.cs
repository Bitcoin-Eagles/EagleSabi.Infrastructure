namespace EagleSabi.Infrastructure.Common.Abstractions.EventSourcing.Models;

public interface IAggregate
{
    IState State { get; }

    void Apply(IEvent ev);
}