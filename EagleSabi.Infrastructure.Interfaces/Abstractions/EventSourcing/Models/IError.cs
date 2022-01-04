namespace EagleSabi.Common.Abstractions.EventSourcing.Models;

public interface IError
{
    string PropertyName { get; }
    string ErrorMessage { get; }
}