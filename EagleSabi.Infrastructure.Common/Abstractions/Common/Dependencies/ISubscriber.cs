namespace EagleSabi.Infrastructure.Common.Abstractions.Common.Dependencies;

public interface ISubscriber<TMessage>
{
    Task Receive(TMessage message);
}