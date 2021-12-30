namespace EagleSabi.Common.Abstractions.EventSourcing.Models;

public interface IAggregate
{
    IState State { get; }

    void Apply(IEvent ev);
}