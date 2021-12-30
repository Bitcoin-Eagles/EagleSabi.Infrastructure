namespace EagleSabi.Common.Abstractions.EventSourcing.Dependencies;

public interface ISubscriber<TMessage>
{
    Task Receive(TMessage message);
}