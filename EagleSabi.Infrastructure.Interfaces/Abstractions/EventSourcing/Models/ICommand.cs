namespace EagleSabi.Common.Abstractions.EventSourcing.Models;

public interface ICommand
{
    Guid IdempotenceId { get; }
}